using System;
using System.Collections.Generic;
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
        const int NUMSTEPS = 400;

        public MainWindow()
        {
            // Stałe
            double G = 6.6743e-11;
            double Ms = 1.989e+30;
            double Me = 5.972e+24;
            double Mm = 7.347e+22;
            double Des = 1.5e+11;
            double Dem = 384400000;
            double dt = 21600;
            double Ve = 29749.15427;
            double Vm = 1018.289046;

            // Zmienne
            double Xm,
                Ym,
                VxM,
                VyM,
                F,
                FxM,
                FyM,
                AxM,
                AyM,
                Xm_2,
                Ym_2,
                Fx_2,
                Fy_2,
                VxM_2,
                VyM_2,
                AxM_2,
                AyM_2,
                DxM,
                DyM,
                DVxM,
                DVyM;

            // Przykładowa liczba kroków
            int numSteps = NUMSTEPS;

            // Współrzędne początkowe
            double Xe = 0, Ye = 0;
            Xm = Xe;
            Ym = Ye + Dem;
            VxM = Vm;
            VyM = 0;
            InitializeComponent();
            var chart = new ChartValues<ObservablePoint>();
            var chart2 = new ChartValues<ObservablePoint>();
            var list = ZiemiaList();
            int i = 0;
            for (int t = 0; t < numSteps; t++)
            {
                // Obliczenia dla kroku t
                Dem = Math.Sqrt(Math.Pow(Xe - Xm, 2) + Math.Pow(Ye - Ym, 2));
                F = G * Me * Mm / Math.Pow(Dem, 2);
                FxM = (Xe - Xm) / Dem * F;
                FyM = (Ye - Ym) / Dem * F;
                AxM = FxM / Mm;
                AyM = FyM / Mm;
                Xm_2 = Xm + VxM * dt / 2;
                Ym_2 = Ym + VyM * dt / 2;
                Fx_2 = (Xe - Xm_2) / Dem * F;
                Fy_2 = (Ye - Ym_2) / Dem * F;
                VxM_2 = VxM + AxM * dt / 2;
                VyM_2 = VyM + AyM * dt / 2;
                AxM_2 = Fx_2 / Mm;
                AyM_2 = Fy_2 / Mm;
                DxM = VxM_2 * dt;
                DyM = VyM_2 * dt;
                DVxM = AxM_2 * dt;
                DVyM = AyM_2 * dt;

                // Aktualizacja zmiennych
                Xm += DxM;
                Ym += DyM;
                VxM += DVxM;
                VyM += DVyM;
                chart.Add(new ObservablePoint(Xm + list[i], Ym + list[i + 1]));
                chart2.Add(new ObservablePoint(list[i], list[i + 1]));
                // chart.Add(new ObservablePoint(list[i], list[i + 1]));
                i += 2;
                // Wypisanie wyników dla kroku t (opcjonalne)
                // Console.WriteLine($"Krok {t}: Xm={Xm}, Ym={Ym}, VxM={VxM}, VyM={VyM}");
            }


            var ksiezycSeries = new LineSeries()
            {
                Title = "KSIEZYC",
                Values = chart,
                // PointGeometry = null // Usuwa punkty z wykresu
            };
            var ziemiaSeries = new LineSeries()
            {
                Title = "ZIEMIA",
                Values = chart,
                // PointGeometry = null // Usuwa punkty z wykresu
            };
            var seria = new SeriesCollection();
            seria.Add(ksiezycSeries);
            seria.Add(ziemiaSeries);
            var cartesianChart = new CartesianChart()
            {
                Series = seria,
            };
            Content = cartesianChart;
        }


        private static List<double> ZiemiaList()
        {
            List<double> list = new List<double>();
            double G = 6.6743e-11;
            double Ms = 1.989e+30;
            double Me = 5.972e+24;
            double Des = 1.5e+11;
            double dt = 21600;
            double Xs = 0;
            double Ys = 0;
            double Ve = 29749.15427;

            // Inicjalizacja zmiennych
            double Xe = Xs;
            double Ye = Ys + Des;
            double VxE = Ve;
            double VyE = 0;
            double F, FxE, FyE, AxE, AyE, Xe_2, Ye_2, Fx_2, Fy_2, VxE_2, VyE_2, AxE_2, AyE_2, DxE, DyE, DVxE, DVyE;

            int steps = NUMSTEPS; // liczba kroków czasowych (można dostosować)

            for (int t = 0; t < steps; t++)
            {
                // Obliczenia dla kroku t
                Des = Math.Sqrt(Math.Pow(Xe - Xs, 2) + Math.Pow(Ye - Ys, 2));
                F = G * Ms * Me / Math.Pow(Des, 2);
                FxE = (Xs - Xe) / Des * F;
                FyE = (Ys - Ye) / Des * F;
                AxE = FxE / Me;
                AyE = FyE / Me;
                Xe_2 = Xe + VxE * dt / 2;
                Ye_2 = Ye + VyE * dt / 2;
                Fx_2 = (Xs - Xe_2) / Des * F;
                Fy_2 = (Ys - Ye_2) / Des * F;
                VxE_2 = VxE + AxE * dt / 2;
                VyE_2 = VyE + AyE * dt / 2;
                AxE_2 = Fx_2 / Me;
                AyE_2 = Fy_2 / Me;
                DxE = VxE_2 * dt;
                DyE = VyE_2 * dt;
                DVxE = AxE_2 * dt;
                DVyE = AyE_2 * dt;

                // Aktualizacja zmiennych
                Xe += DxE;
                Ye += DyE;
                VxE += DVxE;
                VyE += DVyE;

                // Wyświetlanie wyników
                Console.WriteLine($"Krok {t + 1}:");
                Console.WriteLine($"Xe: {Xe}");
                Console.WriteLine($"Ye: {Ye}");
                Console.WriteLine($"VxE: {VxE}");
                Console.WriteLine($"DVyE: {DVyE}");
                Console.WriteLine($"AyE_2: {AyE_2}");
                Console.WriteLine($"Fy_2: {Fy_2}");


                list.Add(Xs / 10);
                list.Add(Ys / 10);
            }


            return list;
        }
    }
}