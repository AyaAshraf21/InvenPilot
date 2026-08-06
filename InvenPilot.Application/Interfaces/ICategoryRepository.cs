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
        public Task CreateCategoryAsync(Category category);
        public Task<bool> isCategoryExistByNameAsync(string categoryName);
        public Task<bool> isCategoryExistByIdAsync(int id);
        public Task<List<Category>> GetAllCategoriesAsync(int page, int perPage);
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task UpdateCategory(Category category);
        public Task DeleteCategoryAsync(Category category);
    }
}
