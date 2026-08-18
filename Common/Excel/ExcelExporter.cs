using System.Reflection;
using ClosedXML.Excel;

namespace LUPA.Api.Common.Excel;

/// <summary>
/// Exporta cualquier lista de objetos a .xlsx usando reflection sobre sus propiedades públicas.
/// Reusable para cualquier entidad/DTO sin escribir código de export por cada una.
/// </summary>
public static class ExcelExporter
{
    public static byte[] Export<T>(IEnumerable<T> items, string sheetName = "Datos")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType.IsPrimitive
                || p.PropertyType == typeof(string)
                || p.PropertyType == typeof(DateTime)
                || p.PropertyType == typeof(DateTime?)
                || p.PropertyType == typeof(bool)
                || p.PropertyType == typeof(bool?)
                || Nullable.GetUnderlyingType(p.PropertyType)?.IsPrimitive == true)
            .ToArray();

        for (int col = 0; col < properties.Length; col++)
        {
            var cell = worksheet.Cell(1, col + 1);
            cell.Value = properties[col].Name;
            cell.Style.Font.Bold = true;
        }

        int row = 2;

        foreach (var item in items)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(item);
                worksheet.Cell(row, col + 1).Value = value?.ToString() ?? string.Empty;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    /// <summary>
    /// Igual que Export&lt;T&gt;, pero para datos cuyas columnas no se conocen en
    /// tiempo de compilación (ej. el resultado de un stored procedure dinámico).
    /// </summary>
    public static byte[] ExportDynamic(List<Dictionary<string, object?>> rows, string sheetName = "Datos")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var columns = rows.Count > 0 ? rows[0].Keys.ToList() : [];

        for (int col = 0; col < columns.Count; col++)
        {
            var cell = worksheet.Cell(1, col + 1);
            cell.Value = columns[col];
            cell.Style.Font.Bold = true;
        }

        for (int row = 0; row < rows.Count; row++)
        {
            for (int col = 0; col < columns.Count; col++)
            {
                worksheet.Cell(row + 2, col + 1).Value = rows[row][columns[col]]?.ToString() ?? string.Empty;
            }
        }

        worksheet.Columns().AdjustToContents();

        using var dynamicStream = new MemoryStream();
        workbook.SaveAs(dynamicStream);
        return dynamicStream.ToArray();
    }
}