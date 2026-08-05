using FluentValidation;
using InvenPilot.Application.Features.Categories.Queries.GetCategoryById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Validators
{
    public class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");
        }
    }
}
