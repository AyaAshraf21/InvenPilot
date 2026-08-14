using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Products.Commands.DeleteProduct;
using InvenPilot.Application.Features.Products.DTO;
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
    public class DeleteProductHandlerTests
    {
        [Fact]
        public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new DeleteProductCommand(1);

            productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(request.id))
                .ReturnsAsync((Product?)null);

            var handler = new DeleteProductHandler(
                productRepositoryMock.Object,
                unitOfWorkMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(request, CancellationToken.None)
                );
        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldDeleteProduct()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new DeleteProductCommand(1);

            var product = new Product
            {
                ID = 1,
                Name = "Laptop",
                Price = 50000,
                Quantity = 10
            };

            productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(request.id))
                .ReturnsAsync(product);

            var handler = new DeleteProductHandler(
                productRepositoryMock.Object,
                unitOfWorkMock.Object
            );

            var result = handler.Handle(request, CancellationToken.None);

            productRepositoryMock.Verify(
                x => x.DeleteProduct(product),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }
    }
}
