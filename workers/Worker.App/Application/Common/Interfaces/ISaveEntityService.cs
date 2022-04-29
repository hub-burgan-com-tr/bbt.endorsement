using Domain.Models;
using Worker.App.Dtos;

namespace Worker.App.Application.Common.Interfaces
{
    public interface ISaveEntityService
    {
        Task<string> GetCustomerAsync(long citizenshipNumber);
        Task<string> CustomerSaveAsync(OrderCustomer customer);
        Task<string> GetApproverAsync(long citizenshipNumber);
        Task<string> ApproverSaveAsync(OrderApprover approver);
        Task<FormDefinitionDto> GetFormDefinition(string formId);
    }
}
