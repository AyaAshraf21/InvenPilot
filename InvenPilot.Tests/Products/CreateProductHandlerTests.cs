using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.Commands.CreateProduct;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Products
{
    public class CreateProductHandlerTests
    {
        [Fact]
        public async Task Handle_WhenProductAlreadyExists_ShouldThrowAlreadyExistsException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var productDTO = new ProductDTO
            {
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 50000,
                Quantity = 10
            };

            var request = new CreateProductCommand(productDTO);

            productRepositoryMock
                .Setup(x => x.IsProductExistByNameAsync(productDTO.Name))
                .ReturnsAsync(true);

            var handler = new CreateProductHandler(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<AlreadyExistsException>(
                () => handler.Handle(request, CancellationToken.None)
             );
        }


        [Fact]
        public async Task Handle_WhenCategoryNotFound_ShouldThrowNotFoundException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var productDTO = new ProductDTO
            {
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 50000,
                Quantity = 10,
                CategoryId = 50
            };

            var request = new CreateProductCommand(productDTO);

            productRepositoryMock
                .Setup(x => x.IsProductExistByNameAsync(productDTO.Name))
                .ReturnsAsync(false);

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByIdAsync(productDTO.CategoryId.Value))
                .ReturnsAsync(false);

            var handler = new CreateProductHandler(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(request, CancellationToken.None)
             );
        }


        [Fact]
        public async Task Handle_WhenRequestValid_ShouldCreateProduct()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var productDTO = new ProductDTO
            {
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 50000,
                Quantity = 10,
                CategoryId = 50
            };

            var request = new CreateProductCommand(productDTO);

            var product = new Product
            {
                ID = 1,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity,
                CategoryId = productDTO.CategoryId
            };

            var response = new ProductResponseDTO
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId
            };

            productRepositoryMock
                .Setup(x => x.IsProductExistByNameAsync(productDTO.Name))
                .ReturnsAsync(false);

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByIdAsync(productDTO.CategoryId.Value))
                .ReturnsAsync(true);

            mapperMock
                .Setup(x => x.Map<Product>(productDTO))
                .Returns(product);

            mapperMock
                .Setup(x => x.Map<ProductResponseDTO>(product))
                .Returns(response);

            var handler = new CreateProductHandler(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(product.ID, result.ID);
            Assert.Equal(product.Name, result.Name);

            productRepositoryMock.Verify(
                x => x.CreateProduct(product),
                Times.Once);

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once);
        }
    }
}
