using ApiTemplate.SharedKernel.Interfaces;
using ApiTemplate.SharedKernel;

namespace ApiTemplate.Core.Entities.UserAggregate;
public class User : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string PhoneNumber { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public UserAction? UserAction { get; set; }
  public Guid UserActionId { get; set; }
}

public class UserAction : EntityBase, IAggregateRoot
{
  public bool IsEmailValidated { get; set; }
  public Guid UserId { get; set; }
}
