using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Customers.Queries.GetCustomerById;
using InvenPilot.Application.Features.Suppliers.Commands.DeleteSupplier;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Features.Suppliers.Queries.GetSupplierById;
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
    public class GetSupplierByIdHandlerTests
    {
        [Fact]
        public async Task Handle_WhenSupplierNotFound_ShouldThrowNotFoundException()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var mapperMock = new Mock<IMapper>();

            var request = new GetSupplierByIdQuery(2);

            supplierRepositoryMock.Setup(x => x.GetSupplierByIdAsync(request.id)).ReturnsAsync((Supplier?)null);

            var handler = new GetSupplierByIdHandler(supplierRepositoryMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));

        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetSupplierById()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var mapperMock = new Mock<IMapper>();

            var supplier = new Supplier
            {
                ID = 1,
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var request = new GetSupplierByIdQuery(1);

            supplierRepositoryMock.Setup(x => x.GetSupplierByIdAsync(request.id)).ReturnsAsync(supplier);

            mapperMock.Setup(x => x.Map<SupplierResponseDTO>(It.Is<Supplier>(s =>
                s.ID == supplier.ID &&
                s.Name == supplier.Name &&
                s.Email == supplier.Email &&
                s.PhoneNumber == supplier.PhoneNumber &&
                s.Address == supplier.Address
            ))).Returns(new SupplierResponseDTO
            {
                ID = supplier.ID,
                Name = supplier.Name,
                Email = supplier.Email,
                PhoneNumber = supplier.PhoneNumber,
                Address = supplier.Address
            });

            var handler = new GetSupplierByIdHandler(supplierRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(supplier.ID, result.ID);
            Assert.Equal(supplier.Name, result.Name);
            Assert.Equal(supplier.Email, result.Email);
            Assert.Equal(supplier.PhoneNumber, result.PhoneNumber);
            Assert.Equal(supplier.Address, result.Address);

            supplierRepositoryMock.Verify(x => x.GetSupplierByIdAsync(1), Times.Once());

        }
    }
}
