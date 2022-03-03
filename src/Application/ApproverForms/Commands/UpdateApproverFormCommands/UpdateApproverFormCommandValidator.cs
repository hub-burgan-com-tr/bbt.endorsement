using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.ApproverForms.Commands.UpdateApproverFormCommands
{
    public class UpdateApproverFormCommandValidator : AbstractValidator<UpdateApproverFormCommand>
    {
        public UpdateApproverFormCommandValidator()
        {

            RuleFor(x => x.CitizenShipNumber).NotNull().DependentRules(() =>
            {
                RuleFor(v => v.CitizenShipNumber).Empty().WithMessage("TCKN girilmelidir.");
                RuleFor(v => v.CitizenShipNumber).MaximumLength(11).MinimumLength(11)
                    .WithMessage("TCKN 11 haneli olmalıdır.");
            });
            RuleFor(x => x.CustomerNumber).NotNull().DependentRules(() =>
            {
                RuleFor(v => v.CustomerNumber).Empty().WithMessage("Müşteri No girilmelidir.");
            });

        }
    }
}
