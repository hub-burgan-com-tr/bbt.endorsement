using Application.Common.Interfaces;
using Application.Common.Models;
using Application.SSOIntegrationService.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Infrastructure.SSOIntegration
{

    public class SSOIntegrationService : ISSOIntegrationService
    {
        private readonly string _SSOIntegrationServiceUrl;
        private readonly string _SSOIntegrationPusulaServiceUrl;
        static XmlDocument doc;

        public SSOIntegrationService()
        {
            _SSOIntegrationServiceUrl = StaticValues.SSOIntegrationService;
            _SSOIntegrationPusulaServiceUrl = StaticValues.SSOIntegrationService.Replace("SSOIntegrationWebService/Service.asmx", "Pusula/Customerservices.asmx");
            doc = new XmlDocument();
        }
        #region SearchUserInfo
        public async Task<Response<string>> SearchUserInfo(string loginName, string firstName = "", string lastName = "")
        {

            var restClient = new RestClient(_SSOIntegrationServiceUrl);
            var restRequest = new RestRequest("?op=SearchUserInfo", Method.Post);
            restRequest.AddHeader("Content-Type", "application/soap+xml");

            #region body
            var body = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
" + "\n" +
@"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
" + "\n" +
@"  <soap12:Body>
" + "\n" +
@"    <SearchUserInfo xmlns=""http://tempuri.org/"">
" + "\n" +
@"      <loginName>{0}</loginName>
" + "\n" +
@"      <firstName>{1}</firstName>
" + "\n" +
@"      <lastName>{2}</lastName>
" + "\n" +
@"    </SearchUserInfo>
" + "\n" +
@"  </soap12:Body>
" + "\n" +
@"</soap12:Envelope>", loginName, firstName, lastName);
            #endregion
            restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);
            var restResponse = await restClient.ExecutePostAsync(restRequest);
            var response = SearchUserInfoResponse(restResponse.Content);

            return Response<string>.Success(response, 200);
        }
        private string SearchUserInfoResponse(string content)
        {
            doc.LoadXml(content);
            return DocSelectField("RegisterId");
        }
        #endregion



        #region GetAuthorityForUser
        public async Task<Response<List<UserAuthority>>> GetAuthorityForUser(string applicationCode, string authorityName, string loginAndDomainName)
        {

            var restClient = new RestClient(_SSOIntegrationServiceUrl);
            var restRequest = new RestRequest("?op=GetAuthorityForUser", Method.Post);
            restRequest.AddHeader("Content-Type", "application/soap+xml");

            #region body
            var body = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
" + "\n" +
@"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
" + "\n" +
@"  <soap12:Body>
" + "\n" +
@"    <GetAuthorityForUser xmlns=""http://tempuri.org/"">
" + "\n" +
@"      <applicationCode>{0}</applicationCode>
" + "\n" +
@"      <authorityName>{1}</authorityName>
" + "\n" +
@"      <loginAndDomainName>{2}</loginAndDomainName>
" + "\n" +
@"    </GetAuthorityForUser>
" + "\n" +
@"  </soap12:Body>
" + "\n" +
@"</soap12:Envelope>", applicationCode, authorityName, loginAndDomainName);
            #endregion
            restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);
            var restResponse = await restClient.ExecutePostAsync(restRequest);
            var response = GetAuthorityForUserParseResponse(restResponse.Content);

            return Response<List<UserAuthority>>.Success(response, 200);
        }
        private List<UserAuthority> GetAuthorityForUserParseResponse(string content)
        {
            doc.LoadXml(content);
            var xmlReader = new XmlNodeReader(doc.SelectSingleNode("//*/NewDataSet"));
            var dataSet = new DataSet();
            dataSet.ReadXml(xmlReader);
            var getAuthorityForUserResponse = dataSet.Tables[0].AsEnumerable().Select(x => new UserAuthority
            {
                PropertyId = x.Field<string>("PropertyId"),
                Name = x.Field<string>("Name"),
                Value = x.Field<string>("Value"),
                PropertyType = x.Field<string>("PropertyType")
            }).ToList();

            return getAuthorityForUserResponse;
        }

        #endregion

        #region GetUserByRegisterId
        public async Task<Response<UserInfo>> GetUserByRegisterId(string registerId)
        {

            var restClient = new RestClient(_SSOIntegrationServiceUrl);
            var restRequest = new RestRequest("?op=GetUserByRegisterId", Method.Post);
            restRequest.AddHeader("Content-Type", "application/soap+xml");

            #region body
            var body = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
" + "\n" +
@"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
" + "\n" +
@"  <soap12:Body>
" + "\n" +
@"    <GetUserByRegisterId xmlns=""http://tempuri.org/"">
" + "\n" +
@"      <RegisterId>{0}</RegisterId>
" + "\n" +
@"    </GetUserByRegisterId>
" + "\n" +
@"  </soap12:Body>
" + "\n" +
@"</soap12:Envelope>", registerId);
            #endregion
            restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);
            var restResponse = await restClient.ExecutePostAsync(restRequest);
            var response = GetUserInfoResponse(restResponse.Content);

            return Response<UserInfo>.Success(response, 200);
        }
        private UserInfo GetUserInfoResponse(string content)
        {
            doc.LoadXml(content);
            var getAuthorityForUserResponse = new UserInfo
            {
                Id = DocSelectField("Id"),
                CitizenshipNumber = DocSelectField("CitizenshipNumber"),
                LoginName = DocSelectField("LoginName"),
                FirstName = DocSelectField("FirstName"),
                Surname = DocSelectField("Surname"),
                Email = DocSelectField("Email"),
                Description = DocSelectField("Description"),
                BranchCode = DocSelectField("BranchCode"),
                RegisterId = DocSelectField("RegisterId"),
                CustomerNo = DocSelectField("CustomerNo")
            };
            return getAuthorityForUserResponse;
        }


        #endregion
        private string DocSelectField(string field)
        {
            var xpath = string.Format("//*/{0}//text()", field);
            return doc.SelectSingleNode(xpath)?.InnerText?.ToString() ?? "";


        }
        private string GetCustomerByCitizenshipNoDocSelectField(string field)
        {
            // diffgram içindeki NewDataSet -> Table1 -> field (örn: ExternalClientNo) alanını seçmek için XPath
            var xpath = $"//NewDataSet/Table1/{field}";
            return doc.SelectSingleNode(xpath)?.InnerText?.Trim() ?? "";
        }
        public async Task<Response<string>> GetCustomerByCitizenshipNo(string citizenshipNo)
        {
            var restClient = new RestClient(_SSOIntegrationPusulaServiceUrl);
            var restRequest = new RestRequest("?op=GetCustomerByCitizenshipNo", Method.Post);
            restRequest.AddHeader("Content-Type", "application/soap+xml");

            #region body

            var body = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +
    @"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">" + "\n" +
    @"  <soap12:Body>" + "\n" +
    @"    <GetCustomerByCitizenshipNo xmlns=""http://intertech.com.tr/Pusula"">" + "\n" +
    @"      <citizenshipNo>35765082602</citizenshipNo>" + "\n" +
    @"    </GetCustomerByCitizenshipNo>" + "\n" +
    @"  </soap12:Body>" + "\n" +
    @"</soap12:Envelope>", citizenshipNo);
            #endregion

            restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);
            var restResponse = await restClient.ExecutePostAsync(restRequest);
            var response = GetCustomerByCitizenshipNoResponse(restResponse.Content);

            return Response<string>.Success(response, 200);
        }

        private string GetCustomerByCitizenshipNoResponse(string content)
        {
            doc.LoadXml(content);
            return GetCustomerByCitizenshipNoDocSelectField("ExternalClientNo");
        }


        public async Task<Response<string>> GetUserInfoByCustomerNo(string customerNo)
        {
            var restClient = new RestClient(_SSOIntegrationServiceUrl); // Pusula değil, doğru URL kullanılıyor.
            var restRequest = new RestRequest("?op=GetUserInfoByCustomerNo", Method.Post);
            restRequest.AddHeader("Content-Type", "application/soap+xml");

            #region body
            var body = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
" + "\n" +
        @"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
" + "\n" +
        @"  <soap12:Body>
" + "\n" +
        @"    <GetUserInfoByCustomerNo xmlns=""http://tempuri.org/"">
" + "\n" +
        @"      <customerNo>{0}</customerNo>
" + "\n" +
        @"    </GetUserInfoByCustomerNo>
" + "\n" +
        @"  </soap12:Body>
" + "\n" +
        @"</soap12:Envelope>", customerNo);
            #endregion

            restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);
            var restResponse = await restClient.ExecutePostAsync(restRequest);
            var response = GetUserInfoByCustomerNoResponse(restResponse.Content);

            return Response<string>.Success(response, 200);
        }

        private string GetUserInfoByCustomerNoResponse(string content)
        {
            doc.LoadXml(content);
            return DocSelectField("LoginName");
        }


    }


}