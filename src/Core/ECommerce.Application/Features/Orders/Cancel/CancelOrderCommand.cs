using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Orders.Cancel;

public record CancelOrderCommand(Guid OrderId) : IRequest<Result>, IValidetableRequest, ITransactionalRequest;
