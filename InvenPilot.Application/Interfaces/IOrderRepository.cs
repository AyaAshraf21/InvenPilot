using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Interfaces
{
    public interface IOrderRepository
    {
        public void CreateOrder(Order order);
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<List<Order>> GetAllOrdersAsync(OrderQueryParameter orderQueryParameter);
        public void UpdateOrderStatus (Order order);
    }
}
