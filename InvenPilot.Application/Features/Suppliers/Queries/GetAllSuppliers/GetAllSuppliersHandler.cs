using AutoMapper;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Queries.GetAllSuppliers
{
    public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, List<SupplierResponseDTO>>
    {
        private readonly ISupplierRepository supplierRepository;
        private readonly IMapper mapper;

        public GetAllSuppliersHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            this.supplierRepository = supplierRepository;
            this.mapper = mapper;
        }

        public async Task<List<SupplierResponseDTO>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliersList = new List<SupplierResponseDTO>();

            var suppliers = await supplierRepository.GetAllSuppliersAsync(request.SupplierQueryParameters);

            return mapper.Map<List<SupplierResponseDTO>>(suppliers);
        }
    }
}
