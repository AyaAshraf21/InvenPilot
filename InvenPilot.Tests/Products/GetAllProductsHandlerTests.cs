using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Features.Products.Queries.GetAllProducts;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Products
{
    public class GetAllProductsHandlerTests
    {

        [Fact]
        public async Task Handle_WhenCategoryInQueryParameterNotFound_ShouldThrowNotFoundException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();

            var productQueryParameters = new ProductQueryParameters
            {
                CategoryId = 1
            };

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByIdAsync(productQueryParameters.CategoryId.Value))
                .ReturnsAsync(false);

            var request = new GetAllProductsQuery(productQueryParameters);

            var handler = new GetAllProductsHandler(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(request, CancellationToken.None)
            );

        }


        [Fact]
        public async Task Handle_WhenRequestValid_ShouldReturnAllProducts()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();

            var products = new List<Product>
            {
                new Product
                {
                    ID = 1,
                    Name = "Laptop",
                    Description = "Gaming Laptop",
                    Price = 50000,
                    Quantity = 10,
                    CategoryId = 5
                },
                new Product
                {
                    ID = 2,
                    Name = "Mouse",
                    Description = "Wireless Mouse",
                    Price = 1000,
                    Quantity = 20,
                    CategoryId = 5
                }
            };

            var expectedResult = new List<ProductResponseDTO>
            {
                new ProductResponseDTO
                {
                    ID = 1,
                    Name = "Laptop",
                    Description = "Gaming Laptop",
                    Price = 50000,
                    Quantity = 10,
                    CategoryId = 5
                },
                new ProductResponseDTO
                {
                    ID = 2,
                    Name = "Mouse",
                    Description = "Wireless Mouse",
                    Price = 1000,
                    Quantity = 20,
                    CategoryId = 5
                }
            };

            var productQueryParameters = new ProductQueryParameters
            {
                CategoryId = 1
            };

            var request = new GetAllProductsQuery(productQueryParameters);

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByIdAsync(productQueryParameters.CategoryId.Value))
                .ReturnsAsync(true);

            productRepositoryMock
                .Setup(x => x.GetAllProductsAsync(productQueryParameters))
                .ReturnsAsync(products);

            mapperMock
                .Setup(x => x.Map<List<ProductResponseDTO>>(products))
                .Returns(expectedResult);

            var handler = new GetAllProductsHandler(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(
                request,
                CancellationToken.None
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(expectedResult[0].ID, result[0].ID);
            Assert.Equal(expectedResult[0].Name, result[0].Name);

            Assert.Equal(expectedResult[1].ID, result[1].ID);
            Assert.Equal(expectedResult[1].Name, result[1].Name);

            productRepositoryMock.Verify(
                x => x.GetAllProductsAsync(productQueryParameters),
                Times.Once
            );

            mapperMock.Verify(
                x => x.Map<List<ProductResponseDTO>>(products),
                Times.Once
            );
        }
    }
}
