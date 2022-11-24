﻿using Application.Common.Interfaces;
using Application.Common.Models;
using RestSharp;
using System.Text;

namespace Infrastructure.Services
{
    public class TemplateEngineService : ITemplateEngineService
    {
        private readonly string _templateEngineUrl;
        public TemplateEngineService()
        {
            _templateEngineUrl = StaticValues.TemplateEngine;
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

            var data = HtmlReplace(response.Content);
            return Response<string>.Success(data, 200);
        }

        public async Task<Response<string>> PdfRender(string templateName, string content)
        {
            var responseContent = "";

            var jsonData = GetJsonData(templateName, content);
            var restClient = new RestClient(_templateEngineUrl);
            var restRequest = new RestRequest("/Template/Render/Pdf", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddStringBody(jsonData, DataFormat.Json);
            var response = await restClient.ExecutePostAsync(restRequest);
            if (response.IsSuccessful)
            {
                var data = PDFReplace(response.Content);
                responseContent = "data:application/pdf;base64," + data;
            }
            return Response<string>.Success(responseContent, 200);
        }
      
        private string HtmlReplace(string content)
        {
            if (string.IsNullOrEmpty(content))
                return "";
            var data = content.Replace(@"\""", String.Empty);
            data = data.Replace("\"", String.Empty);
            data = data.Replace(@"\r\n", String.Empty);
            data = data.Replace(@"\t", String.Empty);
            data = data.Replace("True</p>", "</p>");
            data = data.Replace("true", "X");
            data = data.Replace("True", "X");
            data = data.Replace("false", String.Empty);
            data = data.Replace("False", String.Empty);
            return data;
        }


        private string PDFReplace(string content)
        {
            if (string.IsNullOrEmpty(content))
                return "";
            var data = content.Replace(@"\""", String.Empty);
            data = data.Replace("\"", String.Empty);
            return data;
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

        private string HtmlEncode()
        {

            var templateName = "sigorta_onay_formu.txt";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateName);
            var data = File.ReadAllText(path, Encoding.GetEncoding("Windows-1254"));

            var jsonString = data.Replace("\r\n", string.Empty);

            jsonString = jsonString.Replace(@"\""", String.Empty);
            jsonString = jsonString.Replace("\"", String.Empty);
            jsonString = jsonString.Replace("\t", string.Empty);
            return jsonString;

        }

      
    }
}
