using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.Commands.UpdateProduct;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InvenPilot.Tests.Products
{
    public class UpdateProductHandlerTests
    {
        [Fact]
        public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var newProductDTO = new ProductDTO
            {
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 50000,
                Quantity = 10
            };

            var request = new UpdateProductCommand(1, newProductDTO);

            productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(request.id))
                .ReturnsAsync((Product?)null);

            var handler = new UpdateProductHandler(
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
        public async Task Handle_WhenNewProductNameAlreadyExists_ShouldThrowAlreadyExistsException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var oldProduct = new Product
            {
                ID = 1,
                Name = "Laptop",
                Description = "Old Description",
                Price = 40000,
                Quantity = 5,
                CategoryId = null
            };

            var productDTO = new ProductDTO
            {
                Name = "MacBook",
                Description = "New Description",
                Price = 50000,
                Quantity = 10,
                CategoryId = null
            };

            var request = new UpdateProductCommand(1, productDTO);

            productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(request.id))
                .ReturnsAsync(oldProduct);

            productRepositoryMock
                .Setup(x => x.IsProductExistByNameAsync(request.productDTO.Name))
                .ReturnsAsync(true);

            var handler = new UpdateProductHandler(
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


            var oldProduct = new Product
            {
                ID = 1,
                Name = "Laptop",
                Description = "Old Description",
                Price = 40000,
                Quantity = 5,
                CategoryId = 4
            };

            var productDTO = new ProductDTO
            {
                Name = "MacBook",
                Description = "New Description",
                Price = 50000,
                Quantity = 10,
                CategoryId = 5
            };

            var request = new UpdateProductCommand(1, productDTO);

            productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(request.id))
                .ReturnsAsync(oldProduct);

            productRepositoryMock
                .Setup(x => x.IsProductExistByNameAsync(request.productDTO.Name))
                .ReturnsAsync(false);

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByIdAsync(5))
                .ReturnsAsync(false);

            var handler = new UpdateProductHandler(
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
        public async Task Handle_WhenRequestValid_ShouldUpdateProduct()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();


            var oldProduct = new Product
            {
                ID = 1,
                Name = "Laptop",
                Description = "Old Description",
                Price = 40000,
                Quantity = 5,
                CategoryId = 4
            };

            var productDTO = new ProductDTO
            {
                Name = "MacBook",
                Description = "New Description",
                Price = 50000,
                Quantity = 10,
                CategoryId = 5
            };
            var updatedProduct = new Product
            {
                ID = 1,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity,
                CategoryId = productDTO.CategoryId
            };

            var request = new UpdateProductCommand(1, productDTO);

            productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(request.id))
                .ReturnsAsync(oldProduct);

            productRepositoryMock
                .Setup(x => x.IsProductExistByNameAsync(request.productDTO.Name))
                .ReturnsAsync(false);

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByIdAsync(5))
                .ReturnsAsync(true);

            mapperMock
                .Setup(x => x.Map<Product>(productDTO))
                .Returns(updatedProduct);

            mapperMock
                .Setup(x => x.Map<ProductResponseDTO>(It.IsAny<Product>()))
                .Returns(new ProductResponseDTO
                {
                    ID = updatedProduct.ID,
                    Name = updatedProduct.Name,
                    Description = updatedProduct.Description,
                    Price = updatedProduct.Price,
                    Quantity = updatedProduct.Quantity,
                    CategoryId = updatedProduct.CategoryId
                });


            var handler = new UpdateProductHandler(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(updatedProduct.ID, result.ID);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Description, result.Description);
            Assert.Equal(updatedProduct.Price, result.Price);
            Assert.Equal(updatedProduct.Quantity, result.Quantity);
            Assert.Equal(updatedProduct.CategoryId, result.CategoryId);

            productRepositoryMock.Verify(
                x => x.UpdateProduct(It.IsAny<Product>()),
                Times.Once
                );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None)
                , Times.Once
                );
            
        }


    }
}
