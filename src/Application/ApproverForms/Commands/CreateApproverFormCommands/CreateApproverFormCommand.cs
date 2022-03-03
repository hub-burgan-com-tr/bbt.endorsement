using Application.Common.Models;
using MediatR;

namespace Application.ApproverForms.Commands.CreateApproverFormCommands
{
    public class CreateApproverFormCommand : IRequest<Response<int>>
    {
        /// <summary>
        /// Form Id
        /// </summary>
        public int FormId { get; set; }
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// TCKN
        /// </summary>
        public string CitizenShipNumber { get; set; }
        /// <summary>
        /// Müsteri No
        /// </summary>
        public string CustomerNumber { get; set; }
    }
    /// <summary>
    /// Onaycı Ekle 
    /// </summary>
    public class CreateApproverFormCommandQueryHandler : IRequestHandler<CreateApproverFormCommand, Response<int>>
    {
        public async Task<Response<int>> Handle(CreateApproverFormCommand request, CancellationToken cancellationToken)
        {
            return Response<int>.Success(1, 200);
        }
    }
}
