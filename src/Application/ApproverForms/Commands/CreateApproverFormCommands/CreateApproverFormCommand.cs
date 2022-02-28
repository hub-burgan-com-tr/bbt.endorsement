using Application.Common.Models;
using MediatR;

namespace Application.ApproverForms.Commands.CreateApproverFormCommands
{
    public class CreateApproverFormCommand : IRequest<Response<List<CreateApproverFormCommandDto>>>
    {
        /// <summary>
        /// Form Id
        /// </summary>
        public int FormId { get; set; }
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
    public class CreateApproverFormCommandQueryHandler : IRequestHandler<CreateApproverFormCommand, Response<List<CreateApproverFormCommandDto>>>
    {
        public async Task<Response<List<CreateApproverFormCommandDto>>> Handle(CreateApproverFormCommand request, CancellationToken cancellationToken)
        {
            var list = new List<CreateApproverFormCommandDto>();
            return Response<List<CreateApproverFormCommandDto>>.Success(list, 200);
        }
    }
}
