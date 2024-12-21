using ECommerce.Domain.DomainEvents.Orders;
using MediatR;

namespace ECommerce.Application.Features.Orders.EventHandlers;

public sealed class StockReservedEventHandler(IStockService stockService, IPublisher publisher) : INotificationHandler<StockReservedEvent>
{
    public async Task Handle(StockReservedEvent notification, CancellationToken cancellationToken)
    {
        var result = await stockService.ReserveStock(notification.ProductId, notification.Quantity, cancellationToken);
        if (!result.IsSuccess)
            await publisher.Publish(new StockNotReservedEvent(notification.ProductId, notification.Quantity), cancellationToken);
    }
}
