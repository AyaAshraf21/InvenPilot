using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Categories.Queries.GetCategoryById;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponseDTO>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public GetCustomerByIdHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        public async Task<CustomerResponseDTO> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetCustomerByIdAsync(request.id);

            if(customer == null)
            {
                throw new NotFoundException("Customer", request.id);
            }

            return mapper.Map<CustomerResponseDTO>(customer);
        }

    }
}
