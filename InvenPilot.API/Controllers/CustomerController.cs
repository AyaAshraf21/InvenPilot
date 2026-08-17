using InvenPilot.Application.Common.Authorization;
using InvenPilot.Application.Features.Customers.Commands.CreateCustomer;
using InvenPilot.Application.Features.Customers.Commands.DeleteCustomer;
using InvenPilot.Application.Features.Customers.Commands.UpdateCustomer;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Customers.Queries.GetAllCustomers;
using InvenPilot.Application.Features.Customers.Queries.GetCustomerById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator mediator;

        public CustomerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await mediator.Send(new GetCustomerByIdQuery(id));
            return Ok(customer);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] CustomerQueryParameters customerQueryParameters)
        {
            var customers = await mediator.Send(new GetAllCustomersQuery(customerQueryParameters));
            return Ok(customers);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDTO)
        {
            var customer = await mediator.Send(new CreateCustomerCommand(customerDTO));
            return Ok(new
            {
                Message = "Customer Created Successfully",
                Data = customer
            });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id ,  [FromBody] CustomerDTO customerDTO)
        {
            var customer = await mediator.Send(new UpdateCustomerCommand(id, customerDTO));

            return Ok(new
            {
                Message = "Customer Updated Successfully",
                Data = customer
            });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await mediator.Send(new DeleteCustomerCommand(id));
            return Ok(new
            {
                Message = "Customer Deleted Successfully."
            });
        }

    }
}
