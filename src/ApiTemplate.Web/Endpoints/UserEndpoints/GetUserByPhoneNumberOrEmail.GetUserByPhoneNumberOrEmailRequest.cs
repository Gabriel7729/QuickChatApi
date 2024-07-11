using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Web.Endpoints.UserEndpoints;

public class GetUserByPhoneNumberOrEmailRequest
{
  [FromRoute(Name = "userId")]
  public Guid UserId { get; set; }

  [FromQuery(Name = "phoneNumber")]
  public string? PhoneNumber { get; set; }

  [FromQuery(Name = "email")]
  public string? Email { get; set; }
}
