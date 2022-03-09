using FluentValidation;

namespace Application.OrderForms.Commands.CreateOrderFormCommands
{
    public class CreateOrderFormCommandValidator : AbstractValidator<CreateOrderFormCommand>
    {
        public CreateOrderFormCommandValidator()
        {
            RuleFor(v => v.CitizenShipNumber).Empty().WithMessage("TCKN girilmelidir.");

            RuleFor(v => v.CitizenShipNumber).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");
            RuleFor(v => v.NameAndSurname).Empty().WithMessage("TCKN 11 haneli olmalıdır.");
            RuleFor(v => v.Type).Empty().WithMessage("Lütfen seçim yapınız.");
        }
    }
}
