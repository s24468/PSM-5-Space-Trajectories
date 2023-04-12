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
        private const int Numsteps = 365 * 3; // 3 lata
        private const double Dt = 86400; //jeden dzien
        private const double GravityConstant = 6.6743e-11;
        private const double SunMass = 1.989e+30;
        private const double EarthMass = 5.972e+24;
        private const double MoonMass = 7.347e+22;
        private const double XCoordinatesSun = 0;
        private const double YCoordinatesSun = 0;

        public MainWindow()
        {
            // Stałe
            double distanceEarthMoon = 384400000;
            const double velocityMoon = 1018.289046;

            // Współrzędne początkowe
            const double xCoordinatesEarth = 0;
            const double yCoordinatesEarth = 0;
            var xCoordinatesMoon = xCoordinatesEarth;
            var yCoordinatesMoon = yCoordinatesEarth + distanceEarthMoon;
            var xVelocityMoon = velocityMoon;
            double yVelocityMoon = 0;


            InitializeComponent();
            var chart = new ChartValues<ObservablePoint>();
            var list = EarthList();
            var i = 0;
            for (var t = 0; t < Numsteps; t++)
            {
                // Obliczenia dla kroku t
                distanceEarthMoon = Math.Sqrt(Math.Pow(xCoordinatesEarth - xCoordinatesMoon, 2) +
                                              Math.Pow(yCoordinatesEarth - yCoordinatesMoon, 2));
                var forceGravity = GravityConstant * EarthMass * MoonMass / Math.Pow(distanceEarthMoon, 2);
                var xForceGravity = (xCoordinatesEarth - xCoordinatesMoon) / distanceEarthMoon * forceGravity;
                var yForceGravity = (yCoordinatesEarth - yCoordinatesMoon) / distanceEarthMoon * forceGravity;
                var xAccelerationMoon = xForceGravity / MoonMass;
                var yAccelerationMoon = yForceGravity / MoonMass;
                var xMoonMidPointCoordinates = xCoordinatesMoon + xVelocityMoon * Dt / 2; //yEarthMidPointCoordinates
                var yMoonMidPointCoordinates = yCoordinatesMoon + yVelocityMoon * Dt / 2; //yEarthMidPointCoordinates
                var xMoonMidPointForce =
                    (xCoordinatesEarth - xMoonMidPointCoordinates) / distanceEarthMoon * forceGravity; //
                var yMoonMidPointForce =
                    (yCoordinatesEarth - yMoonMidPointCoordinates) / distanceEarthMoon * forceGravity; //
                var xMoonMidPointVelocity = xVelocityMoon + xAccelerationMoon * Dt / 2; //yMoonMidPointVelocity
                var yMoonMidPointVelocity = yVelocityMoon + yAccelerationMoon * Dt / 2; //yMoonMidPointVelocity
                var xMoonMidPointAcceleration = xMoonMidPointForce / MoonMass; //
                var yMoonMidPointAcceleration = yMoonMidPointForce / MoonMass; //

                // Aktualizacja zmiennych
                xCoordinatesMoon += xMoonMidPointVelocity * Dt;
                yCoordinatesMoon += yMoonMidPointVelocity * Dt;
                xVelocityMoon += xMoonMidPointAcceleration * Dt;
                yVelocityMoon += yMoonMidPointAcceleration * Dt;
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
            var serial = new SeriesCollection() { moonSeries };
            var cartesianChart = new CartesianChart()
            {
                Series = serial,
            };
            Content = cartesianChart;
        }


        private static List<double> EarthList()
        {
            List<double> list = new List<double>();

            var distanceEarthSun = 1.5e+11;
            var velocityEarth = 29749.15427;

            // Inicjalizacja zmiennych
            var xCoordinatesEarth = XCoordinatesSun;
            var yCoordinatesEarth = YCoordinatesSun + distanceEarthSun;
            var xVelocityEarth = velocityEarth;
            double yVelocityEarth = 0;

            for (var t = 0; t < Numsteps; t++) //coordinates
            {
                // Obliczenia dla kroku t
                distanceEarthSun = Math.Sqrt(Math.Pow(xCoordinatesEarth - XCoordinatesSun, 2) +
                                             Math.Pow(yCoordinatesEarth - YCoordinatesSun, 2));
                var forceGravity = GravityConstant * SunMass * EarthMass / Math.Pow(distanceEarthSun, 2);
                var xForceGravity = (XCoordinatesSun - xCoordinatesEarth) / distanceEarthSun * forceGravity;
                var yForceGravity = (YCoordinatesSun - yCoordinatesEarth) / distanceEarthSun * forceGravity;
                var xAccelerationEarth = xForceGravity / EarthMass;
                var yAccelerationEarth = yForceGravity / EarthMass;
                var xEarthMidPointCoordinates = xCoordinatesEarth + xVelocityEarth * Dt / 2;
                var yEarthMidPointCoordinates = yCoordinatesEarth + yVelocityEarth * Dt / 2;
                var xEarthMidPointForce =
                    (XCoordinatesSun - xEarthMidPointCoordinates) / distanceEarthSun * forceGravity;
                var yEarthMidPointForce =
                    (YCoordinatesSun - yEarthMidPointCoordinates) / distanceEarthSun * forceGravity;
                var xEarthMidPointVelocity = xVelocityEarth + xAccelerationEarth * Dt / 2;
                var yEarthMidPointVelocity = yVelocityEarth + yAccelerationEarth * Dt / 2;
                var xEarthMidPointAcceleration = xEarthMidPointForce / EarthMass;
                var yEarthMidPointAcceleration = yEarthMidPointForce / EarthMass;

                // Aktualizacja zmiennych
                xCoordinatesEarth += xEarthMidPointVelocity * Dt;
                yCoordinatesEarth += yEarthMidPointVelocity * Dt;
                xVelocityEarth += xEarthMidPointAcceleration * Dt;
                yVelocityEarth += yEarthMidPointAcceleration * Dt;


                list.Add(xCoordinatesEarth / 10);
                list.Add(yCoordinatesEarth / 10);
            }

            return list;
        }
    }
}