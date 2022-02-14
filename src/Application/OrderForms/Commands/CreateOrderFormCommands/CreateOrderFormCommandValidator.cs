using FluentValidation;

namespace Application.OrderForms.Commands.CreateOrderFormCommands
{
    public class CreateOrderFormCommandValidator : AbstractValidator<CreateOrderFormCommand>
    {
        public CreateOrderFormCommandValidator()
        {
            RuleFor(v => v.TC).Empty().WithMessage("TCKN girilmelidir.");

            RuleFor(v => v.TC).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");
            RuleFor(v => v.NameAndSurname).Empty().WithMessage("TCKN 11 haneli olmalıdır.");
        }
    }
}
