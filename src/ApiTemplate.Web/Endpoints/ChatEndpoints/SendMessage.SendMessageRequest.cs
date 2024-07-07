namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

public class SendMessageRequest
{
  public Guid ChatId { get; set; }
  public Guid SenderId { get; set; }
  public string Content { get; set; } = string.Empty;
}
