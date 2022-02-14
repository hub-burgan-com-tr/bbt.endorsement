using Application.Common.Models;
using MediatR;

namespace Application.NewConfirmationOrders.Commands.NewConfirmationOrderCommands
{
    public class NewConfirmationOrderCommand : IRequest<Response<List<NewConfirmationOrderCommandDto>>>
    {
        public string Title { get; set; }
        public string Process { get; set; }
        public string Stage { get; set; }
        public string TransactionNumber { get; set; }
        public string TimeoutMinutes  { get; set; }
        public string RetryFrequence { get; set; }
        public int MaxRetryCount { get; set; }

        public class NewConfirmationOrderCommandQueryHandler : IRequestHandler<NewConfirmationOrderCommand, Response<List<NewConfirmationOrderCommandDto>>>
        {
            public async Task<Response<List<NewConfirmationOrderCommandDto>>> Handle(NewConfirmationOrderCommand request, CancellationToken cancellationToken)
            {
                var list = new List<NewConfirmationOrderCommandDto>();
                return Response<List<NewConfirmationOrderCommandDto>>.Success(list, 200);
            }
        }
    }
}
