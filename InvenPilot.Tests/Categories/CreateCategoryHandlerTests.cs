using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Categories.Commands.CreateCategory;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Categories
{
    public class CreateCategoryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCategoryNameAlreadyExists_ShouldThrowAlreadyExistsException()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var categoryDTO = new CategoryDTO
            {
                Name = "TVs"
            };

            var request = new CreateCategoryCommand(categoryDTO);

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByNameAsync(request.categoryDTO.Name))
                .ReturnsAsync(true);

            var handler = new CreateCategoryHandler(
                categoryRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            await Assert.ThrowsAsync<AlreadyExistsException>(
                () => handler.Handle(request, CancellationToken.None)
             );

            categoryRepositoryMock.Verify(
                x => x.CreateCategory(It.IsAny<Category>()),
                Times.Never
            );

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_WhenRequestValid_ShouldCreateCategory()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var categoryDTO = new CategoryDTO
            {
                Name = "TVs"
            };

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var categoryResponseDTO = new CategoryResponseDTO
            {
                ID = category.ID,
                Name = category.Name,
            };

            var request = new CreateCategoryCommand(categoryDTO);

            categoryRepositoryMock
                .Setup(x => x.isCategoryExistByNameAsync(request.categoryDTO.Name))
                .ReturnsAsync(false);

            mapperMock
                .Setup(x => x.Map<Category>(categoryDTO))
                .Returns(category);

            mapperMock
                .Setup(x => x.Map<CategoryResponseDTO>(category))
                .Returns(categoryResponseDTO);

            var handler = new CreateCategoryHandler(
                categoryRepositoryMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object
            );

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(categoryResponseDTO.ID, result.ID );
            Assert.Equal(categoryResponseDTO.Name, result.Name );

            categoryRepositoryMock.Verify(x => x.CreateCategory(category), Times.Once);
            unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        }
    }
}
