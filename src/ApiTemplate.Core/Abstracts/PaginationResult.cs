namespace ApiTemplate.Core.Abstracts;
public class PaginationResult<TEntity>
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public int TotalPages { get; set; }
  public int TotalRecords { get; set; }
  public TEntity Items { get; set; }
  public PaginationResult(int pageNumber, TEntity items, int pageSize, int totalPages, int totalRecords)
  {
    PageNumber = pageNumber;
    Items = items;
    PageSize = pageSize;
    TotalPages = totalPages;
    TotalRecords = totalRecords;
  }
}
