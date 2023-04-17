using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Calc calc = new Calc();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*p.Text = calc._p.ToString();
            c.Text = calc._c.ToString();
            T0.Text = calc._T0.ToString();
            Vu.Text = calc._Vu.ToString();
            Tu.Text = calc._Tu.ToString();
            mu0.Text = calc._mu0.ToString();
            Ea.Text = calc._Ea.ToString();
            Tr.Text = calc._Tr.ToString();
            n.Text = calc._n.ToString();
            alphaU.Text = calc._alphaU.ToString();
            W.Text = calc._W.ToString();
            H.Text = calc._H.ToString();
            L.Text = calc._L.ToString();
            step.Text = calc._step.ToString();*/
            tableValueButton.IsEnabled = false;
            target.Background = Brushes.LightPink;
            materialComboBox.SelectedIndex = 0;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            /*if (W.Text == "" || H.Text == "" || L.Text == "" || step.Text == "")
            {
                if (MessageBox.Show("Некоректные данные. Загрузить стандартные данные?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    W.Text = calc._W.ToString();
                    H.Text = calc._H.ToString();
                    L.Text = calc._L.ToString();
                    step.Text = calc._step.ToString();
                }
                else { return; }
            }
            else
            {*/
                calc = new Calc(Convert.ToDouble(W.Text), Convert.ToDouble(H.Text), Convert.ToDouble(L.Text), Convert.ToDouble(step.Text), Convert.ToDouble(p.Text), Convert.ToDouble(c.Text),
                    Convert.ToDouble(T0.Text), Convert.ToDouble(Vu.Text), Convert.ToDouble(Tu.Text), Convert.ToDouble(mu0.Text), Convert.ToDouble(Ea.Text), Convert.ToDouble(Tr.Text),
                    Convert.ToDouble(n.Text), Convert.ToDouble(alphaU.Text));
            //}
            
            List<double> zCoord = new List<double>();
            List<double> temperature = new List<double>();
            List<double> viscosity = new List<double>();
            
            calc.TemperatureAndViscosity(calc, zCoord, temperature, viscosity);
            Table table = new Table(zCoord, temperature, viscosity);
            table.Show();
/*            eff.Content = calc.Efficiency().ToString();
            T.Content = Math.Round(temperature[temperature.Count - 1], 2).ToString();
            visc.Content = Math.Round(viscosity[viscosity.Count - 1], 2).ToString();*/
        }

        private void CheckInputChange(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox a = (System.Windows.Controls.TextBox)e.Source;
            double temp;
            if (double.TryParse(a.Text, out temp) && temp > 0)
            {
                a.Foreground = Brushes.Black;
                a.Background = Brushes.White;
                tableValueButton.IsEnabled = true;
                target.Background = Brushes.White;
            }
            else
            {
                a.Foreground = Brushes.DarkRed;
                a.Background= Brushes.LightPink;
                tableValueButton.IsEnabled = false;
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (materialComboBox.SelectedIndex == 0)
            {
                p.Text = calc._p.ToString();
                c.Text = calc._c.ToString();
                T0.Text = calc._T0.ToString();
                Vu.Text = calc._Vu.ToString();
                Tu.Text = calc._Tu.ToString();
                mu0.Text = calc._mu0.ToString();
                Ea.Text = calc._Ea.ToString();
                Tr.Text = calc._Tr.ToString();
                n.Text = calc._n.ToString();
                alphaU.Text = calc._alphaU.ToString();
                W.Text = calc._W.ToString();
                H.Text = calc._H.ToString();
                L.Text = calc._L.ToString();
                step.Text = calc._step.ToString();
            }
            else
            {
                p.Text = "";
                c.Text = "";
                T0.Text = "";
                Vu.Text = "";
                Tu.Text = "";
                mu0.Text = "";
                Ea.Text = "";
                Tr.Text = "";
                n.Text = "";
                alphaU.Text = "";
                W.Text = "";
                H.Text = "";
                L.Text = "";
                step.Text = "";
            }
        }
    }
}
