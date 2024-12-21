using ECommerce.Domain.Interfaces;

namespace ECommerce.Domain.DomainEvents.Orders;

public record class StockNotReservedEvent(Guid ProductId, int Quantity) : IDomainEvent;
