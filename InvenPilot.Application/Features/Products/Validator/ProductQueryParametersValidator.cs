using FluentValidation;
using InvenPilot.Application.Common.Pagination;
using InvenPilot.Application.Common.Validators;
using InvenPilot.Application.Features.Products.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Validator
{
    public class ProductQueryParametersValidator : AbstractValidator<ProductQueryParameters>
    {
        public ProductQueryParametersValidator()
        {
            Include(new BaseQueryParametersValidator());

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice!.Value)
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

            RuleFor(x => x.MinQuantity)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinQuantity.HasValue);

            RuleFor(x => x.MaxQuantity)
                .GreaterThanOrEqualTo(x => x.MinQuantity!.Value)
                .When(x => x.MinQuantity.HasValue && x.MaxQuantity.HasValue);

            

            
        }
            
    }
}
