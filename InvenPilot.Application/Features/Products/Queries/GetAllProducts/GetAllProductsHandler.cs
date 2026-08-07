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

        public GetAllProductsHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
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
            List<ProductResponseDTO> result = new List<ProductResponseDTO>();

            foreach (var product in products)
            {
                result.Add(new ProductResponseDTO
                {
                    ID = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                });
            }
            return result;
        }
    }
}
