using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.EFCore.Interceptors;

public sealed class DomainEventSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public DomainEventSaveChangesInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return result;

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .SelectMany(e =>
            {
                var events = e.Entity.DomainEvents.ToList();
                e.Entity.ClearDomainEvents();
                return events;
            }).ToList();

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        return result;
    }
}
