using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.SharedKernel;
using ApiTemplate.SharedKernel.Interfaces;

namespace ApiTemplate.Core.Entities.ChatAggregate;
public class Group : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = string.Empty;
  public Guid AdminId { get; set; }
  public User? Admin { get; set; }
  public ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
  public Guid ChatId { get; set; }
  public Chat? Chat { get; set; }
}

public class GroupMember : EntityBase
{
  public Guid GroupId { get; set; }
  public Group? Group { get; set; }
  public Guid UserId { get; set; }
  public User? User { get; set; }
}
