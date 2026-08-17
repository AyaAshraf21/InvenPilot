using InvenPilot.Application.Common.Authorization;
using InvenPilot.Application.Features.Products.Commands.CreateProduct;
using InvenPilot.Application.Features.Products.Commands.DeleteProduct;
using InvenPilot.Application.Features.Products.Commands.UpdateProduct;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Features.Products.Queries.GetAllProducts;
using InvenPilot.Application.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
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

        [Authorize(Roles = Roles.Admin)]
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

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters productQueryParameters)
        {
            var products = await mediator.Send(new GetAllProductsQuery(productQueryParameters));
            return Ok(products);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await mediator.Send(new DeleteProductCommand(id));
            return Ok(new
            {
                Message = "Product Deleted Successfully"
            });
        }
    }
}
