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
        public Task<bool> IsCustomerExistByIdAsync(int id);
        public Task<List<Customer>> GetAllCustomersAsync(CustomerQueryParameters customerQueryParameters);
        public void CreateCustomerAsync(Customer customer);
        public void UpdateCustomerAsync(Customer customer);
        public void DeleteCustomerAsync(Customer customer);
    }
}
