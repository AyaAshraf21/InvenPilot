using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponseDTO>
    {
        private readonly ICategoryRepository categoryRepository;
        public GetCategoryByIdHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            bool isFound = await categoryRepository.isCategoryExistByIdAsync(request.id);
            if (!isFound)
            {
                throw new NotFoundException("Category", request.id);
            }
            var categoryFound = await categoryRepository.GetCategoryByIdAsync(request.id);

            return new CategoryResponseDTO
            {
                ID = categoryFound.ID,
                Name = categoryFound.Name,
            };
        }
    }
}
