using FluentValidation;
using InvenPilot.Application.Features.Authentication.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Authentication.Validators
{
    public class LoginValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.loginDTO.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter valid email address");

            RuleFor(x => x.loginDTO.Password)
                .NotEmpty().WithMessage("Email is required");
        }
    }
}
