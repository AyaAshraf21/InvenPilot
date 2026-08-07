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

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<CustomerResponseDTO> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetCustomerByIdAsync(request.id);

            if(customer == null)
            {
                throw new NotFoundException("Customer", request.id);
            }
            return new CustomerResponseDTO
            {
                ID = customer.ID,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                Email = customer.Email,
            };
        }

    }
}
