using ECommerce.Application.Repositories;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.Update;

public sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository
) : IRequestHandler<UpdateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        category.Update(request.Name);
        var updatedCategory = categoryRepository.Update(category);

        if (updatedCategory is null)
            return Result<Guid>.Failure(CategoryErrors.CategoryNotUpdated);

        return Result<Guid>.Success(updatedCategory.Id);
    }
}