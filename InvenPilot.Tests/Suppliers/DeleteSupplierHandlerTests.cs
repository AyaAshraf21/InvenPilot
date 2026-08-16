using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.Commands.DeleteCustomer;
using InvenPilot.Application.Features.Suppliers.Commands.DeleteSupplier;
using InvenPilot.Application.Features.Suppliers.Commands.UpdateSupplier;
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
    public class DeleteSupplierHandlerTests
    {
        [Fact]
        public async Task Handle_WhenSupplierNotFound_ShouldThrowNotFoundException()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new DeleteSupplierCommand(2);

            supplierRepositoryMock.Setup(x => x.GetSupplierByIdAsync(request.id)).ReturnsAsync((Supplier?)null);

            var handler = new DeleteSupplierHandler(supplierRepositoryMock.Object, unitOfWorkMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));

        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldDeleteSupplier()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var supplier = new Supplier
            {
                ID = 1,
                Name = "TechZone Supplies",
                PhoneNumber = "01012345678",
                Email = "contact@techzone.com",
                Address = "Nasr City, Cairo"
            };

            var request = new DeleteSupplierCommand(1);

            supplierRepositoryMock.Setup(x => x.GetSupplierByIdAsync(request.id)).ReturnsAsync(supplier);

            var handler = new DeleteSupplierHandler(supplierRepositoryMock.Object, unitOfWorkMock.Object);


            var result = handler.Handle(request, CancellationToken.None);

            supplierRepositoryMock.Verify(
                x => x.DeleteSupplier(supplier),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }
    }
}
