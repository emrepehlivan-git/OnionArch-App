using ECommerce.Application.Interfaces.Data;
using ECommerce.EFCore.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.EFCore.Data;

public sealed class UnitOfWork(ECommerceDbContext context) : IUnitOfWork
{
    private readonly ECommerceDbContext _context = context;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
