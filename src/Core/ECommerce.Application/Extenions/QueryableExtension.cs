using ECommerce.Application.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Extenions;

public static class QueryableExtension
{
    public static PaginatedResult<T> ToPaginatedResult<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = query.Count();
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedResult<T>(items, pageNumber, pageSize, totalPages, totalCount);
    }

    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedResult<T>(items, pageNumber, pageSize, totalPages, totalCount);
    }
}
