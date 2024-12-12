using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetById;

public sealed class GetByIdQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetByIdQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        return category is null ? Result<CategoryDto>.Failure(CategoryErrors.CategoryNotFound) : Result<CategoryDto>.Success(category.Adapt<CategoryDto>());
    }
}
