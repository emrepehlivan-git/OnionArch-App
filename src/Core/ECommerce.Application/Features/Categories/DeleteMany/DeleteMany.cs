using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.DeleteMany;

public record class DeleteManyCategoryCommand(List<Guid> Ids) : IRequest<Result>, IValidetableRequest, ITransactionalRequest;
