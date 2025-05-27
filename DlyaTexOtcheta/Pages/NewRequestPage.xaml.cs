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
    /// Логика взаимодействия для NewRequestPage.xaml
    /// </summary>
    public partial class NewRequestPage : Page
    {
        private readonly DbManager dbManager;
        private readonly Employee _currentUser;
        private List<PostOffice> postOffices;

        public NewRequestPage(Employee user)
        {
            InitializeComponent();
            _currentUser = user;
            dbManager = new DbManager();
            LoadPostOffices();
        }

        private void LoadPostOffices()
        {
            try
            {
                postOffices = dbManager.GetPostOffices();
                PostOfficesComboBox.ItemsSource = postOffices;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки отделений: " + ex.Message);
            }
        }

        private void LocationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocationComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var tag = selectedItem.Tag?.ToString();

                MainOfficePanel.Visibility = tag == "MainOffice" ? Visibility.Visible : Visibility.Collapsed;
                ArkhangelskPanel.Visibility = tag == "Arkhangelsk" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            string author = AuthorTextBox.Text.Trim();
            string address = null;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (LocationComboBox.SelectedItem is ComboBoxItem selectedLocation)
            {
                var tag = selectedLocation.Tag?.ToString();

                if (tag == "MainOffice")
                {
                    if (string.IsNullOrWhiteSpace(CabinetTextBox.Text))
                    {
                        MessageBox.Show("Введите кабинет!");
                        return;
                    }
                    address = $"Главный офис, кабинет {CabinetTextBox.Text}";
                }
                else if (tag == "Arkhangelsk")
                {
                    if (PostOfficesComboBox.SelectedItem is PostOffice office)
                    {
                        address = $"{office.IndexCode} - {office.Address} ({office.WorkHours})";
                    }
                    else
                    {
                        MessageBox.Show("Выберите отделение!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Выберите местоположение!");
                    return;
                }
            }

            var request = new Request
            {
                Title = title,
                Description = description,
                Author = author,
                Address = address,
                IsCompleted = false
            };

            try
            {
                dbManager.AddRequest(request);
                MessageBox.Show("Заявка добавлена!");
                NavigationService?.Navigate(new MainWindowApp(_currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainWindowApp(_currentUser));
        }
    }
}
