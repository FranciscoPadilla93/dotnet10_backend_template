namespace LUPA.Api.Entities;

public class ReportParameter : BaseEntity
{
    public int ReportId { get; set; }

    public Report Report { get; set; } = null!;

    /// <summary>Nombre EXACTO del parámetro en el SP, ej. "@FechaInicio".</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Etiqueta para mostrar en el formulario del reporte, ej. "Fecha de inicio".</summary>
    public string Label { get; set; } = string.Empty;

    public ReportParameterType DataType { get; set; }

    public bool IsRequired { get; set; } = true;

    public string? DefaultValue { get; set; }

    public int SortOrder { get; set; }
}