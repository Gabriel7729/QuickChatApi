using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.ChatAggregate.Specifications;
public class GetGroupChatSpec : Specification<Chat>, ISingleResultSpecification
{
  public GetGroupChatSpec(Guid chatId)
  {
    Query
      .Where(chat => chat.Id == chatId && chat.IsGroupChat)
      .Include(chat => chat.Group)
      .ThenInclude(group => group.GroupMembers)
      .ThenInclude(gm => gm.User);
  }
}
