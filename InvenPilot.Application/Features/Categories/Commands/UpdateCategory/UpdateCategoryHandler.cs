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

namespace InvenPilot.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponseDTO>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<CategoryResponseDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
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

            mapper.Map(request.categoryDTO, category);
            
            categoryRepository.UpdateCategory(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CategoryResponseDTO>(category);
        }
    }
}
