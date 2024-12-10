using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest<Result<Guid>>, ITransactionalRequest;
