using Application.BbtInternals.Models;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IInternalsService
    {
        Task<Response<List<PersonResponse>>> GetPersonSearch(string name);
        Task<Response<PersonResponse>> GetPersonById(long id);
    }
}
