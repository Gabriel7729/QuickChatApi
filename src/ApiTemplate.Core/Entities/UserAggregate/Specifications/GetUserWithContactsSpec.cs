using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetUserWithContactsSpec : Specification<User>, ISingleResultSpecification
{
  public GetUserWithContactsSpec(Guid userId, string? email = null, string? phoneNumber = null)
  {
    Query
      .Where(u => u.Id == userId)
      .Include(u => u.Contacts);

    if (!string.IsNullOrEmpty(email))
      Query.Where(u => u.Contacts.Any(c => c.Email.Contains(email)));

    if (!string.IsNullOrEmpty(phoneNumber))
      Query.Where(u => u.Contacts.Any(c => c.PhoneNumber.Contains(phoneNumber)));
  }
}
