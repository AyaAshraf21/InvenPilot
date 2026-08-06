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

        public GetAllProductsHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<List<ProductResponseDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetAllProductsAsync();
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
