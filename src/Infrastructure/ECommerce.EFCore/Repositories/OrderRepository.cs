using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.EFCore.Repositories;

public sealed class OrderRepository(ECommerceDbContext context) :
    RepositoryBase<ECommerceDbContext, Order>(context),
    IOrderRepository
{
}
