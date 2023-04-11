using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
namespace PSM_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var chart = new ChartValues<ObservablePoint>()
            {
                new ObservablePoint(1, 2),
                new ObservablePoint(3, 4),
                new ObservablePoint(8, 1)
            };
            var line = new LineSeries()
            {
                Title = "Jarek",
                Values = chart
            };
            var seria = new SeriesCollection();
            seria.Add(line);
            var cartesianChart = new CartesianChart()
            {
                Series = seria
            };
            Content = cartesianChart;



        }
    }
}