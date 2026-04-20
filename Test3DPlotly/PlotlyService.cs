using Plotly.NET;
namespace Test3DPlotly;

public class PlotlyService
{
    public static GenericChart Plot(double[] xValues, double[] yValues, string label)
    {
        return Chart2D.Chart.Line<double, double, string>(xValues, yValues)
            .WithTitle(label);
    }

    public static GenericChart Surf(double[,] zValues, string label)
    {
        int rows = zValues.GetLength(0);
        int cols = zValues.GetLength(1);

        var zData = Enumerable.Range(0, rows)
                              .Select(i => Enumerable.Range(0, cols)
                                                     .Select(j => zValues[i, j]));

        return Chart3D.Chart.Surface<IEnumerable<double>, double, double, double, string>(zData)
            .WithTitle(label);
    }
}
