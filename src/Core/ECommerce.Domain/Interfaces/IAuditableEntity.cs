namespace ECommerce.Domain.Interfaces;

public interface IAuditableEntity : IEntity
{
    Guid? CreatedBy { get; set; }
    DateTime? CreatedAt { get; set; }
    Guid? UpdatedBy { get; set; }
    DateTime? UpdatedAt { get; set; }
}