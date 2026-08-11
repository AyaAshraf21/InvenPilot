using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;

namespace InvenPilot.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponseDTO>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreateProductHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ProductResponseDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            bool isProductExist =
                await productRepository.IsProductExistByNameAsync(request.productDTO.Name);

            if (isProductExist)
            {
                throw new AlreadyExistsException($"Product '{request.productDTO.Name}'");
            }

            if (request.productDTO.CategoryId.HasValue)
            {
                bool isCategoryExist =
                    await categoryRepository.isCategoryExistByIdAsync(request.productDTO.CategoryId.Value);

                if (!isCategoryExist)
                {
                    throw new NotFoundException("Category", request.productDTO.CategoryId.Value);
                }
            }

            var product = mapper.Map<Product>(request.productDTO);

            productRepository.CreateProduct(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ProductResponseDTO>(product);
        }
    }
}