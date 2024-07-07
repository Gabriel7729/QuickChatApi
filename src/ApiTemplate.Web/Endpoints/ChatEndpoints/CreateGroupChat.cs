using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Entities.UserAggregate.Specifications;
using ApiTemplate.Infrastructure.Dto.GroupDtos;
using ApiTemplate.Infrastructure.Dto.UserDtos;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

[Authorize]
public class CreateGroupChat : EndpointBaseAsync
  .WithRequest<CreateGroupChatRequest>
  .WithActionResult<GroupChatResponseDto>
{
  private readonly IRepository<User> _userRepository;
  private readonly IRepository<Chat> _chatRepository;
  private readonly IRepository<Group> _groupRepository;

  public CreateGroupChat(
    IRepository<User> userRepository,
    IRepository<Chat> chatRepository,
    IRepository<Group> groupRepository)
  {
    _userRepository = userRepository;
    _chatRepository = chatRepository;
    _groupRepository = groupRepository;
  }

  [HttpPost("/api/Chat/CreateGroup")]
  [SwaggerOperation(
          Summary = "Create a new group chat",
          Description = "Create a new group chat with members",
          OperationId = "Chat.CreateGroup",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<GroupChatResponseDto>> HandleAsync([FromBody] CreateGroupChatRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      var admin = await _userRepository.GetByIdAsync(request.AdminId, cancellationToken);
      if (admin == null)
        return NotFound(Result<GroupChatResponseDto>.Error(new string[] { "Admin user not found" }));

      var members = await _userRepository.ListAsync(new GetUserByIdsSpec(request.MemberIds), cancellationToken);
      if (members.Count != request.MemberIds.Count)
        return BadRequest(Result<GroupChatResponseDto>.Error(new string[] { "Some members not found" }));

      var chat = new Chat
      {
        IsGroupChat = true
      };

      await _chatRepository.AddAsync(chat, cancellationToken);

      var group = new Group
      {
        Name = request.GroupName,
        AdminId = admin.Id,
        Chat = chat
      };

      await _groupRepository.AddAsync(group, cancellationToken);

      var userChats = new List<UserChat>();
      var groupMembers = new List<GroupMember>();

      foreach (var member in members)
      {
        userChats.Add(new UserChat
        {
          UserId = member.Id,
          ChatId = chat.Id
        });

        groupMembers.Add(new GroupMember
        {
          UserId = member.Id,
          GroupId = group.Id
        });
      }

      userChats.Add(new UserChat
      {
        UserId = admin.Id,
        ChatId = chat.Id
      });

      groupMembers.Add(new GroupMember
      {
        UserId = admin.Id,
        GroupId = group.Id
      });

      chat.UserChats.AddRange(userChats);
      group.GroupMembers.AddRange(groupMembers);

      await _chatRepository.SaveChangesAsync(cancellationToken);
      await _groupRepository.SaveChangesAsync(cancellationToken);

      var response = new GroupChatResponseDto
      {
        Id = group.Id,
        ChatId = chat.Id,
        GroupName = group.Name,
        Members = members.Select(m => new UserResponseDto
        {
          Id = m.Id,
          Name = m.Name,
          LastName = m.LastName,
          Email = m.Email,
          PhoneNumber = m.PhoneNumber
        }).ToList()
      };

      return Ok(Result<GroupChatResponseDto>.Success(response));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<GroupChatResponseDto>.Error(new string[] { ex.Message }));
    }
  }
}
