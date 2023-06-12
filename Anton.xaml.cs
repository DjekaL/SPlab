﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using static Plot3D.ColorSchema;
using static Plot3D.Graph3D;

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
            List<List<string>> res = new List<List<string>>();
            List<List<double>> eff = new List<List<double>>();
            matrix.Columns.Clear();
            //_calc.Experiment(Convert.ToDouble(VuLow.Text), Convert.ToDouble(VuHigh.Text), Convert.ToDouble(VuStep.Text), Convert.ToDouble(TuLow.Text), Convert.ToDouble(TuHigh.Text), Convert.ToDouble(TuStep.Text), res);
            for (decimal i = Convert.ToDecimal(VuLow.Text); i <= Convert.ToDecimal(VuHigh.Text); i += Convert.ToDecimal(VuStep.Text))
            {
                List<string> list = new List<string>();
                List<double> eff1 = new List<double>();

                for (decimal j = Convert.ToDecimal(TuLow.Text); j <= Convert.ToDecimal(TuHigh.Text); j += Convert.ToDecimal(TuStep.Text))
                {
                    List<double> res1 = new List<double>();
                    _calc.Experiment1((double)i, (double)j, res1);
                    list.Add($"{res1[0]} °С; \n{res1[1]} Па*с; \n{res1[2]} кг/ч");
                    eff1.Add(res1[2]);
                }
                res.Add(list);
                eff.Add(eff1);
            }
            matrix.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            for (decimal j = Convert.ToDecimal(TuLow.Text); j <= Convert.ToDecimal(TuHigh.Text); j += Convert.ToDecimal(TuStep.Text))
            {
                matrix.Columns.Add("value", j.ToString());
            }
            int k = 0;
            foreach (List<string> list in res)
            {
                int j = 0;
                matrix.Rows.Add();
                foreach (string item in list)
                {
                    matrix[j, k].Value = item;
                    j++;
                }
                k++;
            }
            decimal tmp = Convert.ToDecimal(VuLow.Text);
            for (int i = 0; i <= matrix.Rows.Count - 1; i++)
            {
                matrix.Rows[i].HeaderCell.Value = string.Format((tmp).ToString(), "0");
                tmp += Convert.ToDecimal(VuStep.Text);
            }
            
                double[][] arrays = eff.Select(a => a.ToArray()).ToArray();
            set3d(arrays);
        }
        private void set3d(double[][] eff)
        {
            Graph3d.Raster = eRaster.Labels;
            System.Drawing.Color[] c_Colors = GetSchema(eSchema.Hot);
            Graph3d.SetColorScheme(c_Colors, 3);
            int stepQuantity = eff.Length;
            cPoint3D[,] points3d = new cPoint3D[stepQuantity, eff[0].Length];
            int row = 0;
            int col = 0;
            for (decimal i = Convert.ToDecimal(VuLow.Text); i <= Convert.ToDecimal(VuHigh.Text); i += Convert.ToDecimal(VuStep.Text))
            {
                _calc._Vu = (double)i;
                for (decimal j = Convert.ToDecimal(TuLow.Text); j <= Convert.ToDecimal(TuHigh.Text); j += Convert.ToDecimal(TuStep.Text))
                {
                    _calc._Tu = (double)j;
                    _calc.Efficiency();
                    points3d[row, col] = new cPoint3D((double)i, (double)j, eff[row][col]);
                    col++;
                }
                row++;
                col = 0;
            }
            Graph3d.AxisX_Legend = "Скорость крышки, м/с";
            Graph3d.AxisY_Legend = "Температура крышки, °C";
            Graph3d.AxisZ_Legend = "Производительность, кг/ч";


            Graph3d.SetSurfacePoints(points3d, eNormalize.MaintainXY);
        }
        private void CheckInputChange(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox a = (System.Windows.Controls.TextBox)e.Source;
            double temp;
            if (double.TryParse(a.Text, out temp) && temp > 0)
            {
                a.Foreground = Brushes.Black;
                a.Background = Brushes.White;
                Calculate.IsEnabled = true;
            }
            else
            {
                a.Foreground = Brushes.DarkRed;
                a.Background = Brushes.LightPink;
                Calculate.IsEnabled = false;
            }
            if (materialComboBox.SelectedItem == null /*|| modelComboBox.SelectedItem == null*/ || W.Text == "" || H.Text == "" || L.Text == "" ||  VuLow.Text == "" || VuHigh.Text == "" || VuStep.Text == ""
                || TuLow.Text == "" || TuHigh.Text == "" || TuStep.Text == "") Calculate.IsEnabled = false;
            else Calculate.IsEnabled = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
