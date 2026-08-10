using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.DTO
{
    public class OrderItemDTO
    {
        public int Quantity { get; set; }
        public int ProductID { get; set; }
    }
}
