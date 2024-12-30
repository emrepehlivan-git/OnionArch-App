using ECommerce.Application.Behaviors.Cache;
using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryDto>>, ICacheableRequest
{
    public string CacheKey => $"Category-{Id}";

    public TimeSpan CacheExpirationTime => TimeSpan.FromMinutes(5);
}

