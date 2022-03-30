using FluentValidation;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommandValidator : AbstractValidator<NewOrderCommand>
    {
        public NewOrderCommandValidator()
        {
            RuleFor(v => v.StartRequest).SetValidator(new StartRequestValidator());
            RuleFor(model => model.StartRequest.Documents)
           .Must(collection => collection.All(item => item.Type.ToString() == ContentType.PDF.ToString() && !string.IsNullOrEmpty(item.Content)))
           .WithMessage("Lütfen dosya seçiniz");
            RuleFor(model => model.StartRequest.Documents)
           .Must(collection => collection.All(item => item.Type.ToString() == ContentType.PlainText.ToString() && !string.IsNullOrEmpty(item.Content)))
           .WithMessage("Metin girilmelidir.");
            RuleFor(model => model.StartRequest.Documents)
           .Must(collection => collection.All(item => item.Type.ToString() == (ContentType.PlainText).ToString() && string.IsNullOrEmpty(item.Title)))
           .WithMessage("Başlık girilmelidir.");
            RuleFor(model => model.StartRequest.Documents)
         .Must(collection => collection.All(item => item.Actions.All(item2 => string.IsNullOrEmpty(item2.Title))))
         .WithMessage("Lütfen seçenek ekleyiniz.");
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
            RuleFor(v => v.Approver.First).NotEmpty().WithMessage("Ad girilmelidir.");
            RuleFor(v => v.Approver.Last).NotEmpty().WithMessage("Soyad girilmelidir.");
            RuleFor(v => v.Approver.CitizenshipNumber.ToString()).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");



        }
    }
}