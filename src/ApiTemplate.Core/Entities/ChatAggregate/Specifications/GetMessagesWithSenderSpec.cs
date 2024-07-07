using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.ChatAggregate.Specifications;
public class GetMessagesWithSenderSpec : Specification<Chat>, ISingleResultSpecification
{
  public GetMessagesWithSenderSpec(Guid chatId, string? contentFilter)
  {
    Query
        .Where(m => m.Id == chatId)
        .Include(m => m.Messages)
          .ThenInclude(m => m.Sender);

    if (!string.IsNullOrEmpty(contentFilter))
    {
      Query.Where(chat => chat.Messages.Any(m => m.Content.Contains(contentFilter.ToLower())));
    }
  }
}
