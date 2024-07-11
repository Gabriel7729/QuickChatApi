using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetUserByPhoneNumberOrEmailSpec : Specification<User>, ISingleResultSpecification
{
  public GetUserByPhoneNumberOrEmailSpec(string? phoneNumber = null, string? email = null)
  {
    if (phoneNumber != null && email == null)
    {
      Query
        .Where(u => u.PhoneNumber == phoneNumber);
    }
    else if (phoneNumber == null && email != null)
    {
      Query
        .Where(u => u.Email == email);
    }
  }
}
