using ECommerce.Application.Behaviors.Cache;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.GetById;

public sealed record ProductGetByIdQuery(Guid Id) : IRequest<Result<ProductDto>>, IValidetableRequest, ICacheableRequest
{
    public string CacheKey => $"Product-{Id}";

    public TimeSpan CacheExpirationTime => TimeSpan.FromMinutes(1);
}