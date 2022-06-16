using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;


namespace Application.OrderForms.Commands.UpdateFormInformations
{
    public class UpdateFormInformationCommand : IRequest<Response<bool>>
    {
        public string TemplateName { get; set; }
        public int MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }
    }
    public class UpdateFormInformationCommandHandler : IRequestHandler<UpdateFormInformationCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateFormInformationCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(UpdateFormInformationCommand request, CancellationToken cancellationToken)
        {
            UpdateFormInformationCommandValidator validator = new UpdateFormInformationCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            int result = 0;
          var form = _context.FormDefinitions.FirstOrDefault(x=>x.TemplateName==request.TemplateName.Trim());
            if (form!=null)
            {
                form.ExpireInMinutes = request.ExpireInMinutes;
                form.MaxRetryCount = request.MaxRetryCount;
                form.RetryFrequence = request.RetryFrequence;
                form.Created = _dateTime.Now;
                _context.FormDefinitions.Update(form);
                result = _context.SaveChanges();
            }

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }

}
