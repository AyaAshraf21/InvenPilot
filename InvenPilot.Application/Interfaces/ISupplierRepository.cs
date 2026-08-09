using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Interfaces
{
    public interface ISupplierRepository
    {
        public Task<Supplier> GetSupplierByIdAsync(int id);
        public Task<bool> IsSupplierExistByPhoneAsync(string phone);
        public Task<bool> IsSupplierExistByEmailAsync(string email);
        public Task<List<Supplier>> GetAllSuppliersAsync(SupplierQueryParameters supplierQueryParameters);
        public Task CreateSupplierAsync(Supplier supplier);
        public Task UpdateSupplierAsync(Supplier supplier);
        public Task DeleteSupplierAsync(Supplier supplier);
    }
}
