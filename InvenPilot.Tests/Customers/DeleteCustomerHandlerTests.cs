using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Customers.Commands.DeleteCustomer;
using InvenPilot.Application.Features.Customers.Commands.UpdateCustomer;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Products.Commands.DeleteProduct;
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
    public class DeleteCustomerHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCustomerNotFound_ShouldThrowNotFoundException()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new DeleteCustomerCommand(2);

            customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(request.id)).ReturnsAsync((Customer?)null);

            var handler = new DeleteCustomerHandler(customerRepositoryMock.Object, unitOfWorkMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));

        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldDeleteCustomer()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var customer = new Customer
            {
                ID = 1,
                Name = "Ali",
                Email = "ali@gmail.com",
                PhoneNumber = "01114477889",
                Address = "Giza"
            };

            var request = new DeleteCustomerCommand(2);

            customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(request.id)).ReturnsAsync(customer);

            var handler = new DeleteCustomerHandler(
                customerRepositoryMock.Object,
                unitOfWorkMock.Object
            );

            var result = handler.Handle(request, CancellationToken.None);

            customerRepositoryMock.Verify(
                x => x.DeleteCustomer(customer),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(CancellationToken.None),
                Times.Once
            );
        }
    }
}
