namespace ECommerce.Domain.Interfaces;

public interface IUpdatableEntity : IEntity
{
    Guid? UpdatedById { get; set; }
    DateTime? UpdatedAt { get; set; }
}