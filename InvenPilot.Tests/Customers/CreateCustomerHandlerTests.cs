using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.Commands.CreateCustomer;
using InvenPilot.Application.Features.Customers.DTO;
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
    public class CreateCustomerHandlerTests
    {
        [Fact]
        public async Task Handle_WhenEmailAlreadyExist_ShouldThrowAlreadyExistsExcption()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var customer = new Customer
            {
                ID = 1,
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            var customerDTO = new CustomerDTO
            {
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            var request = new CreateCustomerCommand(customerDTO);

            customerRepositoryMock.Setup(x => x.IsCustomerExistByEmailAsync(request.customerDTO.Email))
                .ReturnsAsync(true);

            var handler = new CreateCustomerHandler(customerRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WhenPhoneNumberAlreadyExist_ShouldThrowAlreadyExistsExcption()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var customer = new Customer
            {
                ID = 1,
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            var customerDTO = new CustomerDTO
            {
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            var request = new CreateCustomerCommand(customerDTO);

            customerRepositoryMock.Setup(x => x.IsCustomerExistByEmailAsync(request.customerDTO.Email))
                .ReturnsAsync(false);
            customerRepositoryMock.Setup(x => x.IsCustomerExistByPhoneAsync(request.customerDTO.PhoneNumber))
                .ReturnsAsync(true);

            var handler = new CreateCustomerHandler(customerRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_WhenRequestValid_ShouldCreateCustomer()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var customer = new Customer
            {
                ID = 1,
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            var customerDTO = new CustomerDTO
            {
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            var request = new CreateCustomerCommand(customerDTO);

            customerRepositoryMock.Setup(x => x.IsCustomerExistByEmailAsync(request.customerDTO.Email))
                .ReturnsAsync(false);
            customerRepositoryMock.Setup(x => x.IsCustomerExistByPhoneAsync(request.customerDTO.PhoneNumber))
                .ReturnsAsync(false);

            mapperMock.Setup(x => x.Map<Customer>(customerDTO)).Returns(customer);
            mapperMock.Setup(x => x.Map<CustomerResponseDTO>(It.IsAny<Customer>()))
                .Returns(new CustomerResponseDTO
                {
                    ID = 1,
                    Name = "Ali",
                    Email = "ali@gmail.com",
                    PhoneNumber = "01114477889",
                    Address = "Giza"
                });

            var handler = new CreateCustomerHandler(customerRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(customer.ID, result.ID);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.Email, result.Email);
            Assert.Equal(customer.PhoneNumber, result.PhoneNumber);
            Assert.Equal(customer.Address, result.Address);

            customerRepositoryMock.Verify(x => x.CreateCustomer(customer), Times.Once);
            unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}
