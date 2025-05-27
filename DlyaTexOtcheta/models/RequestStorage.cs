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
    public static class RequestStorage
    {
        private static string FilePath = "request.json";

        public static List<Request> LoadRequests()
        {
            if (!File.Exists(FilePath)) return new List<Request>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Request>>(json) ?? new List<Request>();
        }

        public static void SaveRequests(List<Request> requests)
        {
            var json = JsonSerializer.Serialize(requests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
