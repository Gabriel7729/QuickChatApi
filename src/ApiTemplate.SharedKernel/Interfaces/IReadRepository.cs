using Ardalis.Specification;

namespace ApiTemplate.SharedKernel.Interfaces;

  public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
  {
  }
