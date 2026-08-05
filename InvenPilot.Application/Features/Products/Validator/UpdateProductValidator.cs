using FluentValidation;
using InvenPilot.Application.Features.Products.Commands.UpdateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Validator
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.productDTO.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(p => p.productDTO.Description)
                .MaximumLength(200);

            RuleFor(p => p.productDTO.Price)
                .GreaterThan(0);

            RuleFor(p => p.productDTO.Quantity)
                .GreaterThan(0);

            RuleFor(p => p.productDTO.CategoryId)
                .GreaterThan(0);
        }
    }
}
