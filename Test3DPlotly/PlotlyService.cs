using Plotly.NET;
using static Microsoft.FSharp.Core.ByRefKinds;
namespace Test3DPlotly;

public class PlotlyService
{
    public static GenericChart Plot(double[] xValues, double[] yValues, string label = "")
    {
        return Chart2D.Chart.Line<double, double, string>(xValues, yValues);
    }

    public static GenericChart Surf(double[] xValues, double[] yValues, double[,] zValues, string label = "")
    {
        var rowRange = Enumerable.Range(0, xValues.Length);
        var colRange = Enumerable.Range(0, yValues.Length);
        var zData = rowRange.Select(i => colRange.Select(j => zValues[i, j]));
        return Chart3D.Chart.Surface<IEnumerable<double>, double, double, double, string>(zData, xValues, yValues)
            .WithTitle(label);
    }

    public static GenericChart Surf(double[,] xValues, double[,] yValues, double[,] zValues, string label = "")
    {
        // Step 1: get the dimensions of the input 2D arrays and create new arrays for the output.
        int rows = xValues.GetLength(0), cols = xValues.GetLength(1);

        // Step 2: index array to map 2D indices to 1D indices
        int[,] index = new int[rows, cols];

        // Step 3: arrays for the mesh triangles (2 triangles per grid cell)
        int[] I = new int[2*(rows-1)*(cols-1)], J = new int[2*(rows-1)*(cols-1)], K = new int[2*(rows-1)*(cols-1)];

        // Step 4: create 1D arrays for X, Y, Z values.
        int size = xValues.Length;
        double[] X = new double[size], Y = new double[size], Z = new double[size];

        // Step 5: fill the X, Y, Z arrays and the index mapping.
        int count = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                X[count] = xValues[i, j];
                Y[count] = yValues[i, j];
                Z[count] = zValues[i, j];
                index[i, j] = count++;
            }
        }

        // Step 6: fill the I, J, K arrays to define the triangles for the mesh.
        count = -1;
        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < cols - 1; j++)
            {
                I[++count] = index[i, j]; J[count] = index[i, j + 1]; K[count] = index[i + 1, j + 1];
                I[++count] = index[i, j]; J[count] = index[i + 1, j + 1]; K[count] = index[i + 1, j];
            }
        }
        // Step 7: create the mesh chart using the X, Y, Z arrays and the I, J, K triangle definitions.
        return Chart3D.Chart.Mesh3D<double, double, double, int, int, int, string>(X, Y, Z, I, J, K).WithTitle(label);
    }
}
