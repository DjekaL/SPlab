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
    /// Interaction logic for EditMathModel.xaml
    /// </summary>
    public partial class EditMathModel : Window {
        DB _db;
        public EditMathModel(DB db) {
            InitializeComponent();
            _db = db;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            _db.UpdateModel(modelComboBox.SelectedItem.ToString(), mu0text.Text, int.Parse(mu0.Text), Eatext.Text, int.Parse(Ea.Text), Trtext.Text, int.Parse(Tr.Text), ntext.Text, Convert.ToDouble(n.Text), alphaUtext.Text, int.Parse(alphaU.Text));
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            modelComboBox.Items.Clear();
            List<string> models = new List<string>();
            _db.InitialMaterial(models, "SELECT title FROM flowmodel.mat_model order by mat_model_id asc");
            foreach (string item in models)
            {
                modelComboBox.Items.Add(item);
            }
        }

        private void modelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> modelCoefffs = new List<string>();
            _db.InitialModel(modelComboBox.SelectedItem.ToString(), mu0text.Text, Eatext.Text, Trtext.Text, ntext.Text, alphaUtext.Text, modelCoefffs);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _db.InsertModel(kitName.Text, mu0text.Text, int.Parse(mu01.Text), Eatext.Text, int.Parse(Ea1.Text), Trtext.Text, int.Parse(Tr1.Text), ntext.Text, Convert.ToDouble(n1.Text), alphaUtext.Text, int.Parse(alphaU1.Text));

            TabItem_Loaded(sender, e);
            TabItem_Loaded_1(sender, e);
        }

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            delModels.Items.Clear();
            List<string> models = new List<string>();
            _db.InitialMaterial(models, "SELECT title FROM flowmodel.mat_model order by mat_model_id asc");
            foreach (string item in models)
            {
                delModels.Items.Add(item);
            }
        }

       /* private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> models = new List<string>();
            _db.InitialMaterial(models, "SELECT title FROM flowmodel.mat_model order by mat_model_id asc");
            delModels.Items.Clear();
            modelComboBox.Items.Clear();
            foreach (string item in models)
            {
                delModels.Items.Add(item);
                modelComboBox.Items.Add(item);
            }

        }*/

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _db.DeleteModel(delModels.SelectedItem.ToString());
            TabItem_Loaded(sender, e);
            TabItem_Loaded_1(sender, e);
        }

        /*private void TabControl_Selected(object sender, RoutedEventArgs e)
        {
            List<string> models = new List<string>();
            _db.InitialMaterial(models, "SELECT title FROM flowmodel.mat_model order by mat_model_id asc");
            delModels.Items.Clear();
            modelComboBox.Items.Clear();
            foreach (string item in models)
            {
                delModels.Items.Add(item);
                modelComboBox.Items.Add(item);
            }
        }*/
    }
}
