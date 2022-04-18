using Domain.Enums;
using Domain.Models;
using FluentValidation;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommandValidator : AbstractValidator<NewOrderCommand>
    {
        public NewOrderCommandValidator()
        {

            RuleFor(v => v.StartRequest).SetValidator(new StartRequestValidator());
            RuleFor(x => x.StartRequest.Documents).NotEmpty().WithMessage("Belge eklemeden ilerleyemezsiniz.");

            RuleForEach(x => x.StartRequest.Documents).Where(x => x.Type == (int)ContentType.File).ChildRules(y =>
            {
                y.RuleFor(z => z.Content).NotEmpty().WithMessage("Lütfen dosya seçiniz");

            });

            RuleForEach(x => x.StartRequest.Documents).ChildRules(y => y.RuleFor(z => z.Actions).NotEmpty().WithMessage("Lütfen seçenek ekleyiniz."));

            RuleForEach(x => x.StartRequest.Documents).Where(x => x.Type == (int)ContentType.PlainText).ChildRules(y =>
            {
                y.RuleFor(z => z.Content).NotEmpty().WithMessage("Metin Girilmelidir");
            });

            RuleForEach(x => x.StartRequest.Documents).Where(x => x.Type == (int)ContentType.PlainText).ChildRules(y =>
            {
                y.RuleFor(z => z.Title).NotEmpty().WithMessage("Başlık girilmelidir.");
            });

        }
    }

    public class StartRequestValidator : AbstractValidator<StartRequest>
    {
        public StartRequestValidator()
        {
            RuleFor(v => v.Title).NotEmpty().WithMessage("Başlık girilmelidir.");
            RuleFor(v => v.Reference.Process).NotEmpty().WithMessage("İşlem bilgisi girilmelidir.");
            RuleFor(v => v.Reference.ProcessNo).NotEmpty().WithMessage("İşlem no bilgisi girilmelidir.");
            RuleFor(v => v.Config.ExpireInMinutes).NotEmpty().WithMessage("Geçerlilik girilmelidir.");
            RuleFor(v => v.Config.RetryFrequence).NotEmpty().WithMessage("Hatırlatma frekansı girilmelidir.");
            RuleFor(v => v.Config.MaxRetryCount).NotEmpty().WithMessage("Hatırlatma Sayısı girilmelidir.");
            RuleFor(v => v.Approver.First).NotEmpty().WithMessage("Ad girilmelidir.");
            RuleFor(v => v.Approver.Last).NotEmpty().WithMessage("Soyad girilmelidir.");
            RuleFor(v => v.Approver.CitizenshipNumber.ToString()).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");



        }
    }
}