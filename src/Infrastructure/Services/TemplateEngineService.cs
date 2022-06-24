using Application.Common.Interfaces;
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
            var jsonData = GetJsonData(templateName, content);
            var restClient = new RestClient(_templateEngineUrl);
            var restRequest = new RestRequest("/Template/Render/Pdf", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddStringBody(jsonData, DataFormat.Json);
            var response = await restClient.ExecutePostAsync(restRequest);

            var data = PDFReplace(response.Content);
            var responseContent = "data:application/pdf;base64," + data;
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
            var data = File.ReadAllText(path, Encoding.Default);

            var jsonString = data.Replace("\r\n", string.Empty);

            jsonString = jsonString.Replace(@"\""", String.Empty);
            jsonString = jsonString.Replace("\"", String.Empty);
            jsonString = jsonString.Replace("\t", string.Empty);
            return jsonString;

        }

        //{"approver":{"citizenshipNumber":"55294243060","first":"RAMAZAN","last":"KOÇ","customerNumber":20184378},"content":"{\"FormDefinition_Name\":\"                              SIGORTA BASVURU FORMU\",\"FormInstance_Transaction_Id\":\"999\",\"sigortaEttirenIleSigortaliAyniKisidir\":true,\"FormInstance_Approver_Fullname\":\"RAMAZAN KOÇ\",\"FormInstance_Approver_CitizenshipNumber\":\"55294243060\",\"dainiMurtehin\":\"BURGANBANK A.Ş.\",\"sigortaTuru\":{\"esya\":false,\"konut\":false,\"dask\":false,\"kasko\":false,\"trafik\":false,\"isyeri\":false,\"diger\":true},\"textArea\":\"Kredi teminatı olan sigortalarda, sigorta bedeli, sigortanın konusunun piyasa değeri ve değerleme raporundaki minimum sigorta bedelini karşılar. Poliçede dain-i \\nmürtehin Burgan Bank A.Ş. olur. Sigorta ettiren, sigorta şirketini seçmekte serbesttir.\",\"textArea1\":\"Yukarıda beyan etmiş olduğum bilgiler çerçevesinde sigorta talebimin alınmasını ve tarafıma sigorta teklifinin yapılmasını \\ntalep ederim.\",\"textArea2\":\"Sigorta poliçesi talebiniz kapsamında paylaştığınız kişisel verilerinize ilişkin 6698 sayılı Kişisel Verilerin Korunması Kanunu hakkındaki detaylı bilgilendirmeye \\nwww.burgan.com.tr adresinden ulaşabilirsiniz.\",\"diger\":\"\",\"SigortaEttirenAd\":\"RAMAZAN KOÇ\",\"SigortaEttirenVKN\":\"55294243060\"}","fileType":"","formId":"fd95116e-e7e0-4cdf-b734-11c414c3a471","reference":{"processNo":"999","tagId":["1595c481-f41b-46e5-a747-d35ba7e1b651"],"formId":"fd95116e-e7e0-4cdf-b734-11c414c3a471"},"title":"Sigorta Başvuru Formu","insuranceType":"diger","source":"formio","dependencyOrderId":""}
    }
}