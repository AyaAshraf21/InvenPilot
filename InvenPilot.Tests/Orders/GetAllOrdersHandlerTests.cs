using AutoMapper;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Features.Orders.Queries.GetAllOrders;
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
    public class GetAllOrdersHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetAllOrders()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var mapperMock = new Mock<IMapper>();

            var orderQueryParameter = new OrderQueryParameter();

            var request = new GetAllOrdersQuery(orderQueryParameter);

            var ordersList = new List<Order>
            {
                new Order
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
                },
                new Order
                {
                    ID = 2,
                    CustomerID = null,
                    SupplierID = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    OrderStatus = OrderStatus.Completed,
                    OrderType = OrderType.Purchase,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ID = 3,
                            ProductID = 3,
                            Quantity = 10
                        },
                        new OrderItem
                        {
                            ID = 4,
                            ProductID = 4,
                            Quantity = 5
                        }
                    }
                }
            };

            var ordersResponseList = new List<OrderResponseDTO>
            {
                new OrderResponseDTO
                {
                    ID = 1,
                    CustomerID = 1,
                    SupplierID = null,
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    OrderStatus = OrderStatus.Pending,
                    OrderType = OrderType.Sale,
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
                },
                new OrderResponseDTO
                {
                    ID = 2,
                    CustomerID = null,
                    SupplierID = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    OrderStatus = OrderStatus.Completed,
                    OrderType = OrderType.Purchase,
                    OrderItems = new List<OrderItemResponseDTO>
                    {
                        new OrderItemResponseDTO
                        {
                            ID = 3,
                            ProductID = 3,
                            Quantity = 10
                        },
                        new OrderItemResponseDTO
                        {
                            ID = 4,
                            ProductID = 4,
                            Quantity = 5
                        }
                    }
                }
            };
            orderRepositoryMock.Setup(x => x.GetAllOrdersAsync(orderQueryParameter)).ReturnsAsync(ordersList);

            mapperMock.Setup(x => x.Map<List<OrderResponseDTO>>(ordersList)).Returns(ordersResponseList);

            var handler = new GetAllOrdersHandler(orderRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count);

            Assert.Equal(ordersResponseList[0].ID, result[0].ID);
            Assert.Equal(ordersResponseList[0].CustomerID, result[0].CustomerID);
            Assert.Equal(ordersResponseList[0].SupplierID, result[0].SupplierID);
            Assert.Equal(ordersResponseList[0].OrderStatus, result[0].OrderStatus);
            Assert.Equal(ordersResponseList[0].OrderType, result[0].OrderType);
            Assert.Equal(ordersResponseList[0].OrderDate, result[0].OrderDate);

            Assert.Equal(2, result[0].OrderItems.Count);

            Assert.Equal(ordersResponseList[0].OrderItems[0].ID, result[0].OrderItems[0].ID);
            Assert.Equal(ordersResponseList[0].OrderItems[0].ProductID, result[0].OrderItems[0].ProductID);
            Assert.Equal(ordersResponseList[0].OrderItems[0].Quantity, result[0].OrderItems[0].Quantity);

            Assert.Equal(ordersResponseList[0].OrderItems[1].ID, result[0].OrderItems[1].ID);
            Assert.Equal(ordersResponseList[0].OrderItems[1].ProductID, result[0].OrderItems[1].ProductID);
            Assert.Equal(ordersResponseList[0].OrderItems[1].Quantity, result[0].OrderItems[1].Quantity);


            Assert.Equal(ordersResponseList[1].ID, result[1].ID);
            Assert.Equal(ordersResponseList[1].CustomerID, result[1].CustomerID);
            Assert.Equal(ordersResponseList[1].SupplierID, result[1].SupplierID);
            Assert.Equal(ordersResponseList[1].OrderStatus, result[1].OrderStatus);
            Assert.Equal(ordersResponseList[1].OrderType, result[1].OrderType);
            Assert.Equal(ordersResponseList[1].OrderDate, result[1].OrderDate);

            Assert.Equal(2, result[1].OrderItems.Count);

            Assert.Equal(ordersResponseList[1].OrderItems[0].ID, result[1].OrderItems[0].ID);
            Assert.Equal(ordersResponseList[1].OrderItems[0].ProductID, result[1].OrderItems[0].ProductID);
            Assert.Equal(ordersResponseList[1].OrderItems[0].Quantity, result[1].OrderItems[0].Quantity);

            Assert.Equal(ordersResponseList[1].OrderItems[1].ID, result[1].OrderItems[1].ID);
            Assert.Equal(ordersResponseList[1].OrderItems[1].ProductID, result[1].OrderItems[1].ProductID);
            Assert.Equal(ordersResponseList[1].OrderItems[1].Quantity, result[1].OrderItems[1].Quantity);

            orderRepositoryMock.Verify(x => x.GetAllOrdersAsync(orderQueryParameter), Times.Once);
            mapperMock.Verify(x => x.Map<List<OrderResponseDTO>>(ordersList), Times.Once);
        }
    }
}
