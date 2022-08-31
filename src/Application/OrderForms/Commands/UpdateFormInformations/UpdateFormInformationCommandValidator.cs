using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Commands.UpdateFormInformations
{
    public class UpdateFormInformationCommandValidator : AbstractValidator<UpdateFormInformationCommand>
    {
        public UpdateFormInformationCommandValidator()
        {
            RuleFor(v => v.FormDefinitionId).NotEmpty().WithMessage("Form Id  girilmelidir.");
            RuleFor(v => v.FormDefinitionTagId).NotEmpty().WithMessage("Form Türü  girilmelidir.");
            RuleFor(v => v.ParameterId).NotEmpty().WithMessage("Parametre Adı girilmelidir.");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Form Adı girilmelidir.");
            RuleFor(v => v.TemplateName).NotEmpty().WithMessage("Template Adı girilmelidir.");
            RuleFor(v => v.ExpireInMinutes).NotEmpty().WithMessage("Geçerlilik girilmelidir.");
            RuleFor(v => v.RetryFrequence).NotEmpty().WithMessage("Hatırlatma frekansı girilmelidir.");
            RuleFor(v => v.MaxRetryCount).NotEmpty().WithMessage("Hatırlatma Sayısı girilmelidir.");
        }
    }
}
