using Plotly.NET;
namespace Test3DPlotly;

public class PlotlyService
{
    public static GenericChart Plot(double[] xValues, double[] yValues, string label = "")
    {
        return Chart2D.Chart.Line<double, double, string>(xValues, yValues);
    }

    public static GenericChart Surf(double[] xValues, double[] yValues, double[,] zValues, string label = "")
    {
        int rows = zValues.GetLength(0);
        int cols = zValues.GetLength(1);
        var rowRange = Enumerable.Range(0, rows);
        var colRange = Enumerable.Range(0, cols);
        var zData = rowRange.Select(i => colRange.Select(j => zValues[i, j]));
        return Chart3D.Chart.Surface<IEnumerable<double>, double, double, double, string>(zData, xValues, yValues)
            .WithTitle(label);
    }
}
