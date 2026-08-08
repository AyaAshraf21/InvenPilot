using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetCustomerByIdAsync(request.id);

            if(customer == null)
            {
                throw new NotFoundException("Customer", request.id);
            }

            await customerRepository.DeleteCustomerAsync(customer);
        }
    }
}
