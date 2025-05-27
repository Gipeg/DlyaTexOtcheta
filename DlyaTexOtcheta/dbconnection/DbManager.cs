using DlyaTexOtcheta.data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class DbManager
{
    private string connectionString = @"Server=localhost;Database=AdminRequestsDB;Trusted_Connection=True;";

    // Получить всех сотрудников
    public List<Employee> GetEmployees()
    {
        var list = new List<Employee>();
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "SELECT Id, Username, PasswordHash, IsAdmin FROM Employees";
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Employee
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                IsAdmin = reader.GetBoolean(3)
            });
        }
        return list;
    }

    // Получить заявки
    public List<Request> GetRequests()
    {
        var list = new List<Request>();
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "SELECT Id, Title, Description, Comment, CommentAuthor, Author, Address, Deadline, IsCompleted, CompletedOn FROM Requests";
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Request
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                Comment = reader.IsDBNull(3) ? null : reader.GetString(3),
                CommentAuthor = reader.IsDBNull(4) ? null : reader.GetString(4),
                Author = reader.IsDBNull(5) ? null : reader.GetString(5),
                Address = reader.IsDBNull(6) ? null : reader.GetString(6),
                Deadline = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                IsCompleted = reader.GetBoolean(8),
                CompletedOn = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
            });
        }
        return list;
    }

    // Добавить заявку
    public void AddRequest(Request req)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = @"INSERT INTO Requests (Title, Description, Author, Address, Deadline, IsCompleted)
                   VALUES (@title, @desc, @auth, @addr, @deadline, 0)";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@title", req.Title);
        cmd.Parameters.AddWithValue("@desc", req.Description);
        cmd.Parameters.AddWithValue("@auth", (object)req.Author ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@addr", (object)req.Address ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@deadline", (object)req.Deadline ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    // Обновить комментарий заявки
    public void UpdateRequestComment(int requestId, string comment, string author)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "UPDATE Requests SET Comment = @comment, CommentAuthor = @author WHERE Id = @id";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@comment", comment);
        cmd.Parameters.AddWithValue("@author", author);
        cmd.Parameters.AddWithValue("@id", requestId);
        cmd.ExecuteNonQuery();
    }

    // Завершить заявку
    public void CompleteRequest(int requestId)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "UPDATE Requests SET IsCompleted = 1, CompletedOn = @now WHERE Id = @id";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", requestId);
        cmd.Parameters.AddWithValue("@now", DateTime.Now);
        cmd.ExecuteNonQuery();
    }

    // Получить сообщения чата
    public List<ChatMessage> GetChatMessages()
    {
        var list = new List<ChatMessage>();
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "SELECT Id, Sender, Text, MediaPath, Timestamp FROM ChatMessages ORDER BY Timestamp";
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new ChatMessage
            {
                Id = reader.GetInt32(0),
                Sender = reader.GetString(1),
                Text = reader.IsDBNull(2) ? null : reader.GetString(2),
                MediaPath = reader.IsDBNull(3) ? null : reader.GetString(3),
                Timestamp = reader.GetDateTime(4)
            });
        }
        return list;
    }

    // Добавить сообщение в чат
    public void AddChatMessage(ChatMessage message)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "INSERT INTO ChatMessages (Sender, Text, MediaPath, Timestamp) VALUES (@sender, @text, @media, @timestamp)";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@sender", message.Sender);
        cmd.Parameters.AddWithValue("@text", (object)message.Text ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@media", (object)message.MediaPath ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@timestamp", message.Timestamp);
        cmd.ExecuteNonQuery();
    }

    public void AddEmployee(Employee emp)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "INSERT INTO Employees (Username, PasswordHash, IsAdmin) VALUES (@u, @p, @a)";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@u", emp.Username);
        cmd.Parameters.AddWithValue("@p", emp.Password);
        cmd.Parameters.AddWithValue("@a", emp.IsAdmin);
        cmd.ExecuteNonQuery();
    }

    public void UpdateRequestText(int requestId, string title, string description)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string sql = "UPDATE Requests SET Title = @title, Description = @desc WHERE Id = @id";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@desc", description);
        cmd.Parameters.AddWithValue("@id", requestId);
        cmd.ExecuteNonQuery();
    }
    public List<PostOffice> GetPostOffices()
    {
        var list = new List<PostOffice>();
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        string sql = "SELECT Id, IndexCode, Address, WorkHours FROM PostOffices";
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new PostOffice
            {
                Id = reader.GetInt32(0),
                IndexCode = reader.GetString(1),
                Address = reader.GetString(2),
                WorkHours = reader.GetString(3)
            });
        }
        return list;
    }
    public List<Request> GetSuccessfulRequests()
    {
        return GetRequests().Where(r => r.IsCompleted && r.CompletedOn <= r.Deadline).ToList();
    }

    public List<Request> GetFailedRequests()
    {
        return GetRequests().Where(r => r.IsCompleted && r.CompletedOn > r.Deadline).ToList();
    }

}


