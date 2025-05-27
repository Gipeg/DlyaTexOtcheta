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
    /// Логика взаимодействия для CommentWindow.xaml
    /// </summary>
    public partial class CommentWindow : Window
    {
        private readonly Request currentRequest;
        private readonly Employee currentEmployee;
        private readonly DbManager dbManager = new DbManager();

        public CommentWindow(Request request, Employee employee)
        {
            InitializeComponent();
            currentRequest = request;
            currentEmployee = employee;

            // Показать текущий комментарий (если есть)
            CommentTextBox.Text = currentRequest.Comment ?? string.Empty;
        }

        private void SaveComment_Click(object sender, RoutedEventArgs e)
        {
            string commentText = CommentTextBox.Text.Trim();

            dbManager.UpdateRequestComment(currentRequest.Id, commentText, currentEmployee.Username);

            currentRequest.Comment = commentText;
            currentRequest.CommentAuthor = currentEmployee.Username;

            this.DialogResult = true;
            this.Close();
        }
    }
}
