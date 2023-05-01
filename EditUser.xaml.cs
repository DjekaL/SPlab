using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToastNotifications.Messages;

namespace Don_tKnowHowToNameThis
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        DB _db;
        string _login;
        Notification notification;
        public EditUser(DB db, string login)
        {
            InitializeComponent();
            _db = db;
            _login = login;
            notification = new Notification(this);
        }

        private void dellUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _db.DeleteUser(dellUserComboBox.SelectedItem.ToString());
                notification.Notifier().ShowSuccess("Учетная запись успешно удолена!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при удолении учетной записи.");
            }
            dellUser_Loaded(sender, e);
        }

        private void addNewUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _db.InsertUser(addNewLogin.Text, addNewPassword.Text);
                notification.Notifier().ShowSuccess("Учетная запись успешно создана!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при создании учетной записи.");
            }
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
            if (dellUserComboBox.SelectedItem == null) dellUserButton.IsEnabled = false;
            else dellUserButton.IsEnabled = true;
        }

        private void changePas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(_db.ChangePassword(_login, oldPas.Text, newPas.Text))
                {
                    notification.Notifier().ShowSuccess("Пароль успешно изменен!");
                }
                else notification.Notifier().ShowError("Старый пароль введён некорректно!");
            }
            catch
            {
                notification.Notifier().ShowError("Возникла ошибка при изменении пароля.");
            }
        }

        private void ChangePassword_Loaded(object sender, RoutedEventArgs e)
        {
            currenrtUser.Content += _login.ToString();
            changePasButton.IsEnabled = false;
        }

        private void addNewLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp = _db.GetId($"select user_id from user where login = '{addNewLogin.Text}'", "user_id");
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
            if(addNewLogin.Text == "" || addNewPassword.Text == "") addNewUser.IsEnabled = false;
            else addNewUser.IsEnabled = true;
        }

        private void dellUserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dellUserComboBox.SelectedItem == null) dellUserButton.IsEnabled = false;
            else dellUserButton.IsEnabled = true;
        }

        private void PasChanfe_Changed(object sender, TextChangedEventArgs e)
        {
            if (oldPas.Text == "" || newPas.Text == "")
            {
                changePasButton.IsEnabled = false;
            }
            else
            {
                changePasButton.IsEnabled = true;
            }
        }

        private void addNewUser1_Loaded(object sender, RoutedEventArgs e)
        {
            addNewUser.IsEnabled = false;
        }
    }
}
