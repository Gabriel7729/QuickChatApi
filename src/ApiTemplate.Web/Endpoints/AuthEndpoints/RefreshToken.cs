using System.Security.Authentication;
using ApiTemplate.Core.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.AuthEndpoints;

[Authorize]
public class RefreshToken : EndpointBaseAsync
  .WithoutRequest
  .WithActionResult<RefreshTokenResponse>
{
  private readonly IAuthService _authService;

  public RefreshToken(
    IAuthService authService)
  {
    _authService = authService;
  }

  [HttpGet("/api/Auth/Refresh/Token")]
  [SwaggerOperation(
          Summary = "Refresh user Token",
          Description = "Refresh user Token",
          OperationId = "Refresh.Token",
          Tags = new[] { "AuthEndpoints" })
  ]
  public override async Task<ActionResult<RefreshTokenResponse>> HandleAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      string tokenToRefresh = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
      string token = await _authService.RefreshTokenAsync(tokenToRefresh, cancellationToken);
      RefreshTokenResponse response = new(token);

      return Ok(Result<RefreshTokenResponse>.Success(response));
    }
    catch (AuthenticationException ex)
    {
      return Unauthorized(Result<RefreshTokenResponse>.Error(new string[] { ex.Message }));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<RefreshTokenResponse>.Error(new string[] { ex.Message }));
    }
  }
}
