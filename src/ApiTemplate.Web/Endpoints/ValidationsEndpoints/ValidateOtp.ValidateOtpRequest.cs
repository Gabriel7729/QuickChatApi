namespace ApiTemplate.Web.Endpoints.ValidationsEndpoints;

public class ValidateOtpRequest
{
  public string SentTo { get; set; } = string.Empty;
  public string Code { get; set; } = string.Empty;
}
