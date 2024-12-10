using ECommerce.Application.Repositories;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.Delete;

public sealed class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository
) : IRequestHandler<DeleteCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var deleted = await categoryRepository.DeleteAsync(request.Id, cancellationToken);

        if (!deleted)
            return Result<Guid>.Failure(CategoryErrors.CategoryNotFound);

        return Result<Guid>.Success(request.Id);
    }
}