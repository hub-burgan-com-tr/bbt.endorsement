using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Commands.UpdateTags
{
    public class UpdateTagCommand : IRequest<Response<bool>>
    {
        public string Tag { get; set; }
        public bool IsProcessNo { get; set; }

        public string FormDefinitionTagId { get; set; }
    }

    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateTagCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            UpdateTagCommandValidator validator = new UpdateTagCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            var parameter = _context.FormDefinitionTags.FirstOrDefault(x => x.FormDefinitionTagId == request.FormDefinitionTagId);
            if (parameter!=null)
            {
                var IsText = _context.FormDefinitionTags.Any(x => x.Tag == request.Tag && x.FormDefinitionTagId != parameter.FormDefinitionTagId);

                if (IsText)
                    throw new Exception("Aynı Form Türü Daha Önce Eklenmiştir");
            }
            int result = 0;
            if (parameter != null)
            {
                parameter.Tag = request.Tag;
                parameter.Created = DateTime.Now;
                parameter.IsProcessNo = request.IsProcessNo;
                _context.FormDefinitionTags.Update(parameter);
                result = _context.SaveChanges();
            }

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }


}
