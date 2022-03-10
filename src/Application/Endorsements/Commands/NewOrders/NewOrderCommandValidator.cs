using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using FluentValidation;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommandValidator : AbstractValidator<StartRequest>
    {
        public NewOrderCommandValidator()
        {
            RuleFor(v => v.Title).Empty().WithMessage("Başlık girilmelidir.");
            RuleFor(v => v.Reference.Process).Empty().WithMessage("İşlem bilgisi girilmelidir.");
            RuleFor(v => v.Reference.State).Empty().WithMessage("Aşama bilgisi girilmelidir.");
            RuleFor(v => v.Reference.ProcessNo).Empty().WithMessage("İşlem no bilgisi girilmelidir.");
            RuleFor(v => v.Config.RetryFrequence).Empty().WithMessage("Hatırlatma frekansı girilmelidir.");
            RuleFor(x => x.Documents.Any(x => x.DocumentType == (int)DocumentTypeEnum.Physically)).NotNull().DependentRules(() => {
                RuleFor(v => v.Documents.Any(x => x.Files == null)).Empty().WithMessage("Lütfen dosya seçiniz");
            });
            RuleFor(x => x.Documents.Any(x => x.DocumentType == (int)DocumentTypeEnum.Text)).NotNull().DependentRules(() => {
                RuleFor(v => v.Documents.Any(x => x.Content == null)).Empty().WithMessage("Metin girilmelidir.");
                RuleFor(v => v.Documents.Any(x => x.Title == null)).Empty().WithMessage("Başlık girilmelidir.");
            });
        }
    }
}
