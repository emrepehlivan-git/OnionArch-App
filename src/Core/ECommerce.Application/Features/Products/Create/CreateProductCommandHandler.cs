using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using MediatR;
using ProductEntity = ECommerce.Domain.Entities.Product;

namespace ECommerce.Application.Features.Products.Create;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository
) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Result<Guid>.Failure(ProductErrors.CategoryNotFound);

        var product = ProductEntity.Create(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId
        );

        var addedProduct = await productRepository.AddAsync(product, cancellationToken);

        if (addedProduct is null)
            return Result<Guid>.Failure(ProductErrors.ProductNotAdded);

        return Result<Guid>.Success(addedProduct.Id);
    }
}