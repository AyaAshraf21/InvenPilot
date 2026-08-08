using InvenPilot.Application.Features.Suppliers.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Queries.GetAllSuppliers
{
    public record GetAllSuppliersQuery(SupplierQueryParameters SupplierQueryParameters) : IRequest<List<SupplierResponseDTO>>;
}
