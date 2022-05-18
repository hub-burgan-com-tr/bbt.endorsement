using Domain.Enums;
using Domain.Models;
using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Orders.Queries.CheckOrderDependecyRules;

public class CheckOrderDependecyRulesQuery : IRequest<Response<bool>>
{
    public ContractModel Model { get; set; }
}


public class CheckOrderDependecyRulesQueryHandler : IRequestHandler<CheckOrderDependecyRulesQuery, Response<bool>>
{
    private IApplicationDbContext _context;
    private ISaveEntityService _saveEntityService;
    private IDateTime _dateTime;

    public CheckOrderDependecyRulesQueryHandler(IApplicationDbContext context, ISaveEntityService saveEntityService, IDateTime dateTime)
    {
        _context = context;
        _saveEntityService = saveEntityService;
        _dateTime = dateTime;
    }

    public async Task<Response<bool>> Handle(CheckOrderDependecyRulesQuery request, CancellationToken cancellationToken)
    {
        if (request.Model.FormType == Form.FormOrder)
        {
            var model = request.Model.StartFormRequest;

            var formDefinition = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == model.FormId);
            if (formDefinition != null && formDefinition.DependencyFormId != null && formDefinition.Source == "file")
            {
                var customerId = _saveEntityService.GetCustomerAsync(model.Approver.CitizenshipNumber).Result;
                var dependencyForm = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == formDefinition.DependencyFormId);
                var orders = _context.Orders
                    .Where(x => x.CustomerId == customerId &&
                                x.Documents.Any(y => y.FormDefinitionId == dependencyForm.FormDefinitionId) &&
                                x.State == OrderState.Approve.ToString());
            }
        }


        //var formDefinitionId = model.FormType == Form.Order ? model.StartRequest.Documents: model.StartFormRequest.Approver.CitizenshipNumber;
        ////var dependencyFormId = model.FormType == Form.Order ? model.StartRequest.DependencyFormId : model.StartFormRequest.DependencyFormId;
        ////var dependecyRules = model.FormType == Form.Order ? model.StartRequest.DependecyRules : model.StartFormRequest.DependecyRules;

        //if (dependecyRules == false)
        //{
        //    var customerId = _saveEntityService.GetCustomerAsync(citizenshipNumber).Result;
        //    var order = _context.Orders.FirstOrDefault(x => x.CustomerId == customerId && x.DependencyFormId == dependencyFormId);

        //    dependecyRules = order == null ? true : false;
        //}

        return Response<bool>.Success(true, 200);

    }
}
