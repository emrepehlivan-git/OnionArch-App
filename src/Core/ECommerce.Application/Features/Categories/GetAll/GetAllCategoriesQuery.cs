using ECommerce.Application.Behaviors.Cache;
using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetAll;

public record GetAllCategoriesQuery(PaginationParams Request) : IRequest<PaginatedResult<CategoryDto>>, ICacheableRequest
{
    public string CacheKey => "Categories";

    public TimeSpan CacheExpirationTime => TimeSpan.FromMinutes(10);
}

