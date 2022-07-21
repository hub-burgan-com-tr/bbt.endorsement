using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Commands.UpdateParameters
{
    public class UpdateParameterCommandValidator : AbstractValidator<UpdateParameterCommand>
    {
        public UpdateParameterCommandValidator()
        {
            RuleFor(v => v.Text).NotEmpty().WithMessage("Text girilmelidir.");
            RuleFor(v => v.ParameterId).NotEmpty().WithMessage("Parametre Id girilmelidir.");

        }
    }
}
