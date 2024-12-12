using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Products.GetAll;

public sealed class GetAllProductsQueryHandler(IProductRepository productRepository) : IRequestHandler<GetAllProductsQuery, PaginatedResult<ProductDto>>
{
    public async Task<PaginatedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetAllAsync(
            request.PaginationParams,
            cancellationToken: cancellationToken
        );

        var products = result.Items.Adapt<IEnumerable<ProductDto>>();
        return new PaginatedResult<ProductDto>(products, request.PaginationParams.PageNumber, request.PaginationParams.PageSize, result.TotalPages, result.TotalCount);
    }
}
