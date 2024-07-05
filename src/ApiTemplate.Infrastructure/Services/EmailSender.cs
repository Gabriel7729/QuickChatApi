using System.Text;
using ApiTemplate.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiTemplate.Infrastructure.Services;
public class EmailSender : IEmailSender
{
  private readonly ILogger<EmailSender> _logger;
  private readonly IConfiguration _config;
  private readonly IHttpClientFactory _httpClientFactory;

  public EmailSender(
      ILogger<EmailSender> logger,
      IConfiguration config,
      IHttpClientFactory httpClientFactory)
  {
    _logger = logger;
    _config = config;
    _httpClientFactory = httpClientFactory;
  }

  public async Task SendEmailAsync(List<string> toList, string subject, string body, List<string>? attachments = null)
  {
    var client = _httpClientFactory.CreateClient();
    client.BaseAddress = new Uri(_config["EMAIL_API_BASE_URL"]);
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config["EMAIL_API_TOKEN"]);

    var emailData = new
    {
      from = _config["EMAIL_FROM"],
      to = toList,
      subject,
      html = body,
      attachments = attachments?.Select(filePath => new
      {
        filename = Path.GetFileName(filePath),
        content = Convert.ToBase64String(File.ReadAllBytes(filePath))
      }).ToList()
    };

    var json = JsonConvert.SerializeObject(emailData);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync("emails", content);
    if (response.IsSuccessStatusCode)
    {
      _logger.LogInformation($"Email sent to {string.Join(", ", toList)} with subject {subject}.");
    }
    else
    {
      _logger.LogError($"Failed to send email. Status: {response.StatusCode}");
    }
  }
}
