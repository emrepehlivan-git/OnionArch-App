using FluentValidation;

namespace ECommerce.Application.Features.Categories.DeleteMany;

public sealed class DeleteManyCategoryCommandValidator : AbstractValidator<DeleteManyCategoryCommand>
{
    public DeleteManyCategoryCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
