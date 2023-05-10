using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Commands.CreateParameters
{
    public class CreateParameterCommandValidator : AbstractValidator<CreateParameterCommand>
    {
        public CreateParameterCommandValidator()
        {
            RuleFor(v => v.ParameterTypeId).NotEmpty().WithMessage("Parametre Tipi girilmelidir.");
            RuleFor(v => v.DmsReferenceId).NotEmpty().WithMessage("DmsReferenceId girilmelidir.");
            RuleFor(v => v.Text).NotEmpty().WithMessage("Text girilmelidir.");
          
        }
    }
}
