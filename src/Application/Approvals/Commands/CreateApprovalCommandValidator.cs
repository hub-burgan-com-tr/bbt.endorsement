using FluentValidation;

namespace Application.Approvals.Commands
{
    public class CreateApprovalCommandValidator : AbstractValidator<CreateApprovalCommand>
    {
        public CreateApprovalCommandValidator()
        {
            RuleFor(v => v.InstanceId).NotEmpty();

            RuleFor(v => v.ApprovalTitle).NotEmpty();
        }
    }
}
