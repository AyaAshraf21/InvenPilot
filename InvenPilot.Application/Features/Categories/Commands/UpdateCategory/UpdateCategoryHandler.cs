using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryQuery, CategoryResponseDTO>
    {
        private readonly ICategoryRepository categoryRepository;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDTO> Handle(UpdateCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(request.id);
            if (category == null)
            {
                throw new NotFoundException("Category",request.id);
            }

            bool isFoundName = await categoryRepository.isCategoryExistByNameAsync(request.categoryDTO.Name);
            if (isFoundName)
            {
                throw new AlreadyExistsException($"{request.categoryDTO.Name} Category");
            }

            category.Name = request.categoryDTO.Name;
            await categoryRepository.UpdateCategory(category);

            return new CategoryResponseDTO
            {
                ID = category.ID,
                Name = category.Name,
            };
        }
    }
}
