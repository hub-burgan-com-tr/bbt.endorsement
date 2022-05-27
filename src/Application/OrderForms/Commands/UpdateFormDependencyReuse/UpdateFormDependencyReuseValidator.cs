using FluentValidation;

namespace Application.OrderForms.Commands.UpdateFormDependencyReuse
{
    public class UpdateFormDependencyReuseValidator : AbstractValidator<UpdateFormDependencyReuseCommand>
    {
        public UpdateFormDependencyReuseValidator()
        {
            RuleFor(v => v.TemplateName).NotEmpty().WithMessage("Template Adı girilmelidir.");
            RuleFor(v => v.DependencyReuse).NotEmpty().WithMessage("Yeniden Kullanbilir Alanı girilmelidir.");

        }
    }
}
