using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Entities.UserAggregate.Specifications;
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
public class GetUserChats : EndpointBaseAsync
  .WithRequest<GetUserChatsRequest>
  .WithActionResult<List<ChatResponseDto>>
{
  private readonly IRepository<User> _userRepository;

  public GetUserChats(
    IRepository<User> userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpGet("/api/Chat/User/{userId:guid}")]
  [SwaggerOperation(
          Summary = "Get the chats from a user",
          Description = "Get the chats from a user",
          OperationId = "User.Chats",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<List<ChatResponseDto>>> HandleAsync([FromRoute] GetUserChatsRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      GetUserChatsSpec getUserChatsSpec = new GetUserChatsSpec(request.UserId);
      User? user = await _userRepository.GetBySpecAsync(getUserChatsSpec, cancellationToken);
      if (user == null)
        return NotFound(Result<List<ChatResponseDto>>.Error(new string[] { "User not found" }));

      List<ChatResponseDto> chats = user.UserChats.Select(uc => new ChatResponseDto
      {
        Id = uc.Chat.Id,
        IsGroupChat = uc.Chat.IsGroupChat,
        GroupName = uc.Chat.IsGroupChat ? uc.Chat.Group.Name : null,
        Participants = uc.Chat.UserChats.Select(u => new UserResponseDto
        {
          Id = u.User.Id,
          Name = u.User.Name,
          LastName = u.User.LastName,
          Email = u.User.Email,
          PhoneNumber = u.User.PhoneNumber
        }).ToList(),
        LastMessage = uc.Chat.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Content,
        LastMessageTimestamp = uc.Chat.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Timestamp
      }).ToList();

      var result = Result<List<ChatResponseDto>>.Success(chats);

      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(Result<List<ChatResponseDto>>.Error(new string[] { ex.Message }));
    }
  }
}

