using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToastNotifications.Messages;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for EditMathModel.xaml
    /// </summary>
    public partial class EditMathModel : Window
    {
        DB _db;
        Notification notification;
        public EditMathModel(DB db)
        {
            InitializeComponent();
            _db = db;
            notification = new Notification(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _db.UpdateModel(modelComboBox.SelectedItem.ToString(), mu0text.Text, Convert.ToDouble(mu0.Text), Eatext.Text, Convert.ToDouble(Ea.Text), Trtext.Text, Convert.ToDouble(Tr.Text), ntext.Text, Convert.ToDouble(n.Text), alphaUtext.Text, Convert.ToDouble(alphaU.Text));
                notification.Notifier().ShowSuccess("Коэффициенты модели успешно изменены!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при изменении коэффициентво модели.");
            }
        }


        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            modelComboBox.Items.Clear();
            List<string> models = new List<string>();
            _db.InitialComboBox(models, "SELECT title FROM flowmodel.mat_model order by mat_model_id asc", "title");
            foreach (string item in models)
            {
                modelComboBox.Items.Add(item);
            }
            if (modelComboBox.SelectedItem == null) changeKitButton.IsEnabled = false;
            else changeKitButton.IsEnabled = true;
        }

        private void modelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> modelCoefffs = new List<string>();
            List<string> units = new List<string>();
            if (modelComboBox.SelectedItem != null)
            {
                _db.InitialModel(modelComboBox.SelectedItem.ToString(), mu0text.Text, Eatext.Text, Trtext.Text, ntext.Text, alphaUtext.Text, modelCoefffs, units);
                if (modelCoefffs.Count > 0)
                {
                    mu0.Text = modelCoefffs[0];
                    Ea.Text = modelCoefffs[1];
                    Tr.Text = modelCoefffs[2];
                    n.Text = modelCoefffs[3];
                    alphaU.Text = modelCoefffs[4];
                    mu0Unit.Text = units[0];
                    EaUnit.Text = units[1];
                    TrUnit.Text = units[2];
                    nUnit.Text = units[3];
                    alphaUUnit.Text = units[4];
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
            else
            {
                mu0.Text = "";
                Ea.Text = "";
                Tr.Text = "";
                n.Text = "";
                alphaU.Text = "";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                _db.InsertModel(kitName.Text, mu0text.Text, Convert.ToDouble(mu01.Text), Eatext.Text, Convert.ToDouble(Ea1.Text), Trtext.Text, Convert.ToDouble(Tr1.Text), ntext.Text, Convert.ToDouble(n1.Text), alphaUtext.Text, Convert.ToDouble(alphaU1.Text));
                notification.Notifier().ShowSuccess("Модель коэффициентов успешно добавлена!");
                kitName.Text = "";
                mu01.Text = "";
                Ea1.Text = "";
                Tr1.Text = "";
                n1.Text = "";
                alphaU1.Text = "";
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при добавлении модели коэффициентво.");
            }
            TabItem_Loaded(sender, e);
            TabItem_Loaded_1(sender, e);
        }

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            delModels.Items.Clear();
            List<string> models = new List<string>();
            _db.InitialComboBox(models, "SELECT title FROM flowmodel.mat_model order by mat_model_id asc", "title");
            foreach (string item in models)
            {
                delModels.Items.Add(item);
            }
            if (delModels.SelectedItem == null) dellKitButton.IsEnabled = false;
            else dellKitButton.IsEnabled = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                _db.DeleteModel(delModels.SelectedItem.ToString());
                notification.Notifier().ShowSuccess("Модель коэффициентов успешно удалена!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при удалении модели коэффициентво.");
            }
            TabItem_Loaded(sender, e);
            TabItem_Loaded_1(sender, e);
        }

        private void CheckInputChange(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox a = (System.Windows.Controls.TextBox)e.Source;
            double temp;
            if (double.TryParse(a.Text, out temp) && temp > 0)
            {
                a.Foreground = Brushes.Black;
                a.Background = Brushes.White;
                addNewKitButton.IsEnabled = true;
            }
            else
            {
                a.Foreground = Brushes.DarkRed;
                a.Background = Brushes.LightPink;
                addNewKitButton.IsEnabled = false;
            }
            if (kitName.Text == "" || mu01.Text == "" || Ea1.Text == "" || Tr1.Text == "" || n1.Text == "" || alphaU1.Text == "")
            {
                addNewKitButton.IsEnabled = false;
            }
            else addNewKitButton.IsEnabled = true;
        }

        private void CheckKitChanges(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox a = (System.Windows.Controls.TextBox)e.Source;
            double temp;
            if (double.TryParse(a.Text, out temp) && temp > 0)
            {
                a.Foreground = Brushes.Black;
                a.Background = Brushes.White;
                changeKitButton.IsEnabled = true;
            }
            else
            {
                a.Foreground = Brushes.DarkRed;
                a.Background = Brushes.LightPink;
                changeKitButton.IsEnabled = false;
            }
        }

        private void kitName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp = _db.GetId($"select mat_model_id from mat_model where title = '{kitName.Text}'", "mat_model_id");
            if (tmp != "")
            {
                Errors.Content = "Материал с таким название уже существует.";
                Errors.Foreground = Brushes.Red;
                kitName.Background = Brushes.LightPink;
                addNewKitButton.IsEnabled = false;
            }
            else
            {
                Errors.Content = "";
                addNewKitButton.IsEnabled = true;
                kitName.Background = Brushes.White;
            }
            if (kitName.Text == "" || mu01.Text == "" || Ea1.Text == "" || Tr1.Text == "" || n1.Text == "" || alphaU1.Text == "")
            {
                addNewKitButton.IsEnabled = false;
            }
            else addNewKitButton.IsEnabled = true;
        }

        private void delModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (delModels.SelectedItem == null) dellKitButton.IsEnabled = false;
            else dellKitButton.IsEnabled = true;
        }

        private void TabItem_Loaded_2(object sender, RoutedEventArgs e)
        {
            addNewKitButton.IsEnabled = false;
        }
    }
}
