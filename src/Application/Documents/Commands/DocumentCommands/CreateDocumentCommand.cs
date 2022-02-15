using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Documents.Commands.DocumentCommands
{
    public class CreateDocumentCommand
    {
        public int Type { get; set; }
        public string Name { get; set; }

        public bool IsDocumentApproved { get; set; }

        public string Content { get; set; }
        public int ApprovalId { get; set; }
        public string CitizenShipNumber { get; set; }
        public string NameAndSurname { get; set; }

    }

    //public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Response<List<CreateDocumentCommandDto>>>
    //{
    //    public async Task<Response<List<CreateDocumentCommandDto>>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    //    {
    //        var list = new List<CreateDocumentCommandDto>();
    //        return Response<List<CreateDocumentCommandDto>>.Success(list, 200);
    //    }
    //}
}
