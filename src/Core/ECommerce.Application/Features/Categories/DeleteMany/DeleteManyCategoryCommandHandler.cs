using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Categories.DeleteMany;

public sealed class DeleteManyCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<DeleteManyCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteManyCategoryCommand request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetByCondition(c => request.Ids.Contains(c.Id)).ToListAsync(cancellationToken);
        categoryRepository.DeleteRange(categories);
        return Result.Success();
    }
}
