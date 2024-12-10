namespace ECommerce.Application.Wrappers;

public sealed record PaginationParams(int PageNumber = 1, int PageSize = 10);
