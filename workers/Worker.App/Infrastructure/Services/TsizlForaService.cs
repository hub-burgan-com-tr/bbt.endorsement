using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Xml;
using Worker.App.Application.Common.Models;
using Worker.App.Models;
namespace Worker.App.Infrastructure.Services
{
    public interface ITsizlForaService
    {
        Task<TsizlForaServiceResponse> DoAutomaticEngagementPlain(string accountBranchCode, string accountNumber, string engagementKind);
    }
    public class TsizlForaServiceResponse
    {
        public string ReferenceNumber { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class TsizlForaService : ITsizlForaService
    {
        private readonly string _url;
        static XmlDocument doc;

        static TsizlForaServiceResponse responseTsizl;

        public TsizlForaService()
        {
            //_url = config.GetTsizlUrl();
            _url = StaticValues.TsizlUrl;

            responseTsizl = new TsizlForaServiceResponse();
        }

        public async Task<TsizlForaServiceResponse> DoAutomaticEngagementPlain(string accountBranchCode, string accountNumber, string engagementKind)
        {
            try
            {
                var restClient = new RestClient(_url);
                var restRequest = new RestRequest("?op=DoAutomaticEngagementPlain", Method.Post);
                restRequest.AddHeader("Content-Type", "text/xml; charset=utf-8");

                string engagementDate = DateTime.Now.ToString("yyyy-MM-dd");

                #region body
                var body = string.Format(
                    @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:core=""http://core.intertech.com.tr/"">
                    <soapenv:Header/>
                    <soapenv:Body>
                    <core:DoAutomaticEngagementPlain>
                    <core:accountBranchCode>{0}</core:accountBranchCode>
                    <core:accountNumber>{1}</core:accountNumber>
                    <core:accountSuffix>0</core:accountSuffix>
                    <!--Optional:-->
                    <core:currencyCode>TRY</core:currencyCode>
                    <core:engagementDate>{2}</core:engagementDate>
                    <!--Optional:-->
                    <core:engagementType>G</core:engagementType>
                    <!--Optional:-->
                    <core:engagementKind>{3}</core:engagementKind>
                    <core:engagementAmount>0.01</core:engagementAmount>
                    <!--Optional:-->
                    <core:userCode>EBT\MOBILONAY</core:userCode>
                    </core:DoAutomaticEngagementPlain>
                    </soapenv:Body>
                    </soapenv:Envelope>"
                    , accountBranchCode, accountNumber, engagementDate, engagementKind);
                #endregion

                restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);

                var restResponse = await restClient.ExecutePostAsync(restRequest);
                if (!restResponse.IsSuccessful)
                {
                    responseTsizl.HasError = true;
                    responseTsizl.ErrorMessage = "TSIZL(DoAutomaticEngagementPlain) servisine ulaşılamadı.";
                }
                TsizlResponse(restResponse.Content);
              
                return responseTsizl;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private void TsizlResponse(string content)
        {

            doc = new XmlDocument();
            doc.LoadXml(content);

            responseTsizl.ReferenceNumber = DocSelectField("ReferenceNumber");
            responseTsizl.HasError = Convert.ToBoolean(DocSelectField("HasError"));
            responseTsizl.ErrorMessage = DocSelectField("ErrorMessage");

            //return DocSelectField("ReferenceNumber");
        }

        private string DocSelectField(string field)
        {
            return doc.GetElementsByTagName(field)?.Item(0)?.InnerXml ?? "";
        }
    }


}
