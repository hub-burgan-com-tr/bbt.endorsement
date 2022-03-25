using Worker.App.Application.Common.Interfaces;
using Worker.App.Domain.Entities;
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

        public async Task<string> ApproverSaveAsync(string citizenshipNumber, string firstName, string lastName)
        {
            var entity = _context.Approvers.Add(new Approver
            {
                CitizenshipNumber = citizenshipNumber,
                FirstName = firstName,
                LastName = lastName,
                Created = _dateTime.Now,
            }).Entity;

            _context.SaveChanges();
            return entity.ApproverId;
        }

        public async Task<string> CustomerSaveAsync(string citizenshipNumber, string firstName, string lastName)
        {
            var entity = _context.Customers.Add(new Customer
            {
                CitizenshipNumber = citizenshipNumber,
                FirstName = firstName,
                LastName = lastName,
                Created = _dateTime.Now,
            }).Entity;

            _context.SaveChanges();
            return entity.CustomerId;
        }

        public async Task<string> GetApproverAsync(string citizenshipNumber)
        {
            var response = _context.Approvers.FirstOrDefault(x => x.CitizenshipNumber == citizenshipNumber);
            if (response == null)   
                return null;
            return response.ApproverId;
        }

        public async Task<string> GetCustomerAsync(string citizenshipNumber)
        {
            var response = _context.Customers.FirstOrDefault(x => x.CitizenshipNumber == citizenshipNumber);
            if (response == null)
                return null;
            return response.CustomerId;
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
                    Actions = x.FormDefinitionActions.Select(y => new FormDefinitionActionDto
                    {
                        Title = y.Title,
                        State = y.State,
                        IsDefault = y.IsDefault
                    })
                }).FirstOrDefault();

            return response;
        }
    }
}
