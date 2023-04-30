using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calc calc = new Calc();
        string user;
        DB db = new DB("localhost", 3306, "flowmodel", "root", "Ad1234567890");
        public MainWindow()
        {
            InitializeComponent();
            Authorization authorization = new Authorization(db);
            authorization.ShowDialog();
            user = authorization._res;
        }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            saveToFile.IsEnabled = false;
            target.Background = Brushes.LightPink;
            materialComboBox.SelectedIndex = 0;
            if (user == "lox")
            {
                notifier.ShowSuccess("Привет лох!");
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);

                timer.Interval = new TimeSpan(0, 10, 5);

                timer.Start();
            }
            if (user == "denied") this.Close();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            notifier.Dispose();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            calc = new Calc(materialComboBox.Text, Convert.ToDouble(W.Text), Convert.ToDouble(H.Text), Convert.ToDouble(L.Text), Convert.ToDouble(step.Text), Convert.ToDouble(p.Text), Convert.ToDouble(c.Text),
                Convert.ToDouble(T0.Text), Convert.ToDouble(Vu.Text), Convert.ToDouble(Tu.Text), Convert.ToDouble(mu0.Text), Convert.ToDouble(Ea.Text), Convert.ToDouble(Tr.Text),
                Convert.ToDouble(n.Text), Convert.ToDouble(alphaU.Text));

            List<double> zCoord = new List<double>();
            List<double> temperature = new List<double>();
            List<double> viscosity = new List<double>();
            double timeLost = 0;
            double memLost = 0;

            calc.TemperatureAndViscosity(calc, zCoord, temperature, viscosity, ref timeLost, ref memLost);

            Table table = new Table(calc);
            table.Show();
            saveToFile.IsEnabled = true;

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
                a.Background = Brushes.LightPink;
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

        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == false) return;
            string fileName = sfd.FileName;
            FileWork fileWork;
            if (fileName.Contains(".xlsx")) { fileWork = new FileWork(calc, fileName); }
            else { fileWork = new FileWork(calc, fileName + ".xlsx"); }
            fileWork.SaveToExel();
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e) {
            EditUser window = new EditUser();
            window.Show();
        }

        private void ChangeModel_Click(object sender, RoutedEventArgs e) {
            EditMathModel window = new EditMathModel();
            window.Show();
        }

        private void ChangeMaterial_Click(object sender, RoutedEventArgs e) {
            EditMaterial window = new EditMaterial();
            window.Show();
        }
    }
}
