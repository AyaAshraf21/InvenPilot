using FluentValidation;
using InvenPilot.Application.Features.Customers.Commands.UpdateCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Customers.Validators
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(c => c.customerDTO.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.customerDTO.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid Email Address");


            RuleFor(c => c.customerDTO.PhoneNumber)
                .NotEmpty()
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage("Invalid phone number");

            RuleFor(c => c.customerDTO.Address)
                .MaximumLength(200);
        }
    }
}
