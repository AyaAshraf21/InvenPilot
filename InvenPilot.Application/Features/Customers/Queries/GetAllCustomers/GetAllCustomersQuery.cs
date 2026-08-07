using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Customers.Queries.GetAllCustomers
{
    public record GetAllCustomersQuery(CustomerQueryParameters customerQueryParameters) : IRequest<List<CustomerResponseDTO>>;
}
