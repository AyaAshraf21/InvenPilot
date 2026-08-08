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

        public GetAllCustomersHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<List<CustomerResponseDTO>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customersList = new List<CustomerResponseDTO>();

            var customers = await customerRepository.GetAllCustomersAsync(request.customerQueryParameters);
            foreach(var customer in customers)
            {
                customersList.Add(new CustomerResponseDTO
                {
                    ID = customer.ID,
                    Name = customer.Name,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Email = customer.Email,
                });
            }
            return customersList;
        }
    }
}
