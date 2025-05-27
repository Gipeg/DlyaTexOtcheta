using DlyaTexOtcheta.data;
using DlyaTexOtcheta.models;
using DlyaTexOtcheta.Pages;
using Microsoft.EntityFrameworkCore.Internal;
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
    /// Логика взаимодействия для MainWindowApp.xaml
    /// </summary>
    public partial class MainWindowApp : Window
    {
        private Employee currentUser;
        private readonly DbManager db = new DbManager();

        public MainWindowApp(Employee user)
        {
            InitializeComponent();
            currentUser = user;
            WelcomeText.Text = $"Добро пожаловать, {user.Username}";
            RequestsList.ItemsSource = db.GetRequests();
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).DataContext as Request;
            db.CompleteRequest(req.Id);
            RequestsList.ItemsSource = db.GetRequests();
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).DataContext as Request;
            CommentWindow cw = new CommentWindow(req, currentUser);
            cw.ShowDialog();
            RequestsList.Items.Refresh();
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            var newRequestWindow = new AddRequestWindow();
            bool? result = newRequestWindow.ShowDialog();

            if (result == true)
            {
                LoadRequestsFromDatabase();
            }
        }
        private void LoadRequestsFromDatabase()
        {
            try
            {
                var dbManager = new DbManager();
                var requests = dbManager.GetRequests();

                // Например, если у тебя DataGrid с заявками - обновляем ItemsSource
                RequestsList.ItemsSource = requests;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заявок: " + ex.Message);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as Button).DataContext as Request;
            var editWindow = new EditRequestWindow(request);
            if (editWindow.ShowDialog() == true)
            {
                RequestsList.Items.Refresh(); // Обновим список заявок
            }
        }

        private void OpenChat_Click(object sender, RoutedEventArgs e)
        {
            var chat = new DlyaTexOtcheta.ChatWindow();
            chat.Show();
            MessageBox.Show("Открыт чат. Проверьте новые сообщения.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var registerPage = new RegistrationPage();
            registerPage.ShowDialog();
            this.Close();
        }
    }
}
