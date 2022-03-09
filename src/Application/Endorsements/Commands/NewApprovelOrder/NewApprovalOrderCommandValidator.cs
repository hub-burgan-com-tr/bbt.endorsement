using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
