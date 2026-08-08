using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Suppliers.DTO;
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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly InvenPilotContext context;

        public SupplierRepository(InvenPilotContext context)
        {
            this.context = context;
        }

        public Task<List<Supplier>> GetAllSuppliersAsync(SupplierQueryParameters supplierQueryParameters)
        {
            var query = context.Suppliers.AsQueryable();

            //search
            if (!string.IsNullOrWhiteSpace(supplierQueryParameters.Search))
            {
                query = query.Where(s => s.Name.Contains(supplierQueryParameters.Search) ||
                                         s.Email.Contains(supplierQueryParameters.Search) ||
                                         s.PhoneNumber.Contains(supplierQueryParameters.Search));
            }

            // sorting
            if (supplierQueryParameters.SortBy?.ToLower() == "name")
            {
                if (supplierQueryParameters.Desc)
                {
                    query = query.OrderByDescending(s => s.Name);
                }
                else
                {
                    query = query.OrderBy(s => s.Name);
                }
            }
            else
            {
                query.OrderBy(s => s.ID);
            }

            //pagination
            query = query.Skip((supplierQueryParameters.Page - 1) * supplierQueryParameters.PerPage)
                         .Take(supplierQueryParameters.PerPage);

            return query.ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await context.Suppliers.FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<bool> IsSupplierExistByEmailAsync(string email)
        {
            return await context.Suppliers.AnyAsync(s => s.Email == email);
        }

        public async Task<bool> IsSupplierExistByPhoneAsync(string phone)
        {
            return await context.Suppliers.AnyAsync(s => s.PhoneNumber == phone);
        }
    }
}
