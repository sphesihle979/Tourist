using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist
{
    public static class Db
    {
        public static readonly string DataFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BlueHorizon");

        public static string ImagesFolder
        {
            get { return Path.Combine(DataFolder, "Images"); }
        }

        public const string ConnectionString = "Data Source=BlueHorizon.db;Version=3;";
        // creates the database file and the Events table the first time
        public static void EnsureCreated()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS Events (
    EventId INTEGER PRIMARY KEY AUTOINCREMENT,
    EventName TEXT NOT NULL,
    Category TEXT,
    Rating REAL,
    EventDate TEXT,
    StartTime TEXT,
    Status TEXT DEFAULT 'Active',
    Venue TEXT,
    Price REAL,
    Description TEXT,
    ImagePath TEXT,
    SeatRows INTEGER,
    SeatsPerRow INTEGER);";

            using (var con = new SQLiteConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                    cmd.ExecuteNonQuery();

                AddColumnIfMissing(con, "Events", "StartTime", "TEXT");
                AddColumnIfMissing(con, "Events", "SeatRows", "INTEGER");
                AddColumnIfMissing(con, "Events", "SeatsPerRow", "INTEGER");

                

            }

           

            

        }
        public static DataTable GetActiveEvents()
        {
            string sql = @"SELECT e.EventId, e.EventName, e.Category, e.Rating, e.EventDate, e.StartTime,
                          e.Venue, e.Price, e.Description, e.ImagePath,
                          e.SeatRows * e.SeatsPerRow AS TotalSeats,
                          e.SeatRows * e.SeatsPerRow - COALESCE((SELECT SUM(b.Guests) FROM Bookings b
                              WHERE b.EventId = e.EventId AND b.Status = 'Active'), 0) AS SeatsAvailable
                   FROM Events e
                   WHERE e.Status = 'Active'
                   ORDER BY e.EventDate";

            var table = new DataTable();
            using (var da = new SQLiteDataAdapter(sql, ConnectionString))
                da.Fill(table);
            return table;
        }

        // events visitors can still book: Active and not in the past
        public static int CountLiveEvents()
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            using (var con = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM Events WHERE Status = 'Active' AND EventDate >= @today", con))
            {
                cmd.Parameters.AddWithValue("@today", today);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        // every event, Active and Inactive
        public static int CountAllEvents()
        {
            using (var con = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Events", con))
            {
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static void AddColumnIfMissing(SQLiteConnection con, string table, string column, string type)
        {
            bool exists = false;
            using (var cmd = new SQLiteCommand("PRAGMA table_info(" + table + ")", con))
            using (var r = cmd.ExecuteReader())
                while (r.Read())
                    if (string.Equals(r["name"].ToString(), column, StringComparison.OrdinalIgnoreCase))
                        exists = true;

            if (!exists)
                using (var cmd = new SQLiteCommand("ALTER TABLE " + table + " ADD COLUMN " + column + " " + type, con))
                    cmd.ExecuteNonQuery();
        }

        // the best rated events visitors can still book
        public static DataTable GetTopRatedEvents(int count)
        {
             const string SeatsLeftSql =
        "(e.SeatRows * e.SeatsPerRow - COALESCE((SELECT SUM(b.Guests) FROM Bookings b " +
        "WHERE b.EventId = e.EventId AND b.Status = 'Active'), 0))";

            string sql = @"SELECT e.EventId, e.EventName, e.Category, e.Rating, e.EventDate, e.StartTime,
              e.Venue, e.Price, e.Description, e.ImagePath,
              e.SeatRows * e.SeatsPerRow AS TotalSeats, " +
              SeatsLeftSql + @" AS SeatsAvailable
       FROM Events e
       WHERE e.Status = 'Active' AND e.EventDate >= @today
       ORDER BY e.Rating DESC, e.EventDate ASC
       LIMIT @count";



            var table = new DataTable();
            using (var con = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@today", DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@count", count);
                con.Open();
                using (var da = new SQLiteDataAdapter(cmd))
                    da.Fill(table);
            }
            return table;
        }

        public static bool SaveContactMessage(string name, string email, string subject, string message)
        {
            // Fallback if subject combobox is empty
            if (string.IsNullOrWhiteSpace(subject))
            {
                subject = "General Inquiry";
            }

            try
            {
                using (var con = new SQLiteConnection(ConnectionString))
                {
                    con.Open();
                    string sql = "INSERT INTO ContactMessages (Name, Email, Subject, Message, IsRead, DateSent) VALUES (@name, @email, @subject, @message, 0, @date)";
                    using (var cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@name", name ?? string.Empty);
                        cmd.Parameters.AddWithValue("@email", email ?? string.Empty);
                        cmd.Parameters.AddWithValue("@subject", subject);
                        cmd.Parameters.AddWithValue("@message", message ?? string.Empty);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // This will pop up the exact SQLite error message so you can see the root cause!
                MessageBox.Show("Database Error: " + ex.Message, "SQLite Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static DataTable GetAllContactMessages()
        {
            string sql = "SELECT MessageId, Name, Email, Subject, Message, IsRead, DateSent FROM ContactMessages ORDER BY DateSent DESC";
            var table = new DataTable();
            using (var da = new SQLiteDataAdapter(sql, ConnectionString))
                da.Fill(table);
            return table;
        }

        public static void MarkMessageAsRead(int messageId)
        {
            using (var con = new SQLiteConnection(ConnectionString))
            {
                con.Open();
                string sql = "UPDATE ContactMessages SET IsRead = 1 WHERE MessageId = @id";
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", messageId);
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }
}
