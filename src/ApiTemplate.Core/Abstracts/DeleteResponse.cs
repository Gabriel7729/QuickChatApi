namespace ApiTemplate.Core.Abstracts;
public class DeleteResponse
{
  public DeleteResponse(bool isDeleted = true, string message = "")
  {
    IsDeleted = isDeleted;
    Message = message;
  }
  public bool IsDeleted { get; set; }
  public string Message { get; set; } = string.Empty;
}
