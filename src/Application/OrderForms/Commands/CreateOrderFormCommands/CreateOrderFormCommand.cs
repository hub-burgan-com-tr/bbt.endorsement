using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Commands.CreateOrderFormCommands
{
    public class CreateOrderFormCommand : IRequest<Response<bool>>
    {
        public int ApprovalId { get; set; }
        public string TC { get; set; }
        public string NameAndSurname { get; set; }
    }

    public  class CreateFormApprovalQueryHandler : IRequestHandler<CreateOrderFormCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(CreateOrderFormCommand request, CancellationToken cancellationToken)
        {
            return Response<bool>.Success(true, 200);
        }
    }

}
