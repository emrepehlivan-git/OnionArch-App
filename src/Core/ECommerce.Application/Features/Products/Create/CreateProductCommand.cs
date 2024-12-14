using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Create;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId
) : IRequest<Result<Guid>>, IValidetableRequest, ITransactionalRequest;