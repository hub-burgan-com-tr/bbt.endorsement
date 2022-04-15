using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Application.Models;
using Domain.Enum;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.NewOrderForms
{
    public class NewOrderFormCommand : IRequest<Response<NewOrderFormResponse>>
    {
        public StartFormRequest Request { get; set; }
        public Form FormType { get; set; }
    }

    public class NewFormOrderCommandHandler : IRequestHandler<NewOrderFormCommand, Response<NewOrderFormResponse>>
    {
        private readonly IZeebeService _zeebe;
        private readonly IApplicationDbContext _context;

        public NewFormOrderCommandHandler(IZeebeService zeebe, IApplicationDbContext context)
        {
            _zeebe = zeebe;
            _context = context;
        }

        public async Task<Response<NewOrderFormResponse>> Handle(NewOrderFormCommand request, CancellationToken cancellationToken)
        {
            NewOrderFormCommandValidator validator = new NewOrderFormCommandValidator();
            ValidationResult result = validator.Validate(request);
            validator.ValidateAndThrow(request);
            var instanceId = request.Request.Id;

            var config = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.Request.FormId);

            var expireInMinutes = "PT"+ config.ExpireInMinutes + "M";
            var retryFrequence = "PT" + config.RetryFrequence + "M";

            expireInMinutes = expireInMinutes.Replace(@"\""", String.Empty).Replace("\"", String.Empty);
            retryFrequence = retryFrequence.Replace(@"\""", String.Empty).Replace("\"", String.Empty);

            var model = new ContractModel
            {
                StartFormRequest = request.Request,
                FormType = request.FormType,
                Device = false,
                InstanceId = instanceId,
                ExpireInMinutes = expireInMinutes,
                RetryFrequence = retryFrequence,
                MaxRetryCount = config.MaxRetryCount,
            };
            
            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(instanceId.ToString(), "contact_approval_contract_new", payload);

            return Response<NewOrderFormResponse>.Success(new NewOrderFormResponse { InstanceId = instanceId }, 200);

        }
    }
}
