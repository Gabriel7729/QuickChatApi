namespace ApiTemplate.Core.Abstracts;
public class GeneralResponse
{
  public GeneralResponse(bool isSuccess, string message, string[]? warnings = null)
  {
    IsSuccess = isSuccess;
    Message = message;
    Warnings = warnings;
  }

  public bool IsSuccess { get; set; }
  public string Message { get; set; } = string.Empty;
  public string[]? Warnings { get; set; } = Array.Empty<string>();
}
