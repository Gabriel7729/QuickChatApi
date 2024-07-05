using ApiTemplate.Core.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Infrastructure.Services;
public class PdfService : IPdfService
{
  private readonly IConverter _converter;
  public PdfService(IConverter converter)
  {
    _converter = converter;
  }
  public FileStreamResult ConvertHtmlToPdf(string html, string pdfTitle)
  {
    var globalSettings = new GlobalSettings
    {
      ColorMode = ColorMode.Color,
      Orientation = Orientation.Portrait,
      PaperSize = PaperKind.A4,
      Margins = new MarginSettings { Top = 10 },
      DocumentTitle = pdfTitle
    };
    var objectSettings = new ObjectSettings
    {
      HtmlContent = html,
      WebSettings = { DefaultEncoding = "utf-8" },
    };

    var pdf = new HtmlToPdfDocument()
    {
      GlobalSettings = globalSettings,
      Objects = { objectSettings }
    };

    var fileBytes = _converter.Convert(pdf);
    var pdfFile = GetPdfFile(fileBytes, pdfTitle);

    return pdfFile;
  }

  private static FileStreamResult GetPdfFile(byte[] file, string pdfTitle)
  {
    var stream = new MemoryStream(file);
    stream.Seek(0, SeekOrigin.Begin);

    var fileName = string.Concat(pdfTitle, ".pdf");
    var routeToSave = Path.Combine(Path.GetTempPath(), fileName);

    using (FileStream fileStream = new FileStream(routeToSave, FileMode.Create, FileAccess.Write))
    {
      stream.WriteTo(fileStream);
    }

    stream.Seek(0, SeekOrigin.Begin);

    return new FileStreamResult(stream, "application/pdf")
    {
      FileDownloadName = fileName
    };
  }
}
