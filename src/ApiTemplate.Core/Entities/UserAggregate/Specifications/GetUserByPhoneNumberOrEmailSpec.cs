using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetUserByPhoneNumberOrEmailSpec : Specification<User>, ISingleResultSpecification
{
  public GetUserByPhoneNumberOrEmailSpec(string? phoneNumber = null, string? email = null)
  {
    Query
      .Where(u => phoneNumber == null || u.PhoneNumber == phoneNumber )
      .Where(u => email == null || u.Email == email);
  }
}
