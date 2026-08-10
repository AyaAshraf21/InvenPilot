using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponseDTO>
    {
        private readonly IOrderRepository orderRepository;

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<OrderResponseDTO> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.id);
            if(order == null)
            {
                throw new NotFoundException("Order", request.id);
            }

            var orderResponse = new OrderResponseDTO
            {
                ID = order.ID,
                CustomerID = order.CustomerID,
                SupplierID = order.SupplierID,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                OrderType = order.OrderType,
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    ID = item.ID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                }).ToList()
            };
            return orderResponse;
        }
    }
}
