using FluentValidation;

namespace Application.Parameter.Commands.CreateTags
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(v => v.Tag).NotEmpty().WithMessage("Form Türü girilmelidir.");
            RuleFor(v => v.IsProcessNo).Must(x => x == false || x == true);

        }


    }
}
