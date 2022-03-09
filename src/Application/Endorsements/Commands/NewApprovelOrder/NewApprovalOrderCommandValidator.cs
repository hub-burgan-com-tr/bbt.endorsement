using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using FluentValidation;

namespace Application.Endorsements.Commands.NewApprovelOrder
{
    public class NewApprovalOrderCommandValidator : AbstractValidator<NewApprovalOrderCommand>
    {
        public NewApprovalOrderCommandValidator()
        {
            RuleFor(v => v.Title).Empty().WithMessage("Başlık girilmelidir.");
            RuleFor(v => v.Process).Empty().WithMessage("İşlem bilgisi girilmelidir.");
            RuleFor(v => v.Step).Empty().WithMessage("Aşama bilgisi girilmelidir.");
            RuleFor(v => v.ProcessNo).Empty().WithMessage("İşlem no bilgisi girilmelidir.");
            RuleFor(v => v.ReminderFrequency).Empty().WithMessage("Hatırlatma frekansı girilmelidir.");
            RuleFor(x => x.Approver.Type == (int)ApproverTypeEnum.CitizenShipNumber).Empty().DependentRules(() => {
                RuleFor(v => v.Approver.Value).Empty().WithMessage("TCKN girilmelidir.");
                RuleFor(v => v.Approver.Value).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");
            });
            RuleFor(x => x.Approver.Type == (int)ApproverTypeEnum.CustomerNumber).Empty().DependentRules(() => {
                RuleFor(v => v.Approver.Value).Empty().WithMessage("Müşteri No girilmelidir.");
            });

            RuleFor(x => x.Documents.Any(x=>x.DocumentType==(int)DocumentTypeEnum.Physically)).NotNull().DependentRules(() => {
                RuleFor(v => v.Documents.Any(x=>x.Files==null)).Empty().WithMessage("Lütfen dosya seçiniz");
            });
            RuleFor(x => x.Documents.Any(x => x.DocumentType == (int)DocumentTypeEnum.Text)).NotNull().DependentRules(() => {
                RuleFor(v => v.Documents.Any(x=>x.Content==null)).Empty().WithMessage("Metin girilmelidir.");
                RuleFor(v => v.Documents.Any(x => x.Title == null)).Empty().WithMessage("Başlık girilmelidir.");
            });
            RuleFor(x => x.Documents.Any(x => x.DocumentType == (int)DocumentTypeEnum.Text)).NotNull().DependentRules(() => {
                RuleFor(v => v.Documents.Any(x => x.Content == null)).Empty().WithMessage("Metin girilmelidir.");
                RuleFor(v => v.Documents.Any(x => x.Title == null)).Empty().WithMessage("Başlık girilmelidir.");
            });
            RuleFor(x => x.Documents.Any(x => x.DocumentType == (int)DocumentTypeEnum.Form)).NotNull().DependentRules(() => {
                RuleFor(v => v.Documents.Any(x=>x.IdentityNo==null)).Empty().WithMessage("TCKN girilmelidir.");
                RuleFor(v => v.Documents.Any(x => x.IdentityNo.Length !=11)).Empty().WithMessage("TCKN 11 haneli olmalıdır.");
                RuleFor(v => v.Documents.Any(x => x.FormId == null)).Empty().WithMessage("Lütfen seçim yapınız");
                RuleFor(v => v.Documents.Any(x => x.NameSurname == null)).Empty().WithMessage("Ad Soyad girilmelidir.");

            });
        }
    }
}
