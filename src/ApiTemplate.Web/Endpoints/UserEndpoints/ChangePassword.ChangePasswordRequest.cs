using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Web.Endpoints.UserEndpoints;

public class ChangePasswordRequest
{
  [FromRoute(Name = "userId")] 
  public Guid UserId { get; set; }
  
  [FromBody]
  public ChangePasswordModel Body { get; set; } = new();
}

public class ChangePasswordModel
{
  public string OldPassword { get; set; } = string.Empty;
  public string NewPassword { get; set; } = string.Empty;
  public string ConfirmNewPassword { get; set; } = string.Empty;
}
