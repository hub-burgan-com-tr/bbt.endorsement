using Infrastructure.Configuration.Options;
using System.Text;

namespace Migration.Console.App;

public static class StaticValuesExtensions
{
    public static void SetStaticValues(AppSettings settings)
    {
        StaticValues.TemplateEngine = settings.Entegration.TemplateEngine;
    }

    public static string HtmlToString(string templateName)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateName);
        var data = File.ReadAllText(path, Encoding.Default);

        var htmlTemplate = data.Replace("\r\n", string.Empty);
        htmlTemplate = htmlTemplate.Replace(@"\""", String.Empty);
        htmlTemplate = htmlTemplate.Replace("\"", String.Empty);
        htmlTemplate = htmlTemplate.Replace("\t", string.Empty);
        return htmlTemplate;
    }
}

