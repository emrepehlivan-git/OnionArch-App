using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Delete;

public record DeleteProductCommand(Guid Id) : IRequest<Result<Guid>>, ITransactionalRequest, IValidetableRequest;
