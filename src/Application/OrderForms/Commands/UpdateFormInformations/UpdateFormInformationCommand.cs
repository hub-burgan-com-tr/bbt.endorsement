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
        public string FormDefinitionId { get; set; }

        public string FormDefinitionTagId { get; set; }
        public string ParameterId { get; set; }
        public string Name { get; set; }
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
          var form = _context.FormDefinitions.FirstOrDefault(x=>x.FormDefinitionId==request.FormDefinitionId);
                if(form==null)
                return Response<bool>.NotFoundException("Form bulunamadı", 200);
            var formDefinitionTag = _context.FormDefinitionTags.FirstOrDefault(x => x.FormDefinitionTagId == request.FormDefinitionTagId);
            if (formDefinitionTag == null)
                return Response<bool>.NotFoundException("Form Türü bulunamadı", 200);
            var tagmap = _context.FormDefinitionTagMaps.FirstOrDefault(x => x.FormDefinitionId == request.FormDefinitionId);
            if (tagmap.FormDefinitionTagId!=request.FormDefinitionTagId)
            {
                tagmap.FormDefinitionTagId = request.FormDefinitionTagId;
                _context.FormDefinitionTagMaps.Update(tagmap);
            }

            var parametre = _context.Parameters.FirstOrDefault(x => x.ParameterId == request.ParameterId);
            if (parametre == null)
                return Response<bool>.NotFoundException("parametre bulunamadı", 200);
            var templateName = request.TemplateName + "-" + parametre.Text;
            var name = request.Name + " - " + parametre.Text;
            if (form!=null)
            {
                form.ExpireInMinutes = request.ExpireInMinutes;
                form.MaxRetryCount = request.MaxRetryCount;
                form.RetryFrequence = request.RetryFrequence;
                form.ParameterId = request.ParameterId;
                form.Name = name;
                form.TemplateName = templateName;
                form.Created = _dateTime.Now;
                _context.FormDefinitions.Update(form);
                result = _context.SaveChanges();
            }

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }

}
