using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourist
{
    public static class ContactService
    {
        // Added trailing semicolon for clean SQLite connection string formatting
        private static string connectionString = "Data Source=BlueHorizon.db;Version=3;";

        public static bool SaveMessage(string name, string email, string subject, string message)
        {
            // Changed SqlConnection to SQLiteConnection
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Changed GETDATE() to DATETIME('now') for SQLite
                string query = "INSERT INTO ContactMessages (Name, Email, Subject, Message, IsRead, DateSent) VALUES (@Name, @Email, @Subject, @Message, 0, DATETIME('now'))";

                // Changed SqlCommand to SQLiteCommand
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Message", message);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static List<ContactMessage> GetAllMessages()
        {
            var list = new List<ContactMessage>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Email, Subject, Message, IsRead, DateSent FROM ContactMessages ORDER BY DateSent DESC";

                using (var cmd = new SQLiteCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ContactMessage
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Subject = reader.GetString(3),
                            Message = reader.GetString(4),
                            IsRead = Convert.ToInt32(reader["IsRead"]) == 1,
                            DateSent = Convert.ToDateTime(reader["DateSent"])
                        });
                    }
                }
            }
            return list;
        }

        public static void MarkAsRead(int messageId)
        {
            // Changed SqlConnection to SQLiteConnection
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE ContactMessages SET IsRead = 1 WHERE Id = @Id";

                // Changed SqlCommand to SQLiteCommand
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", messageId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
