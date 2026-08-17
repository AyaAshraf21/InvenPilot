using InvenPilot.Application.Common.Authorization;
using InvenPilot.Application.Features.Orders.Commands;
using InvenPilot.Application.Features.Orders.Commands.CreateOrder;
using InvenPilot.Application.Features.Orders.Commands.UpdateOrder;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Features.Orders.Queries.GetAllOrders;
using InvenPilot.Application.Features.Orders.Queries.GetOrderById;
using InvenPilot.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            var order = await mediator.Send(new CreateOrderCommand(orderDTO));
            return Ok(new
            {
                Message = "Order Created Successfully",
                Data = order
            });
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await mediator.Send(new GetOrderByIdQuery(id));
            return Ok(order);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderQueryParameter orderQueryParameter)
        {
            var orders = await mediator.Send(new GetAllOrdersQuery(orderQueryParameter));
            return Ok(orders);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}/{orderStatus}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatus orderStatus)
        {
            var order = await mediator.Send(new UpdateOrderCommand(id,orderStatus));
            return Ok(new
            {
                Message = "Order Status Updated Successfully",
                Data = order
            });
        }
    }
}
