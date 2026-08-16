using AutoMapper;
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
        private readonly IMapper mapper;
        public GetCategoryByIdHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<CategoryResponseDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {            
            var category = await categoryRepository.GetCategoryByIdAsync(request.id);

            if (category == null )
            {
                throw new NotFoundException("Category", request.id);
            }
            return mapper.Map<CategoryResponseDTO>(category);
        }
    }
}
