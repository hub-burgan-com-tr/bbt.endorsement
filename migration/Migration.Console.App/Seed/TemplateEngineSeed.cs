using Infrastructure.Persistence;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace Migration.Console.App.Seed;

public static class TemplateEngineSeed
{
    public static async Task TemplateDefinitionAsync(ApplicationDbContext _context)
    {
        var formDefinitions = _context.FormDefinitions.Where(x => x.Source == "formio");
        foreach (var formDefinition in formDefinitions)
        {
            var htmlTemplate = formDefinition.HtmlTemplate.Replace("\r\n", string.Empty);
            htmlTemplate = htmlTemplate.Replace(@"\""", String.Empty);
            htmlTemplate = htmlTemplate.Replace("\"", String.Empty);
            htmlTemplate = htmlTemplate.Replace("\t", string.Empty);

            var restClient = new RestClient(StaticValues.TemplateEngine);
            var restRequest = new RestRequest("/Template/Definition", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "text/plain");

            var body = new TemplateDefinitionRoot
            {
                MasterTemplate = "",
                template = htmlTemplate,
                name = formDefinition.TemplateName,
                SemanticVersion= "1.0.0"

            };
            restRequest.AddBody(body);
            var response = restClient.ExecutePostAsync(restRequest).Result;
        }
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
