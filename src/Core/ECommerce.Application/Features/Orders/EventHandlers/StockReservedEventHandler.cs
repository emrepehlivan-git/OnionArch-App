using ECommerce.Domain.DomainEvents.Orders;
using MediatR;

namespace ECommerce.Application.Features.Orders.EventHandlers;

public sealed class StockReservedEventHandler(IStockService stockService) : INotificationHandler<StockReservedEvent>
{
    public async Task Handle(StockReservedEvent notification, CancellationToken cancellationToken)
    {
        await stockService.ReserveStock(notification.ProductId, notification.Quantity, cancellationToken);
    }
}
