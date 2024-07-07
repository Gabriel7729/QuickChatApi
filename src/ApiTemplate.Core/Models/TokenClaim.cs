namespace ApiTemplate.Core.Models;
public class TokenClaim
{
  public Guid UserId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string PhoneNumber { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public bool IsEmailValidated { get; set; }
  public string Token { get; set; } = string.Empty;
}
