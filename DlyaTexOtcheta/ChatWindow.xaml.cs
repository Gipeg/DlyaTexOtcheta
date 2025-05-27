using DlyaTexOtcheta.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace DlyaTexOtcheta
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private List<ChatMessage> messages = new();
        private const string ChatFilePath = "chat.json";

        public ChatWindow()
        {
            InitializeComponent();
            LoadMessages();
        }

        private void LoadMessages()
        {
            if (File.Exists(ChatFilePath))
            {
                string json = File.ReadAllText(ChatFilePath);
                messages = JsonSerializer.Deserialize<List<ChatMessage>>(json);
                MessagesList.ItemsSource = messages;
                MessagesList.Items.Refresh();
            }
        }

        private void SaveMessages()
        {
            string json = JsonSerializer.Serialize(messages);
            File.WriteAllText(ChatFilePath, json);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageBox.Text))
            {
                messages.Add(new ChatMessage { Text = MessageBox.Text, Time = DateTime.Now });
                SaveMessages();
                MessagesList.Items.Refresh();
                MessageBox.Clear();
            }
        }

        private void AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Media Files|*.jpg;*.jpeg;*.png;*.gif;*.mp4";

            if (dlg.ShowDialog() == true)
            {
                messages.Add(new ChatMessage { Text = $"[Файл: {System.IO.Path.GetFileName(dlg.FileName)}]", Time = DateTime.Now });
                SaveMessages();
                MessagesList.Items.Refresh();
            }
        }
    }

    public class ChatMessage
    {
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
