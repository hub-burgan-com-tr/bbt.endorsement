using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Commands.CreateTags
{
    public class CreateTagCommand : IRequest<Response<bool>>
    {
        public string Tag { get; set; }
        public bool IsProcessNo { get; set; }
    }
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateTagCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            CreateTagCommandValidator validator = new CreateTagCommandValidator();
            var response = validator.Validate(request);
            validator.ValidateAndThrow(request);
            var IsText = _context.FormDefinitionTags.Any(x => x.Tag == request.Tag.Trim());
            if (IsText)
                throw new Exception("Aynı Form Türü Daha Önce Eklenmiştir");
            int result = 0;
            var parameter = _context.FormDefinitionTags.Add(new Domain.Entities.FormDefinitionTag
            {
                FormDefinitionTagId=Guid.NewGuid().ToString(),
                IsProcessNo=request.IsProcessNo,
                Tag=request.Tag.Trim(),
                Created = DateTime.Now,
            });

            result = _context.SaveChanges();
            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }

}
