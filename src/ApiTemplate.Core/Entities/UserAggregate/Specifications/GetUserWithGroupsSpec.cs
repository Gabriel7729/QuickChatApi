using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetUserWithGroupsSpec : Specification<User>
{
  public GetUserWithGroupsSpec(List<Guid> userIds)
  {
    Query
      .Where(user => userIds.Contains(user.Id))
      .Include(user => user.GroupMembers)
      .ThenInclude(gm => gm.Group);
  }
}
