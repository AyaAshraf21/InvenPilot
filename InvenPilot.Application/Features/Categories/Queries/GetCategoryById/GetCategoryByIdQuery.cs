using InvenPilot.Application.Features.Categories.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int id) : IRequest<CategoryResponseDTO> { }
}
