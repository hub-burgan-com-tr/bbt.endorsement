using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;
using Application.Common.Interfaces;
using System.Xml;
using Application.Common.Models;

namespace Infrastructure.TsizlLFora
{
    public class TsizlFora
    {
        private readonly string _url;
        static XmlDocument doc;

        static ResponseTsizl responseTsizl;

        public TsizlFora()
        {
            //_url = config.GetTsizlUrl();
            _url = StaticValues.TsizlUrl;

            responseTsizl= new ResponseTsizl();
        }

        public async Task<Response<ResponseTsizl>> DoAutomaticEngagementPlain(string accountBranchCode, string accountNumber)
        {
            var restClient = new RestClient(_url);
            var restRequest = new RestRequest("?op=DoAutomaticEngagementPlain", Method.Post);
            restRequest.AddHeader("Content-Type", "application/soap+xml");

            string engagementDate = DateTime.Now.ToString("yyyy-MM-dd");

            #region body
            var body = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                " + "\n" +
                @"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://         www.w3.org/2003/05/ soap-envelope"">
                " + "\n" +
                @"  <soap12:Body>
                " + "\n" +
                @"    <DoAutomaticEngagementPlain xmlns=""http://core.intertech.com.tr/"">
                " + "\n" +
                @"      <accountBranchCode>{0}</accountBranchCode>
                " + "\n" +
                @"      <accountNumber>{1}</accountNumber>
                " + "\n" +
                @"      <accountSuffix>0</accountSuffix>
                " + "\n" +
                @"      <currencyCode>TRY</currencyCode>
                " + "\n" +
                @"      <engagementDate>{2}</engagementDate>
                " + "\n" +
                @"      <engagementType>G</engagementType>
                " + "\n" +
                @"      <engagementKind>S9</engagementKind>
                " + "\n" +
                @"      <engagementAmount>0.01</engagementAmount>
                " + "\n" +
                @"      <userCode>EBT\MOBILONAY</userCode>
                " + "\n" +
                @"    </DoAutomaticEngagementPlain>
                " + "\n" +
                @"  </soap12:Body>
                " + "\n" +
                @"</soap12:Envelope>", accountBranchCode, accountNumber, engagementDate);
            #endregion

            restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);
            var restResponse = await restClient.ExecutePostAsync(restRequest);
            TsizlResponse(restResponse.Content);
            
            if (!restResponse.IsSuccessful)
            { 
                responseTsizl.HasError = true; 
                responseTsizl.ErrorMessage = "TSIZL(DoAutomaticEngagementPlain) servisine ulaşılamadı.";

                return Response<ResponseTsizl>.Success(responseTsizl, 417);
            }

            return Response<ResponseTsizl>.Success(responseTsizl, 200);
        }

        private void TsizlResponse(string content)
        {
            doc.LoadXml(content);

            responseTsizl.ReferenceNumber = DocSelectField("ReferenceNumber");
            responseTsizl.HasError = Convert.ToBoolean(DocSelectField("HasError"));
            responseTsizl.ErrorMessage = DocSelectField("ErrorMessage");
            
            //return DocSelectField("ReferenceNumber");
        }

        private string DocSelectField(string field)
        {
            var xpath = string.Format("//*/{0}//text()", field);
            return doc.SelectSingleNode(xpath)?.InnerText?.ToString() ?? "";
        }
    }

    public class ResponseTsizl
    {
        public ResponseTsizl() { }

        public string ReferenceNumber { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
