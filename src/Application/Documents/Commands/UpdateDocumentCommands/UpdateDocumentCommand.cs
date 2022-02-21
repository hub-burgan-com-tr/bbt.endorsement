using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Documents.Commands.CreateDocumentCommands;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Commands.UpdateDocumentCommands
{
    public class UpdateDocumentCommand :CreateDocumentCommand, IRequest<Response<List<UpdateDocumentCommandDto>>>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
       
    }
    /// <summary>
    /// Belge Guncelleme
    /// </summary>
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, Response<List<UpdateDocumentCommandDto>>>
    {
        private readonly IApplicationDbContext _context;
        public UpdateDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<UpdateDocumentCommandDto>>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var list = new List<UpdateDocumentCommandDto>();
            return Response<List<UpdateDocumentCommandDto>>.Success(list, 200);
        }

    }

}
