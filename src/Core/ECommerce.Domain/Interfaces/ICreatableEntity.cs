namespace ECommerce.Domain.Interfaces;

public interface ICreatedByEntity : IEntity
{
    Guid? CreatedById { get; set; }
    DateTime? CreatedAt { get; set; }
}
