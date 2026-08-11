using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDTO>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<ProductResponseDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await productRepository.IsProductExistByIDAsync(request.id);
            if (!isExist)
            {
                throw new NotFoundException("Product", request.id);
            }
            var product = await productRepository.GetProductByIdAsync(request.id);

            return mapper.Map<ProductResponseDTO>(product);
        }
    }
}
