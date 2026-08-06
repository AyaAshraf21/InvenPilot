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
    public class ProductRepository : IProductRepository
    {
        private readonly InvenPilotContext context;

        public ProductRepository(InvenPilotContext context)
        {
            this.context = context;
        }

        public async Task CreateProductAsync(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<bool> IsProductExistByIDAsync(int id)
        {
            return await context.Products.AnyAsync(p => p.ID == id);
        }

        public async Task<bool> IsProductExistByNameAsync(string name)
        {
            return await context.Products.AnyAsync(p => p.Name == name);
        }

        public async Task UpdateProductAsync(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync(int page, int perPage)
        {
            return await context.Products.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
