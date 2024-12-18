public interface IStockService
{
    Task<bool> IsStockAvailable(Guid productId, int quantity);
    Task<bool> ReserveStock(Guid productId, int quantity);
    Task ReleaseStock(Guid productId, int quantity);
}