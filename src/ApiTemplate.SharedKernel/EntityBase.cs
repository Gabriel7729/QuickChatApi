using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTemplate.SharedKernel;

  // This can be modified to EntityBase<TId> to support multiple key types (e.g. Guid)
  public abstract class EntityBase
  {
      public Guid Id { get; set; }
      public bool Deleted { get; set; }
      public DateTimeOffset? DeletedDate { get; set; }
      public DateTimeOffset CreatedDate { get; set; }
      public DateTimeOffset? UpdatedDate { get; set; }
      public string? CreatedBy { get; set; }
      public string? DeletedBy { get; set; }
      public string? UpdatedBy { get; set; }

      private List<DomainEventBase> _domainEvents = new();
      [NotMapped]
      public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();
      protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
      internal void ClearDomainEvents() => _domainEvents.Clear();
  }
