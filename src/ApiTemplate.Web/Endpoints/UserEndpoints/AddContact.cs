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
public class AddContact : EndpointBaseAsync
  .WithRequest<AddContactRequest>
  .WithActionResult<Result<UserResponseDto>>
{
  private readonly IRepository<User> _userRepository;

  public AddContact(IRepository<User> userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpPost("/api/User/AddContact")]
  [SwaggerOperation(
          Summary = "Add a contact to a user",
          Description = "Add a contact to a user",
          OperationId = "User.AddContact",
          Tags = new[] { "UserEndpoints" })
  ]
  public override async Task<ActionResult<Result<UserResponseDto>>> HandleAsync([FromBody] AddContactRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      GetUserWithContactsSpec getUserWithContactsSpec = new GetUserWithContactsSpec(request.UserId);
      User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

      GetUserByPhoneNumberOrEmailSpec getUserByPhoneNumberOrEmailSpec = new GetUserByPhoneNumberOrEmailSpec(request.PhoneNumber, request.Email);
      User? contact = await _userRepository.GetBySpecAsync(getUserByPhoneNumberOrEmailSpec, cancellationToken);

      if (user == null || contact == null)
        return NotFound(Result<UserResponseDto>.Error(new string[] { "User or contact not found" }));

      if (user.Contacts.Any(c => c.Id == contact.Id))
        return BadRequest(Result<UserResponseDto>.Error(new string[] { "Contact already added" }));

      user.Contacts.Add(contact);

      await _userRepository.UpdateAsync(user, cancellationToken);

      var userResponse = new UserResponseDto
      {
        Id = contact.Id,
        Name = contact.Name,
        LastName = contact.LastName
      };

      var result = Result<UserResponseDto>.Success(userResponse);

      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(Result<UserResponseDto>.Error(new string[] { ex.Message }));
    }
  }
}
