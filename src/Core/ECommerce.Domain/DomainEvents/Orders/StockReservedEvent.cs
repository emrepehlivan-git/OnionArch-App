using ECommerce.Domain.Interfaces;

namespace ECommerce.Domain.DomainEvents.Orders;

public sealed record StockReservedEvent(Guid ProductId, int Quantity) : IDomainEvent;
