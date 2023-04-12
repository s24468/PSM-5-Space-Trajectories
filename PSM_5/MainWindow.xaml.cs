using System;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace PSM_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        const int Numsteps = 365 * 3; // 3 lata
        const double Dt = 86400; //jeden dzien
        private const double GravityConstant = 6.6743e-11;
        const double SunMass = 1.989e+30;
        const double EarthMass = 5.972e+24;
        const double MoonMass = 7.347e+22;

        public MainWindow()
        {
            // Stałe
            double DistanceEarthSun = 1.5e+11;
            double DistanceEarthMoon = 384400000;
            double velocityEarth = 29749.15427;
            double velocityMoon = 1018.289046;

            // Zmienne
            double xAccelerationMoon, yAccelerationMoon;
            double forceGravity, xForceGravity, yForceGravity;
            double Xm_2, Ym_2;
            double Fx_2, Fy_2;
            double VxM_2, VyM_2;
            double AxM_2, AyM_2;
            double DxM, DyM;
            double DVxM, DVyM;

            // liczba kroków
            int numSteps = Numsteps;

            // Współrzędne początkowe
            double xCoordinatesEarth = 0, yCoordinatesEarth = 0;
            var xCoordinatesMoon = xCoordinatesEarth;
            var yCoordinatesMoon = yCoordinatesEarth + DistanceEarthMoon;
            var xVelocityMoon = velocityMoon;
            double yVelocityMoon = 0;
            
            
            InitializeComponent();
            var chart = new ChartValues<ObservablePoint>();
            var list = EarthList();
            int i = 0;
            for (int t = 0; t < numSteps; t++)
            {
                // Obliczenia dla kroku t
                DistanceEarthMoon = Math.Sqrt(Math.Pow(xCoordinatesEarth - xCoordinatesMoon, 2) +
                                              Math.Pow(yCoordinatesEarth - yCoordinatesMoon, 2));
                forceGravity = GravityConstant * EarthMass * MoonMass / Math.Pow(DistanceEarthMoon, 2);
                xForceGravity = (xCoordinatesEarth - xCoordinatesMoon) / DistanceEarthMoon * forceGravity;
                yForceGravity = (yCoordinatesEarth - yCoordinatesMoon) / DistanceEarthMoon * forceGravity;
                xAccelerationMoon = xForceGravity / MoonMass;
                yAccelerationMoon = yForceGravity / MoonMass;
                Xm_2 = xCoordinatesMoon + xVelocityMoon * Dt / 2;
                Ym_2 = yCoordinatesMoon + yVelocityMoon * Dt / 2;
                Fx_2 = (xCoordinatesEarth - Xm_2) / DistanceEarthMoon * forceGravity;
                Fy_2 = (yCoordinatesEarth - Ym_2) / DistanceEarthMoon * forceGravity;
                VxM_2 = xVelocityMoon + xAccelerationMoon * Dt / 2;
                VyM_2 = yVelocityMoon + yAccelerationMoon * Dt / 2;
                AxM_2 = Fx_2 / MoonMass;
                AyM_2 = Fy_2 / MoonMass;
                DxM = VxM_2 * Dt;
                DyM = VyM_2 * Dt;
                DVxM = AxM_2 * Dt;
                DVyM = AyM_2 * Dt;

                // Aktualizacja zmiennych
                xCoordinatesMoon += DxM;
                yCoordinatesMoon += DyM;
                xVelocityMoon += DVxM;
                yVelocityMoon += DVyM;
                chart.Add(new ObservablePoint(xCoordinatesMoon + list[i], yCoordinatesMoon + list[i + 1]));
                i += 2;
            }

            //tworzenie wykresu
            var moonSeries = new LineSeries()
            {
                Title = "Moon",
                Values = chart,
                PointGeometry = null // Usuwa punkty z wykresu
            };
            var seria = new SeriesCollection();
            seria.Add(moonSeries);
            var cartesianChart = new CartesianChart()
            {
                Series = seria,
            };
            Content = cartesianChart;
        }


        private static List<double> EarthList()
        {
            List<double> list = new List<double>();

            double distanceEarthSun = 1.5e+11;
            const double xCoordinatesSun = 0;
            const double yCoordinatesSun = 0;
            double velocityEarth = 29749.15427;

            // Inicjalizacja zmiennych
            double xCoordinatesEarth = xCoordinatesSun;
            double yCoordinatesEarth = yCoordinatesSun + distanceEarthSun;
            double xVelocityEarth = velocityEarth;
            double yVelocityEarth = 0;


            double Xe_2, Ye_2, Fx_2, Fy_2, VxE_2, VyE_2, AxE_2, AyE_2, DxE, DyE, DVxE, DVyE;

            int steps = Numsteps; // liczba kroków czasowych (można dostosować)

            for (int t = 0; t < steps; t++)//coordinates
            {
                // Obliczenia dla kroku t
                distanceEarthSun = Math.Sqrt(Math.Pow(xCoordinatesEarth - xCoordinatesSun, 2) +
                                             Math.Pow(yCoordinatesEarth - yCoordinatesSun, 2));
                var forceGravity = GravityConstant * SunMass * EarthMass / Math.Pow(distanceEarthSun, 2);
                var xForceGravity = (xCoordinatesSun - xCoordinatesEarth) / distanceEarthSun * forceGravity;
                var yForceGravity = (yCoordinatesSun - yCoordinatesEarth) / distanceEarthSun * forceGravity;
                var xAccelerationEarth = xForceGravity / EarthMass;
                var yAccelerationEarth = yForceGravity / EarthMass;
                Xe_2 = xCoordinatesEarth + xVelocityEarth * Dt / 2;
                Ye_2 = yCoordinatesEarth + yVelocityEarth * Dt / 2;
                Fx_2 = (xCoordinatesSun - Xe_2) / distanceEarthSun * forceGravity;
                Fy_2 = (yCoordinatesSun - Ye_2) / distanceEarthSun * forceGravity;
                VxE_2 = xVelocityEarth + xAccelerationEarth * Dt / 2;
                VyE_2 = yVelocityEarth + yAccelerationEarth * Dt / 2;
                AxE_2 = Fx_2 / EarthMass;
                AyE_2 = Fy_2 / EarthMass;
                DxE = VxE_2 * Dt;
                DyE = VyE_2 * Dt;
                DVxE = AxE_2 * Dt;
                DVyE = AyE_2 * Dt;

                // Aktualizacja zmiennych
                xCoordinatesEarth += DxE;
                yCoordinatesEarth += DyE;
                xVelocityEarth += DVxE;
                yVelocityEarth += DVyE;


                list.Add(xCoordinatesEarth / 10);
                list.Add(yCoordinatesEarth / 10);
            }


            return list;
        }
    }
}