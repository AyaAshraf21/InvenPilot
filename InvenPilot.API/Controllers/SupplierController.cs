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
    }
}
