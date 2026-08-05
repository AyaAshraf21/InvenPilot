using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Domain.Entities
{
    public class Order
    {
        public int ID { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public int? SupplierID { get; set; }
        public Supplier? Supplier { get; set; }
        public int? CustomerID { get; set; }
        public Customer? Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
