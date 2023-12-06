using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Логика взаимодействия для Table.xaml
    /// </summary>
    public partial class Table : Window
    {
        List<double> zCoords = new List<double>();
        List<double> temperature = new List<double>();
        List<double> viscosity = new List<double>();
        BindingList<List> data = new BindingList<List>();
        Calc _calc;
        Stopwatch _t;
        double _memLost;
        public Table(Calc calc, Stopwatch t, double memLost)
        {
            InitializeComponent();
            /*zCoords = z;
            temperature = T;
            viscosity = n;*/
            _calc = calc;
            zCoords = calc.zCoords;
            temperature = calc.temperature;
            viscosity = calc.viscosity;
            _t = t;
            _memLost = memLost;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < temperature.Count; i++)
            {
                data.Add(new List { _z = zCoords[i], _T = temperature[i], _n = viscosity[i] });
            }
            table.ItemsSource = data;

            eff.Content = _calc.Q;
            T.Content = _calc.temperature[_calc.temperature.Count -1];
            visc.Content = _calc.viscosity[_calc.viscosity.Count -1];

            Chart temperatureChart = new Chart(zCoords, temperature, "Температура, °C", "Температура");
            tempChart.DataContext = temperatureChart;
            Chart viscosityChart = new Chart(zCoords, viscosity, "Вязкость, Па * с", "Вязкость");
            visChart.DataContext = viscosityChart;

            _t.Stop();
            double timeLost = _t.ElapsedMilliseconds;
            double tmp = GC.GetTotalMemory(false);
            _memLost = Math.Abs((tmp - _memLost) / 1024);
            RAM.Content = Math.Round(_memLost, 0);
            time.Content = timeLost.ToString();

        }
    }
}
