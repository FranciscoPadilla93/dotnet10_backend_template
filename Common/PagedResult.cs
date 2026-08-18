namespace LUPA.Api.Common;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];

    public int TotalRecords { get; init; }
}
