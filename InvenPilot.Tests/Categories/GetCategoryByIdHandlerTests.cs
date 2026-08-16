using AutoMapper;
using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Features.Categories.Commands.UpdateCategory;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Categories.Queries.GetCategoryById;
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
    public class GetCategoryByIdHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCategoryIDNotFound_ShouldThrowNotFoundException()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var request = new GetCategoryByIdQuery(1);

            categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync((Category?)null);

            var handler = new GetCategoryByIdHandler(categoryRepositoryMock.Object, mapperMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }
        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetCategoryById()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();

            var category = new Category
            {
                ID = 1,
                Name = "TVs"
            };

            var request = new GetCategoryByIdQuery(1);

            categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync(category);

            mapperMock
                .Setup(x => x.Map<CategoryResponseDTO>(It.Is<Category>(c => c.ID == category.ID && c.Name == category.Name)))
                .Returns(new CategoryResponseDTO
                {
                    ID = category.ID,
                    Name = category.Name
                });

            var handler = new GetCategoryByIdHandler(categoryRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(category.ID, result.ID);
            Assert.Equal(category.Name, result.Name);

            categoryRepositoryMock.Verify(x => x.GetCategoryByIdAsync(1), Times.Once());
        }
    }
}
