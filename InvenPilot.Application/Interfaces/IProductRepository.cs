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
        public Task<List<Product>> GetAllProductsAsync(int page, int perPage);
        public Task DeleteProductAsync(Product product);
    }
}
