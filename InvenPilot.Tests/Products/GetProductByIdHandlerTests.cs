using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.Commands.UpdateProduct;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Features.Products.Queries.GetProductById;
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
    public class GetProductByIdHandlerTests
    {
        [Fact]
        public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var mapperMock = new Mock<IMapper>();

            var request = new GetProductByIdQuery(1);

            productRepositoryMock
                .Setup(x => x.IsProductExistByIDAsync(request.id))
                .ReturnsAsync(false);

            var handler = new GetProductByIdHandler(
                productRepositoryMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(request, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldReturnProduct()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var mapperMock = new Mock<IMapper>();

            var product = new Product
            {
                ID = 1,
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 50000,
                Quantity = 10,
                CategoryId = 50
            };

            var productResponseDTO = new ProductResponseDTO
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId
            };

            var request = new GetProductByIdQuery(1);

            productRepositoryMock
                .Setup(x => x.IsProductExistByIDAsync(request.id))
                .ReturnsAsync(true);

            mapperMock
                .Setup(x => x.Map<ProductResponseDTO>(It.IsAny<Product>()))
                .Returns(productResponseDTO);

            var handler = new GetProductByIdHandler(
               productRepositoryMock.Object,
               mapperMock.Object
           );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(productResponseDTO.ID, result.ID);
            Assert.Equal(productResponseDTO.Name ,result.Name);
            Assert.Equal(productResponseDTO.Description, result.Description);
            Assert.Equal(productResponseDTO.Price, result.Price);
            Assert.Equal(productResponseDTO.Quantity, result.Quantity);
            Assert.Equal(productResponseDTO.CategoryId, result.CategoryId);

            productRepositoryMock.Verify(
                x => x.GetProductByIdAsync(result.ID),
                Times.Once
                );
        }
    }
}
