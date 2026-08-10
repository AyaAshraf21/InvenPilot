using FluentValidation;
using InvenPilot.Application.Common.Pagination;
using InvenPilot.Application.Common.Validators;
using InvenPilot.Application.Features.Orders.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Validators
{
    public class OrderQueryParameterValidator : AbstractValidator<OrderQueryParameter>
    {
        public OrderQueryParameterValidator()
        {
            Include(new BaseQueryParametersValidator());
            RuleFor(x => x.OrderType)
                .IsInEnum()
                .When(x => x.OrderType.HasValue);

            RuleFor(x => x.OrderStatus)
                .IsInEnum()
                .When(x => x.OrderStatus.HasValue);
        }
    }
}
