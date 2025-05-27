using DlyaTexOtcheta.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace DlyaTexOtcheta.models
{
    public static class ChatStorage
    {
        private static string FilePath = "chat.json";

        public static List<ChatMessage> LoadMessages()
        {
            if (!File.Exists(FilePath)) return new List<ChatMessage>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<ChatMessage>>(json) ?? new List<ChatMessage>();
        }

        public static void SaveMessages(List<ChatMessage> messages)
        {
            var json = JsonSerializer.Serialize(messages, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
