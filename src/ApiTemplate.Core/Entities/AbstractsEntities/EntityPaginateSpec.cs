using ApiTemplate.SharedKernel;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.AbstractsEntities;
public class EntityPaginateSpec<T> : Specification<T> where T : EntityBase, IAggregateRoot
{
  public EntityPaginateSpec(int pageNumber, int pageSize)
  {
    Query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize);
  }
}
