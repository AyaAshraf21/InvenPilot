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
        public Task<bool> isProductExistByNameAsync(string name);
    }
}
