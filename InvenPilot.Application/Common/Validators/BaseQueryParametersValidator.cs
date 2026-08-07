using FluentValidation;
using InvenPilot.Application.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Common.Validators
{
    public class BaseQueryParametersValidator : AbstractValidator<BaseQueryParamerters>
    {
        public BaseQueryParametersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0);

            RuleFor(x => x.PerPage)
                .InclusiveBetween(1, 100);
            
            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search));

        }
    }
}
