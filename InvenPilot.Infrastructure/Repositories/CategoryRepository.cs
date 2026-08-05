using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using InvenPilot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly InvenPilotContext context;

        public CategoryRepository(InvenPilotContext context)
        {
            this.context = context;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }
        
        public async Task<bool> isCategoryExistByNameAsync(string categoryName)
        {
            return await context.Categories.AnyAsync(s => s.Name == categoryName);
        }
        public async Task<bool> isCategoryExistByIdAsync(int id)
        {
            return await context.Categories.AnyAsync(s => s.ID == id);
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task UpdateCategory(Category category)
        {
            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }
    }
}
