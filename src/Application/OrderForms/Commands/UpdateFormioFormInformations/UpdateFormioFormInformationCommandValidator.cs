using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Commands.UpdateFormioFormInformations
{
    public class UpdateFormioFormInformationCommandValidator : AbstractValidator<UpdateFormioFormInformationCommand>
    {
        public UpdateFormioFormInformationCommandValidator()
        {
            RuleFor(v => v.FormDefinitionId).NotEmpty().WithMessage("Form Id  girilmelidir.");
            RuleFor(v => v.SemanticVersion).NotEmpty().WithMessage("Versiyon  girilmelidir.");
            RuleFor(v => v.Json).NotEmpty().WithMessage("Formio json girilmelidir.");
            RuleFor(v => v.HtmlTemplate).NotEmpty().WithMessage("Html girilmelidir.");

        }
    }
}
