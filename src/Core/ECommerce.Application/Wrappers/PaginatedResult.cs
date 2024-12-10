namespace ECommerce.Application.Wrappers;

public sealed class PaginatedResult<T>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public IEnumerable<T> Items { get; }

    public PaginatedResult(IEnumerable<T> items, int pageNumber, int pageSize, int totalPages, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalCount = totalCount;
    }
}
