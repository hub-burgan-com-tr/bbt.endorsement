using Application.Common.Interfaces;
using Application.Common.Models;
using Application.SSOIntegrationService.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Infrastructure.SSOIntegration
{
    public class SSOIntegrationService : ISSOIntegrationService
    {
        private readonly string _SSOIntegrationServiceUrl;
        private static readonly RestClient _restClient;
        private static readonly RestClient _restClientPusula;

        static SSOIntegrationService()
        {
            _restClient = new RestClient(StaticValues.SSOIntegrationService);
            _restClientPusula = new RestClient(StaticValues.SSOIntegrationService.Replace("SSOIntegrationWebService/Service.asmx", "Pusula/Customerservices.asmx"));
        }

        public SSOIntegrationService()
        {
            _SSOIntegrationServiceUrl = StaticValues.SSOIntegrationService;
        }

        #region SearchUserInfo
        public async Task<Response<string>> SearchUserInfo(string loginName, string firstName = "", string lastName = "")
        {
            try
            {
                var requestBody = GenerateSoapRequestBody("SearchUserInfo", new
                {
                    loginName
                });

                var restRequest = CreateRestRequest("?op=SearchUserInfo", requestBody);
                var restResponse = await _restClient.ExecutePostAsync(restRequest);
                var response = ParseResponse<string>(restResponse.Content, "RegisterId");

                return Response<string>.Success(response, 200);
            }
            catch (Exception ex)
            {
                // Log and handle error as needed
                return Response<string>.Fail(ex.Message, 500);
            }
        }
        #endregion
         #region GetUserInfoByCustomerNo
        public async Task<Response<string>> GetUserInfoByCustomerNo(string customerNo)
        {
            try
            {
                var requestBody = GenerateSoapRequestBody("GetUserInfoByCustomerNo", new
                {
                    customerNo
                });

                var restRequest = CreateRestRequest("?op=GetUserInfoByCustomerNo", requestBody);
                var restResponse = await _restClient.ExecutePostAsync(restRequest);
                var response = ParseResponse<string>(restResponse.Content, "LoginName");

                return Response<string>.Success(response, 200);
            }
            catch (Exception ex)
            {
                // Log and handle error as needed
                return Response<string>.Fail(ex.Message, 500);
            }
        }
        #endregion
        #region GetCustomerByCitizenshipNo
        public async Task<Response<long>> GetCustomerByCitizenshipNo(string  citizenshipNo)
        {
            try
            {
                var requestBody = GenerateSoapRequestBody("GetCustomerByCitizenshipNo", new
                {
                    citizenshipNo
                });

                var restRequest = CreateRestRequest("?op=GetCustomerByCitizenshipNo", requestBody);
                var restResponse = await _restClientPusula.ExecutePostAsync(restRequest);
                var response = ParseResponse<long>(restResponse.Content, "ExternalClientNo");

                return Response<long>.Success(response, 200);
            }
            catch (Exception ex)
            {
                // Log and handle error as needed
                return Response<long>.Fail(ex.Message, 500);
            }
        }
        #endregion
        #region GetAuthorityForUser
        public async Task<Response<List<UserAuthority>>> GetAuthorityForUser(string applicationCode, string authorityName, string loginAndDomainName)
        {
            try
            {
                var requestBody = GenerateSoapRequestBody("GetAuthorityForUser", new
                {
                    applicationCode,
                    authorityName,
                    loginAndDomainName
                });

                var restRequest = CreateRestRequest("?op=GetAuthorityForUser", requestBody);
                var restResponse = await _restClient.ExecutePostAsync(restRequest);
                var response = ParseAuthorityResponse(restResponse.Content);

                return Response<List<UserAuthority>>.Success(response, 200);
            }
            catch (Exception ex)
            {
                // Log and handle error as needed
                return Response<List<UserAuthority>>.Fail(ex.Message, 500);
            }
        }
        #endregion

        #region GetUserByRegisterId
        public async Task<Response<UserInfo>> GetUserByRegisterId(string registerId)
        {
            try
            {
                var requestBody = GenerateSoapRequestBody("GetUserByRegisterId", new { RegisterId = registerId });

                var restRequest = CreateRestRequest("?op=GetUserByRegisterId", requestBody);
                var restResponse = await _restClient.ExecutePostAsync(restRequest);
                var response = ParseUserInfoResponse(restResponse.Content);

                return Response<UserInfo>.Success(response, 200);
            }
            catch (Exception ex)
            {
                // Log and handle error as needed
                return Response<UserInfo>.Fail(ex.Message, 500);
            }
        }
        #endregion

        #region Helper Methods
        private string GenerateSoapRequestBody(string operation, object parameters)
        {
            var xmlTemplate = $@"
                <?xml version=""1.0"" encoding=""utf-8""?>
                <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                    <soap12:Body>
                        <{operation} xmlns=""http://tempuri.org/"">
                            {string.Join(Environment.NewLine, parameters.GetType().GetProperties().Select(p => $"<${p.Name}>{p.GetValue(parameters)}</{p.Name}>"))}
                        </{operation}>
                    </soap12:Body>
                </soap12:Envelope>";
            return xmlTemplate;
        }

        private RestRequest CreateRestRequest(string resource, string body)
        {
            var restRequest = new RestRequest(resource, Method.Post);
            restRequest.AddHeader("Content-Type", "application/soap+xml");
            restRequest.AddParameter("application/soap+xml", body, ParameterType.RequestBody);
            return restRequest;
        }

        private T ParseResponse<T>(string content, string field)
        {
            var doc = new XmlDocument();
            doc.LoadXml(content);
            var xpath = $"//*/{field}//text()";
            return (T)Convert.ChangeType(doc.SelectSingleNode(xpath)?.InnerText ?? string.Empty, typeof(T));
        }

        private List<UserAuthority> ParseAuthorityResponse(string content)
        {
            var doc = new XmlDocument();
            doc.LoadXml(content);
            var xmlReader = new XmlNodeReader(doc.SelectSingleNode("//*/NewDataSet"));
            var dataSet = new DataSet();
            dataSet.ReadXml(xmlReader);
            return dataSet.Tables[0].AsEnumerable().Select(x => new UserAuthority
            {
                PropertyId = x.Field<string>("PropertyId"),
                Name = x.Field<string>("Name"),
                Value = x.Field<string>("Value"),
                PropertyType = x.Field<string>("PropertyType")
            }).ToList();
        }

        private UserInfo ParseUserInfoResponse(string content)
        {
            var doc = new XmlDocument();
            doc.LoadXml(content);
            return new UserInfo
            {
                Id = ParseResponse<string>(content, "Id"),
                CitizenshipNumber = ParseResponse<string>(content, "CitizenshipNumber"),
                LoginName = ParseResponse<string>(content, "LoginName"),
                FirstName = ParseResponse<string>(content, "FirstName"),
                Surname = ParseResponse<string>(content, "Surname"),
                Email = ParseResponse<string>(content, "Email"),
                Description = ParseResponse<string>(content, "Description"),
                BranchCode = ParseResponse<string>(content, "BranchCode"),
                RegisterId = ParseResponse<string>(content, "RegisterId"),
                CustomerNo = ParseResponse<string>(content, "CustomerNo")
            };
        }
        #endregion
    }
}
