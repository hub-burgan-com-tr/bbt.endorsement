using Worker.App.Application.Common.Models;
using Worker.App.Application.Internals.Models;

namespace Worker.App.Application.Common.Interfaces
{
    public interface IInternalsService
    {
        Task<Response<CustomerResponse>> GetCustomerSearch(CustomerRequest person);

        Task<Response<List<PersonResponse>>> GetPersonSearch(string name);
        Task<Response<PersonResponse>> GetPersonById(long id);
    }
}
