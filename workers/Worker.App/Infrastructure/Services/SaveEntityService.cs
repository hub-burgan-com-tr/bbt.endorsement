using Domain.Entities;
using Domain.Models;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Dtos;

namespace Worker.App.Infrastructure.Services
{
    public class SaveEntityService : ISaveEntityService
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public SaveEntityService(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }


        public async Task<string> GetCustomerAsync(long citizenshipNumber)
        {
            var response = _context.Customers.FirstOrDefault(x => x.CitizenshipNumber == citizenshipNumber);
            if (response == null)
                return null;
            return response.CustomerId;
        }

        public async Task<string> CustomerSaveAsync(OrderCustomer customer)
        {
            var entity = _context.Customers.Add(new Customer
            {
                CustomerId = Guid.NewGuid().ToString(),
                CitizenshipNumber = customer.CitizenshipNumber,
                CustomerNumber = customer.CustomerNumber,
                FirstName = customer.First,
                LastName = customer.Last,
                Created = _dateTime.Now,
                BranchCode = customer.BranchCode,
                BusinessLine = customer.BusinessLine
            }).Entity;

            _context.SaveChanges();
            return entity.CustomerId;
        }

        public async Task<string> GetPersonAsync(long citizenshipNumber)
        {
            var response = _context.Persons.FirstOrDefault(x => x.CitizenshipNumber == citizenshipNumber);
            if (response == null)
                return null;
            return response.PersonId;
        }

        public async Task<string> PersonSaveAsync(OrderPerson customer)
        {

            var entity = _context.Persons.Add(new Person
            {
                PersonId = Guid.NewGuid().ToString(),
                CitizenshipNumber = customer.CitizenshipNumber,
                CustomerNumber = customer.CustomerNumber,
                FirstName = customer.First,
                LastName = customer.Last,
                Created = _dateTime.Now,
                BusinessLine=customer.BusinessLine,
                BranchCode=customer.BranchCode,
            }).Entity;

            _context.SaveChanges();
            return entity.PersonId;
        }

        public async Task<FormDefinitionDto> GetFormDefinition(string formId)
        {
            var response = _context.FormDefinitions
                .Where(x => x.FormDefinitionId == formId)
                .Select(x => new FormDefinitionDto
                {
                    ExpireInMinutes = x.ExpireInMinutes,
                    RetryFrequence = x.RetryFrequence,
                    MaxRetryCount = x.MaxRetryCount,
                    Type = x.Type,
                    DocumentSystemId=x.DocumentSystemId,
                    Actions = x.FormDefinitionActions.Select(y => new FormDefinitionActionDto
                    {
                        Title = y.Title,
                        State = y.State,
                        Choice = y.Choice
                    })
                }).FirstOrDefault();

            return response;
        }
    }
}
