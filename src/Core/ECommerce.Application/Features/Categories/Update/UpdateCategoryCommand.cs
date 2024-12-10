using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.Update;

public sealed record UpdateCategoryCommand(Guid Id, string Name) : IRequest<Result<Guid>>, IValidetableRequest, ITransactionalRequest;
