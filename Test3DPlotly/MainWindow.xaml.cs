using Microsoft.Web.WebView2.Core;
using Plotly.NET;
using System.Windows;

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
            var chart = TestSurf();

            await DisplayChart(chart);
            //WebView.CoreWebView2.NavigateToString(html);
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
        int size = 50;
        double[,] z = new double[size, size];

        double scale = 0.5;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                double x = i * scale;
                double y = j * scale;

                z[i, j] = Math.Sin(x) + Math.Sin(y);
            }
        }

        return PlotlyService.Surf(z, "3D Sine Surface");
    }

    private async Task DisplayChart(GenericChart chart)
    {
        await WebView.EnsureCoreWebView2Async();

        string html = GenericChart.toEmbeddedHTML(chart);

        WebView.NavigateToString(html);
    }
}