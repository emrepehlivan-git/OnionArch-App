using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> IsProductInStock(Guid productId, int quantity);
}
