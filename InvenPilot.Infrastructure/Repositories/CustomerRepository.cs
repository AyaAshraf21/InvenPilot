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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly InvenPilotContext context;

        public CustomerRepository(InvenPilotContext context)
        {
            this.context = context;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await context.Customers.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<bool> IsCustomerExistByEmailAsync(string email)
        {
            return await context.Customers.AnyAsync(c => c.Email == email);
        }

        public async Task<bool> IsCustomerExistByPhoneAsync(string phone)
        {
            return await context.Customers.AnyAsync(c => c.PhoneNumber == phone);
        }
    }
}
