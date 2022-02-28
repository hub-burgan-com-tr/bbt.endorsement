using Domain.Enum;
using FluentValidation;

namespace Application.Documents.Commands.UpdateDocumentCommands
{
    public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
    {
        public UpdateDocumentCommandValidator()
        {
            RuleFor(x => x.Type == (int)DocumentTypeEnum.Physically).NotNull().DependentRules(() => {
                RuleFor(v => v.File).Null().WithMessage("Lütfen dosya seçiniz");
            });
            RuleFor(x => x.Type == (int)DocumentTypeEnum.Text).NotNull().DependentRules(() => {
                RuleFor(v => v.Title).Empty().WithMessage("Başlık girilmelidir.");
                RuleFor(v => v.Content).Null().WithMessage("Metin girilmelidir.");

            });
            RuleFor(x => x.Type == (int)DocumentTypeEnum.Form).NotNull().DependentRules(() => {
                RuleFor(v => v.FormId).Empty().WithMessage("Lütfen seçim yapınız");
                RuleFor(v => v.CitizenShipNumber).Null().WithMessage("TCKN girilmelidir.");
                RuleFor(v => v.NameAndSurname).Null().WithMessage("Ad Soyad girilmelidir.");

            });
        }
    }

}
