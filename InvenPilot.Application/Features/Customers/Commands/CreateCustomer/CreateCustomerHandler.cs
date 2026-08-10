using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerResponseDTO>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponseDTO> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            bool isEmailExist = await customerRepository.IsCustomerExistByEmailAsync(request.customerDTO.Email);
            if(isEmailExist)
            {
                throw new AlreadyExistsException("Email");
            }
            bool isPhoneExist = await customerRepository.IsCustomerExistByPhoneAsync(request.customerDTO.PhoneNumber);
            if (isPhoneExist)
            {
                throw new AlreadyExistsException("Phone Number");
            }

            var customer = new Customer
            {
                Name = request.customerDTO.Name,
                Email = request.customerDTO.Email,
                PhoneNumber = request.customerDTO.PhoneNumber,
                Address = request.customerDTO.Address,
            };

            customerRepository.CreateCustomer(customer);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CustomerResponseDTO
            {
                ID = customer.ID,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
            };
        }
    }
}
