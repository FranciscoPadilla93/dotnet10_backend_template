namespace LUPA.Api.Entities;

public class Report : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    /// <summary>
    /// Nombre EXACTO del stored procedure a ejecutar (ej. "sp_ReporteVentasPorFecha").
    /// Solo se define desde el CRUD de Reports (protegido por permisos), nunca desde
    /// input directo del usuario final que ejecuta el reporte.
    /// </summary>
    public string StoredProcedureName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<ReportParameter> Parameters { get; set; } = [];
}