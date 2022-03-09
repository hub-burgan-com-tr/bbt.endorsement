using Domain.Enum;
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

              RuleFor(x => x.Type==(int)ApproverTypeEnum.CitizenShipNumber).Empty().DependentRules(() => {
                RuleFor(v => v.Value).Empty().WithMessage("TCKN girilmelidir.");
                RuleFor(v => v.Value).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");
            });
              RuleFor(x => x.Type == (int)ApproverTypeEnum.CustomerNumber).Empty().DependentRules(() => {
                  RuleFor(v => v.Value).Empty().WithMessage("Müşteri No girilmelidir.");
                 
              });

        }
    }
}
