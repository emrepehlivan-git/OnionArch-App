using System.Linq.Expressions;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : IEntity
{
   IQueryable<TEntity> GetByCondition(
      Expression<Func<TEntity, bool>> expression,
      Expression<Func<TEntity, string>>[]? includes = null,
      bool trackChanges = false);
   Task<PaginatedResult<TEntity>> GetAllAsync(
      PaginationParams paginationParams,
      Expression<Func<TEntity, bool>>? expression = null,
      Expression<Func<TEntity, string>>[]? includes = null,
      bool trackChanges = false,
      CancellationToken cancellationToken = default);
   Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
   Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
   Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
   Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
   TEntity Update(TEntity entity);
   IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);
   bool Delete(TEntity entity);
   IEnumerable<bool> DeleteRange(IEnumerable<TEntity> entities);
   Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
   Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
}
