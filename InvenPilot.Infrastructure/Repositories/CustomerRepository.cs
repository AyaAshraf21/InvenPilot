using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Customers.DTO;
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

        public async Task CreateCustomerAsync(Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAllCustomersAsync(CustomerQueryParameters customerQueryParameters)
        {
            var query = context.Customers.AsQueryable();

            //search
            if (!string.IsNullOrWhiteSpace(customerQueryParameters.Search))
            {
                query = query.Where(c =>
                    c.Name.Contains(customerQueryParameters.Search) ||
                    c.Email.Contains(customerQueryParameters.Search) ||
                    c.PhoneNumber.Contains(customerQueryParameters.Search));
            }

            //sorting

            if (customerQueryParameters.SortBy?.ToLower() == "name")
            {
                if (customerQueryParameters.Desc)
                {
                    query = query.OrderByDescending(x => x.Name);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }
            }
            else
            {
                query.OrderBy(x => x.ID);
            }

            //pagination
            query = query.Skip((customerQueryParameters.Page - 1) * customerQueryParameters.PerPage)
                         .Take(customerQueryParameters.PerPage);

            return await query.ToListAsync();
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

        public async Task UpdateCustomerAsync(Customer customer)
        {
            context.Customers.Update(customer);
            await context.SaveChangesAsync();
        }
    }
}
