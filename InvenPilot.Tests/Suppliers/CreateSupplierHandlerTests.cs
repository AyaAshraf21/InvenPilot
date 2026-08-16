using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.Commands.CreateCustomer;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Suppliers.Commands.CreateSupplier;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Suppliers
{
    public class CreateSupplierHandlerTests
    {
        [Fact]
        public async Task Handle_WhenEmailAlreadyExist_ShouldThrowAlreadyExistsExcption()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var supplier = new Supplier
            {
                ID = 1,
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var supplierDTO = new SupplierDTO
            {
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var request = new CreateSupplierCommand(supplierDTO);

            supplierRepositoryMock.Setup(x => x.IsSupplierExistByEmailAsync(request.supplierDTO.Email))
                .ReturnsAsync(true);

            var handler = new CreateSupplierHandler(supplierRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WhenPhoneNumberAlreadyExist_ShouldThrowAlreadyExistsExcption()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var supplier = new Supplier
            {
                ID = 1,
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var supplierDTO = new SupplierDTO
            {
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var request = new CreateSupplierCommand(supplierDTO);

            supplierRepositoryMock.Setup(x => x.IsSupplierExistByEmailAsync(request.supplierDTO.Email))
                .ReturnsAsync(false);

            supplierRepositoryMock.Setup(x => x.IsSupplierExistByPhoneAsync(request.supplierDTO.PhoneNumber))
                .ReturnsAsync(true);

            var handler = new CreateSupplierHandler(supplierRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);


            await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WhenRequestValid_ShouldCreateCustomer()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var supplier = new Supplier
            {
                ID = 1,
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var supplierDTO = new SupplierDTO
            {
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var request = new CreateSupplierCommand(supplierDTO);

            supplierRepositoryMock.Setup(x => x.IsSupplierExistByEmailAsync(request.supplierDTO.Email))
                .ReturnsAsync(false);

            supplierRepositoryMock.Setup(x => x.IsSupplierExistByEmailAsync(request.supplierDTO.PhoneNumber))
                .ReturnsAsync(false);


            mapperMock.Setup(x => x.Map<Supplier>(supplierDTO)).Returns(supplier);
            mapperMock.Setup(x => x.Map<SupplierResponseDTO>(It.IsAny<Supplier>()))
                .Returns(new SupplierResponseDTO
                {
                    ID = supplier.ID,
                    Name = supplier.Name,
                    PhoneNumber = supplier.PhoneNumber,
                    Email = supplier.Email,
                    Address = supplier.Address
                });

            var handler = new CreateSupplierHandler(supplierRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(supplier.ID, result.ID);
            Assert.Equal(supplier.Name, result.Name);
            Assert.Equal(supplier.Email, result.Email);
            Assert.Equal(supplier.PhoneNumber, result.PhoneNumber);
            Assert.Equal(supplier.Address, result.Address);

            supplierRepositoryMock.Verify(x => x.CreateSupplier(supplier), Times.Once);
            unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}
