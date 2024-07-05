namespace ApiTemplate.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(List<string> toList, string subject, string body, List<string>? attachments = null);
}
