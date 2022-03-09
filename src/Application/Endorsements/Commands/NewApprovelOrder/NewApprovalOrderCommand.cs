using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Endorsements.Commands.NewApprovelOrder
{
    public class NewApprovalOrderCommand:IRequest<Response<Guid>>
    {
        public Approver Approver { get; set; }
        public List<Document> Documents { get; set; }
        public string Title { get; set; }
        public string Process { get; set; }
        public string Step { get; set; }
        public string ProcessNo { get; set; }
        public string Validity { get; set; }
        public string ReminderFrequency { get; set; }
        public string ReminderCount { get; set; }


    }
    public class NewApprovalOrderCommandHandler : IRequestHandler<NewApprovalOrderCommand, Response<Guid>>
    {
        public async Task<Response<Guid>> Handle(NewApprovalOrderCommand request, CancellationToken cancellationToken)
        {
            var quid = new Guid();
            return Response<Guid>.Success(quid, 200);
        }
    }
    public class Document
    {
        public int DocumentType { get; set; }
        public List<Option> Options { get; set; }
        public List<IFormFile> Files { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FormId { get; set; }
        public string IdentityNo { get; set; }
        public string NameSurname { get; set; }
        public string ChoiceText { get; set; }
    }
    public class Approver
    {
        public int Type { get; set; }
        public string Value { get; set; }
        public string NameSurname { get; set; }
    }

    public class Option
    {
        public string Title { get; set; }
        public string Choice { get; set; }
    }

}
