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
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Window {
        DB _db;
        string _login;
        public EditUser(DB db, string login) {
            InitializeComponent();
            _db = db;
            _login = login;
        }

        private void dellUser_Click(object sender, RoutedEventArgs e)
        {
            _db.DeleteUser(dellUserComboBox.SelectedItem.ToString());
            dellUser_Loaded(sender, e);
        }

        private void addNewUser_Click(object sender, RoutedEventArgs e)
        {
            _db.InsertUser(addNewLogin.Text, addNewPassword.Text);
            dellUser_Loaded(sender, e);
            addNewLogin.Text = "";
            addNewPassword.Text = "";
        }

        private void dellUser_Loaded(object sender, RoutedEventArgs e)
        {
            dellUserComboBox.Items.Clear();
            List<string> users = new List<string>();
            _db.InitialComboBox(users, "SELECT login FROM flowmodel.user inner join category on cat_id = category_cat_id where user_cat = 'default'", "login");
            foreach (string item in users)
            {
                dellUserComboBox.Items.Add(item);
            }
        }

        private void changePas_Click(object sender, RoutedEventArgs e)
        {
            _db.ChangePassword(_login, oldPas.Text, newPas.Text);
        }

        private void ChangePassword_Loaded(object sender, RoutedEventArgs e)
        {
            currenrtUser.Content += _login.ToString();
        }

        private void addNewLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp = _db.GetUserId(addNewLogin.Text);
            if (tmp != "")
            {
                Errors.Content = "Пользователь с таким логином уже существует.";
                Errors.Foreground = Brushes.Red;
                addNewLogin.Background = Brushes.LightPink;
                addNewUser.IsEnabled = false;
            }
            else
            {
                Errors.Content = "";
                addNewUser.IsEnabled = true;
                addNewLogin.Background = Brushes.White;
            }
        }
    }
}
