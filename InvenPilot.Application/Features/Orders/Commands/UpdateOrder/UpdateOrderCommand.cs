using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(int id, OrderStatus orderStatus) : IRequest<OrderResponseDTO>;
}
