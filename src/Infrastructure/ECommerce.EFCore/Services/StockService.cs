using ECommerce.Application.Wrappers;
using ECommerce.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.EFCore.Services;

public sealed class StockService(
    ECommerceDbContext context,
    ILogger<StockService> logger) : IStockService
{
    public async Task<bool> IsStockAvailable(Guid productId, int quantity, CancellationToken cancellationToken = default)
        => await context.Products.AnyAsync(p => p.Id == productId && p.Stock >= quantity, cancellationToken);

    public async Task<Result> ReserveStock(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        try
        {
            var productStock = context.ProductStocks.FirstOrDefault(ps => ps.ProductId == productId && ps.Stock >= quantity);
            if (productStock is null)
                return Result.Failure("Stock not available");
            productStock.DecreaseStock(quantity);
            context.ProductStocks.Update(productStock);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reserving stock for product {ProductId}", productId);
            return Result.Failure("Error reserving stock");
        }
    }

    public async Task ReleaseStock(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        try
        {
            var productStock = context.ProductStocks.FirstOrDefault(ps => ps.ProductId == productId);
            if (productStock is not null)
            {
                productStock.IncreaseStock(quantity);
                context.ProductStocks.Update(productStock);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error releasing stock for product {ProductId}", productId);
            throw;
        }
    }
}
