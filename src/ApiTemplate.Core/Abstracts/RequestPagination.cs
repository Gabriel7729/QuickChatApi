namespace ApiTemplate.Core.Abstracts;
public class RequestPagination
{
  //
  // Summary:
  //Número de página que se desea consultar.
  public int PageNumber { get; set; } = 1;


  //
  // Summary:
  //Se utiliza para indicar la cantidad de registros a ser retornados por página.
  public int PageSize { get; set; } = 10;
}
