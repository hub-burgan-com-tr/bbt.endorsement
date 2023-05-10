using Domain.Models;
using Worker.App.Dtos;

namespace Worker.App.Application.Common.Interfaces
{
    public interface ISaveEntityService
    {
        Task<string> GetCustomerAsync(long citizenshipNumber);
        Task<string> CustomerSaveAsync(OrderCustomer customer);
        Task<string> CustomerUpdateAsync(OrderCustomer customer);

        Task<string> GetPersonAsync(long citizenshipNumber);
        Task<string> PersonSaveAsync(OrderPerson person);
        Task<string> PersonUpdateAsync(OrderPerson person);
        Task<FormDefinitionDto> GetFormDefinition(string formId);
    }
}
