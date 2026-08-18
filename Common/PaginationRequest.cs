namespace LUPA.Api.Common;

public class PaginationRequest
{
    private const int MaxPageSize = 100;

    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0
            ? 10
            : value > MaxPageSize
                ? MaxPageSize
                : value;
    }

    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public bool Descending { get; set; }
}
