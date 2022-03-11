using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Commands.CreateOrderForm
{
    public class CreateOrderFormCommand : IRequest<Response<bool>>
    {
        /// <summary>
        /// FormId
        /// </summary>
        public int FormId { get; set; }
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        public Dictionary<string, string> FormParameters { get; set; }
        public int Type { get; set; }
        public string Value { get; set; }
        public string NameSurname { get; set; }
    }
    /// <summary>
    /// Form İle Emir Ekleme
    /// </summary>
    public  class CreateFormApprovalQueryHandler : IRequestHandler<CreateOrderFormCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(CreateOrderFormCommand request, CancellationToken cancellationToken)
        {
            return Response<bool>.Success(true, 200);
        }
    }

}
