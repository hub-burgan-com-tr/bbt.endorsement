using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Documents.Commands.CreateDocumentCommands;
using MediatR;

namespace Application.Documents.Commands.DeleteDocumentCommands
{
    public class DeleteDocumentCommand : IRequest<Response<List<DeleteDocumentCommandDto>>>
    { 
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
    }

    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Response<List<DeleteDocumentCommandDto>>>
    {
        private readonly IApplicationDbContext _context;
        public DeleteDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Belge Silme
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<List<DeleteDocumentCommandDto>>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var list = new List<DeleteDocumentCommandDto>();
            return Response<List<DeleteDocumentCommandDto>>.Success(list, 200);
        }

    }

}
