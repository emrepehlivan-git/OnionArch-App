using ECommerce.Application.Wrappers;

public interface IStockService
{
    Task<bool> IsStockAvailable(Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task<Result> ReserveStock(Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task ReleaseStock(Guid productId, int quantity, CancellationToken cancellationToken = default);
}
