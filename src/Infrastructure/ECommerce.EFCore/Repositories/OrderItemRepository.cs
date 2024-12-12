using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.EFCore.Contexts;

namespace ECommerce.EFCore.Repositories;

public sealed class OrderItemRepository(ECommerceDbContext context) : RepositoryBase<ECommerceDbContext, OrderItem>(context), IOrderItemRepository
{
}
