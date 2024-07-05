using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetUserByEmailSpec : Specification<User>, ISingleResultSpecification
{
  public GetUserByEmailSpec(string email)
  {
    Query
      .Where(user => user.Email == email)
      .Include(user => user.UserAction);
  }
}
