using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.UserAggregate.Specifications;
public class GetUserChatsSpec : Specification<User>, ISingleResultSpecification
{
  public GetUserChatsSpec(Guid userId)
  {
    Query
      .Where(u => u.Id == userId)
      .Include(u => u.UserChats)
        .ThenInclude(uc => uc.Chat)
          .ThenInclude(c => c.Messages)
       .Include(u => u.UserChats)
          .ThenInclude(uc => uc.Chat)
            .ThenInclude(c => c.UserChats)
              .ThenInclude(uc => uc.User);
  }
}
