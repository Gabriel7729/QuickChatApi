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
public class GetUserByPhoneNumberOrEmail : EndpointBaseAsync
  .WithRequest<GetUserByPhoneNumberOrEmailRequest>
  .WithActionResult<Result<UserResponseDto>>
{
  private readonly IRepository<User> _userRepository;

  public GetUserByPhoneNumberOrEmail(IRepository<User> userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpGet("/api/User/{userId:guid}/Exists")]
  [SwaggerOperation(
          Summary = "Get If user exists",
          Description = "Get If user exists",
          OperationId = "User.GetIfAlreadyExists",
          Tags = new[] { "UserEndpoints" })
  ]
  public override async Task<ActionResult<Result<UserResponseDto>>> HandleAsync([FromRoute] GetUserByPhoneNumberOrEmailRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

      GetUserByPhoneNumberOrEmailSpec getUserByPhoneNumberOrEmailSpec = new GetUserByPhoneNumberOrEmailSpec(request.PhoneNumber, request.Email);
      User? contact = await _userRepository.GetBySpecAsync(getUserByPhoneNumberOrEmailSpec, cancellationToken);

      if (user == null || contact == null)
        return NotFound(Result<UserResponseDto>.Error(new string[] { "User or contact not found" }));

      if (user.Contacts.Any(c => c.Id == contact.Id))
        return BadRequest(Result<UserResponseDto>.Error(new string[] { "Contact already added" }));

      var userResponse = new UserResponseDto
      {
        Id = contact.Id,
        Name = contact.Name,
        LastName = contact.LastName,
        Email = contact.Email,
        PhoneNumber = contact.PhoneNumber
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
