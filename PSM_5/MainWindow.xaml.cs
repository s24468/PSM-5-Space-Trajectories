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
            int numSteps = 1000;

            // Współrzędne początkowe
            double Xe = 0, Ye = 0;
            Xm = Xe;
            Ym = Ye + Dem;
            VxM = Vm;
            VyM = 0;
            InitializeComponent();
            var chart = new ChartValues<ObservablePoint>()
                { };
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
                i += 2;
                // Wypisanie wyników dla kroku t (opcjonalne)
                // Console.WriteLine($"Krok {t}: Xm={Xm}, Ym={Ym}, VxM={VxM}, VyM={VyM}");
            }


            var line = new LineSeries()
            {
                Title = "Jarek",
                Values = chart,
                // PointGeometry = null // Usuwa punkty z wykresu
            };
            var seria = new SeriesCollection();
            seria.Add(line);
            var cartesianChart = new CartesianChart()
            {
                Series = seria,
            };
            Content = cartesianChart;
        }

        private static List<double> ZiemiaList()
        {
            double G = 6.6743e-11;
            double Ms = 1.989e+30;
            double Me = 5.972e+24;
            double Des = 1.5e+11;
            double dt = 21600;
            double Ve = 29749.15427;

            // Zmienne
            double Xs,
                Ys,
                VxS,
                VyS,
                F,
                FxS,
                FyS,
                AxS,
                AyS,
                Xs_2,
                Ys_2,
                Fx_2,
                Fy_2,
                VxS_2,
                VyS_2,
                AxS_2,
                AyS_2,
                DxS,
                DyS,
                DVxS,
                DVyS;

            // Przykładowa liczba kroków
            int numSteps = 1000;

            // Współrzędne początkowe
            double Xe = 0, Ye = 0;
            Xs = Xe;
            Ys = Ye + Des;
            VxS = Ve;
            VyS = 0;
            List<double> list = new List<double>();

            for (int t = 0; t < numSteps; t++)
            {
                // Obliczenia dla kroku t
                Des = Math.Sqrt(Math.Pow(Xs - Xe, 2) + Math.Pow(Ys - Ye, 2));
                F = G * Ms * Me / Math.Pow(Des, 2);
                FxS = (Xs - Xe) / Des * F;
                FyS = (Ys - Ye) / Des * F;
                AxS = FxS / Me;
                AyS = FyS / Me;
                Xs_2 = Xs + VxS * dt / 2;
                Ys_2 = Ys + VyS * dt / 2;
                Fx_2 = (Xs_2 - Xe) / Des * F;
                Fy_2 = (Ys_2 - Ye) / Des * F;
                VxS_2 = VxS + AxS * dt / 2;
                VyS_2 = VyS + AyS * dt / 2;
                AxS_2 = Fx_2 / Me;
                AyS_2 = Fy_2 / Me;
                DxS = VxS_2 * dt;
                DyS = VyS_2 * dt;
                DVxS = AxS_2 * dt;
                DVyS = AyS_2 * dt;

                // Aktualizacja zmiennych
                Xs += DxS;
                Ys += DyS;
                VxS += DVxS;
                VyS += DVyS;

                list.Add(Xe);
                list.Add(Ye);
                // Wypisanie wyników dla kroku t (opcjonalne)
                // Console.WriteLine($"Krok {t}: Xs={Xs}, Ys={Ys}, VxS={VxS}, VyS={VyS}");
            }

            return list;
        }
    }
}