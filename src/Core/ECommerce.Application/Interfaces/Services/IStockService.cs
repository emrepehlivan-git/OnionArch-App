public interface IStockService
{
    Task<bool> IsStockAvailable(Guid productId, int quantity);
    Task ReserveStock(Guid productId, int quantity);
    Task ReleaseStock(Guid productId, int quantity);
}