using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Commands.DeleteSupplier
{
    public class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand>
    {
        private readonly ISupplierRepository supplierRepository;

        public DeleteSupplierHandler(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetSupplierByIdAsync(request.id);
            if(supplier == null)
            {
                throw new NotFoundException("Supplier", request.id);
            }
            await supplierRepository.DeleteSupplierAsync(supplier);
        }
    }
}
