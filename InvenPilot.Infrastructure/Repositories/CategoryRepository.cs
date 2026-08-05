using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using InvenPilot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
