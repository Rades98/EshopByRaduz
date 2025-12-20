namespace Mediator.Request.Query;

public class PaginatedResult<T>(IEnumerable<T> items, int totalCount, int currentPage, int totalPages)
{
    public IEnumerable<T> Items { get; } = items;

    public int TotalCount { get; } = totalCount;

    public int CurrentPage { get; } = currentPage;

    public int TotalPages { get; } = totalPages;
}
