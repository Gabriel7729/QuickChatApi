using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Infrastructure.Dto.ChatDtos;
using ApiTemplate.Infrastructure.Dto.UserDtos;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

[Authorize]
public class StartChat : EndpointBaseAsync
  .WithRequest<StartChatRequest>
  .WithActionResult<ChatResponseDto>
{
  private readonly IRepository<User> _userRepository;
  private readonly IRepository<Chat> _chatRepository;

  public StartChat(
    IRepository<User> userRepository,
    IRepository<Chat> chatRepository)
  {
    _userRepository = userRepository;
    _chatRepository = chatRepository;
  }

  [HttpPost("/api/Chat/Start")]
  [SwaggerOperation(
          Summary = "Start a new chat",
          Description = "Start a new chat with a contact",
          OperationId = "Chat.Start",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<ChatResponseDto>> HandleAsync([FromBody] StartChatRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
      User? contact = await _userRepository.GetByIdAsync(request.ContactId, cancellationToken);

      if (user == null || contact == null)
        return NotFound(Result<ChatResponseDto>.Error(new string[] { "User or contact not found" }));

      var chat = new Chat
      {
        IsGroupChat = false
      };

      await _chatRepository.AddAsync(chat, cancellationToken);

      var userChat = new UserChat
      {
        UserId = user.Id,
        ChatId = chat.Id
      };

      var contactChat = new UserChat
      {
        UserId = contact.Id,
        ChatId = chat.Id
      };

      chat.UserChats.Add(userChat);
      chat.UserChats.Add(contactChat);

      await _chatRepository.SaveChangesAsync(cancellationToken);

      var chatResponse = new ChatResponseDto
      {
        Id = chat.Id,
        IsGroupChat = chat.IsGroupChat,
        Participants = chat.UserChats.Select(uc => new UserResponseDto
        {
          Id = uc.User.Id,
          Name = uc.User.Name,
          LastName = uc.User.LastName,
          Email = uc.User.Email,
          PhoneNumber = uc.User.PhoneNumber
        }).ToList(),
        LastMessage = null // No messages yet in a new chat
      };

      var result = Result<ChatResponseDto>.Success(chatResponse);

      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(Result<ChatResponseDto>.Error(new string[] { ex.Message }));
    }
  }
}
