using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Commands.NewTagForms
{
    public class NewTagCommandValidator : AbstractValidator<NewTagCommand>
    {
        public NewTagCommandValidator()
        {
            RuleFor(v => v.Tag).NotEmpty().WithMessage("Etiket girilmelidir.");
           
        }
    }
}
