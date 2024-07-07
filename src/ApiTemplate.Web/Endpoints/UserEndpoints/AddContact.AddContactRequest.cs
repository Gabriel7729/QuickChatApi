namespace ApiTemplate.Web.Endpoints.UserEndpoints;

public class AddContactRequest
{
  public Guid UserId { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
}
