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
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

[Authorize]
public class UpdateGroupChat : EndpointBaseAsync
  .WithRequest<UpdateGroupChatRequest>
  .WithActionResult<GroupChatResponseDto>
{
  private readonly IRepository<User> _userRepository;
  private readonly IRepository<Chat> _chatRepository;
  private readonly IRepository<Group> _groupRepository;

  public UpdateGroupChat(
    IRepository<User> userRepository,
    IRepository<Chat> chatRepository,
    IRepository<Group> groupRepository)
  {
    _userRepository = userRepository;
    _chatRepository = chatRepository;
    _groupRepository = groupRepository;
  }

  [HttpPut("/api/Chat/Group")]
  [SwaggerOperation(
          Summary = "Update a group chat",
          Description = "Update the group name and manage members",
          OperationId = "Chat.UpdateGroup",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<GroupChatResponseDto>> HandleAsync([FromBody] UpdateGroupChatRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
      if (group == null)
        return NotFound(Result<GroupChatResponseDto>.Error(new string[] { "Group not found" }));

      if (!string.IsNullOrEmpty(request.NewGroupName))
      {
        group.Name = request.NewGroupName;
      }

      var chat = await _chatRepository.GetByIdAsync(group.ChatId, cancellationToken);
      if (chat == null)
        return NotFound(Result<GroupChatResponseDto>.Error(new string[] { "Chat not found" }));

      if (request.MembersToAdd != null && request.MembersToAdd.Any())
      {
        var membersToAdd = await _userRepository.ListAsync(new GetUserByIdsSpec(request.MembersToAdd), cancellationToken);
        if (membersToAdd.Count != request.MembersToAdd.Count)
          return BadRequest(Result<GroupChatResponseDto>.Error(new string[] { "Some members to add not found" }));

        foreach (var member in membersToAdd)
        {
          if (!chat.UserChats.Any(uc => uc.UserId == member.Id))
          {
            chat.UserChats.Add(new UserChat { UserId = member.Id, ChatId = chat.Id });
            group.GroupMembers.Add(new GroupMember { UserId = member.Id, GroupId = group.Id });
          }
        }
      }

      if (request.MembersToRemove != null && request.MembersToRemove.Any())
      {
        var membersToRemove = await _userRepository.ListAsync(new GetUserByIdsSpec(request.MembersToRemove), cancellationToken);
        if (membersToRemove.Count != request.MembersToRemove.Count)
          return BadRequest(Result<GroupChatResponseDto>.Error(new string[] { "Some members to remove not found" }));

        foreach (var member in membersToRemove)
        {
          var userChat = chat.UserChats.FirstOrDefault(uc => uc.UserId == member.Id);
          if (userChat != null)
          {
            chat.UserChats.Remove(userChat);
          }

          var groupMember = group.GroupMembers.FirstOrDefault(gm => gm.UserId == member.Id);
          if (groupMember != null)
          {
            group.GroupMembers.Remove(groupMember);
          }
        }
      }

      await _chatRepository.SaveChangesAsync(cancellationToken);
      await _groupRepository.SaveChangesAsync(cancellationToken);

      var response = new GroupChatResponseDto
      {
        Id = group.Id,
        ChatId = chat.Id,
        GroupName = group.Name,
        Members = group.GroupMembers.Select(gm => new UserResponseDto
        {
          Id = gm.User.Id,
          Name = gm.User.Name,
          LastName = gm.User.LastName,
          Email = gm.User.Email,
          PhoneNumber = gm.User.PhoneNumber
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
