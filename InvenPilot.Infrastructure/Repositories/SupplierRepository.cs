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
