using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryQuery>
    {
        private readonly ICategoryRepository categoryRepository;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task Handle(DeleteCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(request.id);
            if (category == null)
            {
                throw new NotFoundException("Category", request.id);
            }

            await categoryRepository.DeleteCategoryAsync(category);
        }
    }
}
