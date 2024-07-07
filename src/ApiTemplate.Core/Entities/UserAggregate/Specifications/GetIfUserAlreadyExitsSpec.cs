using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetIfUserAlreadyExitsSpec : Specification<User>, ISingleResultSpecification
{
  public GetIfUserAlreadyExitsSpec(string phoneNumber, string email)
  {
    Query.Where(user => user.Email == email || user.PhoneNumber == phoneNumber);
  }
}
