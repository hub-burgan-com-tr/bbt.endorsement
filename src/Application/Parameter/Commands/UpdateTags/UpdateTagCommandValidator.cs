using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Commands.UpdateTags
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(v => v.Tag).NotEmpty().WithMessage("Form Türü girilmelidir.");
            RuleFor(v => v.FormDefinitionTagId).NotEmpty().WithMessage("Form Türü Id girilmelidir.");
            RuleFor(v => v.IsProcessNo).NotEmpty().WithMessage("İşlem No  girilmelidir.");


        }
    }
}
