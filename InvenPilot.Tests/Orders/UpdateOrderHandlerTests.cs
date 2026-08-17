using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Orders.Commands.UpdateOrder;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Orders
{
    public class UpdateOrderHandlerTests
    {
        [Fact]
        public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var request = new UpdateOrderCommand(1, OrderStatus.Completed);

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync((Order?)null);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(request, CancellationToken.None)
            );

            orderRepositoryMock.Verify(
                x => x.UpdateOrderStatus(It.IsAny<Order>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_WhenOrderIsCancelled_ShouldThrowBadRequestException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                OrderStatus = OrderStatus.Cancelled,
                OrderType = OrderType.Sale
            };

            var request = new UpdateOrderCommand(1, OrderStatus.Completed);

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync(order);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<BadRequestException>(
                () => handler.Handle(request, CancellationToken.None)
            );

            orderRepositoryMock.Verify(
                x => x.UpdateOrderStatus(It.IsAny<Order>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_WhenOrderIsCompleted_ShouldThrowBadRequestException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                OrderStatus = OrderStatus.Completed,
                OrderType = OrderType.Sale
            };

            var request = new UpdateOrderCommand(1, OrderStatus.Cancelled);

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync(order);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

                await Assert.ThrowsAsync<BadRequestException>(
                    () => handler.Handle(request, CancellationToken.None)
                );
        }


        [Fact]
        public async Task Handle_WhenNewStatusIsSameAsCurrentStatus_ShouldThrowBadRequestException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                OrderStatus = OrderStatus.Pending,
                OrderType = OrderType.Sale
            };

            var request = new UpdateOrderCommand(1, OrderStatus.Pending);

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync(order);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<BadRequestException>(
                () => handler.Handle(request, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_WhenPendingOrderIsCompleted_ShouldUpdateStatus()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                CustomerID = 1,
                SupplierID = null,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                OrderStatus = OrderStatus.Pending,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ID = 1,
                        ProductID = 1,
                        Quantity = 2
                    }
                }
            };

            var response = new OrderResponseDTO
            {
                ID = order.ID,
                CustomerID = order.CustomerID,
                SupplierID = order.SupplierID,
                OrderDate = order.OrderDate,
                OrderStatus = OrderStatus.Completed,
                OrderType = order.OrderType
            };

            var request = new UpdateOrderCommand(1, OrderStatus.Completed);

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync(order);

            mapperMock
                .Setup(x => x.Map<OrderResponseDTO>(It.IsAny<Order>()))
                .Returns(response);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Completed, result.OrderStatus);

            orderRepositoryMock.Verify(
                x => x.UpdateOrderStatus(order),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WhenPendingSaleOrderIsCancelled_ShouldRestoreProductQuantity()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                OrderStatus = OrderStatus.Pending,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                ID = 1,
                ProductID = 1,
                Quantity = 3
            },
            new OrderItem
            {
                ID = 2,
                ProductID = 2,
                Quantity = 2
            }
        }
            };

            var products = new List<Product>
            {
                new Product
                {
                    ID = 1,
                    Quantity = 10
                },
                new Product
                {
                    ID = 2,
                    Quantity = 20
                }
            };

            var response = new OrderResponseDTO
            {
                ID = order.ID,
                OrderStatus = OrderStatus.Cancelled,
                OrderType = order.OrderType
            };

            var request = new UpdateOrderCommand(1, OrderStatus.Cancelled);

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync(order);

            productRepositoryMock
                .Setup(x => x.GetProductsByIdAsync(
                    It.Is<List<int>>(ids => ids.SequenceEqual(new[] { 1, 2 }))))
                .ReturnsAsync(products);

            mapperMock
                .Setup(x => x.Map<OrderResponseDTO>(It.IsAny<Order>()))
                .Returns(response);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Cancelled, result.OrderStatus);

            Assert.Equal(13, products[0].Quantity);
            Assert.Equal(22, products[1].Quantity);

            productRepositoryMock.Verify(
                x => x.UpdateProduct(It.IsAny<Product>()),
                Times.Exactly(2)
            );

            orderRepositoryMock.Verify(
                x => x.UpdateOrderStatus(order),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }


        [Fact]
        public async Task Handle_WhenPendingPurchaseOrderIsCancelled_ShouldDecreaseProductQuantity()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                OrderStatus = OrderStatus.Pending,
                OrderType = OrderType.Purchase,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ID = 1,
                        ProductID = 1,
                        Quantity = 3
                    }
                }
            };

            var products = new List<Product>
            {
                new Product
                {
                    ID = 1,
                    Quantity = 10
                }
            };

            var response = new OrderResponseDTO
            {
                ID = order.ID,
                OrderStatus = OrderStatus.Cancelled,
                OrderType = order.OrderType
            };

            var request = new UpdateOrderCommand(1, OrderStatus.Cancelled);

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync(order);

            productRepositoryMock
                .Setup(x => x.GetProductsByIdAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(products);

            mapperMock
                .Setup(x => x.Map<OrderResponseDTO>(It.IsAny<Order>()))
                .Returns(response);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Cancelled, result.OrderStatus);
            Assert.Equal(7, products[0].Quantity);

            productRepositoryMock.Verify(
                x => x.UpdateProduct(It.IsAny<Product>()),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WhenPendingOrderHasInvalidNewStatus_ShouldThrowBadRequestException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                OrderStatus = OrderStatus.Pending,
                OrderType = OrderType.Sale
            };

            var request = new UpdateOrderCommand(
                1,
                (OrderStatus)999
            );

            orderRepositoryMock
                .Setup(x => x.GetOrderByIdAsync(request.id))
                .ReturnsAsync(order);

            var handler = new UpdateOrderHandler(
                orderRepositoryMock.Object,
                unitOfWorkMock.Object,
                productRepositoryMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<BadRequestException>(
                () => handler.Handle(request, CancellationToken.None)
            );

            orderRepositoryMock.Verify(
                x => x.UpdateOrderStatus(It.IsAny<Order>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

    }
}
