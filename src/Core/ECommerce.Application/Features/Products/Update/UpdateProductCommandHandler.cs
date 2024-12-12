using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Update;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository
) : IRequestHandler<UpdateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        product!.Update(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId
        );

        var updatedProduct = productRepository.Update(product);
        return Result<Guid>.Success(updatedProduct.Id);
    }
}