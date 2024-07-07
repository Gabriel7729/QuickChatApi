namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

public class CreateGroupChatRequest
{
  public Guid AdminId { get; set; }
  public string GroupName { get; set; } = string.Empty;
  public List<Guid> MemberIds { get; set; } = new List<Guid>();
}

