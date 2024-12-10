using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Repositories;
using ECommerce.Application.Wrappers;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetAll;

public sealed class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetAllCategoriesQuery, PaginatedResult<CategoryDto>>
{
    public async Task<PaginatedResult<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetAllAsync(
            request.Request,
            cancellationToken:cancellationToken
        );
        var items = result.Items.Adapt<List<CategoryDto>>();
        return new PaginatedResult<CategoryDto>(items, request.Request.PageNumber, request.Request.PageSize, result.TotalPages, result.TotalCount);
    }
}

