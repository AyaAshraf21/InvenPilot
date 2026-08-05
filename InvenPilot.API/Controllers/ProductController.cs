using InvenPilot.Application.Features.Products.Commands.CreateProduct;
using InvenPilot.Application.Features.Products.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDTO productDTO)
        {
            var product = await mediator.Send(new CreateProductCommand(productDTO));
            return Ok(new
            {
                Message = "Product Created Successfully",
                Data = product
            });
        }

    }
}
