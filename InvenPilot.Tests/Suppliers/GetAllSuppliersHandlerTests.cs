using AutoMapper;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Customers.Queries.GetAllCustomers;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;
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
    public class GetAllSuppliersHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetAllSuppliers()
        {
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var mapperMock = new Mock<IMapper>();

            var suppliersList = new List<Supplier>
            {
                new Supplier
                {
                    ID = 1,
                    Name = "TechZone Supplies",
                    PhoneNumber = "01012345678",
                    Email = "contact@techzone.com",
                    Address = "Nasr City, Cairo"
                },
                new Supplier
                {
                    ID = 2,
                    Name = "Global Electronics",
                    PhoneNumber = "01198765432",
                    Email = "sales@globalelectronics.com",
                    Address = "Dokki, Giza"
                }
            };

            var suppliersResponseList = new List<SupplierResponseDTO>
            {
                new SupplierResponseDTO
                {
                    ID = 1,
                    Name = "TechZone Supplies",
                    PhoneNumber = "01012345678",
                    Email = "contact@techzone.com",
                    Address = "Nasr City, Cairo"
                },
                new SupplierResponseDTO
                {
                    ID = 2,
                    Name = "Global Electronics",
                    PhoneNumber = "01198765432",
                    Email = "sales@globalelectronics.com",
                    Address = "Dokki, Giza"
                }
            };

            var suppliersQueryParameters = new SupplierQueryParameters();

            var request = new GetAllSuppliersQuery(suppliersQueryParameters);

            supplierRepositoryMock.Setup(x => x.GetAllSuppliersAsync(suppliersQueryParameters)).ReturnsAsync(suppliersList);
            mapperMock.Setup(x => x.Map<List<SupplierResponseDTO>>(suppliersList)).Returns(suppliersResponseList);

            var handler = new GetAllSuppliersHandler(supplierRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(suppliersResponseList[0].ID, result[0].ID);
            Assert.Equal(suppliersResponseList[0].Name, result[0].Name);
            Assert.Equal(suppliersResponseList[0].Email, result[0].Email);
            Assert.Equal(suppliersResponseList[0].PhoneNumber, result[0].PhoneNumber);
            Assert.Equal(suppliersResponseList[0].Address, result[0].Address);

            Assert.Equal(suppliersResponseList[1].ID, result[1].ID);
            Assert.Equal(suppliersResponseList[1].Name, result[1].Name);
            Assert.Equal(suppliersResponseList[1].Email, result[1].Email);
            Assert.Equal(suppliersResponseList[1].PhoneNumber, result[1].PhoneNumber);
            Assert.Equal(suppliersResponseList[1].Address, result[1].Address);

            supplierRepositoryMock.Verify(x => x.GetAllSuppliersAsync(suppliersQueryParameters), Times.Once);

        }
    }
}
