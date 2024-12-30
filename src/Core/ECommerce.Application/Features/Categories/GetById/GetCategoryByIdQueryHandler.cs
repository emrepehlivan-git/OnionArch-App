using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetById;

public sealed class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        return category is null ? Result<CategoryDto>.Failure(CategoryErrors.CategoryNotFound) : Result<CategoryDto>.Success(category.Adapt<CategoryDto>());
    }
}
