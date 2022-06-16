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
            RuleFor(v => v.Name).NotEmpty().WithMessage("Adı girilmelidir.");
            RuleFor(v => v.TemplateName).NotEmpty().WithMessage("Template Adı girilmelidir.");
            RuleFor(v => v.ExpireInMinutes).NotEmpty().WithMessage("Geçerlilik girilmelidir.");
            RuleFor(v => v.RetryFrequence).NotEmpty().WithMessage("Hatırlatma frekansı girilmelidir.");
            RuleFor(v => v.MaxRetryCount).NotEmpty().WithMessage("Hatırlatma Sayısı girilmelidir.");
        }
    }
}
