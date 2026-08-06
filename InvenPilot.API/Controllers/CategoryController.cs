using InvenPilot.Application.Features.Categories.Commands.CreateCategory;
using InvenPilot.Application.Features.Categories.Commands.DeleteCategory;
using InvenPilot.Application.Features.Categories.Commands.UpdateCategory;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Categories.Queries.GetAllCategories;
using InvenPilot.Application.Features.Categories.Queries.GetCategoryById;
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
        public async Task<IActionResult> GetAllCategories([FromQuery] int page, [FromQuery] int perPage)
        {
            var categories = await mediator.Send(new GetAllCategoriesQuery(page, perPage));
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDTO categoryDTO, int id)
        {
            var updatedCategory = await mediator.Send(new UpdateCategoryCommand(categoryDTO, id));
            return Ok(new
            {
                Message = "Category Updated Successfully",
                Data = updatedCategory
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await mediator.Send(new DeleteCategoryCommand(id));
            return Ok(new
            {
                Message = "Category Deleted Successfully"
            });
        }
    }
}
