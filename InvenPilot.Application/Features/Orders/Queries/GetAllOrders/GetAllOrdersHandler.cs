using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderResponseDTO>>
    {
        private readonly IOrderRepository orderRepository;

        public GetAllOrdersHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<List<OrderResponseDTO>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetAllOrdersAsync();

            var ordersResponse = orders.Select(item => new OrderResponseDTO
            {
                ID = item.ID,
                CustomerID = item.CustomerID,
                SupplierID = item.SupplierID,
                OrderDate = item.OrderDate,
                OrderStatus = item.OrderStatus,
                OrderType = item.OrderType,

                OrderItems = item.OrderItems.Select(i => new OrderItemResponseDTO
                {
                    ID = i.ID,
                    ProductID = i.ProductID,
                    Quantity = i.Quantity,
                }).ToList()
            }).ToList();

            return ordersResponse;

        }
    }
}
