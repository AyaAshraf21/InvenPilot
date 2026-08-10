using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Commands.CreateSupplier
{
    public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, SupplierResponseDTO>
    {
        private readonly ISupplierRepository supplierRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateSupplierHandler(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
        {
            this.supplierRepository = supplierRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<SupplierResponseDTO> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            bool isEmailExist = await supplierRepository.IsSupplierExistByEmailAsync(request.supplierDTO.Email);
            if (isEmailExist)
            {
                throw new AlreadyExistsException("Email");
            }
            bool isPhoneExist = await supplierRepository.IsSupplierExistByPhoneAsync(request.supplierDTO.PhoneNumber);
            if (isPhoneExist)
            {
                throw new AlreadyExistsException("Phone Number");
            }

            var supplier = new Supplier
            {
                Name = request.supplierDTO.Name,
                Email = request.supplierDTO.Email,
                PhoneNumber = request.supplierDTO.PhoneNumber,
                Address = request.supplierDTO.Address,
            };

            supplierRepository.CreateSupplier(supplier);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new SupplierResponseDTO
            {
                ID = supplier.ID,
                Name = supplier.Name,
                Email = supplier.Email,
                PhoneNumber = supplier.PhoneNumber,
                Address = supplier.Address,
            };
        }
    }
}
