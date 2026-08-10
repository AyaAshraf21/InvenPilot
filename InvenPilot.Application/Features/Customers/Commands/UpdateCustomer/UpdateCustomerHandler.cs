using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerResponseDTO>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponseDTO> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetCustomerByIdAsync(request.id);

            if(customer == null)
            {
                throw new NotFoundException("Customer",request.id);
            }

            if(customer.Email != request.customerDTO.Email)
            {
                bool isEmailExist = await customerRepository.IsCustomerExistByEmailAsync(request.customerDTO.Email);
                if (isEmailExist)
                {
                    throw new AlreadyExistsException("Email");
                }
            }

            if(customer.PhoneNumber != request.customerDTO.PhoneNumber)
            {
                bool isPhoneExist = await customerRepository.IsCustomerExistByPhoneAsync(request.customerDTO.PhoneNumber);
                if (isPhoneExist)
                {
                    throw new AlreadyExistsException("Phone Number");
                }
            }

            customer.Name = request.customerDTO.Name;
            customer.Email = request.customerDTO.Email;
            customer.PhoneNumber = request.customerDTO.PhoneNumber;
            customer.Address = request.customerDTO.Address;

            customerRepository.UpdateCustomer(customer);
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
