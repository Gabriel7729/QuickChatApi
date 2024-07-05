using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.SharedKernel;
using ApiTemplate.SharedKernel.Interfaces;

namespace ApiTemplate.Core.Entities.ChatAggregate;
public class Message : EntityBase, IAggregateRoot
{
  public string Content { get; set; } = string.Empty;
  public DateTime Timestamp { get; set; }
  public Guid SenderId { get; set; }
  public User? Sender { get; set; }
  public Guid ChatId { get; set; }
  public Chat? Chat { get; set; }
}
