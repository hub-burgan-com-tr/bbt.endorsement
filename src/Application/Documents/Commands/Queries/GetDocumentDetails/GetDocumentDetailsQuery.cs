using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Documents.Commands.Queries.GetDocumentDetails
{
    public class GetDocumentDetailsQuery : IRequest<Response<GetDocumentDetailsDto>>
    {
        public int Id { get; set; }
    }

    /// <summary>
    ///Belge Detay Sayfası
    /// </summary>
    public class GetDocumentDetailsQueryHandler : IRequestHandler<GetDocumentDetailsQuery, Response<GetDocumentDetailsDto>>
    {
        public async Task<Response<GetDocumentDetailsDto>> Handle(GetDocumentDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetDocumentDetailsDto();
            return Response<GetDocumentDetailsDto>.Success(result, 200);
        }
    }
}
