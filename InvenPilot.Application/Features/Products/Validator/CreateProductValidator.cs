using FluentValidation;
using InvenPilot.Application.Features.Products.Commands.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Validator
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
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
