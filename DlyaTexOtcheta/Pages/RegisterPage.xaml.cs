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

namespace DlyaTexOtcheta.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private DbManager db = new DbManager();

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            var existing = db.GetEmployees().FirstOrDefault(u => u.Username == username);
            if (existing != null)
            {
                MessageBox.Show("Пользователь уже существует.");
                return;
            }

            db.AddEmployee(new Employee
            {
                Username = username,
                Password = password,
                IsAdmin = false
            });

            MessageBox.Show("Регистрация успешна. Вернитесь к входу.");
            NavigationService.GoBack(); // или GoForward если используете стек навигации
        }
    }
}
