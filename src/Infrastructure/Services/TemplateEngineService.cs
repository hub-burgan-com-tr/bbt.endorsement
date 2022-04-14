using Application.Common.Interfaces;
using Application.Common.Models;
using RestSharp;

namespace Infrastructure.Services
{
    public class TemplateEngineService : ITemplateEngineService
    {
        private readonly string _templateEngineUrl;
        public TemplateEngineService()
        {
            _templateEngineUrl = "http://20.126.170.150:5000";
        }

        public async Task<Response<string>> HtmlRender(string templateName, string content)
        {
            var jsonData = GetJsonData(templateName, content);

            var restClient = new RestClient(_templateEngineUrl);
            var restRequest = new RestRequest("/Template/Render", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddStringBody(jsonData, DataFormat.Json);
            var response = await restClient.ExecutePostAsync(restRequest);

            var html = response.Content;

            html = html.Replace(@"\""", String.Empty);
            html = html.Replace("\"", String.Empty);

            return Response<string>.Success(html, 200);
        }

        public async Task<Response<string>> PdfRender(string templateName, string content)
        {
            var jsonData = GetJsonData(templateName, content);

            var restClient = new RestClient(_templateEngineUrl);
            var restRequest = new RestRequest("/Template/Render/Pdf", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddStringBody(jsonData, DataFormat.Json);
            var response = await restClient.ExecutePostAsync(restRequest);

            var html = response.Content;

            html = html.Replace(@"\""", String.Empty);
            html = html.Replace("\"", String.Empty);

            return Response<string>.Success(html, 200);
        }

        private string GetJsonData(string templateName, string content)
        {
            return @"{" +
                            "\"name\":" + "\"" + templateName + "\"" + "," +
                            "\"render-id\":" + "\"" + Guid.NewGuid().ToString() + "\"" + "," +
                            "\"render-data\": " + content + "," +
                            "\"render-data-for-log\":  " + content +
          "}";
        }
    }
}
