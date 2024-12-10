using ECommerce.Domain.Interfaces;

namespace ECommerce.Domain.Entities;

public abstract class BaseEntity : IEntity, ICreatedByEntity, IUpdatableEntity
{
    public Guid Id { get; set; }
    public Guid? CreatedById { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? UpdatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
}