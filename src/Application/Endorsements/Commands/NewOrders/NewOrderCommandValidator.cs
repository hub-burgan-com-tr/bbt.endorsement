using FluentValidation;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommandValidator : AbstractValidator<NewOrderCommand>
    {
        public NewOrderCommandValidator()
        {
            RuleFor(v => v.StartRequest.Title).Empty().WithMessage("Başlık girilmelidir.");
            RuleFor(v => v.StartRequest.Reference.Process).Empty().WithMessage("İşlem bilgisi girilmelidir.");
            RuleFor(v => v.StartRequest.Reference.State).Empty().WithMessage("Aşama bilgisi girilmelidir.");
            RuleFor(v => v.StartRequest.Reference.ProcessNo).Empty().WithMessage("İşlem no bilgisi girilmelidir.");
            RuleFor(v => v.StartRequest.Config.RetryFrequence).Empty().WithMessage("Hatırlatma frekansı girilmelidir.");
            RuleFor(x => x.StartRequest.Documents.Any(x => x.Type.ToString() == ContentType.PDF.ToString())).NotNull().DependentRules(() =>
            {
                RuleFor(v => v.StartRequest.Documents.Any(x => x.Content == null)).Empty().WithMessage("Lütfen dosya seçiniz");
            });
            RuleFor(x => x.StartRequest.Documents.Any(x => x.Type.ToString() == ContentType.PlainText.ToString())).NotNull().DependentRules(() =>
            {
                RuleFor(v => v.StartRequest.Documents.Any(x => x.Content == null)).Empty().WithMessage("Metin girilmelidir.");
                RuleFor(v => v.StartRequest.Documents.Any(x => x.Title == null)).Empty().WithMessage("Başlık girilmelidir.");
            });
        }
    }
}
