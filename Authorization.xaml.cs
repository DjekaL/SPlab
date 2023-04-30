﻿using System;
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

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        int _tryes = 1;
        public string _res { get; set; }
        public Authorization()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errText.Content = "";
            if (_tryes < 3)
            {
                if (login.Text == "da" && password.Text == "da")
                {
                    this.Close();
                    _res = "lox";
                }
                else
                {
                    errText.Content = "Неверный логин или пароль!";
                    errText.Foreground = Brushes.Red;
                    _tryes++;
                    _res = "denied";
                }
            }
            else
            {
                errText.Foreground = Brushes.DarkRed;
                errText.Content = "Превышено количество попыток авторизации! \rПопробуйте еще раз позже.";
            }
        }
    }
}