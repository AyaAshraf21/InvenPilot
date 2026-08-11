using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponseDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetAllProductsHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<List<ProductResponseDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            if (request.productQueryParameters.CategoryId.HasValue)
            {
                bool isCategoryExist =
                    await categoryRepository.isCategoryExistByIdAsync(request.productQueryParameters.CategoryId.Value);

                if (!isCategoryExist)
                {
                    throw new NotFoundException("Category", request.productQueryParameters.CategoryId.Value);
                }
            }
            var products = await productRepository.GetAllProductsAsync(request.productQueryParameters);

            return mapper.Map<List<ProductResponseDTO>>(products);
        }
    }
}
