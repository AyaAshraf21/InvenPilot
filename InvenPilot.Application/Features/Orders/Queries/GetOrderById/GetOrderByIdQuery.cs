using InvenPilot.Application.Features.Orders.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(int id) : IRequest<OrderResponseDTO>;
}
