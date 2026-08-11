using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderResponseDTO>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public UpdateOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<OrderResponseDTO> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.id);
            if(order == null)
            {
                throw new NotFoundException("Order", request.id);
            }

            if(order.OrderStatus == OrderStatus.Cancelled)
            {
                throw new BadRequestException("Cancelled orders cannot be updated");
            }

            if(order.OrderStatus == OrderStatus.Completed)
            {
                throw new BadRequestException("Completed orders cannot be updated");
            }
            
            if(order.OrderStatus == request.orderStatus)
            {
                throw new BadRequestException("Order already has this status");
            }

            if(order.OrderStatus == OrderStatus.Pending)
            {
                if(request.orderStatus == OrderStatus.Completed)
                {
                    order.OrderStatus = OrderStatus.Completed;
                }
                else if(request.orderStatus == OrderStatus.Cancelled)
                {
                    var productIDs = order.OrderItems.Select(i => i.ProductID).ToList();
                    var products = await productRepository.GetProductsByIdAsync(productIDs);

                    if(order.OrderType == OrderType.Sale)
                    {
                        foreach(var item in order.OrderItems)
                        {
                            var product = products.First(x => x.ID == item.ProductID);
                            product.Quantity += item.Quantity;
                            productRepository.UpdateProduct(product);
                        }
                    }
                    else if(order.OrderType == OrderType.Purchase)
                    {
                        foreach (var item in order.OrderItems)
                        {
                            var product = products.First(x => x.ID == item.ProductID);
                            product.Quantity -= item.Quantity;
                            productRepository.UpdateProduct(product);
                        }
                    }
                    order.OrderStatus = OrderStatus.Cancelled;
                }
                else
                {
                    throw new BadRequestException("Invalid Order Status.");
                }
            }
            orderRepository.UpdateOrderStatus(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<OrderResponseDTO>(order);
        }
    }
}
