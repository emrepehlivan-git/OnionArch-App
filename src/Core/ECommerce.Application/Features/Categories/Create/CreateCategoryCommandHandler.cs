using ECommerce.Application.Repositories;
using ECommerce.Application.Wrappers;
using MediatR;
using CategoryEntity = ECommerce.Domain.Entities.Category;

namespace ECommerce.Application.Features.Categories.Create;

public sealed class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository
) : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = CategoryEntity.Create(request.Name);
        var addedCategory = await categoryRepository.AddAsync(category, cancellationToken);

        if (addedCategory is null)
            return Result<Guid>.Failure(CategoryErrors.CategoryNotAdded);

        return Result<Guid>.Success(addedCategory.Id);
    }
}

