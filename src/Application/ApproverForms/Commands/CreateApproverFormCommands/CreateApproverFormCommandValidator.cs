using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using FluentValidation;

namespace Application.ApproverForms.Commands.CreateApproverFormCommands
{
    public class CreateApproverFormCommandValidator:AbstractValidator<CreateApproverFormCommand>
    {
        public CreateApproverFormCommandValidator()
        {

            RuleFor(x => x.CitizenShipNumber).NotNull().DependentRules(() => {
                RuleFor(v => v.CitizenShipNumber).Empty().WithMessage("TCKN girilmelidir.");
                RuleFor(v => v.CitizenShipNumber).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");
            });
            RuleFor(x => x.CustomerNumber).NotNull().DependentRules(() => {
                RuleFor(v => v.CustomerNumber).Empty().WithMessage("Müşteri No girilmelidir.");
            });
            
        }
    }
}
