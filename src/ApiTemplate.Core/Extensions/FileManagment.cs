using System.Globalization;

namespace ApiTemplate.Core.Extensions;
public static class FileManagment
{
  public static string ReadEmailTemplate(string templpateRouteEmail, string fileName, string? templateRoute = null)
  {
    templateRoute ??= templpateRouteEmail;
    string textEmailTemplate = Path.Combine(templateRoute, fileName);
    string route = Path.Combine(templateRoute, CultureInfo.CurrentUICulture.Name);

    if (!Directory.Exists(route))
      return File.ReadAllText(textEmailTemplate);

    var fileRoutePlainText = Path.Combine(route, fileName);
    if (File.Exists(fileRoutePlainText))
      textEmailTemplate = fileRoutePlainText;

    return File.ReadAllText(textEmailTemplate);
  }
}
