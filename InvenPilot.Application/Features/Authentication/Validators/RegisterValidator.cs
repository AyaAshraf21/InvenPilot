using FluentValidation;
using InvenPilot.Application.Features.Authentication.Commands;
using InvenPilot.Application.Features.Authentication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Authentication.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.registerDto.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("This is too long name");

            RuleFor(x => x.registerDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter valid email address");

            RuleFor(x => x.registerDto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        }
    }
}
