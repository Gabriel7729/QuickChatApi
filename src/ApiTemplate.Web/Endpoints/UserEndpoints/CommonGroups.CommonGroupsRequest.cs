using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Web.Endpoints.UserEndpoints;

public class CommonGroupsRequest
{
  [FromRoute(Name = "userId")]
  public Guid UserId { get; set; }

  [FromRoute(Name = "contactId")]
  public Guid ContactId { get; set; }
}

