using FluentValidation;
using InvenPilot.Application.Features.Orders.Commands;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.orderDTO.OrderItems)
                .NotNull()
                .NotEmpty()
                .WithMessage("Order must contain at least one item.");


            RuleFor(x => x.orderDTO.OrderType)
                .IsInEnum()
                .WithMessage("invalid order type.");

            RuleFor(x => x.orderDTO.CustomerID)
                .GreaterThan(0).
                When(x => x.orderDTO.CustomerID.HasValue)
                .WithMessage("Customer ID must be greater than 0.");

            RuleFor(x => x.orderDTO.SupplierID)
                .GreaterThan(0).
                When(x => x.orderDTO.SupplierID.HasValue)
                .WithMessage("Supplier ID must be greater than 0.");

            RuleForEach(x => x.orderDTO.OrderItems)
                .ChildRules(item =>
                {
                    item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than 0.");

                    item.RuleFor(i => i.ProductID)
                    .GreaterThan(0)
                    .WithMessage("Product ID must be greater than 0");
                });

            RuleFor(x => x.orderDTO.OrderItems)
                .Must(items => items.Select(i => i.ProductID).Distinct().Count() == items.Count)
                .WithMessage("An order cannot contain the same product more than once.");
             }
    }
}
