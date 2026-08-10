using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.DTO
{
    public class OrderDTO
    {
        public OrderType OrderType { get; set; }
        public int? SupplierID { get; set; }
        public int? CustomerID { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
