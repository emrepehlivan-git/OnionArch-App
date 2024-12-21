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
        var productStock = context.ProductStocks.FirstOrDefault(ps => ps.ProductId == productId && ps.Stock >= quantity);
        if (productStock is not null)
        {
            context.ProductStocks.Update(productStock);
            await context.SaveChangesAsync();
        }
        else
        {
            logger.LogError("Stock not available for product {ProductId}", productId);
        }
    }

    public async Task ReleaseStock(Guid productId, int quantity)
    {
        var productStock = context.ProductStocks.FirstOrDefault(ps => ps.ProductId == productId);
        if (productStock is not null)
        {
            context.ProductStocks.Update(productStock);
            await context.SaveChangesAsync();
        }
    }
}
