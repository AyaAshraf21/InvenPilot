using AutoMapper;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerResponseDTO>>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public GetAllCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        public async Task<List<CustomerResponseDTO>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customersList = new List<CustomerResponseDTO>();

            var customers = await customerRepository.GetAllCustomersAsync(request.customerQueryParameters);

            return mapper.Map<List<CustomerResponseDTO>>(customers);
        }
    }
}
