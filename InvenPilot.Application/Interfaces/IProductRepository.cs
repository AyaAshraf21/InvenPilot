using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Interfaces
{
    public interface IProductRepository
    {
        public Task CreateProductAsync(Product product);
        public Task<bool> IsProductExistByNameAsync(string name);
        public Task UpdateProductAsync(Product product);
        public Task<Product> GetProductByIdAsync(int id);
        public Task<bool> IsProductExistByIDAsync(int id);
        public Task<List<Product>> GetAllProductsAsync(ProductQueryParameters productQueryParameters);
        public Task DeleteProductAsync(Product product);
    }
}
