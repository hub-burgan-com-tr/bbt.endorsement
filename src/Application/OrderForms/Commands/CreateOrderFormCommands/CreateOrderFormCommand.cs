using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Commands.CreateOrderFormCommands
{
    public class CreateOrderFormCommand : IRequest<Response<bool>>
    {
        /// <summary>
        /// FormId
        /// </summary>
        public int FormId { get; set; }
        /// <summary>
        /// TCKN
        /// </summary>
        public string CitizenShipNumber { get; set; }
        /// <summary>
        /// Ad Soyad
        /// </summary>
        public string NameAndSurname { get; set; }
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
