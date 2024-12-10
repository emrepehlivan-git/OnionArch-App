using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.EFCore.Contexts;

namespace ECommerce.EFCore.Repositories;

public class CategoryRepository : RepositoryBase<ECommerceDbContext, Category>, ICategoryRepository
{
    public CategoryRepository(ECommerceDbContext context) : base(context) { }
}
