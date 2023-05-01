using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Don_tKnowHowToNameThis {
    /// <summary>
    /// Interaction logic for EditMaterial.xaml
    /// </summary>
    public partial class EditMaterial : Window {
        DB _db;
        public EditMaterial(DB db) {
            InitializeComponent();
            _db = db;
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            _db.DeleteMaterial(dellMaterialCombo.SelectedItem.ToString());
            UpdateMatrialTab_Loaded(sender, e);
            DeleteMaterialTab_Loaded(sender, e);
        }

        private void UpdateMatrialTab_Loaded(object sender, RoutedEventArgs e)
        {
            addMaterialCombo.Items.Clear();
            List<string> materials = new List<string>();
            _db.InitialComboBox(materials, "SELECT title FROM flowmodel.material", "title");
            foreach (string item in materials)
            {
                addMaterialCombo.Items.Add(item);
            }
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
        }

        private void UpdateMaterial_Click(object sender, RoutedEventArgs e)
        {
            _db.UpdateMaterial(addMaterialCombo.SelectedItem.ToString(), pName.Text, Convert.ToDouble(p.Text), cName.Text, Convert.ToDouble(c.Text), T0Name.Text, Convert.ToDouble(T0.Text));
        }

        private void AddMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> matParams= new List<string>();
            _db.InitialMaterial(addMaterialCombo.SelectedItem.ToString(), pName.Text, cName.Text, T0Name.Text, matParams);
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

        private void InsertMaterial_Click(object sender, RoutedEventArgs e)
        {
            _db.InsertMaterial(addMaterial.Text, pName.Text, Convert.ToDouble(addp.Text), cName.Text, Convert.ToDouble(addc.Text), T0Name.Text, Convert.ToDouble(addT0.Text));

            UpdateMatrialTab_Loaded(sender, e);
            DeleteMaterialTab_Loaded(sender, e);
        }
    }
}
