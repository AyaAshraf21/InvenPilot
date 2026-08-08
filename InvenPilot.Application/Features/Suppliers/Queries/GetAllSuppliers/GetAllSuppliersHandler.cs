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

        public GetAllSuppliersHandler(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task<List<SupplierResponseDTO>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliersList = new List<SupplierResponseDTO>();

            var suppliers = await supplierRepository.GetAllSuppliersAsync(request.SupplierQueryParameters);

            foreach(var supplier in suppliers)
            {
                suppliersList.Add(new SupplierResponseDTO
                {
                    ID = supplier.ID,
                    Name = supplier.Name,
                    Address = supplier.Address,
                    Email = supplier.Email,
                    PhoneNumber = supplier.PhoneNumber,
                });
            }

            return suppliersList;
        }
    }
}
