using InvenPilot.Application.Features.Customers.Commands.UpdateCustomer;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Suppliers.Commands.CreateSupplier;
using InvenPilot.Application.Features.Suppliers.Commands.DeleteSupplier;
using InvenPilot.Application.Features.Suppliers.Commands.UpdateSupplier;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;
using InvenPilot.Application.Features.Suppliers.Queries.GetSupplierById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase 
    {
        private readonly IMediator mediator;

        public SupplierController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await mediator.Send(new GetSupplierByIdQuery(id));
            return Ok(supplier);
        }

        [HttpGet("GetAllSuppliers")]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] SupplierQueryParameters supplierQueryParameters)
        {
            var suppliers = await mediator.Send(new GetAllSuppliersQuery(supplierQueryParameters));
            return Ok(suppliers);
        }

        [HttpPost("CreateSupplier")]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDTO supplierDTO)
        {
            var supplier = await mediator.Send(new CreateSupplierCommand(supplierDTO));
            return Ok(new
            {
                Message = "Supplier Created Successfully.",
                Data = supplier
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDTO supplierDTO)
        {
            var supplier = await mediator.Send(new UpdateSupplierCommand(id, supplierDTO));

            return Ok(new
            {
                Message = "Supplier Updated Successfully",
                Data = supplier
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            await mediator.Send(new DeleteSupplierCommand(id));
            return Ok(new
            {
                Message = "Supplier Deleted Successfully"
            });
        }
    }
}
