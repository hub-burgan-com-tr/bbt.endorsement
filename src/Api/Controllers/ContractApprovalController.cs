using Application.Approvals.Commands.CreateApprovalCommands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("contract-approval")]
    [ApiController]
    public class ContractApprovalController : ApiControllerBase
    {

        [HttpPost("contracts")]
        public async Task<object> Contracts(CreateApprovalRequest request)
        {
            //var model = new ContractApprovalData
            //{
            //    Request = request,
            //    InstanceId = Guid.NewGuid().ToString(),
            //    Limit = 0,
            //    Device = false
            //};
            //model.InstanceId = Guid.NewGuid().ToString();

            //string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            //var response = await ZeebeService.SendMessage(model.InstanceId, "contact_approval_contract_new", payload);


            //return response;

            return null;
        }
    }
}
