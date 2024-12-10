using ECommerce.Application.Repositories;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Delete;

public sealed class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        await productRepository.DeleteAsync(product.Id, cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}
