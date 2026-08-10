using InvenPilot.Application.Features.Orders.Commands;
using InvenPilot.Application.Features.Orders.DTO;
using MediatR;
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
    }
}
