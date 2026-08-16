using AutoMapper;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Categories.Queries.GetAllCategories;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Tests.Categories
{
    public class GetAllCategoriesHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestValid_ShouldGetAllCategories()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();

            var categoriesList = new List<Category>
            {
                new Category
                {
                    ID = 1,
                    Name = "TVs"
                },
                new Category
                {
                    ID = 2,
                    Name = "Phones"
                }
            };

            var categoriesResponsesList = new List<CategoryResponseDTO>
            {
                new CategoryResponseDTO
                {
                    ID = 1,
                    Name = "TVs"
                },
                new CategoryResponseDTO
                {
                    ID = 2,
                    Name = "Phones"
                }
            };
            var categoryQueryParameters = new CategoryQueryParameters();
            var request = new GetAllCategoriesQuery(categoryQueryParameters);

            categoryRepositoryMock.Setup(x => x.GetAllCategoriesAsync(categoryQueryParameters)).ReturnsAsync(categoriesList);

            mapperMock.Setup(x => x.Map<List<CategoryResponseDTO>>(categoriesList)).Returns(categoriesResponsesList);

            var handler = new GetAllCategoriesHandler(categoryRepositoryMock.Object, mapperMock.Object);

            var result = await handler.Handle(request,CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(categoriesResponsesList[0].ID, result[0].ID);
            Assert.Equal(categoriesResponsesList[0].Name, result[0].Name);
            Assert.Equal(categoriesResponsesList[1].ID, result[1].ID);
            Assert.Equal(categoriesResponsesList[1].Name, result[1].Name);
            
            categoryRepositoryMock.Verify(x => x.GetAllCategoriesAsync(categoryQueryParameters), Times.Once());
            mapperMock.Verify(
                x => x.Map<List<CategoryResponseDTO>>(categoriesList),
                Times.Once
            );
        }
    }
}
