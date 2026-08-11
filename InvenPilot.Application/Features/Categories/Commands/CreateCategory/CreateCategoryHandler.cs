using AutoMapper;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<CategoryResponseDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            bool isExist = await categoryRepository.isCategoryExistByNameAsync(request.categoryDTO.Name);

            if (isExist)
            {
                throw new AlreadyExistsException($"{request.categoryDTO.Name} Category");
            }

            var category = mapper.Map<Category>(request.categoryDTO);
            
            categoryRepository.CreateCategory(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CategoryResponseDTO>(category);
        }
    }
}
