﻿using Application.Endorsements.Commands.NewOrders;
using Domain.Models;
using FluentValidation;

namespace Application.Endorsements.Commands.NewOrderForms
{
    public class NewOrderFormCommandValidator : AbstractValidator<NewOrderFormCommand>
    {
        public NewOrderFormCommandValidator()
        {
            RuleFor(v => v.Request).SetValidator(new StartFormRequestValidator());
        }

        public class StartFormRequestValidator : AbstractValidator<StartFormRequest>
        {
            public StartFormRequestValidator()
            {
                RuleFor(v => v.Title).NotEmpty().WithMessage("Başlık girilmelidir.");
                RuleFor(v => v.Approver.First).NotEmpty().WithMessage("Ad girilmelidir.");
                RuleFor(v => v.Approver.Last).NotEmpty().WithMessage("Soyad girilmelidir.");
                RuleFor(v => v.Approver.CitizenshipNumber.ToString()).MaximumLength(11).MinimumLength(11).WithMessage("TCKN 11 haneli olmalıdır.");
                

            }
        }

    }
}
