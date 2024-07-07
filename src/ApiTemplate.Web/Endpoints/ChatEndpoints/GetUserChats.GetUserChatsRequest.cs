using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

public class GetUserChatsRequest
{
  [FromRoute(Name = "userId")]
  public Guid UserId { get; set; }
}
