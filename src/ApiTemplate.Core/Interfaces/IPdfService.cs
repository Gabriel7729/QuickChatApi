using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Core.Interfaces;
public interface IPdfService
{
  FileStreamResult ConvertHtmlToPdf(string html, string pdfTitle);
}
