using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.OrderForms.Commands.CreateDependencyFormInformations;

public class CreateDependencyFormInformationCommand : IRequest<Response<bool>>
{
    public string DependencyFormId { get; set; }
    public string Name { get; set; }    
}

public class CreateDependencyFormInformationCommandHandler : IRequestHandler<CreateDependencyFormInformationCommand, Response<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public CreateDependencyFormInformationCommandHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<Response<bool>> Handle(CreateDependencyFormInformationCommand request, CancellationToken cancellationToken)
    {
        var dependencyForm = _context.FormDefinitions.FirstOrDefault(x => x.DependencyFormId == request.DependencyFormId);
        if (dependencyForm != null)
            return Response<bool>.NotFoundException("form (" + dependencyForm.Name + ") formuna bağlı", 200);

        var formDefinition = _context.FormDefinitions
            .Include(x => x.Parameter)
            .FirstOrDefault(x => x.FormDefinitionId == request.DependencyFormId);

        var formDefinitionTag = _context.FormDefinitionTagMaps.FirstOrDefault(x => x.FormDefinitionId == formDefinition.FormDefinitionId);

        if (formDefinition == null)
            return Response<bool>.NotFoundException("form bulunamadı", 200);


        var name = "Sigorta Teklif Formu - " + formDefinition.Parameter.Text;

        var formdefinitionTeklif = _context.FormDefinitions.Add(new FormDefinition
        {
            FormDefinitionId = Guid.NewGuid().ToString(),
            DependencyFormId = formDefinition.FormDefinitionId,
            ParameterId = formDefinition.Parameter.ParameterId,
            DocumentSystemId = "b25635e8-1abd-4768-ab97-e1285999a62b",
            Name = name,
            Source = "file",
            Label = formDefinition.Label,
            Created = DateTime.Now,
            Tags = "",
            TemplateName = formDefinition.TemplateName,
            Mode = "Completed",
            Url = "",
            Type = ContentType.PDF.ToString(),
            RetryFrequence = formDefinition.RetryFrequence,
            ExpireInMinutes = formDefinition.ExpireInMinutes,
            MaxRetryCount = formDefinition.MaxRetryCount

        });
        formdefinitionTeklif.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
        formdefinitionTeklif.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });

        formdefinitionTeklif.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap
        {
            FormDefinitionTagMapId = Guid.NewGuid().ToString(),
            FormDefinitionId = formdefinitionTeklif.Entity.FormDefinitionId,
            FormDefinitionTagId = formDefinitionTag.FormDefinitionTagId
        });

        var result = _context.SaveChanges();

        return Response<bool>.Success(result > 0 ? true : false, 200);
    }
}