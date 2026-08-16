using AutoMapper;
using Castle.Core.Resource;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Customers.Queries.GetAllCustomers;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Customers
{
    public class GetAllCustomersHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetAllCustomers()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var mapperMock = new Mock<IMapper>();

            var customersList = new List<Customer>
            {
                new Customer
                {
                    ID = 1,
                    Name = "Ali",
                    Email = "ali@gmail.com",
                    PhoneNumber = "01114477889",
                    Address = "Giza"
                },
                new Customer
                {
                    ID = 2,
                    Name = "ahmed",
                    Email = "ahmed@gmail.com",
                    PhoneNumber = "01145879652",
                    Address = "Cairo"
                }
            };

            var customersResponseList = new List<CustomerResponseDTO>
            {
                new CustomerResponseDTO
                {
                    ID = 1,
                    Name = "Ali",
                    Email = "ali@gmail.com",
                    PhoneNumber = "01114477889",
                    Address = "Giza"
                },
                new CustomerResponseDTO
                {
                    ID = 2,
                    Name = "ahmed",
                    Email = "ahmed@gmail.com",
                    PhoneNumber = "01145879652",
                    Address = "Cairo"
                }
            };

            var customersQueryParameters = new CustomerQueryParameters();

            var request = new GetAllCustomersQuery(customersQueryParameters);

            customerRepositoryMock.Setup(x => x.GetAllCustomersAsync(customersQueryParameters)).ReturnsAsync(customersList);
            mapperMock.Setup(x => x.Map<List<CustomerResponseDTO>>(customersList)).Returns(customersResponseList);

            var handler = new GetAllCustomersHandler(customerRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(customersResponseList[0].ID, result[0].ID);
            Assert.Equal(customersResponseList[0].Name, result[0].Name);
            Assert.Equal(customersResponseList[0].Email, result[0].Email);
            Assert.Equal(customersResponseList[0].PhoneNumber, result[0].PhoneNumber);
            Assert.Equal(customersResponseList[0].Address, result[0].Address);

            Assert.Equal(customersResponseList[1].ID, result[1].ID);
            Assert.Equal(customersResponseList[1].Name, result[1].Name);
            Assert.Equal(customersResponseList[1].Email, result[1].Email);
            Assert.Equal(customersResponseList[1].PhoneNumber, result[1].PhoneNumber);
            Assert.Equal(customersResponseList[1].Address, result[1].Address);

            customerRepositoryMock.Verify(x => x.GetAllCustomersAsync(customersQueryParameters), Times.Once);

        }
    }
}
