using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LUPA.Api.Common.Pdf;

/// <summary>
/// Genera un PDF tabular a partir de cualquier lista de objetos, usando reflection
/// sobre sus propiedades públicas (mismo enfoque que ExcelExporter, para consistencia).
/// </summary>
public static class PdfReportGenerator
{
    public static byte[] GenerateTableReport<T>(IEnumerable<T> items, string title)
    {
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

        var itemsList = items.ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header()
                    .Text(title)
                    .SemiBold().FontSize(16);

                page.Content()
                    .PaddingTop(15)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var _ in properties)
                            {
                                columns.RelativeColumn();
                            }
                        });

                        table.Header(header =>
                        {
                            foreach (var prop in properties)
                            {
                                header.Cell().Element(HeaderCell).Text(prop.Name);
                            }
                        });

                        foreach (var item in itemsList)
                        {
                            foreach (var prop in properties)
                            {
                                var value = prop.GetValue(item)?.ToString() ?? string.Empty;
                                table.Cell().Element(BodyCell).Text(value);
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    /// <summary>
    /// Igual que GenerateTableReport&lt;T&gt;, pero para datos cuyas columnas no se
    /// conocen en tiempo de compilación (ej. el resultado de un stored procedure dinámico).
    /// </summary>
    public static byte[] GenerateDynamicTableReport(List<Dictionary<string, object?>> rows, string title)
    {
        var columns = rows.Count > 0 ? rows[0].Keys.ToList() : [];

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header()
                    .Text(title)
                    .SemiBold().FontSize(16);

                page.Content()
                    .PaddingTop(15)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            foreach (var _ in columns)
                            {
                                cols.RelativeColumn();
                            }
                        });

                        table.Header(header =>
                        {
                            foreach (var column in columns)
                            {
                                header.Cell().Element(HeaderCell).Text(column);
                            }
                        });

                        foreach (var row in rows)
                        {
                            foreach (var column in columns)
                            {
                                var value = row[column]?.ToString() ?? string.Empty;
                                table.Cell().Element(BodyCell).Text(value);
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static QuestPDF.Infrastructure.IContainer HeaderCell(QuestPDF.Infrastructure.IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten2)
            .Padding(4)
            .DefaultTextStyle(x => x.Bold());
    }

    private static QuestPDF.Infrastructure.IContainer BodyCell(QuestPDF.Infrastructure.IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(4);
    }
}