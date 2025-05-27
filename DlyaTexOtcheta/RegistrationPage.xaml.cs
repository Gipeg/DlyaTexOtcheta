using DlyaTexOtcheta.data;
using DlyaTexOtcheta.models;
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

namespace DlyaTexOtcheta
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Window
    {
        private Employee user;

        public RegistrationPage()
        {
            InitializeComponent();
        }

        private DbManager db = new DbManager();

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            var employees = db.GetEmployees();
            var found = employees.FirstOrDefault(emp => emp.Username == username && emp.Password == password);

            if (found != null)
            {
                MainWindowApp main = new MainWindowApp(found);
                main.Show();
                Window.GetWindow(this)?.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.ShowDialog();
            this.Close();
        }
    }
}
