using QuestPDF.Infrastructure;
using ScottPlot;

namespace LUPA.Api.Common.Charts;

/// <summary>
/// Genera imágenes PNG de gráficas a partir de pares etiqueta/valor. Genérico a propósito:
/// cualquier endpoint que tenga un "Dictionary&lt;string,int&gt;" (conteos, totales, etc.)
/// puede convertirlo en gráfica sin escribir código de ScottPlot de nuevo.
/// </summary>
public static class ChartGenerator
{
    public static byte[] GenerateBarChart(
        string title,
        IReadOnlyList<string> labels,
        IReadOnlyList<double> values,
        int width = 900,
        int height = 500)
    {
        var plot = new Plot();

        double[] positions = Enumerable.Range(0, labels.Count).Select(i => (double)i).ToArray();

        plot.Add.Bars(positions, values.ToArray());
        plot.Axes.Bottom.SetTicks(positions, labels.ToArray());
        plot.Title(title);

        return plot.GetImageBytes(width, height, ScottPlot.ImageFormat.Png);
    }

    public static byte[] GenerateLineChart(
        string title,
        IReadOnlyList<string> labels,
        IReadOnlyList<double> values,
        int width = 900,
        int height = 500)
    {
        var plot = new Plot();

        double[] positions = Enumerable.Range(0, labels.Count).Select(i => (double)i).ToArray();

        plot.Add.Scatter(positions, values.ToArray());
        plot.Axes.Bottom.SetTicks(positions, labels.ToArray());
        plot.Title(title);

        return plot.GetImageBytes(width, height, ScottPlot.ImageFormat.Png);
    }
}