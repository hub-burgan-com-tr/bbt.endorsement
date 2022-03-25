using Worker.App.Dtos;
using Worker.App.Models;

namespace Worker.App.Application.Common.Interfaces
{
    public interface ISaveEntityService
    {
        Task<string> GetCustomerAsync(long citizenshipNumber);
        Task<string> CustomerSaveAsync(OrderApprover approver);
        Task<string> GetApproverAsync(long citizenshipNumber);
        Task<string> ApproverSaveAsync(long citizenshipNumber, string firstName, string lastName);
        Task<FormDefinitionDto> GetFormDefinition(string formId);
    }
}
