using System.Text;
using ApiTemplate.Core.Constants;
using ApiTemplate.Core.Extensions;
using ApiTemplate.Core.Interfaces;
using ApiTemplate.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Core.Services;
public class ChatService : IChatService
{
  private readonly IPdfService _pdfService;
  public ChatService(IPdfService pdfService)
  {
    _pdfService = pdfService;
  }

  public FileStreamResult GenerateChatMessagesReport(List<MessageResponseDto> messages)
  {
    string template = FileManagment.ReadEmailTemplate(RouteConstans.PdfTemplateRoute, TemplateConstants.ChatMessagesTemplate);

    StringBuilder messagesHtml = new StringBuilder();
    foreach (var message in messages)
    {
      messagesHtml.AppendLine($@"
            <div class='message'>
                <div class='sender'>{message.SenderName}</div>
                <div class='timestamp'>{message.Timestamp:yyyy-MM-dd HH:mm}</div>
                <div class='content'>{message.Content}</div>
            </div>");
    }

    string htmlContent = template.Replace("{{ ChatMessages }}", messagesHtml.ToString());

    FileStreamResult pdfFile = _pdfService.ConvertHtmlToPdf(htmlContent, "ChatMessagesReport.pdf");
    return pdfFile;
  }
}
