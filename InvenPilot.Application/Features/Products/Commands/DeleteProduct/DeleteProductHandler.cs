using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private IProductRepository productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetProductByIdAsync(request.id);
            if (product == null)
            {
                throw new NotFoundException("Product", request.id);
            }
            await productRepository.DeleteProductAsync(product);
        }
    }
}
