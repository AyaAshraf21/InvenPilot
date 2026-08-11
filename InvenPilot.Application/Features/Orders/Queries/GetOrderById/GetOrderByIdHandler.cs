using AutoMapper;
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
        private readonly IMapper mapper;

        public GetOrderByIdHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<OrderResponseDTO> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.id);
            if(order == null)
            {
                throw new NotFoundException("Order", request.id);
            }
            return mapper.Map<OrderResponseDTO>(order);
        }
    }
}
