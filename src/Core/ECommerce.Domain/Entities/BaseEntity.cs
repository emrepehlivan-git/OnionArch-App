using ECommerce.Domain.Interfaces;

namespace ECommerce.Domain.Entities;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; }
}