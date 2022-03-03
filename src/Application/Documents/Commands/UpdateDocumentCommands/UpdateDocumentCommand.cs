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
    public class UpdateDocumentCommand :CreateDocumentCommand, IRequest<Response<int>>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
       
    }
    /// <summary>
    /// Belge Guncelleme
    /// </summary>
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, Response<int>>
    {
        private readonly IApplicationDbContext _context;
        public UpdateDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<int>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            return Response<int>.Success(1, 200);
        }

    }

}
