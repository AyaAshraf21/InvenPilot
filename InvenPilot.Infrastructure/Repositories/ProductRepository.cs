using InvenPilot.Application.Features.Products.DTO;
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

        public async Task<List<Product>> GetAllProductsAsync(ProductQueryParameters productQueryParameters)
        {
            var query = context.Products.AsQueryable();

            // --------------> search
            if (!string.IsNullOrWhiteSpace(productQueryParameters.Search))
            {
                query = query.Where(p => p.Name.Contains(productQueryParameters.Search));
            }

            // --------------> filter with category id
            if (productQueryParameters.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId ==  productQueryParameters.CategoryId.Value);
            }

            // --------------> filter with min price
            if (productQueryParameters.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= productQueryParameters.MinPrice);
            }

            // --------------> filter with max price
            if (productQueryParameters.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= productQueryParameters.MaxPrice);
            }

            // --------------> filter min quantity
            if (productQueryParameters.MinQuantity.HasValue)
            {
                query = query.Where(p => p.Price >= productQueryParameters.MinQuantity);
            }

            // --------------> filter with max quantity
            if (productQueryParameters.MaxQuantity.HasValue)
            {
                query = query.Where(p => p.Price >= productQueryParameters.MaxQuantity);
            }

            // --------------> filter with out of stock
            if(productQueryParameters.OutOfStock == true)
            {
                query = query.Where(p => p.Quantity == 0);
            }

            // --------------> sotring 
            query = productQueryParameters.SortBy?.ToLower() switch
            {
                "name" => productQueryParameters.Desc
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),

                "price" => productQueryParameters.Desc
                    ? query.OrderByDescending(x => x.Price)
                    : query.OrderBy(x => x.Price),

                "quantity" => productQueryParameters.Desc
                    ? query.OrderByDescending(x => x.Quantity)
                    : query.OrderBy(x => x.Quantity),

                _ => query.OrderBy(x => x.ID),
            };

            // --------------> pagination
            query = query.Skip((productQueryParameters.Page - 1) * productQueryParameters.PerPage)
                         .Take(productQueryParameters.PerPage);

            return await query.ToListAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
