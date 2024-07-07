using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetUserByIdsSpec : Specification<User>
{
  public GetUserByIdsSpec(List<Guid> userIds)
  {
    Query.Where(u => userIds.Contains(u.Id));
  }
}
