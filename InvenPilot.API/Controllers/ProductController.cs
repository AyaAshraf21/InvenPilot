using InvenPilot.Application.Features.Products.Commands.CreateProduct;
using InvenPilot.Application.Features.Products.Commands.UpdateProduct;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Features.Products.Queries.GetAllProducts;
using InvenPilot.Application.Features.Products.Queries.GetProductById;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            var product = await mediator.Send(new UpdateProductCommand(id,productDTO));
            return Ok(new
            {
                Message = "Product Updated Successfully",
                Data = product
            });
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }
    }
}
