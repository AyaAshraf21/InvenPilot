using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.Commands.DeleteCustomer;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Customers.Queries.GetCustomerById;
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
    public class GetCustomerByIdHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCustomerNotFound_ShouldThrowNotFoundException()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var mapperMock = new Mock<IMapper>();

            var request = new GetCustomerByIdQuery(1);

            customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(request.id)).ReturnsAsync((Customer?)null);

            var handler = new GetCustomerByIdHandler(customerRepositoryMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));

        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetCustomerById()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var mapperMock = new Mock<IMapper>();

            var request = new GetCustomerByIdQuery(1);

            var customer = new Customer
            {
                ID = 1,
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(request.id)).ReturnsAsync(customer);

            mapperMock.Setup(x => x.Map<CustomerResponseDTO>(It.Is<Customer>(c =>
                c.ID == customer.ID &&
                c.Name == customer.Name &&
                c.Email == customer.Email &&
                c.PhoneNumber == customer.PhoneNumber &&
                c.Address == customer.Address
            ))).Returns(new CustomerResponseDTO
            {
                ID = customer.ID,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            });

            var handler = new GetCustomerByIdHandler(customerRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(customer.ID, result.ID);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.Email, result.Email);
            Assert.Equal(customer.PhoneNumber, result.PhoneNumber);
            Assert.Equal(customer.Address, result.Address);

            customerRepositoryMock.Verify(x => x.GetCustomerByIdAsync(1), Times.Once());

        }
    }
}
