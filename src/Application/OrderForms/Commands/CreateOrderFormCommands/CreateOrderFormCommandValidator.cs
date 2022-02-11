using FluentValidation;

namespace Application.OrderForms.Commands.CreateOrderFormCommands
{
    public class CreateOrderFormCommandValidator : AbstractValidator<CreateOrderFormCommand>
    {
        public CreateOrderFormCommandValidator()
        {
            RuleFor(v => v.TC).MaximumLength(11).MinimumLength(11).NotEmpty().WithMessage("");
        }
    }
}
