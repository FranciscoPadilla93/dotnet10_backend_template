using ClosedXML.Excel;

namespace LUPA.Api.Common.Excel;

/// <summary>
/// Lee cualquier .xlsx y regresa cada fila como un diccionario columna->valor,
/// usando la primera fila como encabezados. No sabe nada de entidades específicas;
/// el mapeo a una entidad concreta (ej. User) se hace en el servicio que lo consume.
/// </summary>
public static class ExcelImporter
{
    public static List<Dictionary<string, string>> ReadRows(Stream fileStream)
    {
        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheets.First();

        var headers = worksheet.Row(1).CellsUsed()
            .Select(c => c.GetString().Trim())
            .ToList();

        var rows = new List<Dictionary<string, string>>();

        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            var dict = new Dictionary<string, string>();

            for (int i = 0; i < headers.Count; i++)
            {
                dict[headers[i]] = row.Cell(i + 1).GetString().Trim();
            }

            rows.Add(dict);
        }

        return rows;
    }
}