using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Endorsements.Commands.NewApprovelOrder
{
    internal class NewApprovalOrderCommandValidator : AbstractValidator<NewApprovalOrderCommand>
    {
        public NewApprovalOrderCommandValidator()
        {
            
        }
    }
}
