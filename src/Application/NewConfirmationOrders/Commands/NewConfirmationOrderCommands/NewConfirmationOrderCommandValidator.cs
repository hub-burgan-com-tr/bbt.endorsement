using FluentValidation;

namespace Application.NewConfirmationOrders.Commands.NewConfirmationOrderCommands
{
    public class NewConfirmationOrderCommandValidator : AbstractValidator<NewConfirmationOrderCommand>
    {
        public NewConfirmationOrderCommandValidator()
        {
            RuleFor(v => v.Title).Empty().WithMessage("Başlık girilmelidir.");
            RuleFor(v => v.Process).Empty().WithMessage("İşlem bilgisi girilmelidir.");
            RuleFor(v => v.Stage).Empty().WithMessage("Aşama bilgisi girilmelidir.");
            RuleFor(v => v.TransactionNumber).Empty().WithMessage("İşlem no bilgisi girilmelidir.");
            RuleFor(v => v.RetryFrequence).Empty().WithMessage("Hatırlatma frekansı girilmelidir.");
           

        }
    }
}
