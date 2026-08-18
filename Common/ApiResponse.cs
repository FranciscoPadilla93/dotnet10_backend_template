namespace LUPA.Api.Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public IReadOnlyCollection<string> Errors { get; init; } = [];

    public string? TraceId { get; init; }

    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}