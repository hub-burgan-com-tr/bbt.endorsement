using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace Application.OrderForms.Commands.CreateFormInformationsText
{
    /// <summary>
    /// Name: Sigorta Teklif Formu - Konut Eşya
    /// </summary>
    public class CreateFormInformationTextCommand : IRequest<Response<bool>>
    {
        public string FormDefinitionTagId { get; set; }
        public string ParameterId { get; set; }
        public string Name { get; set; }
        public string TemplateName { get; set; }
        public string SemanticVersion { get; set; }
        public int MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }

        public string Json { get; set; }
        public string HtmlTemplate { get; set; }
    }

    public class CreateFormInformationTextCommandHandler : IRequestHandler<CreateFormInformationTextCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateFormInformationTextCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(CreateFormInformationTextCommand request, CancellationToken cancellationToken)
        {

            CreateFormInformationTextCommandValidator validator = new CreateFormInformationTextCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            int result = 0;
            //var templateName = request.TemplateName + ".txt";
            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateName);
            //var label = File.ReadAllText(path, Encoding.Default);

            var formDefinitionTag = _context.FormDefinitionTags.FirstOrDefault(x => x.FormDefinitionTagId == request.FormDefinitionTagId);
            if (formDefinitionTag == null)
                return Response<bool>.NotFoundException("Form Türü bulunamadı", 200);


            var parametre = _context.Parameters.FirstOrDefault(x => x.ParameterId == request.ParameterId);
            if(parametre == null)
                return Response<bool>.NotFoundException("parametre bulunamadı", 200);
            var label = request.Json; //.Replace("\"", String.Empty);
            var template = GeneralExtensions.HtmlToString(request.HtmlTemplate);
            var templateName = request.TemplateName + "-" + parametre.Text;
            var name = request.Name + " - " + parametre.Text;
            var Form = _context.FormDefinitions.Any(x => x.Name == name);
            if (!Form)
            {
                var formdefinition = _context.FormDefinitions.Add(new FormDefinition
                {
                    FormDefinitionId = Guid.NewGuid().ToString(),
                    ParameterId = request.ParameterId,
                    DocumentSystemId = "b25635e8-1abd-4768-ab97-e1285999a62b",
                    Name = name,
                    Source = "formio",
                    Label = label,
                    HtmlTemplate = template,
                    Created = DateTime.Now,
                    Tags = "",
                    TemplateName = templateName,
                    RetryFrequence = request.RetryFrequence,
                    Mode = "Completed",
                    Url = "",
                    Type = ContentType.PDF.ToString(),
                    ExpireInMinutes = request.ExpireInMinutes,
                    MaxRetryCount = request.MaxRetryCount,
                    DependencyReuse = false,

                });
                formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
                formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });

                var tagmap = _context.FormDefinitionTagMaps.Any(x => x.FormDefinitionId == formdefinition.Entity.FormDefinitionId && x.FormDefinitionTagId == request.FormDefinitionTagId);
                if (!tagmap)
                {
                    formdefinition.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap
                    {
                        FormDefinitionTagMapId = Guid.NewGuid().ToString(),
                        FormDefinitionId = formdefinition.Entity.FormDefinitionId,
                        FormDefinitionTagId = request.FormDefinitionTagId
                    });
                }
               
                    var restClient = new RestClient(StaticValues.TemplateEngine);
                    var restRequest = new RestRequest("/Template/Definition", Method.Post);
                    restRequest.AddHeader("Content-Type", "application/json");
                    restRequest.AddHeader("Accept", "text/plain");

                    var body = new TemplateDefinitionRoot
                    {
                        MasterTemplate = "",
                        template = template,
                        name = templateName,
                        SemanticVersion=request.SemanticVersion
                    };
                    var json = JsonConvert.SerializeObject(body);

                    restRequest.RequestFormat = DataFormat.Json;
                    restRequest.AddJsonBody(json);
                    var response = restClient.ExecutePostAsync(restRequest).Result;
                if (response.StatusCode.ToString() == "OK")
                {
                    result = _context.SaveChanges();

                }
                else
                {
                    return Response<bool>.NotFoundException(response.ErrorException.ToString(), 200);

                }

            }
            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }


}
