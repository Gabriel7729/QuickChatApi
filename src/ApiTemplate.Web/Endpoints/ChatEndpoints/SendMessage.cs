using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Core.Entities.ChatAggregate.Specifications;
using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Models;
using ApiTemplate.Infrastructure.Dto.MessageDtos;
using ApiTemplate.SharedKernel.Interfaces;
using ApiTemplate.Web.Hubs;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;

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
        Timestamp = DateTime.Now,
        SenderId = request.SenderId
      };

      await _messageRepository.AddAsync(message, cancellationToken);

      GetMessagesWithSenderSpec getMessagesWithSenderSpec = new GetMessagesWithSenderSpec(request.ChatId);
      var newChats = await _chatRepository.GetBySpecAsync(getMessagesWithSenderSpec, cancellationToken);
      if (newChats == null)
        return NotFound(Result<List<MessageResponseDto>>.Error(new string[] { "Chat not found" }));

      var messages = newChats.Messages
        .Select(m => new MessageResponseDto
        {
          Id = m.Id,
          Content = m.Content,
          Timestamp = m.Timestamp,
          SenderId = m.SenderId,
          SenderName = m.Sender.Name // Assuming that the Sender navigation property is loaded
        }).ToList().OrderBy(x => x.Timestamp);

      await _chatHubContext.Clients.All.SendAsync("ReceiveMessages", messages, cancellationToken: cancellationToken);

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
