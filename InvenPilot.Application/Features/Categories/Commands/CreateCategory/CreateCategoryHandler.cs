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

namespace InvenPilot.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponseDTO>
    {
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            bool isExist = await categoryRepository.isCategoryExistByNameAsync(request.categoryDTO.Name);

            if (isExist)
            {
                throw new AlreadyExistsException($"{request.categoryDTO.Name} Category");
            }

            var category = new Category
            {
                Name = request.categoryDTO.Name
            };
            await categoryRepository.CreateCategoryAsync(category);
            return new CategoryResponseDTO
            {
                ID = category.ID,
                Name = category.Name,
            };
        }
    }
}
