using Newtonsoft.Json;
using System.Text;

namespace Application.Common.Extensions;

public static class GeneralExtensions
{
    public static string HtmlToString(string html)
    {
        var data = html; // File.ReadAllText(html, Encoding.Default);

        var htmlTemplate = data.Replace("\r\n", string.Empty);
        htmlTemplate = htmlTemplate.Replace(@"\""", String.Empty);
        htmlTemplate = htmlTemplate.Replace("\"", String.Empty);
        htmlTemplate = htmlTemplate.Replace("\t", string.Empty);
        return htmlTemplate;
    }
}

public class TemplateDefinitionRoot
{
    public string name { get; set; }

    [JsonProperty("master-template")]
    public string MasterTemplate { get; set; }
    public string template { get; set; }
    [JsonProperty("semantic-version")]
    public string SemanticVersion { get; set; }
}