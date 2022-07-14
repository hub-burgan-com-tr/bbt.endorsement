using FluentValidation;

namespace Application.Parameter.Commands.CreateTags
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(v => v.Tag).NotEmpty().WithMessage("Form Türü girilmelidir.");

        }


    }
}
