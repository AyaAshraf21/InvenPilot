using InvenPilot.Application.Common.Authorization;
using InvenPilot.Application.Features.Categories.Commands.CreateCategory;
using InvenPilot.Application.Features.Categories.Commands.DeleteCategory;
using InvenPilot.Application.Features.Categories.Commands.UpdateCategory;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Categories.Queries.GetAllCategories;
using InvenPilot.Application.Features.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
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

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] CategoryQueryParameters categoryQueryParameters)
        {
            var categories = await mediator.Send(new GetAllCategoriesQuery(categoryQueryParameters));
            return Ok(categories);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employee}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(category);
        }

        [Authorize(Roles = Roles.Admin)]
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

        [Authorize(Roles = Roles.Admin)]
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
