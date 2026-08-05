using InvenPilot.Application.Features.Categories.Commands.CreateCategory;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Categories.Queries.GetAllCategories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }
            
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody]CategoryDTO categoryDTO)
        {
            var categoryResponseDTO = await mediator.Send(new CreateCategoryCommand(categoryDTO));
            return Ok(new
            {
                Message = "Category created successfully.",
                Data = categoryResponseDTO
            });
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }

    }
}
