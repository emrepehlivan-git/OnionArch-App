using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.EFCore.Contexts;

namespace ECommerce.EFCore.Repositories;

public class ProductRepository : RepositoryBase<ECommerceDbContext, Product>, IProductRepository
{
    public ProductRepository(ECommerceDbContext context) : base(context) { }
}
