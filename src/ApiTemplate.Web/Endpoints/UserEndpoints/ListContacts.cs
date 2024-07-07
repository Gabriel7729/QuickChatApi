using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Entities.UserAggregate.Specifications;
using ApiTemplate.Infrastructure.Dto.UserDtos;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.UserEndpoints;

[Authorize]
public class ListContacts : EndpointBaseAsync
  .WithRequest<ListContactsRequest>
  .WithActionResult<List<UserResponseDto>>
{
  private readonly IRepository<User> _userRepository;

  public ListContacts(IRepository<User> userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpGet("/api/User/{userId:guid}/Contacts")]
  [SwaggerOperation(
          Summary = "List the contacts of a user with optional filters",
          Description = "List the contacts of a user with optional filters by email or phone number",
          OperationId = "User.ListContacts",
          Tags = new[] { "UserEndpoints" })
  ]
  public override async Task<ActionResult<List<UserResponseDto>>> HandleAsync([FromRoute] ListContactsRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      var spec = new GetUserWithContactsSpec(request.UserId, request.Email, request.PhoneNumber);
      User? user = await _userRepository.GetBySpecAsync(spec, cancellationToken);
      if (user == null)
        return NotFound(Result<List<UserResponseDto>>.Error(new string[] { "User not found" }));

      var contacts = user.Contacts.Select(c => new UserResponseDto
      {
        Id = c.Id,
        Name = c.Name,
        LastName = c.LastName,
        Email = c.Email,
        PhoneNumber = c.PhoneNumber
      }).ToList();

      return Ok(Result<List<UserResponseDto>>.Success(contacts));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<List<UserResponseDto>>.Error(new string[] { ex.Message }));
    }
  }
}
