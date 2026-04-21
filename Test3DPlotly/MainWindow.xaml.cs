using Microsoft.Web.WebView2.Core;
using Plotly.NET;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Test3DPlotly;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }
    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        try
        {
            await WebView.EnsureCoreWebView2Async();
            
            // test plot
            // var chart = TestPlot();

            // test surf
            // var chart = TestSurf();

            // test mesh
            var chart = TestMesh();

            await DisplayChart(chart);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"WebView2 initialization failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private GenericChart TestPlot()
    {
        var xValues = Enumerable.Range(0, 21).Select(i => i - 10).Select(d => (double)d).ToArray();
        var yValues = xValues.Select(t => t * t).ToArray();

        return PlotlyService.Plot(xValues, yValues, "Simple Parabola");
    }

    private GenericChart TestSurf()
    {
        int size = 100; double scale = double.Pi/50;
        double[] X = [.. Enumerable.Range(0, size).Select(i => i * scale)];
        double[] Y = [.. Enumerable.Range(0, size).Select(j => j * scale)];
        double[,] z = new double[size, size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                z[i, j] = Math.Sin(X[i]) + Math.Sin(Y[j]);
        return PlotlyService.Surf(X, Y, z, "3D Sine Surface");
    }

    private GenericChart TestMesh()
    {
        int size = 51; double scale = double.Pi/50, theta, phi;
        double[,] X = new double[size, size];
        double[,] Y = new double[size, size];
        double[,] z = new double[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                theta = 2 * i * scale; // Azimuthal angle
                phi = j * scale;
                X[i, j] = Math.Cos(theta) * Math.Sin(phi);
                Y[i, j] = Math.Sin(theta) * Math.Sin(phi);
                z[i, j] = Math.Cos(phi);
            }
        }
        return PlotlyService.Surf(X, Y, z, "3D Sine Surface");
    }

    private async Task DisplayChart(GenericChart chart)
    {
        await WebView.EnsureCoreWebView2Async();
        string html = GenericChart.toEmbeddedHTML(chart);
        WebView.CoreWebView2.NavigateToString(html);
    }
}