using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Commands
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponseDTO>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrderHandler(IOrderRepository orderRepository,
                                  ICustomerRepository customerRepository,
                                  ISupplierRepository supplierRepository,
                                  IProductRepository productRepository , 
                                  IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.supplierRepository = supplierRepository;
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<OrderResponseDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if(request.orderDTO.OrderType == OrderType.Sale)
            {
                if(request.orderDTO.CustomerID == null)
                {
                    throw new BadRequestException("Customer ID is required for sale orders.");
                }
                
                bool isCustomerExist = await customerRepository.IsCustomerExistByIdAsync(request.orderDTO.CustomerID.Value);

                if(!isCustomerExist)
                {
                    throw new NotFoundException("Customer", request.orderDTO.CustomerID);
                }
            }

            else if(request.orderDTO.OrderType == OrderType.Purchase)
            {
                if (request.orderDTO.SupplierID == null)
                {
                    throw new BadRequestException("Supplier ID is required for purchase orders.");
                }

                bool isSupplierExist = await supplierRepository.IsSupplierExistByIdAsync(request.orderDTO.SupplierID.Value);

                if (!isSupplierExist)
                {
                    throw new NotFoundException("Supplier", request.orderDTO.SupplierID);
                }
            }

            var productIds = request.orderDTO.OrderItems.Select(o => o.ProductID).Distinct().ToList();
            var products = await productRepository.GetProductsByIdAsync(productIds);

            if(products.Count != productIds.Count)
            {
                var missingProductId = productIds.Except(products.Select(p => p.ID)).First();
                throw new NotFoundException("Product", missingProductId);
            }

            
            if(request.orderDTO.OrderType == OrderType.Sale)
            {
                foreach(var item in request.orderDTO.OrderItems)
                {
                    var product = products.First(p => p.ID == item.ProductID);
                    if(product.Quantity < item.Quantity)
                    {
                        throw new BadRequestException($"Insufficient stock for product {product.ID}");
                    }
                }
            }

            Order order = new Order
            {
                CustomerID = request.orderDTO.CustomerID,
                SupplierID = request.orderDTO.SupplierID,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                OrderType = request.orderDTO.OrderType,
            };

            foreach(var item in request.orderDTO.OrderItems)
            {
                var product = products.First(product => product.ID == item.ProductID);
                order.OrderItems.Add(new OrderItem
                {
                    ProductID = product.ID,
                    Quantity = item.Quantity,
                });

                if(request.orderDTO.OrderType == OrderType.Sale)
                {
                    product.Quantity -= item.Quantity;
                }
                else if(request.orderDTO.OrderType== OrderType.Purchase)
                {
                    product.Quantity += item.Quantity;
                }

                productRepository.UpdateProduct(product);
            }

            orderRepository.CreateOrder(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new OrderResponseDTO
            {
                ID = order.ID,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                OrderType = order.OrderType,
                CustomerID = order.CustomerID,
                SupplierID = order.SupplierID,

                OrderItems = order.OrderItems.Select(i => new OrderItemResponseDTO
                {
                    ID = i.ID,
                    ProductID = i.ProductID,
                    Quantity = i.Quantity,
                }).ToList()
            };
        }
    }
}
