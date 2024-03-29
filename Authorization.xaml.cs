﻿using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        int _tryes = 1;
        DB _db;
        public string _res { get; set; }
        public string _login { get; set; }
        public bool _autho { get; set; }
        public Authorization(DB db, string login)
        {
            InitializeComponent();
            _db = db;
            _login = login;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = $"SELECT user_cat FROM flowmodel.category inner join user on cat_id = category_cat_id where login = '{login.Text}' and password = '{password.Password}'";
            MySqlCommand command = new MySqlCommand(query, _db._connection);
            _db._connection.Open();
            string cat = "";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cat = reader["user_cat"].ToString();
                }
            }
           
            List<string> list = new List<string>();
            query = $"SELECT user_cat FROM flowmodel.category";
            command = new MySqlCommand(query, _db._connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader["user_cat"].ToString());
                }
            }
            _db._connection.Close();
            errText.Content = "";
            if (_tryes < 3)
            {
                if (list.Contains(cat))
                {
                    this.Close();
                    _res = cat;
                    _login = login.Text;
                    _autho = true;
                }
                else
                {
                    errText.Content = "Неверный логин или пароль!";
                    errText.Foreground = Brushes.Red;
                    _tryes++;
                }
            }
            else
            {
                errText.Foreground = Brushes.DarkRed;
                errText.Content = "Превышено количество попыток авторизации! \rПопробуйте еще раз позже.";
                authoBut.IsEnabled = false;
                _res = "denied";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _res = "denied";
        }
    }
}
