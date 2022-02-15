using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace Application.Documents.Commands.CreateDocumentCommands
{
    public class CreateDocumentCommand : IRequest<Response<List<CreateDocumentCommandDto>>>
    {
        public IFormFile File { get; set; }
        public int ApprovalId { get; set; }
        public int Type { get; set; }
        public bool IsDocumentApproved { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int FormId { get; set; }
        public string CitizenShipNumber { get; set; }
        public string NameAndSurname { get; set; }

    }
    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Response<List<CreateDocumentCommandDto>>>
    {
        private readonly IApplicationDbContext _context;
        public CreateDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<CreateDocumentCommandDto>>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var list = new List<CreateDocumentCommandDto>();
            return Response<List<CreateDocumentCommandDto>>.Success(list, 200);
        }
       
    }

  
}
