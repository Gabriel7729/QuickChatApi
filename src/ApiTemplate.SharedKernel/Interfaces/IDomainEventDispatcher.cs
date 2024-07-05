
namespace ApiTemplate.SharedKernel.Interfaces;

public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents(IEnumerable<EntityBase> entitiesWithEvents);
  Task DispatchSingleEvent(EntityBase entityWithEvent);
}
