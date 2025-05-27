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
using System.Windows.Shapes;

namespace DlyaTexOtcheta
{
    /// <summary>
    /// Логика взаимодействия для AddRequestWindow.xaml
    /// </summary>
    public partial class AddRequestWindow : Window
    {
        private DbManager dbManager;
        private List<PostOffice> postOffices;

        public AddRequestWindow()
        {
            InitializeComponent();
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
                MessageBox.Show("Ошибка загрузки отделений почты: " + ex.Message);
            }
        }

        private void LocationComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LocationComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem selectedItem)
            {
                var tag = selectedItem.Tag?.ToString();

                if (tag == "MainOffice")
                {
                    MainOfficePanel.Visibility = Visibility.Visible;
                    ArkhangelskPanel.Visibility = Visibility.Collapsed;
                }
                else if (tag == "Arkhangelsk")
                {
                    MainOfficePanel.Visibility = Visibility.Collapsed;
                    ArkhangelskPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    MainOfficePanel.Visibility = Visibility.Collapsed;
                    ArkhangelskPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            string author = AuthorTextBox.Text.Trim();
            string address = null;

            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text) || string.IsNullOrWhiteSpace(AuthorTextBox.Text))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Введите название заявки");
                return;
            }
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Введите описание заявки");
                return;
            }
            if (string.IsNullOrEmpty(author))
            {
                MessageBox.Show("Введите автора заявки");
                return;
            }

            // Определяем адрес в зависимости от выбора
            if (LocationComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem selectedLocation)
            {
                var tag = selectedLocation.Tag?.ToString();

                if (tag == "MainOffice")
                {
                    if (string.IsNullOrWhiteSpace(CabinetTextBox.Text))
                    {
                        MessageBox.Show("Введите номер кабинета для главного офиса");
                        return;
                    }
                    address = $"Главный офис, кабинет {CabinetTextBox.Text.Trim()}";
                }
                else if (tag == "Arkhangelsk")
                {
                    if (PostOfficesComboBox.SelectedItem is PostOffice selectedOffice)
                    {
                        address = $"{selectedOffice.IndexCode} - {selectedOffice.Address} (Режим работы: {selectedOffice.WorkHours})";
                    }
                    else
                    {
                        MessageBox.Show("Выберите отделение в Архангельске");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Выберите местоположение");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Выберите местоположение");
                return;
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
                MessageBox.Show("Заявка успешно добавлена");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении заявки: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
