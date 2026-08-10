using InvenPilot.Application.Features.Categories.DTO;
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

        public void CreateCategoryAsync(Category category)
        {
            context.Categories.Add(category);
        }
        
        public async Task<bool> isCategoryExistByNameAsync(string categoryName)
        {
            return await context.Categories.AnyAsync(s => s.Name == categoryName);
        }
        public async Task<bool> isCategoryExistByIdAsync(int id)
        {
            return await context.Categories.AnyAsync(s => s.ID == id);
        }
        public async Task<List<Category>> GetAllCategoriesAsync(CategoryQueryParameters categoryQueryParameters)
        {
            var query = context.Categories.AsQueryable();

            // search
            if (!string.IsNullOrWhiteSpace(categoryQueryParameters.Search))
            {
                query = query.Where(x => x.Name.Contains(categoryQueryParameters.Search));
            }

            // sorting
            if (categoryQueryParameters.SortBy?.ToLower() == "name") 
            {
                if (categoryQueryParameters.Desc)
                {
                    query = query.OrderByDescending(x => x.Name);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }
            }
            else
            {
                query.OrderBy(x => x.ID);
            }

                //pagination
                query = query.Skip((categoryQueryParameters.Page - 1) * categoryQueryParameters.PerPage)
                                  .Take(categoryQueryParameters.PerPage);

            return await query.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.ID == id);
        }

        public void UpdateCategory(Category category)
        {
            context.Categories.Update(category);
        }

        public void DeleteCategoryAsync(Category category)
        {
            context.Categories.Remove(category);
        }
    }
}
