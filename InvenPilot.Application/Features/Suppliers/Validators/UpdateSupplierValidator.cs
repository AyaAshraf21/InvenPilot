using FluentValidation;
using InvenPilot.Application.Features.Suppliers.Commands.UpdateSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Validators
{
    public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierCommand>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(c => c.supplierDTO.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.supplierDTO.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid Email Address"); 


            RuleFor(c => c.supplierDTO.PhoneNumber)
                .NotEmpty()
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage("Invalid phone number");

            RuleFor(c => c.supplierDTO.Address)
                .MaximumLength(200);
        }
    }
}
