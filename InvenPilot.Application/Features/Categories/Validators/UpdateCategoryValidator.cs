using FluentValidation;
using InvenPilot.Application.Features.Categories.Commands.UpdateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryQuery>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0);
            RuleFor(x => x.categoryDTO.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
