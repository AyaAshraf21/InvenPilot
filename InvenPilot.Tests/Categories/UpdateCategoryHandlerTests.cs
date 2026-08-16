using AutoMapper;
using InvenPilot.Application.Exceptions;
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
    public class UpdateCategoryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCategoryIDNotFound_ShouldThrowNotFoundException()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var categoryDTO = new CategoryDTO
            {
                Name = "TVs"
            };

            var request = new UpdateCategoryCommand(categoryDTO, 1);

            categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync((Category?)null);

            var handler = new UpdateCategoryHandler(categoryRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));

            categoryRepositoryMock.Verify(
                x => x.UpdateCategory(It.IsAny<Category>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }


        [Fact]
        public async Task Handle_WhenCategoryNameAlreadyExist_ShouldThrowAlreadyExistsException()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var categoryDTO = new CategoryDTO
            {
                Name = "Phones"
            };

            var request = new UpdateCategoryCommand(categoryDTO, 1);

            categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync(category);
            categoryRepositoryMock.Setup(x => x.isCategoryExistByNameAsync(request.categoryDTO.Name)).ReturnsAsync(true);

            var handler = new UpdateCategoryHandler(categoryRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(request, CancellationToken.None));

            categoryRepositoryMock.Verify(
                x => x.UpdateCategory(It.IsAny<Category>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }


        [Fact]
        public async Task Handle_WhenRequestValid_ShouldUpdateCategory()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var categoryDTO = new CategoryDTO
            {
                Name = "Phones"
            };
            var updatedCategory = new Category
            {
                ID = 1,
                Name = "Phones"
            };

            var request = new UpdateCategoryCommand(categoryDTO, 1);

            categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync(category);
            categoryRepositoryMock.Setup(x => x.isCategoryExistByNameAsync(request.categoryDTO.Name)).ReturnsAsync(false);

            var handler = new UpdateCategoryHandler(categoryRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);

            mapperMock.Setup(x => x.Map<Category>(categoryDTO)).Returns(updatedCategory);

            mapperMock.Setup(x => x.Map<CategoryResponseDTO>(It.IsAny<Category>())).Returns(new CategoryResponseDTO
            {
                ID = updatedCategory.ID,
                Name = updatedCategory.Name
            });

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(updatedCategory.ID, result.ID);
            Assert.Equal(updatedCategory.Name, result.Name);
            
            categoryRepositoryMock.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once());

            unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once());
        }
    }
}
