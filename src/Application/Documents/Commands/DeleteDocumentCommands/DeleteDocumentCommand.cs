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
    public class DeleteDocumentCommand : IRequest<Response<int>>
    { 
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
    }

    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Response<int>>
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
        public async Task<Response<int>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            return Response<int>.Success(1, 200);
        }

    }

}
