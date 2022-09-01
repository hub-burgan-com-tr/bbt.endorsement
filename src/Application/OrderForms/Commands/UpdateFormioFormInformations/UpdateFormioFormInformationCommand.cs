using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace Application.OrderForms.Commands.UpdateFormioFormInformations
{
    public class UpdateFormioFormInformationCommand : IRequest<Response<bool>>
    {
        public string FormDefinitionId { get; set; }
        public string SemanticVersion { get; set; }
        public IFormFile Json { get; set; }
        public IFormFile HtmlTemplate { get; set; }

    }

    public class UpdateFormioFormInformationCommandHandler : IRequestHandler<UpdateFormioFormInformationCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateFormioFormInformationCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(UpdateFormioFormInformationCommand request, CancellationToken cancellationToken)
        {

            UpdateFormioFormInformationCommandValidator validator = new UpdateFormioFormInformationCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            int result = 0;
            var labelJson = new StringBuilder();
            using (var reader = new StreamReader(request.Json.OpenReadStream()))
            {
                while (reader.Peek() > 0)
                    labelJson.AppendLine(reader.ReadLine());
            }

            var label = labelJson.ToString(); //.Replace("\"", String.Empty);

            var htmlTemplate = new StringBuilder();
            using (var reader = new StreamReader(request.HtmlTemplate.OpenReadStream()))
            {
                while (reader.Peek() > 0)
                    htmlTemplate.AppendLine(reader.ReadLine());
            }
            var template = GeneralExtensions.HtmlToString(htmlTemplate.ToString());
            var form = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.FormDefinitionId);
            if (form == null)
                return Response<bool>.NotFoundException("Form bulunamadı", 200);         
            if (form != null)
            {
                form.Label = label;
                form.HtmlTemplate = template;          
                _context.FormDefinitions.Update(form);
                result = _context.SaveChanges();


                if (result > 0)
                {
                    var restClient = new RestClient(StaticValues.TemplateEngine);
                    var restRequest = new RestRequest("/Template/Definition", Method.Post);
                    //restRequest.AddHeader("Content-Type", "application/json");
                    //restRequest.AddHeader("Accept", "text/plain");
                    //restRequest.AddHeader("Accept", "text/plain");

                    var body = new TemplateDefinitionRoot
                    {
                        MasterTemplate = "",
                        template = template,
                        name = form.TemplateName,
                        SemanticVersion=request.SemanticVersion
                    };
                    var json = JsonConvert.SerializeObject(body);

                    restRequest.RequestFormat = DataFormat.Json;
                    restRequest.AddJsonBody(json);
                    var response = restClient.ExecutePostAsync(restRequest).Result;
                }
            }

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }















}
