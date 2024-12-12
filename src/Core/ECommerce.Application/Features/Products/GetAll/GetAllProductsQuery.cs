using ECommerce.Application.Behaviors.Cache;
using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.GetAll;

public sealed record GetAllProductsQuery(PaginationParams PaginationParams) : IRequest<PaginatedResult<ProductDto>>, ICacheableRequest
{
    public string CacheKey => "Products";

    public TimeSpan CacheExpirationTime => TimeSpan.FromMinutes(10);
}
