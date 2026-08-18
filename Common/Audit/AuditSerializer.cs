using System.Text.Json;
using System.Text.Json.Serialization;

namespace LUPA.Api.Common.Audit;

public static class AuditSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false
    };

    public static string? Serialize(object? value)
    {
        return value is null ? null : JsonSerializer.Serialize(value, Options);
    }
}