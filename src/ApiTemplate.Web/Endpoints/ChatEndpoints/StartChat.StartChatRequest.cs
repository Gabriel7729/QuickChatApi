namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

public class StartChatRequest
{
  public Guid UserId { get; set; }
  public Guid ContactId { get; set; }
}
