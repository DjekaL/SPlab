using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToastNotifications.Messages;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for EditMaterial.xaml
    /// </summary>
    public partial class EditMaterial : Window
    {
        DB _db;
        Notification notification;
        List<Property> materialProperties = new List<Property>();
        List<Property> modelProperties = new List<Property>();
        public EditMaterial(DB db)
        {
            InitializeComponent();
            _db = db;
            notification = new Notification(this);
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string material = dellMaterialCombo.SelectedItem.ToString();
                string model = _db.GetModelTitleFromMaterial(material);
                _db.DeleteMaterial(material);
                _db.DeleteModel(model);
                notification.Notifier().ShowSuccess("Материал успешно удален!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при удалении метериала.");
            }
            UpdateMatrialTab_Loaded(sender, e);
            DeleteMaterialTab_Loaded(sender, e);
        }

        private void UpdateMatrialTab_Loaded(object sender, RoutedEventArgs e)
        {
            addMaterialCombo.Items.Clear();
            List<string> materials = new List<string>();
            _db.InitialComboBox(materials, "SELECT title FROM flowmodel.material", "title");
            foreach (string item in materials) {
                addMaterialCombo.Items.Add(item);
            }
            if (addMaterialCombo.SelectedItem == null)
                changeMatParamsButton.IsEnabled = false;
            else
                changeMatParamsButton.IsEnabled = true;
        }

        private void DeleteMaterialTab_Loaded(object sender, RoutedEventArgs e)
        {
            dellMaterialCombo.Items.Clear();
            List<string> materials = new List<string>();
            _db.InitialComboBox(materials, "SELECT title FROM flowmodel.material", "title");
            foreach (string item in materials)
            {
                dellMaterialCombo.Items.Add(item);
            }
            if (dellMaterialCombo.SelectedItem == null) dellMaterialButton.IsEnabled = false;
            else dellMaterialButton.IsEnabled = true;
        }

        private void UpdateMaterial_Click(object sender, RoutedEventArgs e)
        {
            try {
                string material = addMaterialCombo.SelectedItem.ToString();
                string model = _db.GetModelTitleFromMaterial(material);
                foreach (Property prop in materialProperties) {
                    _db.UpdatePropValue(material, prop.Title, prop.Value);
                }
                foreach (Property coeff in modelProperties) {
                    _db.UpdateCoeffValue(model, coeff.Title, coeff.Value);
                }
                notification.Notifier().ShowSuccess("Параметры материала успешно изменены!");
            }
            catch {
                notification.Notifier().ShowError("Возникла ошибка при изменении параметров материала.");
            }
        }

        private void AddMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            materialProperties = new List<Property>();
            modelProperties = new List<Property>();
            if (addMaterialCombo.SelectedItem != null) {
                string material = addMaterialCombo.SelectedItem.ToString();
                string model = _db.GetModelTitleFromMaterial(material);

                List<string> propTitles = new List<string>();
                List<double> propValues = new List<double>();
                List<string> propUnits = new List<string>();

                List<string> modelTitles = new List<string>();
                List<double> modelValues = new List<double>();
                List<string> modelUnits = new List<string>();

                _db.InitialMaterial(material, propTitles, propValues, propUnits);
                _db.InitialModel(model, modelTitles, modelValues, modelUnits);

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
            }
            propTable.ItemsSource = materialProperties;
            coefTable.ItemsSource = modelProperties;
        }

        private void InsertMaterial_Click(object sender, RoutedEventArgs e)
        {
/*            try
            {
                _db.InsertMaterial(addMaterial.Text, pName.Text, Convert.ToDouble(addp.Text), cName.Text, Convert.ToDouble(addc.Text), T0Name.Text, Convert.ToDouble(addT0.Text));
                notification.Notifier().ShowSuccess("Материал успешно добавлен!");
                addMaterial.Text = "";
                addp.Text = "";
                addc.Text = "";
                addT0.Text = "";
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при добавлении материала.");
            }
            UpdateMatrialTab_Loaded(sender, e);
            DeleteMaterialTab_Loaded(sender, e);*/
        }

        private void matParamsChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox a = (System.Windows.Controls.TextBox)e.Source;
            double temp;
            if (double.TryParse(a.Text, out temp) && temp > 0)
            {
                a.Foreground = Brushes.Black;
                a.Background = Brushes.White;
                changeMatParamsButton.IsEnabled = true;
            }
            else
            {
                a.Foreground = Brushes.DarkRed;
                a.Background = Brushes.LightPink;
                changeMatParamsButton.IsEnabled = false;
            }
        }

        private void CheckInputChange(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox a = (System.Windows.Controls.TextBox)e.Source;
            double temp;
            if (double.TryParse(a.Text, out temp) && temp > 0)
            {
                a.Foreground = Brushes.Black;
                a.Background = Brushes.White;
                addNewMaterialButton.IsEnabled = true;
            }
            else
            {
                a.Foreground = Brushes.DarkRed;
                a.Background = Brushes.LightPink;
                addNewMaterialButton.IsEnabled = false;
            }
            if (addMaterial.Text == "" || addp.Text == "" || addc.Text == "" || addT0.Text == "")
            {
                addNewMaterialButton.IsEnabled = false;
            }
            else addNewMaterialButton.IsEnabled = true;
        }

        private void dellMaterialCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dellMaterialCombo.SelectedItem == null) dellMaterialButton.IsEnabled = false;
            else dellMaterialButton.IsEnabled = true;
        }

        private void AddMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            addNewMaterialButton.IsEnabled = false;
        }

        private void addMaterial_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp = _db.GetId($"select material_id from material where title = '{addMaterial.Text}'", "material_id");
            if (tmp != "")
            {
                Errors.Content = "Материал с таким название уже существует.";
                Errors.Foreground = Brushes.Red;
                addMaterial.Background = Brushes.LightPink;
                addNewMaterialButton.IsEnabled = false;
            }
            else
            {
                Errors.Content = "";
                addMaterial.IsEnabled = true;
                addMaterial.Background = Brushes.White;
            }
        }

        private void table_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            bool errors = Validation.GetHasError(e.Row);
            if (!errors) {
                changeMatParamsButton.IsEnabled = true;
            } else {
                changeMatParamsButton.IsEnabled = false;
            }
        }
    }
}
