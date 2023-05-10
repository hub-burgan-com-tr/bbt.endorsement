using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Commands.CreateFormInformations
{
    public class CreateFormInformationCommandValidator : AbstractValidator<CreateFormInformationCommand>
    {
 
        public CreateFormInformationCommandValidator()
        {
            RuleFor(v => v.FormDefinitionTagId).NotEmpty().WithMessage("Form Türü  girilmelidir.");
            RuleFor(v => v.ParameterId).NotEmpty().WithMessage("Parametre Adı girilmelidir.");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Form Adı girilmelidir.");
            RuleFor(v => v.TemplateName).NotEmpty().WithMessage("Template Adı girilmelidir.");
            RuleFor(v => v.ExpireInMinutes).NotEmpty().WithMessage("Geçerlilik girilmelidir.");
            RuleFor(v => v.RetryFrequence).NotEmpty().WithMessage("Hatırlatma frekansı girilmelidir.");
            RuleFor(v => v.MaxRetryCount).NotEmpty().WithMessage("Hatırlatma Sayısı girilmelidir.");
            RuleFor(v => v.SemanticVersion).NotEmpty().WithMessage("Version  girilmelidir.");
            RuleFor(v => v.Json).NotEmpty().WithMessage("Formio json girilmelidir.");
            RuleFor(v => v.HtmlTemplate).NotEmpty().WithMessage("Html girilmelidir.");

        }
    }
}
