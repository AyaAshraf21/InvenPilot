using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InvenPilot.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductResponseDTO>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public UpdateProductHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<ProductResponseDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var oldProduct = await productRepository.GetProductByIdAsync(request.id);

            if(oldProduct.Name != request.productDTO.Name)
            {
                bool isNameExist = await productRepository.isProductExistByNameAsync(request.productDTO.Name);
                if(isNameExist)
                {
                    throw new AlreadyExistsException($"Product : {request.productDTO.Name}");
                }
            }

            if (oldProduct.CategoryId != request.productDTO.CategoryId)
            {
                bool isCategoryExist = await categoryRepository.isCategoryExistByIdAsync(request.productDTO.CategoryId.Value);
                if(!isCategoryExist)
                {
                    throw new NotFoundException("Category", request.productDTO.CategoryId);
                }
            }
            oldProduct.Name = request.productDTO.Name;
            oldProduct.Price = request.productDTO.Price;
            oldProduct.Quantity = request.productDTO.Quantity;
            oldProduct.Description = request.productDTO.Description;
            oldProduct.CategoryId = request.productDTO.CategoryId;

            await productRepository.UpdateProductAsync(oldProduct);

            return new ProductResponseDTO
            {
                ID = oldProduct.ID,
                Name = oldProduct.Name,
                Price = oldProduct.Price,
                Quantity = oldProduct.Quantity,
                Description = oldProduct.Description,
                CategoryId = oldProduct.CategoryId,
            };
        }
    }
}
