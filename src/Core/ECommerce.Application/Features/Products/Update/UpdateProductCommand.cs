using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Update;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid CategoryId
) : IRequest<Result<Guid>>, IValidetableRequest, ITransactionalRequest;