using Application.Approvals.Queries.CreateFormCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApproverForms.Commands.CreateFormCommands
{
    public class CreateOrderFormCommandValidator : AbstractValidator<CreateOrderFormCommand>
    {
        public CreateOrderFormCommandValidator()
        {
            RuleFor(v => v.TC).MaximumLength(11).MinimumLength(11).NotEmpty().WithMessage("");
        }
    }
}
