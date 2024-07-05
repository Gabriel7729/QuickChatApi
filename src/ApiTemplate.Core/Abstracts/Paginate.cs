using ApiTemplate.SharedKernel;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.Specification;

namespace ApiTemplate.Core.Abstracts;
public class Paginate<Entity, EntityResult> where Entity : EntityBase, IAggregateRoot
{
  private readonly IRepository<Entity> _repository;
  public Paginate(IRepository<Entity> repository)
  {
    _repository = repository;
  }
  public async Task<PaginationResult<EntityResult>> GetResponse(int pageNumber, int pageSize, EntityResult property, ISpecification<Entity>? specification = null)
  {
    var total = specification is not null ? await _repository.CountAsync(specification) : await _repository.CountAsync();
    var totalPages = (int)Math.Ceiling(total / (double)pageSize);
    var totalRecords = total;
    var response = new PaginationResult<EntityResult>(pageNumber, property, pageSize, totalPages, totalRecords);
    return response;
  }
}
