using ApiTemplate.Core.Interfaces;
using ApiTemplate.Core.Models;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.AuthEndpoints;

[AllowAnonymous]
public class Login : EndpointBaseAsync
  .WithRequest<LoginRequest>
  .WithActionResult<TokenClaim>
{
  private readonly IAuthService _authService;

  public Login(
    IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("/api/Auth/Login")]
  [SwaggerOperation(
          Summary = "Login a user",
          Description = "Login a user",
          OperationId = "Login.User",
          Tags = new[] { "AuthEndpoints" })
  ]
  public override async Task<ActionResult<TokenClaim>> HandleAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      string token = await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
      TokenClaim claims = _authService.DecryptToken(token);

      return Ok(Result<TokenClaim>.Success(claims));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<TokenClaim>.Error(new string[] { ex.Message }));
    }
  }
}

