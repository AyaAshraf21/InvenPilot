using InvenPilot.Application.Features.Products.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(int id ,ProductDTO productDTO) : IRequest<ProductResponseDTO>;
}
