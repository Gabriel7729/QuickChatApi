namespace ApiTemplate.Core.Models;
public class TokenClaim
{
  public Guid UserId { get; set; }
  public string Email { get; set; } = string.Empty;
  public bool IsEmailValidated { get; set; }
  public string Token { get; set; } = string.Empty;
}
