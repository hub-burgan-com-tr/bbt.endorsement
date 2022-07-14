using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Commands.CreateFormInformations
{
    public class CreateFormInformationCommand : IRequest<Response<bool>>
    {
        public string Name { get; set; }
        public string TemplateName { get; set; }
        public int MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }
        public List<string> FormDefinitionTags { get; set; }
    }

    public class CreateFormInformationCommandHandler : IRequestHandler<CreateFormInformationCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateFormInformationCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(CreateFormInformationCommand request, CancellationToken cancellationToken)
        {
            CreateFormInformationCommandValidator validator = new CreateFormInformationCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            int result = 0;
            var templateName = request.TemplateName + ".txt";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateName);
            var label = File.ReadAllText(path, Encoding.Default);
            var Form = _context.FormDefinitions.Any(x => x.Name == request.Name);
            if (!Form)
            {
                var formdefinition = _context.FormDefinitions.Add(new FormDefinition
                {
                    FormDefinitionId = Guid.NewGuid().ToString(),
                    DocumentSystemId = "b25635e8-1abd-4768-ab97-e1285999a62b",
                    Name = request.Name,
                    Label = label.ToString(),
                    Created = DateTime.Now,
                    Tags = "",
                    TemplateName = request.TemplateName,
                    RetryFrequence = request.RetryFrequence,
                    Mode = "Completed",
                    Url = "",
                    Type = ContentType.PDF.ToString(),
                    ExpireInMinutes = request.ExpireInMinutes,
                    MaxRetryCount = request.MaxRetryCount,
                    DependencyReuse = false,
                    DependencyFormId = Guid.NewGuid().ToString(),

                });
                formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
                formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });

                foreach (var item in request.FormDefinitionTags)
                {
                    var IsTag = _context.FormDefinitionTags.FirstOrDefault(x => x.Tag == item);
                    if (IsTag == null)
                    {
                        var tag = _context.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = item });
                        formdefinition.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap { FormDefinitionTagMapId = Guid.NewGuid().ToString(), FormDefinitionId = formdefinition.Entity.FormDefinitionId, FormDefinitionTagId = tag.Entity.FormDefinitionTagId });
                    }
                    else
                    {
                        var tagmap = _context.FormDefinitionTagMaps.Any(x => x.FormDefinitionId == formdefinition.Entity.FormDefinitionId && x.FormDefinitionTagId == IsTag.FormDefinitionTagId);
                        if (!tagmap)
                        {
                            formdefinition.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap { FormDefinitionTagMapId = Guid.NewGuid().ToString(), FormDefinitionId = formdefinition.Entity.FormDefinitionId, FormDefinitionTagId = IsTag.FormDefinitionTagId });
                        }
                    }
                }

                result = _context.SaveChanges();
            }

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }


}
