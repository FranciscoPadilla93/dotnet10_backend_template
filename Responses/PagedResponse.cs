namespace LUPA.Api.Responses;

public class PagedResponse<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalRecords { get; init; }

    public int TotalPages =>
        TotalRecords == 0
            ? 0
            : (int)Math.Ceiling((double)TotalRecords / PageSize);
}
