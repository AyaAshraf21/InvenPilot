using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Interfaces
{
    public interface ICategoryRepository 
    {
        public void CreateCategoryAsync(Category category);
        public Task<bool> isCategoryExistByNameAsync(string categoryName);
        public Task<bool> isCategoryExistByIdAsync(int id);
        public Task<List<Category>> GetAllCategoriesAsync(CategoryQueryParameters categoryQueryParameters);
        public Task<Category> GetCategoryByIdAsync(int id);
        public void UpdateCategory(Category category);
        public void DeleteCategoryAsync(Category category);
    }
}
