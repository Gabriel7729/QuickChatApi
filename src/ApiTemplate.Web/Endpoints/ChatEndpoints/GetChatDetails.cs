using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Core.Entities.ChatAggregate.Specifications;
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
public class GetChatDetails : EndpointBaseAsync
  .WithRequest<Guid>
  .WithActionResult<ChatDetailsResponseDto>
{
  private readonly IRepository<Chat> _chatRepository;
  private readonly IRepository<User> _userRepository;

  public GetChatDetails(IRepository<Chat> chatRepository, IRepository<User> userRepository)
  {
    _chatRepository = chatRepository;
    _userRepository = userRepository;
  }

  [HttpGet("/api/Chat/{chatId:guid}/Details")]
  [SwaggerOperation(
          Summary = "Get the details of a chat",
          Description = "Get the details of a chat including common groups or group members",
          OperationId = "Chat.GetDetails",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<ChatDetailsResponseDto>> HandleAsync([FromRoute] Guid chatId, CancellationToken cancellationToken = default)
  {
    try
    {
      var chat = await _chatRepository.GetByIdAsync(chatId, cancellationToken);
      if (chat == null)
        return NotFound(Result<ChatDetailsResponseDto>.Error(new string[] { "Chat not found" }));

      if (chat.IsGroupChat)
      {
        // It's a group chat, get the group details
        var group = await _chatRepository
          .GetBySpecAsync(new GetGroupChatSpec(chat.Id), cancellationToken);

        if (group == null)
        {
          return NotFound(Result<ChatDetailsResponseDto>.Error(new string[] { "Group not found" }));
        }

        var response = new ChatDetailsResponseDto
        {
          Id = chat.Id,
          IsGroupChat = true,
          GroupName = group.Group.Name,
          Members = group.Group.GroupMembers.Select(gm => new UserResponseDto
          {
            Id = gm.User.Id,
            Name = gm.User.Name,
            LastName = gm.User.LastName,
            Email = gm.User.Email,
            PhoneNumber = gm.User.PhoneNumber
          }).ToList()
        };

        return Ok(Result<ChatDetailsResponseDto>.Success(response));
      }
      else
      {
        // It's a direct chat, get common groups
        var userIds = chat.UserChats.Select(uc => uc.UserId).ToList();
        var users = await _userRepository.ListAsync(new GetUserWithGroupsSpec(userIds), cancellationToken);

        if (users == null || users.Count != userIds.Count)
        {
          return NotFound(Result<ChatDetailsResponseDto>.Error(new string[] { "One or more users not found" }));
        }

        var commonGroups = users
          .SelectMany(u => u.GroupMembers)
          .GroupBy(gm => gm.GroupId)
          .Where(g => g.Count() > 1)
          .Select(g => g.First().Group.Name)
          .ToList();

        var response = new ChatDetailsResponseDto
        {
          Id = chat.Id,
          IsGroupChat = false,
          CommonGroups = commonGroups,
          Members = users.Select(u => new UserResponseDto
          {
            Id = u.Id,
            Name = u.Name,
            LastName = u.LastName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber
          }).ToList()
        };

        return Ok(Result<ChatDetailsResponseDto>.Success(response));
      }
    }
    catch (Exception ex)
    {
      return BadRequest(Result<ChatDetailsResponseDto>.Error(new string[] { ex.Message }));
    }
  }
}
