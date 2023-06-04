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
using static Plot3D.Graph3D;
using static Plot3D.ColorSchema;
using System.Threading;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for Anton.xaml
    /// </summary>
    public partial class Anton : Window
    {
        DB _db;
        Calc _calc = new Calc();

        double _p;
        double _c;
        double _T0;
        double _mu0;
        double _Ea;
        double _Tr;
        double _n;
        double _alphaU;
        public Anton(DB dB)
        {
            InitializeComponent();
            _db = dB;
        }

        private void materialComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (materialComboBox.SelectedItem != null)
            {
                
                string material = materialComboBox.SelectedItem.ToString();
                string model = _db.GetModelTitleFromMaterial(material);

                List<string> propTitles = new List<string>();
                List<string> propValues = new List<string>();
                List<string> propUnits = new List<string>();

                List<string> modelTitles = new List<string>();
                List<string> modelValues = new List<string>();
                List<string> modelUnits = new List<string>();

                List<Property> materialProperties = new List<Property>();
                List<Property> modelProperties = new List<Property>();

                _db.InitialMaterial(material, propTitles, propValues, propUnits);
                _db.InitialModel(model, modelTitles, modelValues, modelUnits);

                for (int i = 0; i < propTitles.Count; i++)
                {
                    materialProperties.Add(new Property
                    {
                        Title = propTitles[i],
                        Value = propValues[i],
                        Unit = propUnits[i]
                    });
                }
                for (int i = 0; i < modelTitles.Count; i++)
                {
                    modelProperties.Add(new Property
                    {
                        Title = modelTitles[i],
                        Value = modelValues[i],
                        Unit = modelUnits[i]
                    });
                }

                foreach (Property property in materialProperties)
                {
                    if (property.Title == "Плотность")
                    {
                        _p = Convert.ToDouble(property.Value);
                        continue;
                    }
                    if (property.Title == "Удельная теплоёмкость")
                    {
                        _c = Convert.ToDouble(property.Value);
                        continue;
                    }
                    if (property.Title == "Температура плавления")
                    {
                        _T0 = Convert.ToDouble(property.Value);
                        continue;
                    }
                }

                foreach (Property property in modelProperties)
                {
                    if (property.Title == "Коэффициент консистенции при температуре приведения")
                    {
                        _mu0 = Convert.ToDouble(property.Value);
                        continue;
                    }
                    if (property.Title == "Энергия активации вязкого течения материала")
                    {
                        _Ea = Convert.ToDouble(property.Value);
                        continue;
                    }
                    if (property.Title == "Температура приведения")
                    {
                        _Tr = Convert.ToDouble(property.Value);
                        continue;
                    }
                    if (property.Title == "Индекс течения")
                    {
                        _n = Convert.ToDouble(property.Value);
                        continue;
                    }
                    if (property.Title == "Коэффициент теплоотдачи от крышки")
                    {
                        _alphaU = Convert.ToDouble(property.Value);
                        continue;
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> materials = new List<string>();
            materialComboBox.Items.Clear();
            _db.InitialComboBox(materials, "SELECT title FROM flowmodel.material order by material_id asc", "title");
            foreach (string item in materials)
            {
                materialComboBox.Items.Add(item);
            }
            materialComboBox.SelectedIndex = 0;
            W.Text = _calc._W.ToString();
            H.Text = _calc._H.ToString();
            L.Text = _calc._L.ToString();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            _calc = new Calc(materialComboBox.Text, Convert.ToDouble(W.Text), Convert.ToDouble(H.Text), Convert.ToDouble(L.Text), _p, _c,
               _T0, _mu0, _Ea, _Tr, _n, _alphaU);
        }
    }
}
