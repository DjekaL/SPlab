using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ToastNotifications.Messages;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calc calc = new Calc();
        string userCat;
        string login;
        DB db = new DB("localhost", 3306, "flowmodel", "root", "Ad1234567890");
        Notification notification;
        public MainWindow()
        {
            InitializeComponent();
            notification = new Notification(this);
            Authorization authorization = new Authorization(db);
            authorization.ShowDialog();
            userCat = authorization._res;
            login = authorization._login;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            saveToFile.IsEnabled = false;
            target.Background = Brushes.LightPink;
            materialComboBox.SelectedIndex = 0;
            if (userCat == "admin")
            {
                notification.Notifier().ShowSuccess("Добро пожаловать. \rВы авторизовались по аккаунтом администратора");
            }
            else
            {
                if (userCat == "default")
                {
                    notification.Notifier().ShowSuccess("Добро пожаловать. \rВы авторизовались по аккаунтом исследователя");
                }
            }
            if (userCat == "denied") this.Close();
            List<string> materials = new List<string>();
            List<string> models = new List<string>();
            db.InitialComboBox(materials, "SELECT title FROM flowmodel.material", "title");
            foreach (string item in materials)
            {
                materialComboBox.Items.Add(item);
            }
            db.InitialComboBox(models, "SELECT title FROM flowmodel.mat_model order by mat_model_id asc", "title");
            foreach (string item in models)
            {
                modelComboBox.Items.Add(item);
            }
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
                /*p.Text = calc._p.ToString();
                c.Text = calc._c.ToString();
                T0.Text = calc._T0.ToString();*/
                Vu.Text = calc._Vu.ToString();
                Tu.Text = calc._Tu.ToString();
                /*mu0.Text = calc._mu0.ToString();
                Ea.Text = calc._Ea.ToString();
                Tr.Text = calc._Tr.ToString();
                n.Text = calc._n.ToString();
                alphaU.Text = calc._alphaU.ToString();*/
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

            List<string> matParams = new List<string>();
            db.InitialMaterial(materialComboBox.SelectedItem.ToString(), ptext.Content.ToString(), ctext.Text, T0text.Text, matParams);
            if (matParams.Count > 0)
            {
                p.Text = matParams[0];
                c.Text = matParams[1];
                T0.Text = matParams[2];
            }
            else
            {
                p.Text = "";
                c.Text = "";
                T0.Text = "";
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

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            EditUser window = new EditUser(db, login);
            window.Show();
        }

        private void ChangeModel_Click(object sender, RoutedEventArgs e)
        {
            EditMathModel window = new EditMathModel(db);
            window.Show();
        }

        private void ChangeMaterial_Click(object sender, RoutedEventArgs e)
        {
            EditMaterial window = new EditMaterial(db);
            window.Show();
        }

        private void ChangeModelKit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.UpdateModel(modelComboBox.SelectedItem.ToString(), mu0text.Text, int.Parse(mu0.Text), Eatext.Text, int.Parse(Ea.Text), Trtext.Text, int.Parse(Tr.Text), ntext.Text, Convert.ToDouble(n.Text), alphaUtext.Text, int.Parse(alphaU.Text));
                notification.Notifier().ShowSuccess("Коэффициенты модели успешно изменены!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при изменении коэффициентво модели.");
            }
        }

        private void modelComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<string> modelCoefffs = new List<string>();
            db.InitialModel(modelComboBox.SelectedItem.ToString(), mu0text.Text, Eatext.Text, Trtext.Text, ntext.Text, alphaUtext.Text, modelCoefffs);
            if (modelCoefffs.Count > 0)
            {
                mu0.Text = modelCoefffs[0];
                Ea.Text = modelCoefffs[1];
                Tr.Text = modelCoefffs[2];
                n.Text = modelCoefffs[3];
                alphaU.Text = modelCoefffs[4];
            }
            else
            {
                mu0.Text = "";
                Ea.Text = "";
                Tr.Text = "";
                n.Text = "";
                alphaU.Text = "";
            }
        }

        private void ChangeMaterila_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.UpdateMaterial(materialComboBox.SelectedItem.ToString(), ptext.Content.ToString(), Convert.ToDouble(p.Text), ctext.Text, Convert.ToDouble(c.Text), T0text.Text, Convert.ToDouble(T0.Text));
                notification.Notifier().ShowSuccess("Свойства материала успешно сохранены!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при сохранении свойст материфла.");
            }
        }
    }
}
