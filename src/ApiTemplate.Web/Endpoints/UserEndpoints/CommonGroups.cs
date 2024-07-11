using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Entities.UserAggregate.Specifications;
using ApiTemplate.Infrastructure.Dto.GroupDtos;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.UserEndpoints;

[Authorize]
public class GetCommonGroups : EndpointBaseAsync
  .WithRequest<CommonGroupsRequest>
  .WithActionResult<List<GroupResponseDto>>
{
  private readonly IRepository<User> _userRepository;

  public GetCommonGroups(IRepository<User> userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpGet("/api/Chat/User/{userId:guid}/Contact/{contactId:guid}/CommonGroups")]
  [SwaggerOperation(
          Summary = "Get common groups between two users",
          Description = "Get the groups that are common between two users",
          OperationId = "Chat.GetCommonGroups",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<List<GroupResponseDto>>> HandleAsync([FromQuery] CommonGroupsRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      var userSpec = new GetUserWithGroupsInCommonWithContactSpec(request.UserId);
      var contactSpec = new GetUserWithGroupsInCommonWithContactSpec(request.ContactId);

      var user = await _userRepository.GetBySpecAsync(userSpec, cancellationToken);
      var contact = await _userRepository.GetBySpecAsync(contactSpec, cancellationToken);

      if (user == null || contact == null)
      {
        return NotFound(Result<List<GroupResponseDto>>.Error(new string[] { "User or contact not found" }));
      }

      var userGroupIds = user.GroupMembers.Select(gm => gm.GroupId).ToList();
      var contactGroupIds = contact.GroupMembers.Select(gm => gm.GroupId).ToList();

      var commonGroupIds = userGroupIds.Intersect(contactGroupIds).ToList();
      var commonGroups = user.GroupMembers
        .Where(gm => commonGroupIds.Contains(gm.GroupId))
        .Select(gm => new GroupResponseDto
        {
          Id = gm.Group.Id,
          Name = gm.Group.Name,
          TotalMembers = gm.Group.GroupMembers.Count
        }).ToList();

      return Ok(Result<List<GroupResponseDto>>.Success(commonGroups));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<List<GroupResponseDto>>.Error(new string[] { ex.Message }));
    }
  }
}
