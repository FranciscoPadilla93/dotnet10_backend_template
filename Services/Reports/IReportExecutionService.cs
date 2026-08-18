namespace LUPA.Api.Services.Reports;

public interface IReportExecutionService
{
    /// <summary>
    /// Ejecuta el stored procedure configurado para el reporte, con los valores de
    /// parámetro que llegan como texto (se convierten según el DataType configurado
    /// en cada ReportParameter). Regresa cada fila como columna->valor, porque las
    /// columnas del SP no se conocen en tiempo de compilación.
    /// </summary>
    Task<List<Dictionary<string, object?>>> ExecuteAsync(int reportId, Dictionary<string, string> parameterValues);
}