using AutoMapper;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderResponseDTO>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetAllOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<OrderResponseDTO>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetAllOrdersAsync(request.orderQueryParameters);

            return mapper.Map<List<OrderResponseDTO>>(orders);
        }
    }
}
