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

        public async Task<string> ApproverSaveAsync(long citizenshipNumber, string firstName, string lastName)
        {
            var entity = _context.Approvers.Add(new Approver
            {
                ApproverId = Guid.NewGuid().ToString(),
                CitizenshipNumber = citizenshipNumber,
                FirstName = firstName,
                LastName = lastName,
                Created = _dateTime.Now,
            }).Entity;

            _context.SaveChanges();
            return entity.ApproverId;
        }

        public async Task<string> CustomerSaveAsync(OrderApprover approver)
        {
            var entity = _context.Customers.Add(new Customer
            {
                CustomerId = Guid.NewGuid().ToString(),
                CitizenshipNumber = approver.CitizenshipNumber,
                FirstName = approver.First,
                LastName = approver.Last,
                Created = _dateTime.Now,
            }).Entity;

            _context.SaveChanges();
            return entity.CustomerId;
        }

        public async Task<string> GetApproverAsync(long citizenshipNumber)
        {
            var response = _context.Approvers.FirstOrDefault(x => x.CitizenshipNumber == citizenshipNumber);
            if (response == null)   
                return null;
            return response.ApproverId;
        }

        public async Task<string> GetCustomerAsync(long citizenshipNumber)
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
