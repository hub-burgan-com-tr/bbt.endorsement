using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Commands.NewTagForms
{
    public class NewTagCommand : IRequest<Response<bool>>
    {
        public string Tag { get; set; }
    }

    public class NewTagCommandHandler : IRequestHandler<NewTagCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public NewTagCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(NewTagCommand request, CancellationToken cancellationToken)
        {
            NewTagCommandValidator validator = new NewTagCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            var IsTag = _context.FormDefinitionTags.Any(x => x.Tag == request.Tag);
            if (IsTag)
            {
                throw new Exception("Aynı Etiket Daha Önce Eklenmiştir");
            }
            int result = 0;
            _context.FormDefinitionTags.Add(new Domain.Entities.FormDefinitionTag { FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = request.Tag,Created= _dateTime.Now});         
            result = _context.SaveChanges();
            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }

}
