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
    /// Логика взаимодействия для EditRequestWindow.xaml
    /// </summary>
    public partial class EditRequestWindow : Window
    {
        private readonly Request currentRequest;
        private readonly DbManager dbManager = new DbManager();

        public EditRequestWindow(Request request)
        {
            InitializeComponent();
            currentRequest = request;

            TitleBox.Text = request.Title;
            DescriptionBox.Text = request.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newTitle = TitleBox.Text.Trim();
            string newDescription = DescriptionBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(newTitle) || string.IsNullOrWhiteSpace(newDescription))
            {
                MessageBox.Show("Название и описание не должны быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dbManager.UpdateRequestText(currentRequest.Id, newTitle, newDescription);

            currentRequest.Title = newTitle;
            currentRequest.Description = newDescription;

            this.DialogResult = true;
            this.Close();
        }
    }
}
