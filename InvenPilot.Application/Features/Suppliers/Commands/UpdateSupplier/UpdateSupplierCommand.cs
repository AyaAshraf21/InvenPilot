using InvenPilot.Application.Features.Suppliers.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Commands.UpdateSupplier
{
    public record UpdateSupplierCommand(int id, SupplierDTO supplierDTO): IRequest<SupplierResponseDTO>;
}
