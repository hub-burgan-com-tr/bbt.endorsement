using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Documents.Commands.Queries.GetDocuments
{
    public class GetDocumentsQueryDto : IRequest<Response<List<GetDocumentsDto>>>
    {
        public int ApprovalId { get; set; }
    }
    public class GetDocumentQueryQueryHandler : IRequestHandler<GetDocumentsQueryDto, Response<List<GetDocumentsDto>>>
    {
        public async Task<Response<List<GetDocumentsDto>>> Handle(GetDocumentsQueryDto request, CancellationToken cancellationToken)
        {
            var list = new List<GetDocumentsDto>();
            return Response<List<GetDocumentsDto>>.Success(list, 200);
        }
    }
}
