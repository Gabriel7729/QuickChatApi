using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

public class GetChatMessagesRequest
{
  [FromRoute(Name = "chatId")]
  public Guid ChatId { get; set; }

  [FromQuery(Name = "contentFilter")]
  public string? ContentFilter { get; set; }
}
