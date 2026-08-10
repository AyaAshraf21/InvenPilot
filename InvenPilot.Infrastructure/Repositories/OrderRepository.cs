using InvenPilot.Application.Features.Orders.DTO;
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

        public async Task<List<Order>> GetAllOrdersAsync(OrderQueryParameter orderQueryParameter)
        {
            var query = context.Orders.Include(x => x.OrderItems).AsQueryable();

            if (orderQueryParameter.CustomerID.HasValue)
            {
                query = query.Where(x => x.CustomerID ==  orderQueryParameter.CustomerID.Value);
            }

            if (orderQueryParameter.SupplierID.HasValue)
            {
                query = query.Where(x => x.SupplierID ==  orderQueryParameter.SupplierID.Value);
            }

            if (orderQueryParameter.OrderType.HasValue)
            {
                query = query.Where(x => x.OrderType == orderQueryParameter.OrderType.Value);
            }

            if (orderQueryParameter.OrderStatus.HasValue)
            {
                query = query.Where(x => x.OrderStatus == orderQueryParameter.OrderStatus.Value);
            }

            if(orderQueryParameter.SortBy != null)
            {
                if(orderQueryParameter.SortBy?.ToLower() == "date")
                {
                    query = orderQueryParameter.Desc 
                        ? query.OrderByDescending(x => x.OrderDate) 
                        : query.OrderBy(x => x.OrderDate);

                        
                }
                else
                {
                    query = query.OrderBy(x => x.ID);
                }
            }

            query = query.Skip((orderQueryParameter.Page - 1) * orderQueryParameter.PerPage).Take(orderQueryParameter.PerPage);

            return await query.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.ID == id);
        }

        public void UpdateOrderStatus(Order order)
        {
            context.Orders.Update(order);
        }
    }
}
