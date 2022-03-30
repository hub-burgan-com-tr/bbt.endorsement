using FluentValidation;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommandValidator : AbstractValidator<NewOrderCommand>
    {
        public NewOrderCommandValidator()
        {
            RuleFor(v => v.StartRequest).SetValidator(new StartRequestValidator());

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

    public class StartRequestValidator : AbstractValidator<StartRequest>
    {
        public StartRequestValidator()
        {
            RuleFor(v => v.Title).NotEmpty().WithMessage("Başlık girilmelidir.");
            RuleFor(v => v.Reference.Process).NotEmpty().WithMessage("İşlem bilgisi girilmelidir.");
            RuleFor(v => v.Reference.State).NotEmpty().WithMessage("Aşama bilgisi girilmelidir.");
            RuleFor(v => v.Reference.ProcessNo).NotEmpty().WithMessage("İşlem no bilgisi girilmelidir.");
            RuleFor(v => v.Config.RetryFrequence).NotEmpty().WithMessage("Hatırlatma frekansı girilmelidir.");
        }
    }
}