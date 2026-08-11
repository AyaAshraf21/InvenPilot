using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Suppliers.Commands.UpdateSupplier
{
    public class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, SupplierResponseDTO>
    {
        private readonly ISupplierRepository supplierRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateSupplierHandler(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.supplierRepository = supplierRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<SupplierResponseDTO> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetSupplierByIdAsync(request.id);

            if (supplier == null)
            {
                throw new NotFoundException("Supplier", request.id);
            }

            if (supplier.Email != request.supplierDTO.Email)
            {
                bool isEmailExist = await supplierRepository.IsSupplierExistByEmailAsync(request.supplierDTO.Email);
                if (isEmailExist)
                {
                    throw new AlreadyExistsException("Email");
                }
            }

            if (supplier.PhoneNumber != request.supplierDTO.PhoneNumber)
            {
                bool isPhoneExist = await supplierRepository.IsSupplierExistByPhoneAsync(request.supplierDTO.PhoneNumber);
                if (isPhoneExist)
                {
                    throw new AlreadyExistsException("Phone Number");
                }
            }

            mapper.Map(request.supplierDTO, supplier);

            supplierRepository.UpdateSupplier(supplier);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<SupplierResponseDTO>(supplier);
        }
    }
}
