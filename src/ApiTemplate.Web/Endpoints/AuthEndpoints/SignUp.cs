using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Entities.UserAggregate.Specifications;
using ApiTemplate.Core.Interfaces;
using ApiTemplate.Core.Models;
using ApiTemplate.Infrastructure.Dto.UserDtos;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.AuthEndpoints;

[AllowAnonymous]
public class SignUp : EndpointBaseAsync
  .WithRequest<UserDto>
  .WithActionResult<TokenClaim>
{
  private readonly IRepository<User> _userRepository;
  private readonly IAuthService _authService;
  private readonly IMapper _mapper;

  public SignUp(
    IRepository<User> userRepository,
    IAuthService authService,
    IMapper mapper)
  {
    _userRepository = userRepository;
    _authService = authService;
    _mapper = mapper;
  }

  [HttpPost("/api/Auth/SignUp")]
  [SwaggerOperation(
          Summary = "Sign Up a User",
          Description = "Sign Up a User",
          OperationId = "SignUp.User",
          Tags = new[] { "AuthEndpoints" })
  ]
  public override async Task<ActionResult<TokenClaim>> HandleAsync([FromBody] UserDto request, CancellationToken cancellationToken = default)
  {
    try
    {
      GetIfUserAlreadyExitsSpec spec = new(request.PhoneNumber, request.Email);
      User? userExists = await _userRepository.GetBySpecAsync(spec, cancellationToken);
      if (userExists is not null)
        return BadRequest(Result<TokenClaim>.Error("The user already exists"));

      User user = _mapper.Map<User>(request);
      user.UserAction = new UserAction
      {
        IsEmailValidated = false,
      };

      user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
      User userResponse = await _userRepository.AddAsync(user, cancellationToken);

      string token = await _authService.LoginAsync(userResponse.Email, request.Password, cancellationToken);
      TokenClaim claims = _authService.DecryptToken(token);
      return Ok(Result<TokenClaim>.Success(claims));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<TokenClaim>.Error(new string[] { "An error has occurred creating the user", ex.Message }));
    }
  }
}
