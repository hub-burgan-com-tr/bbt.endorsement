using FluentValidation;

namespace Application.Approvals.Commands.CreateApprovalCommands
{
    public class CreateApprovalCommandValidator : AbstractValidator<CreateApprovalCommand>
    {
        public CreateApprovalCommandValidator()
        {
            RuleFor(v => v.InstanceId).NotEmpty();
        }
    }
}
