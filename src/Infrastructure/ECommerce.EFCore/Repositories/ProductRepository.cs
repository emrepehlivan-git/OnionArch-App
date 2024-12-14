using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.EFCore.Contexts;

namespace ECommerce.EFCore.Repositories;

public class ProductRepository : RepositoryBase<ECommerceDbContext, Product>, IProductRepository
{
    public ProductRepository(ECommerceDbContext context) : base(context) { }

    public async Task<bool> IsProductInStock(Guid productId, int quantity)
    {
        var product = await GetByIdAsync(productId);
        if (product == null)
            return false;

        return product.Stock >= quantity;
    }
}
