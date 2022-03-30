using FluentValidation;

namespace Application.Endorsements.Commands.NewOrderForms
{
    public class NewFormOrderCommandValidator : AbstractValidator<NewOrderFormCommand>
    {
        public NewFormOrderCommandValidator()
        {
            RuleFor(v => v.Request.Title).Empty().WithMessage("Başlık girilmelidir.");
            RuleFor(v => v.Request.Reference.Process).Empty().WithMessage("İşlem bilgisi girilmelidir.");
            RuleFor(v => v.Request.Reference.State).Empty().WithMessage("Aşama bilgisi girilmelidir.");
            RuleFor(v => v.Request.Reference.ProcessNo).Empty().WithMessage("İşlem no bilgisi girilmelidir.");

        }
    }
}
