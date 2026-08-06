using InvenPilot.Application.Features.Products.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery : IRequest<List<ProductResponseDTO>>;
}
