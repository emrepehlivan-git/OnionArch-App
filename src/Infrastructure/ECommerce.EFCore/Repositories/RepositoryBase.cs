using System.Linq.Expressions;
using ECommerce.Application.Extenions;
using ECommerce.Application.Repositories;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.EFCore.Repositories;

public abstract class RepositoryBase<TContext, TEntity> : IRepository<TEntity>
where TContext : DbContext
where TEntity : class, IEntity
{
    protected readonly TContext _context;
    private DbSet<TEntity> Table => _context.Set<TEntity>();

    protected RepositoryBase(TContext context) => _context = context;

    public IQueryable<TEntity> GetByCondition(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, string>>[]? includes = null,
        bool trackChanges = false)
    {
        var query = Table.AsQueryable();
        if (includes is not null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        return trackChanges ? query.Where(expression) : query.AsNoTracking().Where(expression);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Table.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await Table.AnyAsync(expression, cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Table.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public TEntity Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public bool Delete(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Deleted;
        return true;
    }

    public IEnumerable<bool> DeleteRange(IEnumerable<TEntity> entities)
    {
        Table.RemoveRange(entities);
        return entities.Select(entity => true);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return false;
        Delete(entity);
        return true;
    }

    public async Task<PaginatedResult<TEntity>> GetAllAsync(
        PaginationParams paginationParams,
        Expression<Func<TEntity, bool>>? expression = null,
        Expression<Func<TEntity, string>>[]? includes = null,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        var query = GetByCondition(expression, includes, trackChanges);
        return await query.ToPaginatedResultAsync(paginationParams.PageNumber, paginationParams.PageSize, cancellationToken);
    }
}
