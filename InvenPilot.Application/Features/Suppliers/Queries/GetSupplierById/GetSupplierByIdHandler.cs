using AutoMapper;
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
        private readonly IMapper mapper;

        public GetSupplierByIdHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            this.supplierRepository = supplierRepository;
            this.mapper = mapper;
        }

        public async Task<SupplierResponseDTO> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetSupplierByIdAsync(request.id);

            if(supplier == null)
            {
                throw new NotFoundException("Supplier", request.id);
            }

            return mapper.Map<SupplierResponseDTO>(supplier);
        }
    }
}
