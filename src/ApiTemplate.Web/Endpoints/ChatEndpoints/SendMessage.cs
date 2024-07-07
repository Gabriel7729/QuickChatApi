using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Infrastructure.Dto.MessageDtos;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;
using ApiTemplate.Web.Hubs;
using ApiTemplate.Core.Entities.UserAggregate;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

[Authorize]
public class SendMessage : EndpointBaseAsync
  .WithRequest<SendMessageRequest>
  .WithActionResult<SendMessageResponseDto>
{
  private readonly IRepository<Chat> _chatRepository;
  private readonly IRepository<User> _userRepository;
  private readonly IRepository<Message> _messageRepository;
  private readonly IHubContext<ChatHub> _chatHubContext;

  public SendMessage(
    IRepository<Chat> chatRepository, 
    IRepository<User> userRepository, 
    IRepository<Message> messageRepository, 
    IHubContext<ChatHub> chatHubContext)
  {
    _chatRepository = chatRepository;
    _userRepository = userRepository;
    _messageRepository = messageRepository;
    _chatHubContext = chatHubContext;
  }

  [HttpPost("/api/Chat/SendMessage")]
  [SwaggerOperation(
          Summary = "Send a message to a chat",
          Description = "Send a message to a chat or group",
          OperationId = "Chat.SendMessage",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<SendMessageResponseDto>> HandleAsync([FromBody] SendMessageRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      var chat = await _chatRepository.GetByIdAsync(request.ChatId, cancellationToken);
      if (chat == null)
        return NotFound(Result<SendMessageResponseDto>.Error(new string[] { "Chat not found" }));

      var user = await _userRepository.GetByIdAsync(request.SenderId, cancellationToken);
      if (user == null)
        return NotFound(Result<SendMessageResponseDto>.Error(new string[] { "User not found" }));

      var message = new Message
      {
        ChatId = request.ChatId,
        Content = request.Content,
        Timestamp = DateTime.UtcNow,
        SenderId = request.SenderId
      };

      await _messageRepository.AddAsync(message, cancellationToken);

      // Notify all clients in the chat
      await _chatHubContext.Clients.Group(chat.Id.ToString()).SendAsync("ReceiveMessage", new
      {
        message.Id,
        message.Content,
        message.Timestamp,
        message.SenderId,
        SenderName = user.Name + " " + user.LastName
      }, cancellationToken: cancellationToken);

      var response = new SendMessageResponseDto
      {
        Id = message.Id,
        Content = message.Content,
        Timestamp = message.Timestamp,
        SenderId = message.SenderId,
        SenderName = user.Name + " " + user.LastName
      };

      return Ok(Result<SendMessageResponseDto>.Success(response));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<SendMessageResponseDto>.Error(new string[] { ex.Message }));
    }
  }
}
