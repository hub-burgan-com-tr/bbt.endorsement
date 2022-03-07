using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommandValidator : AbstractValidator<StartRequest>
    {
        public NewOrderCommandValidator()
        {
           
        }
    }
}
