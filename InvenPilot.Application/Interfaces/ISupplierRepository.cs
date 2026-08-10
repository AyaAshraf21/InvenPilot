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
        public Task<bool> IsSupplierExistByIdAsync(int id);
        public Task<List<Supplier>> GetAllSuppliersAsync(SupplierQueryParameters supplierQueryParameters);
        public void CreateSupplierAsync(Supplier supplier);
        public void UpdateSupplierAsync(Supplier supplier);
        public void DeleteSupplierAsync(Supplier supplier);
    }
}
