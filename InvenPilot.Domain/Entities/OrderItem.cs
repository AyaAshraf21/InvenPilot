using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Domain.Entities
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; } = null;
        public int OrderID { get; set; }
        public Order Order { get; set; } = null;

    }
}
