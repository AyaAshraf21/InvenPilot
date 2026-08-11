using AutoMapper;
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
        private readonly IMapper mapper;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
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

            var customer = mapper.Map<Customer>(request.customerDTO);

            customerRepository.CreateCustomer(customer);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CustomerResponseDTO>(customer);
        }
    }
}
