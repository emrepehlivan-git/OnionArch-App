using ECommerce.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.EFCore.Services;

public sealed class StockService(
    ECommerceDbContext context,
    ILogger<StockService> logger) : IStockService
{
    public async Task<bool> IsStockAvailable(Guid productId, int quantity)
    {
        return await context.Products.AnyAsync(p => p.Id == productId && p.Stock >= quantity);
    }

    public async Task ReserveStock(Guid productId, int quantity)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null || product.Stock < quantity)
        {
            logger.LogError("Stock not available for product {ProductId}", productId);
            return;
        }

        product.DecreaseStock(quantity);
        await context.SaveChangesAsync();
    }

    public async Task ReleaseStock(Guid productId, int quantity)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null)
            return;

        product.IncreaseStock(quantity);
        await context.SaveChangesAsync();
    }
}
