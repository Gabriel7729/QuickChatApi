using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiTemplate.SharedKernel.Interfaces;
using ApiTemplate.SharedKernel;
using ApiTemplate.Core.Entities.UserAggregate;

namespace ApiTemplate.Core.Entities.ChatAggregate;
public class Chat : EntityBase, IAggregateRoot
{
  public ICollection<UserChat> UserChats { get; set; } = new List<UserChat>();
  public ICollection<Message> Messages { get; set; } = new List<Message>();
  public bool IsGroupChat { get; set; }
  public Group? Group { get; set; }
}

public class UserChat : EntityBase
{
  public Guid UserId { get; set; }
  public User? User { get; set; }
  public Guid ChatId { get; set; }
  public Chat? Chat { get; set; }
}
