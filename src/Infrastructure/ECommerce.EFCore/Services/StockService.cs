using ECommerce.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

public sealed class StockService : IStockService
{
    private readonly ECommerceDbContext context;

    public StockService(ECommerceDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> IsStockAvailable(Guid productId, int quantity)
    {
        return await context.Products.AnyAsync(p => p.Id == productId && p.Stock >= quantity);
    }

    public async Task<bool> ReserveStock(Guid productId, int quantity)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null || product.Stock < quantity)
            return false;

        product.DecreaseStock(quantity);
        return true;
    }

    public async Task ReleaseStock(Guid productId, int quantity)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null)
            return;

        product.IncreaseStock(quantity);
    }
}