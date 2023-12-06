using Microsoft.Win32;
using Org.BouncyCastle.Asn1.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        string login = "";
        bool isAuthor = false;
        DB db;
        string user = "root";
        string password = "root";
        string host = "192.168.51.179";
        Notification notification;
        public MainWindow()
        {
            InitializeComponent();
            db = new DB(host, 3306, "flowmodel", user, password);
            notification = new Notification(this);
            Authorization authorization = new Authorization(db, login);
            authorization.ShowDialog();
            userCat = authorization._res;
            login = authorization._login;
            isAuthor = authorization._autho;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            saveToFile.IsEnabled = false;
            target.Background = Brushes.LightPink;
            if (userCat == "admin")
            {
                notification.Notifier().ShowSuccess("Добро пожаловать. \rВы авторизовались в аккаунт администратора");
                baseEditor.IsEnabled = true;
                baseEditor.Visibility = Visibility.Visible;
            }
            else
            {
                if (userCat == "default")
                {
                    notification.Notifier().ShowSuccess("Добро пожаловать. \rВы авторизовались в аккаунт исследователя");
                    baseEditor.IsEnabled = false;
                    baseEditor.Visibility = Visibility.Collapsed;
                }
            }
            if (userCat == "denied")
            {
                if (login == "")
                {
                    this.Close();
                }
            }
            List<string> materials = new List<string>();
            List<string> models = new List<string>();
            materialComboBox.Items.Clear();
            db.InitialComboBox(materials, "SELECT title FROM flowmodel.material order by material_id asc", "title");
            foreach (string item in materials)
            {
                materialComboBox.Items.Add(item);
            }
            p.Text = "";
            c.Text = "";
            Tu.Text = "";
            mu0.Text = "";
            Ea.Text = "";
            Tr.Text = "";
            n.Text = "";
            alphaU.Text = "";
            materialComboBox.SelectedIndex = 0;
        }
        private void Window_Reload()
        {
            List<string> materials = new List<string>();
            List<string> models = new List<string>();
            materialComboBox.Items.Clear();
            db.InitialComboBox(materials, "SELECT title FROM flowmodel.material order by material_id asc", "title");
            foreach (string item in materials)
            {
                materialComboBox.Items.Add(item);
            }
            materialComboBox.SelectedIndex = 0;
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
            double memLost = GC.GetTotalMemory(false); ;
            Stopwatch t = new Stopwatch();
            t.Start();

            calc.TemperatureAndViscosity(calc, zCoord, temperature, viscosity, ref timeLost, ref memLost);

            Table table = new Table(calc, t, memLost);
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
            if (materialComboBox.SelectedItem == null /*|| modelComboBox.SelectedItem == null*/ || W.Text == "" || H.Text == "" || L.Text == "" || Vu.Text == "" || Tu.Text == "" || step.Text == "") tableValueButton.IsEnabled = false;
            else tableValueButton.IsEnabled = true;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Vu.Text = calc._Vu.ToString();
            Tu.Text = calc._Tu.ToString();
            W.Text = calc._W.ToString();
            H.Text = calc._H.ToString();
            L.Text = calc._L.ToString();
            step.Text = calc._step.ToString();

            List<string> matParams = new List<string>();
            List<string> units = new List<string>();
            List<string> modelCoefffs = new List<string>();
            if (materialComboBox.SelectedItem != null)
            {
                p.Text = "";
                c.Text = "";
                T0.Text = "";

                mu0.Text = "";
                Ea.Text = "";
                Tr.Text = "";
                n.Text = "";
                alphaU.Text = "";

                string material = materialComboBox.SelectedItem.ToString();
                string model = db.GetModelTitleFromMaterial(material);

                List<string> propTitles = new List<string>();
                List<string> propValues = new List<string>();
                List<string> propUnits = new List<string>();

                List<string> modelTitles = new List<string>();
                List<string> modelValues = new List<string>();
                List<string> modelUnits = new List<string>();

                List<Property> materialProperties = new List<Property>();
                List<Property> modelProperties = new List<Property>();

                db.InitialMaterial(material, propTitles, propValues, propUnits);
                db.InitialModel(model, modelTitles, modelValues, modelUnits);

                for (int i = 0; i < propTitles.Count; i++) {
                    materialProperties.Add(new Property {
                        Title = propTitles[i],
                        Value = propValues[i],
                        Unit = propUnits[i]
                    });
                }
                for (int i = 0; i < modelTitles.Count; i++) {
                    modelProperties.Add(new Property {
                        Title = modelTitles[i],
                        Value = modelValues[i],
                        Unit = modelUnits[i]
                    });
                }

                foreach (Property property in materialProperties) {
                    if (property.Title == ptext.Content.ToString()) {
                        p.Text = property.Value.ToString();
                        continue;
                    }
                    if (property.Title == ctext.Text) {
                        c.Text = property.Value.ToString();
                        continue;
                    }
                    if (property.Title == T0text.Text) {
                        T0.Text = property.Value.ToString();
                        continue;
                    }
                }

                foreach (Property property in modelProperties) {
                    if (property.Title == mu0text.Text) {
                        mu0.Text = property.Value.ToString();
                        continue;
                    }
                    if (property.Title == Eatext.Text) {
                        Ea.Text = property.Value.ToString();
                        continue;
                    }
                    if (property.Title == Trtext.Text) {
                        Tr.Text = property.Value.ToString();
                        continue;
                    }
                    if (property.Title == ntext.Text) {
                        n.Text = property.Value.ToString();
                        continue;
                    }
                    if (property.Title == alphaUtext.Text) {
                        alphaU.Text = property.Value.ToString();
                        continue;
                    }
                }
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
            try
            {
                fileWork.SaveToExсel();
                notification.Notifier().ShowSuccess("Отчет о моделировании процесса сохранен!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при сохранении отчета о моделировании процесса.");
            }
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            EditUser window = new EditUser(db, login);
            window.ShowDialog();
            Window_Reload();
        }

        private void ChangeMaterial_Click(object sender, RoutedEventArgs e)
        {
            EditMaterial window = new EditMaterial(db);
            window.ShowDialog();
            Window_Reload();
        }

        private void ChangeMaterila_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string material = materialComboBox.SelectedItem.ToString();
                string model = db.GetModelTitleFromMaterial(material);
                db.UpdateMaterial(material, ptext.Content.ToString(), Convert.ToDouble(p.Text), ctext.Text, Convert.ToDouble(c.Text), T0text.Text, Convert.ToDouble(T0.Text));
                db.UpdateModel(model, mu0text.Text, int.Parse(mu0.Text), Eatext.Text, int.Parse(Ea.Text), Trtext.Text, int.Parse(Tr.Text), ntext.Text, Convert.ToDouble(n.Text), alphaUtext.Text, int.Parse(alphaU.Text));
                notification.Notifier().ShowSuccess("Свойства материала успешно сохранены!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при сохранении свойств материала.");
            }
        }

        private void ChangeProfile_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization(db, login);
            authorization.ShowDialog();
            userCat = authorization._res;
            login = authorization._login;
            Window_Loaded(sender, e);
        }

        private void CopyDataBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.DataBaseExport(user, password, host);
                notification.Notifier().ShowSuccess("Резервная копия успешно создана!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при создании резервной копии.");
            }
        }

        private void UpdateDataBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckDumpExist())
                {
                    throw new Exception();
                }

                db.DataBaseImport();
                notification.Notifier().ShowSuccess("Копия базы данных успешно загружена!");
                Window_Reload();
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при загрузке резервной копии.\r\nВозможно резервной копии не существует.");
            }
        }
        private bool CheckDumpExist()
        {
            string v = $@"{Environment.CurrentDirectory}\dump.sql";
            FileInfo file = new FileInfo(v);
            return file.Exists;
        }

        private void Experiment_Click(object sender, RoutedEventArgs e)
        {
            Anton anton = new Anton(db);
            anton.Show();
        }
    }
}
