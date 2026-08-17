using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Orders.Commands.CreateOrder;
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
    public class CreateOrderHandlerTests
    {
        [Fact]
        public async Task Handle_WhenSaleAndCustomerIsNull_ShouldThrowBadRequestException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                CustomerID = null,
                SupplierID = 1,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItemDTO
                    {
                        ProductID = 2,
                        Quantity = 1
                    }
                }
            };

            var request = new CreateOrderCommand(orderDTO);

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WhenSaleAndCustomerNotFound_ShouldThrowNotFoundException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                CustomerID = 5,
                SupplierID = null,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItemDTO
                    {
                        ProductID = 2,
                        Quantity = 1
                    }
                }
            };

            var request = new CreateOrderCommand(orderDTO);

            customerRepositoryMock.Setup(x => x.IsCustomerExistByIdAsync(request.orderDTO.CustomerID.Value)).ReturnsAsync(false);

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenPurchaseAndSupplierIsNull_ShouldThrowBadRequestException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                CustomerID = 1,
                SupplierID = null,
                OrderType = OrderType.Purchase,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItemDTO
                    {
                        ProductID = 2,
                        Quantity = 1
                    }
                }
            };

            var request = new CreateOrderCommand(orderDTO);

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WhenPurchaseAndSupplierNotFound_ShouldThrowNotFoundException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                CustomerID = null,
                SupplierID = 5,
                OrderType = OrderType.Purchase,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItemDTO
                    {
                        ProductID = 2,
                        Quantity = 1
                    }
                }
            };

            var request = new CreateOrderCommand(orderDTO);

            supplierRepositoryMock.Setup(x => x.IsSupplierExistByIdAsync(request.orderDTO.SupplierID.Value)).ReturnsAsync(false);

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                CustomerID = 1,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItemDTO
                    {
                        ProductID = 2,
                        Quantity = 1
                    }
                }
            };

            var request = new CreateOrderCommand(orderDTO);

            customerRepositoryMock
                .Setup(x => x.IsCustomerExistByIdAsync(1))
                .ReturnsAsync(true);

            productRepositoryMock
                .Setup(x => x.GetProductsByIdAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Product>
                {
                    new Product
                    {
                        ID = 1,
                        Name = "Laptop",
                        Quantity = 10
                    }
                });

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(request, CancellationToken.None)
            );

            orderRepositoryMock.Verify(
                x => x.CreateOrder(It.IsAny<Order>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_WhenSaleQuantityExceedsStock_ShouldThrowBadRequestException()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                CustomerID = 1,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 10
                    }
                }
            };

            var request = new CreateOrderCommand(orderDTO);

            customerRepositoryMock
                .Setup(x => x.IsCustomerExistByIdAsync(1))
                .ReturnsAsync(true);

            productRepositoryMock
                .Setup(x => x.GetProductsByIdAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Product>
                {
                    new Product
                    {
                        ID = 1,
                        Name = "Laptop",
                        Quantity = 5
                    }
                });

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<BadRequestException>(
                () => handler.Handle(request, CancellationToken.None)
            );

            orderRepositoryMock.Verify(
                x => x.CreateOrder(It.IsAny<Order>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_WhenValidSaleOrder_ShouldCreateOrderAndDecreaseStock()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                CustomerID = 1,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 2
                    },
                    new OrderItemDTO
                    {
                        ProductID = 2,
                        Quantity = 1
                    }
                }
             };

            var request = new CreateOrderCommand(orderDTO);

            var products = new List<Product>
            {
                new Product
                {
                    ID = 1,
                    Name = "Laptop",
                    Quantity = 10
                },
                new Product
                {
                    ID = 2,
                    Name = "Mouse",
                    Quantity = 20
                }
            };

            var order = new Order
            {
                CustomerID = 1,
                OrderType = OrderType.Sale,
                OrderItems = new List<OrderItem>()
            };

            var response = new OrderResponseDTO
            {
                ID = 1,
                CustomerID = 1,
                OrderType = OrderType.Sale,
                OrderStatus = OrderStatus.Pending
            };

            customerRepositoryMock
                .Setup(x => x.IsCustomerExistByIdAsync(1))
                .ReturnsAsync(true);

            productRepositoryMock
                .Setup(x => x.GetProductsByIdAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(products);

            mapperMock
                .Setup(x => x.Map<Order>(orderDTO))
                .Returns(order);

            mapperMock
                .Setup(x => x.Map<OrderResponseDTO>(order))
                .Returns(response);

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(response.ID, result.ID);
            Assert.Equal(response.CustomerID, result.CustomerID);
            Assert.Equal(response.OrderType, result.OrderType);
            Assert.Equal(OrderStatus.Pending, order.OrderStatus);

            Assert.Equal(8, products[0].Quantity);
            Assert.Equal(19, products[1].Quantity);

            productRepositoryMock.Verify(
                x => x.UpdateProduct(It.IsAny<Product>()),
                Times.Exactly(2)
            );

            orderRepositoryMock.Verify(
                x => x.CreateOrder(order),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }


        [Fact]
        public async Task Handle_WhenValidPurchaseOrder_ShouldCreateOrderAndIncreaseStock()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var orderDTO = new OrderDTO
            {
                SupplierID = 1,
                OrderType = OrderType.Purchase,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductID = 1,
                        Quantity = 5
                    }
                }
            };

            var request = new CreateOrderCommand(orderDTO);

            var products = new List<Product>
            {
                new Product
                {
                    ID = 1,
                    Name = "Laptop",
                    Quantity = 10
                }
            };

            var order = new Order
            {
                SupplierID = 1,
                OrderType = OrderType.Purchase,
                OrderItems = new List<OrderItem>()
            };

            var response = new OrderResponseDTO
            {
                ID = 1,
                SupplierID = 1,
                OrderType = OrderType.Purchase,
                OrderStatus = OrderStatus.Pending
            };

            supplierRepositoryMock
                .Setup(x => x.IsSupplierExistByIdAsync(1))
                .ReturnsAsync(true);

            productRepositoryMock
                .Setup(x => x.GetProductsByIdAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(products);

            mapperMock
                .Setup(x => x.Map<Order>(orderDTO))
                .Returns(order);

            mapperMock
                .Setup(x => x.Map<OrderResponseDTO>(order))
                .Returns(response);

            var handler = new CreateOrderHandler(
                orderRepositoryMock.Object,
                customerRepositoryMock.Object,
                supplierRepositoryMock.Object,
                productRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(response.ID, result.ID);
            Assert.Equal(response.SupplierID, result.SupplierID);
            Assert.Equal(response.OrderType, result.OrderType);
            Assert.Equal(OrderStatus.Pending, order.OrderStatus);

            Assert.Equal(15, products[0].Quantity);

            productRepositoryMock.Verify(
                x => x.UpdateProduct(It.IsAny<Product>()),
                Times.Once
            );

            orderRepositoryMock.Verify(
                x => x.CreateOrder(order),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }
    }
}
