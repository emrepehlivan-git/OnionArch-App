using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Products.GetById;

public sealed class ProductGetByIdQueryHandler(IProductRepository productRepository) : IRequestHandler<ProductGetByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(ProductGetByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        return product is null ? Result<ProductDto>.Failure(ProductErrors.ProductNotFound) : Result<ProductDto>.Success(product.Adapt<ProductDto>());
    }
}
