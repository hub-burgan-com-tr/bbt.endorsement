using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace Application.Documents.Commands.CreateDocumentCommands
{
    public class CreateDocumentCommand : IRequest<Response<int>>
    {
        /// <summary>
        /// Dosya
        /// </summary>
        public IFormFile File { get; set; }
        /// <summary>
        /// OnayId
        /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// Belge Tipi
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Belge Onaylı Mı
        /// </summary>
        public bool IsDocumentApproved { get; set; }
        /// <summary>
        /// Baslik
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Metin
        /// </summary>
        public string Content { get; set; }
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

        public List<CreateDocumentCommandApprovedDto> CreateDocumentCommandApprovedDto { get; set; }

    }
    public class CreateDocumentCommandApprovedDto
    {
        public string ApprovedName { get; set; }
        public bool IsApproved { get; set; }
    }
    /// <summary>
    /// Belge Ekleme Servisi
    /// </summary>
    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Response<int>>
    {
        private readonly IApplicationDbContext _context;
        public CreateDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<int>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            return Response<int>.Success(1, 200);
        }
       
    }

  
}
