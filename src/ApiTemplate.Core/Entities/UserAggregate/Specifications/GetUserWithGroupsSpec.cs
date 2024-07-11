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

public class GetUserWithGroupsInCommonWithContactSpec : Specification<User>, ISingleResultSpecification
{
  public GetUserWithGroupsInCommonWithContactSpec(Guid userId)
  {
    Query
        .Where(u => u.Id == userId)
        .Include(u => u.GroupMembers)
            .ThenInclude(gm => gm.Group)
              .ThenInclude(g => g.GroupMembers); ;
  }
}
