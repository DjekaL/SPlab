using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
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
        List<Property> propertiesList = new List<Property>();

        List<Property> newMatProperties = new List<Property>();
        List<Property> newMatCoeffs = new List<Property>();
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
                List<string> propValues = new List<string>();
                List<string> propUnits = new List<string>();

                List<string> modelTitles = new List<string>();
                List<string> modelValues = new List<string>();
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
            coeffTable.ItemsSource = modelProperties;
        }

        private void InsertMaterial_Click(object sender, RoutedEventArgs e)
        {
            
            bool isError = _db.InsertMaterial(addMaterial.Text, newMatProperties, newMatCoeffs);
            if (!isError) {
                notification.Notifier().ShowSuccess("Материал успешно добавлен!");
            }
            else {
                notification.Notifier().ShowError("Возникла ошибка при добавлении материала.");
            }
            UpdateMatrialTab_Loaded(sender, e);
            DeleteMaterialTab_Loaded(sender, e);
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
            newMatProperties = new List<Property>();
            newMatCoeffs = new List<Property>();

            List<int> propIds= new List<int>();
            List<string> propTitles = new List<string>();
            List<string> propUnits = new List<string>();

            List<int> coeffIds = new List<int>();
            List<string> coeffTitles = new List<string>();
            List<string> coeffUnits = new List<string>();

            _db.GetProperties(propTitles, propIds, propUnits);
            _db.GetCoeffs(coeffTitles, coeffIds, coeffUnits);

            propertiesList = new List<Property>();

            for (int i = 0; i < propTitles.Count; i++) {
                propertiesList.Add(new Property {
                    Title = propTitles[i],
                    Id = propIds[i],
                    Unit = propUnits[i]
                });
            }

            for (int i = 0; i < coeffTitles.Count; i++) {
                propertiesList.Add(new Property {
                    Title = coeffTitles[i],
                    Id = coeffIds[i],
                    Unit = coeffUnits[i]
                });
            }

            propComboBox.ItemsSource = propTitles;
            coeffComboBox.ItemsSource = coeffTitles;

            newPropTable.ItemsSource = newMatProperties;
            newCoeffTable.ItemsSource = newMatCoeffs;
            addNewMaterialButton.IsEnabled = false;
        }

        private void addMaterial_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool titleErrors = string.IsNullOrEmpty(addMaterial.Text);
            bool valuesErrors = CheckErrors(newPropTable, newCoeffTable);
            bool propsEmpty = newMatProperties.Count == 0;
            bool coeffsEmpty = newMatCoeffs.Count == 0;
            if (!valuesErrors & !titleErrors & !propsEmpty & !coeffsEmpty) {
                string tmp = _db.GetId($"select material_id from material where title = '{addMaterial.Text}'", "material_id");
                if (!string.IsNullOrEmpty(tmp)) {
                    notification.Notifier().ShowError("Материал с таким названием уже существует.");
                    addMaterial.Background = Brushes.LightPink;
                    addNewMaterialButton.IsEnabled = false;
                }
                else {
                    addNewMaterialButton.IsEnabled = true;
                    addMaterial.Background = Brushes.White;
                }
            } else {
                addNewMaterialButton.IsEnabled = false;
            }
        }

        private void updateTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            bool errors = CheckErrors(propTable, coeffTable);
            if (!errors) {
                changeMatParamsButton.IsEnabled = true;
            } else {
                changeMatParamsButton.IsEnabled = false;
            }
        }

        private void addTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e) {
            DataGrid? grid = sender as DataGrid;
            if (grid != null) {
                Property? row = grid.SelectedItem as Property;
                if (row != null && row.Title != null) {
                    Property? updatedProp = (from property in propertiesList where property.Title == row.Title select property).FirstOrDefault();
                    if (updatedProp != null) {
                        row.Unit = updatedProp.Unit;
                        row.Id = updatedProp.Id;
                    }
                }
            }
            bool titleErrors = string.IsNullOrEmpty(addMaterial.Text);
            bool valuesErrors = CheckErrors(newPropTable, newCoeffTable);
            bool propsEmpty = newMatProperties.Count == 0;
            bool coeffsEmpty = newMatCoeffs.Count == 0;
            if (!valuesErrors & !titleErrors & !propsEmpty & !coeffsEmpty) {
                addNewMaterialButton.IsEnabled = true;
            }
            else {
                addNewMaterialButton.IsEnabled = false;
            }
            (sender as DataGrid).RowEditEnding -= addTable_RowEditEnding;
            (sender as DataGrid).CommitEdit();
            (sender as DataGrid).Items.Refresh();
            (sender as DataGrid).RowEditEnding += addTable_RowEditEnding;
        }

        private bool CheckErrors(DataGrid propTable, DataGrid coeffTable) {
            for (int i = 0; i < propTable.Items.Count; i++) {
                Property? updatedProp = (propTable.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow).Item as Property;
                if (updatedProp != null) {
                    double value;
                    if (!double.TryParse(updatedProp.Value, out value) || value <= 0 || string.IsNullOrEmpty(updatedProp.Title)) {
                        return true;
                    }
                }
            }
            for (int i = 0; i < coeffTable.Items.Count; i++) {
                Property? updatedCoeff = (coeffTable.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow).Item as Property;
                if (updatedCoeff != null) {
                    double value;
                    if (!double.TryParse(updatedCoeff.Value, out value) || value <= 0 || string.IsNullOrEmpty(updatedCoeff.Title)) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
