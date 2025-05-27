using DlyaTexOtcheta.data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DlyaTexOtcheta
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly DbManager db = new DbManager();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            var isAdmin = IsAdminCheck.IsChecked == true;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите имя и пароль");
                return;
            }

            var exists = db.GetEmployees().Any(e => e.Username == username);
            if (exists)
            {
                MessageBox.Show("Пользователь уже существует");
                return;
            }

            db.AddEmployee(new Employee
            {
                Username = username,
                Password = password,
                IsAdmin = isAdmin
            });

            MessageBox.Show("Пользователь успешно зарегистрирован");
            this.Close();
            RegistrationPage regPage = new RegistrationPage();
            regPage.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage regPage = new RegistrationPage();
            regPage.Show();
            this.Close();
        }
    }
}
