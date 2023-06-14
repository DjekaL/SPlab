using Microsoft.Win32;
using Plot3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using ToastNotifications.Core;
using static Plot3D.ColorSchema;
using static Plot3D.Graph3D;
using ToastNotifications.Messages;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for Anton.xaml
    /// </summary>
    public partial class Anton : Window
    {
        DB _db;
        Calc _calc = new Calc();
        Notification _notification;
        List<string> _rows = new List<string>();
        List<string> _columns = new List<string>();
        List<List<string>> _resualt = new List<List<string>>();

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
            _notification = new Notification(this);
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
            Save.IsEnabled = false;
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            Save.IsEnabled = true;
            _rows.Clear();
            _columns.Clear();
            _resualt.Clear();
            double timeLost = 0;
            double memLost = GC.GetTotalMemory(false); ;
            Stopwatch t = new Stopwatch();
            t.Start();
            _calc = new Calc(materialComboBox.Text, Convert.ToDouble(W.Text), Convert.ToDouble(H.Text), Convert.ToDouble(L.Text), _p, _c,
               _T0, _mu0, _Ea, _Tr, _n, _alphaU);
            List<List<string>> res = new List<List<string>>();
            List<List<double>> temp = new List<List<double>>();
            List<List<double>> visc = new List<List<double>>();
            matrix.Columns.Clear();
            //_calc.Experiment(Convert.ToDouble(VuLow.Text), Convert.ToDouble(VuHigh.Text), Convert.ToDouble(VuStep.Text), Convert.ToDouble(TuLow.Text), Convert.ToDouble(TuHigh.Text), Convert.ToDouble(TuStep.Text), res);
            for (decimal i = Convert.ToDecimal(VuLow.Text); i <= Convert.ToDecimal(VuHigh.Text); i += Convert.ToDecimal(VuStep.Text))
            {
                List<string> list = new List<string>();
                List<double> tempTemp = new List<double>();
                List<double> viscTemp = new List<double>();

                for (decimal j = Convert.ToDecimal(TuLow.Text); j <= Convert.ToDecimal(TuHigh.Text); j += Convert.ToDecimal(TuStep.Text))
                {
                    List<double> res1 = new List<double>();
                    _calc.Experiment1((double)i, (double)j, res1);
                    list.Add($"{res1[0]} °С; \n{res1[1]} Па*с; \n{res1[2]} кг/ч");
                    tempTemp.Add(res1[0]);
                    viscTemp.Add(res1[1]);
                }
                res.Add(list);
                temp.Add(tempTemp);
                visc.Add(viscTemp);
            }
            _resualt = res;
            matrix.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            for (decimal j = Convert.ToDecimal(TuLow.Text); j <= Convert.ToDecimal(TuHigh.Text); j += Convert.ToDecimal(TuStep.Text))
            {
                _columns.Add(j.ToString());
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
                _rows.Add(tmp.ToString());
                matrix.Rows[i].HeaderCell.Value = string.Format((tmp).ToString(), "0");
                tmp += Convert.ToDecimal(VuStep.Text);
            }

            double[][] arrays = temp.Select(a => a.ToArray()).ToArray();
            set3d(Temp3d, arrays);
            Temp3d.AxisZ_Legend = "Температура продукта, °C";
            arrays = visc.Select(a => a.ToArray()).ToArray();
            Visc3d.AxisZ_Legend = "Вязкость продукта, Па*с";
            set3d(Visc3d, arrays);

            t.Stop();
            timeLost = t.ElapsedMilliseconds;
            memLost = Math.Abs((GC.GetTotalMemory(false) - memLost) / 1024);
            RAM.Content = Math.Round(memLost, 0);
            time.Content = timeLost.ToString();
        }
        private void set3d(Graph3D name, double[][] eff)
        {
            name.Raster = eRaster.Labels;
            System.Drawing.Color[] c_Colors = GetSchema(eSchema.Hot);
            //name.SetColorScheme(c_Colors, 3);
            int stepQuantity = eff.Length;
            cPoint3D[,] points3d = new cPoint3D[eff[0].Length, stepQuantity];
            int row = 0;
            int col = 0;
            for (decimal i = Convert.ToDecimal(TuLow.Text); i <= Convert.ToDecimal(TuHigh.Text); i += Convert.ToDecimal(TuStep.Text))
            {
                _calc._Vu = (double)i;
                for (decimal j = Convert.ToDecimal(VuLow.Text); j <= Convert.ToDecimal(VuHigh.Text); j += Convert.ToDecimal(VuStep.Text))
                {
                    _calc._Tu = (double)j;
                    _calc.Efficiency();
                    points3d[row, col] = new cPoint3D((double)i, (double)j, eff[col][row]);
                    col++;
                }
                row++;
                col = 0;
            }
            name.AxisX_Legend = "Температура крышки, °C";
            name.AxisY_Legend = "Скорость крышки, м/с";

            name.SetSurfacePoints(points3d, eNormalize.MaintainXY);
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
            if (materialComboBox.SelectedItem == null || W.Text == "" || H.Text == "" || L.Text == "" || VuLow.Text == "" || VuHigh.Text == "" || VuStep.Text == ""
                || TuLow.Text == "" || TuHigh.Text == "" || TuStep.Text == "") Calculate.IsEnabled = false;
            else Calculate.IsEnabled = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == false) return;
            string fileName = sfd.FileName;
            FileWork fileWork;
            if (fileName.Contains(".xlsx")) { fileWork = new FileWork(_calc, fileName); }
            else { fileWork = new FileWork(_calc, fileName + ".xlsx"); }
            try
            {
                fileWork.SaveToExсel(_rows, _columns, _resualt);
                _notification.Notifier().ShowSuccess("Отчет о моделировании процесса сохранен!");
            }
            catch
            {
                _notification.Notifier().ShowError("Возникла ошибка при сохранении отчета о моделировании процесса.");
            }
        }
    }

}
