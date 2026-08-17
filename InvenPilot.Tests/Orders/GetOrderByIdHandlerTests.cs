using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Features.Orders.Queries.GetOrderById;
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
    public class GetOrderByIdHandlerTests
    {
        [Fact]
        public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var mapperMock = new Mock<IMapper>();

            var request = new GetOrderByIdQuery(2);

            orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(request.id)).ReturnsAsync((Order?)null);

            var handler = new GetOrderByIdHandler(orderRepositoryMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetOrderById()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var mapperMock = new Mock<IMapper>();

            var order = new Order
            {
                ID = 1,
                CustomerID = 1,
                SupplierID = null,
                OrderDate = DateTime.UtcNow.AddDays(-2),
                OrderStatus = OrderStatus.Pending,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ID = 1,
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItem
                    {
                        ID = 2,
                        ProductID = 2,
                        Quantity = 1
                    }
                }
            };

            var request = new GetOrderByIdQuery(1);

            orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(request.id)).ReturnsAsync(order);
            
            mapperMock.Setup(x => x.Map<OrderResponseDTO>(
                 It.Is<Order>(o =>
                     o.ID == 1 &&
                     o.CustomerID == 1 &&
                     o.SupplierID == null &&
                     o.OrderType == OrderType.Sale &&
                     o.OrderStatus == OrderStatus.Pending &&
                     o.OrderItems.Count == 2 &&
                     o.OrderItems[0].ProductID == 1 &&
                     o.OrderItems[0].Quantity == 2 &&
                     o.OrderItems[1].ProductID == 2 &&
                     o.OrderItems[1].Quantity == 1
                 )))
             .Returns(new OrderResponseDTO
             {
                 ID = 1,
                 CustomerID = 1,
                 SupplierID = null,
                 OrderType = OrderType.Sale,
                 OrderStatus = OrderStatus.Pending,
                 OrderItems = new List<OrderItemResponseDTO>
                 {
                    new OrderItemResponseDTO
                    {
                        ID = 1,
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItemResponseDTO
                    {
                        ID = 2,
                        ProductID = 2,
                        Quantity = 1
                    }
                 }
             });

            var handler = new GetOrderByIdHandler(orderRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);

            Assert.Equal(order.ID, result.ID);
            Assert.Equal(order.CustomerID, result.CustomerID);
            Assert.Equal(order.SupplierID,result.SupplierID);
            Assert.Equal(order.OrderType, result.OrderType);
            Assert.Equal(order.OrderStatus, result.OrderStatus);

            Assert.NotNull(result.OrderItems);
            Assert.Equal(2, result.OrderItems.Count);

            Assert.Equal(order.OrderItems[0].ID, result.OrderItems[0].ID);
            Assert.Equal(order.OrderItems[0].ProductID, result.OrderItems[0].ProductID);
            Assert.Equal(order.OrderItems[0].Quantity, result.OrderItems[0].Quantity);

            Assert.Equal(order.OrderItems[1].ID, result.OrderItems[1].ID);
            Assert.Equal(order.OrderItems[1].ProductID, result.OrderItems[1].ProductID);
            Assert.Equal(order.OrderItems[1].Quantity, result.OrderItems[1].Quantity);

            orderRepositoryMock.Verify(x => x.GetOrderByIdAsync(request.id), Times.Once);
        }
    }
}
