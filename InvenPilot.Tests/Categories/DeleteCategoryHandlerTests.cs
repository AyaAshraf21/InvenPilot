using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Categories.Commands.DeleteCategory;
using InvenPilot.Application.Features.Categories.Commands.UpdateCategory;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Categories
{
    public class DeleteCategoryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCategoryIDNotFound_ShouldThrowNotFoundException()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var request = new DeleteCategoryCommand(1);

            categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync((Category?)null);

            var handler = new DeleteCategoryHandler(categoryRepositoryMock.Object, unitOfWorkMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));

            categoryRepositoryMock.Verify(
                x => x.DeleteCategory(It.IsAny<Category>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldDeleteCategory()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var request = new DeleteCategoryCommand(1);

            categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync(category);

            var handler = new DeleteCategoryHandler(categoryRepositoryMock.Object, unitOfWorkMock.Object);

            var result = handler.Handle(request, CancellationToken.None);

            categoryRepositoryMock.Verify(
                x => x.DeleteCategory(It.IsAny<Category>()),
                Times.Once
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
