using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Commands.UpdateFormDependencyReuse
{
    public class UpdateFormDependencyReuseCommand : IRequest<Response<bool>>
    {
        public string TemplateName { get; set; }
        public bool? DependencyReuse { get; set; }

    }

    public class UpdateFormDependencyReuseCommandHandler : IRequestHandler<UpdateFormDependencyReuseCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateFormDependencyReuseCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(UpdateFormDependencyReuseCommand request, CancellationToken cancellationToken)
        {
            UpdateFormDependencyReuseValidator validator = new UpdateFormDependencyReuseValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            int result = 0;
            var form = _context.FormDefinitions.FirstOrDefault(x => x.TemplateName == request.TemplateName.Trim());
            if (form != null)
            {
                form.DependecyReuse = request.DependencyReuse;
                form.Created = _dateTime.Now;
                _context.FormDefinitions.Update(form);
                result = _context.SaveChanges();
            }

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }



}
