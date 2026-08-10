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
    public class OrderRepository : IOrderRepository
    {
        private readonly InvenPilotContext context;

        public OrderRepository(InvenPilotContext context)
        {
            this.context = context;
        }

        public void CreateOrder(Order order)
        {
            context.Orders.Add(order);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.ID == id);
        }
    }
}
