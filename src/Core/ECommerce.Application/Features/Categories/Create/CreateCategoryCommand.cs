using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.Create;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result<Guid>>, IValidetableRequest, ITransactionalRequest;

