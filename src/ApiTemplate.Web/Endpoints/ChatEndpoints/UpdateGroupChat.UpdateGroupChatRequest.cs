namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

public class UpdateGroupChatRequest
{
  public Guid GroupId { get; set; }
  public string? NewGroupName { get; set; }
  public List<Guid>? MembersToAdd { get; set; }
  public List<Guid>? MembersToRemove { get; set; }
}

