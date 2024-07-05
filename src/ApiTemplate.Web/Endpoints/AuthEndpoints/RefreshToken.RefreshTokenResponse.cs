namespace ApiTemplate.Web.Endpoints.AuthEndpoints;

public class RefreshTokenResponse
{
  public RefreshTokenResponse(string token)
  {
    Token = token;
  }
  public string Token { get; set; } = string.Empty;
}
