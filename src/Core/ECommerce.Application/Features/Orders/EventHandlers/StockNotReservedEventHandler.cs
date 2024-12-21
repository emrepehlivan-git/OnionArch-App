using ECommerce.Domain.DomainEvents.Orders;
using MediatR;

namespace ECommerce.Application.Features.Orders.EventHandlers;

public sealed class StockNotReservedEventHandler(IStockService stockService) : INotificationHandler<StockNotReservedEvent>
{
    public async Task Handle(StockNotReservedEvent notification, CancellationToken cancellationToken)
    {
        await stockService.ReleaseStock(notification.ProductId, notification.Quantity);
    }
}
