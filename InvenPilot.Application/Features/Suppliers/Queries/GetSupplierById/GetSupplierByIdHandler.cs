using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, SupplierResponseDTO>
    {
        private readonly ISupplierRepository supplierRepository;

        public GetSupplierByIdHandler(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task<SupplierResponseDTO> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetSupplierByIdAsync(request.id);

            if(supplier == null)
            {
                throw new NotFoundException("Supplier", request.id);
            }

            return new SupplierResponseDTO
            {
                ID = supplier.ID,
                Name = supplier.Name,
                Address = supplier.Address,
                Email = supplier.Email,
                PhoneNumber = supplier.PhoneNumber,
            };
        }
    }
}
