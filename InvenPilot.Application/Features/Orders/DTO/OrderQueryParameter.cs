using InvenPilot.Application.Common.Pagination;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Orders.DTO
{
    public class OrderQueryParameter : BaseQueryParamerters
    {
        public OrderType? OrderType { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public int? CustomerID { get; set; }
        public int? SupplierID { get; set; }

    }
}
