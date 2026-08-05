using FluentValidation;
using InvenPilot.Application.Features.Categories.Commands.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Validators
{
    public class CategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.categoryDTO.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
