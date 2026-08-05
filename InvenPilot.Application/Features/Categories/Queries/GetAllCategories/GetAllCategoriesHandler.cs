using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponseDTO>>
    {
        private readonly ICategoryRepository categoryRepository;

        public GetAllCategoriesHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponseDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryResponseDTO
            {
                ID = c.ID,
                Name = c.Name,
            }).ToList();
        }
    }
}
