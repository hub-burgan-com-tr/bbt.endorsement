using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommand : IRequest<Response<StartResponse>>
    {
        public StartRequest StartRequest { get; set; }
        public OrderPerson Person { get; set; }
        public Form FormType { get; set; }
    }

    public class NewOrderCommandHandler : IRequestHandler<NewOrderCommand, Response<StartResponse>>
    {
        private readonly IZeebeService _zeebe;
        private readonly IApplicationDbContext _context;

        public NewOrderCommandHandler(IZeebeService zeebe, IApplicationDbContext context)
        {
            _zeebe = zeebe;
            _context = context;
        }

        public async Task<Response<StartResponse>> Handle(NewOrderCommand request, CancellationToken cancellationToken)
        {
            NewOrderCommandValidator validator = new NewOrderCommandValidator();
            ValidationResult result = validator.Validate(request);
            validator.ValidateAndThrow(request);

            var instanceId = request.StartRequest.Id;
            var expireInMinutes = "PT" + request.StartRequest.Config.ExpireInMinutes + "M";
            var retryFrequence = "PT" + request.StartRequest.Config.RetryFrequence + "M";

            var model = new ContractModel
            {
                StartRequest = request.StartRequest,
                FormType = request.FormType,
                Device = false,
                InstanceId = instanceId,
                ExpireInMinutes = expireInMinutes,
                RetryFrequence = retryFrequence,
                MaxRetryCount = request.StartRequest.Config.MaxRetryCount,
                Person = request.Person
            };

            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(instanceId.ToString(), "contact_approval_contract_new", payload);

            return Response<StartResponse>.Success(new StartResponse { InstanceId = instanceId }, 200);
        }
    }
}