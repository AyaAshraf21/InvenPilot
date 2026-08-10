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
        public void CreateProductAsync(Product product);
        public Task<bool> IsProductExistByNameAsync(string name);
        public void UpdateProductAsync(Product product);
        public Task<Product> GetProductByIdAsync(int id);
        public Task<bool> IsProductExistByIDAsync(int id);
        public Task<List<Product>> GetAllProductsAsync(ProductQueryParameters productQueryParameters);
        public void DeleteProductAsync(Product product);
        public Task<List<Product>> GetProductsByIdAsync(List<int> productIDs);
    }
}
