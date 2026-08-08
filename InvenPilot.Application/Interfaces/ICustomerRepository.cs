using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomerByIdAsync(int id);
        public Task<bool> IsCustomerExistByPhoneAsync(string phone);
        public Task<bool> IsCustomerExistByEmailAsync(string email);
        public Task<List<Customer>> GetAllCustomersAsync(CustomerQueryParameters customerQueryParameters);
        public Task CreateCustomerAsync(Customer customer);
        public Task UpdateCustomerAsync(Customer customer);
    }
}
