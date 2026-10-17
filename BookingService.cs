using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Tourist
{

    public static class AppSession
    {
        public static string VisitorId = "";   // set this when the visitor logs in
    }

    public static class Money
    {
        public static readonly CultureInfo Za = new CultureInfo("en-ZA");

        public static string Zar(decimal v)
        {
            CultureInfo customZa = (CultureInfo)Za.Clone();
            customZa.NumberFormat.CurrencySymbol = "R";
            customZa.NumberFormat.CurrencyPositivePattern = 2;   // R 1 250,00
            customZa.NumberFormat.CurrencyNegativePattern = 12;  // -R 1 250,00
            customZa.NumberFormat.CurrencyGroupSeparator = " ";
            customZa.NumberFormat.CurrencyDecimalSeparator = ",";

            return v.ToString("C", customZa);
        }
    }

    public class EventInfo
    {
        public int Id;
        public string Name;
        public string Venue;
        public string DateText;
        public double Rating;
        public decimal PricePerGuest;
        public int SeatsAvailable;
    }

    public class BookingResult
    {
        public bool Success;
        public string Error;
        public int BookingId;
        public decimal Total;
        public decimal PricePerGuest;
        public List<string> TicketNumbers = new List<string>();
    }

    public static class BookingService
    {
        public static event Action BookingCompleted;
        public static void NotifyBookingCompleted() { var h = BookingCompleted; if (h != null) h(); }

        static SQLiteConnection Open()
        {
            var c = new SQLiteConnection(Db.ConnectionString);
            c.Open();
            return c;
        }

        // total seats minus guests already booked, for an Events row aliased as e
        const string SeatsLeftSql =
            "(e.SeatRows * e.SeatsPerRow - COALESCE((SELECT SUM(b.Guests) FROM Bookings b " +
            "WHERE b.EventId = e.EventId AND b.Status = 'Active'), 0))";

        static decimal Dec(object v)
        {
            if (v == null || v == DBNull.Value) return 0m;
            decimal d;
            decimal.TryParse(Convert.ToString(v, CultureInfo.InvariantCulture),
                             NumberStyles.Any, CultureInfo.InvariantCulture, out d);
            return d;
        }

        static string MakeDateText(object date, object time)
        {
            string d = date == DBNull.Value ? "" : Convert.ToString(date);
            string t = time == DBNull.Value ? "" : Convert.ToString(time);
            DateTime dt;
            if (DateTime.TryParse(d, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                d = dt.ToString("dddd, d MMMM yyyy", Money.Za);
            return string.IsNullOrWhiteSpace(t) ? d : d + " at " + t;
        }

        // Call once at startup (Program.cs, before Application.Run). Also needed before the events list loads.
        public static void EnsureSchema()
        {
            using (var c = Open())
            {
                new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS Bookings(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EventId INTEGER NOT NULL,
                        VisitorId TEXT NOT NULL,
                        FullName TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        Guests INTEGER NOT NULL,
                        PricePerGuest REAL NOT NULL,
                        TotalPaid REAL NOT NULL,
                        Status TEXT NOT NULL DEFAULT 'Active',
                        CreatedAt TEXT NOT NULL)", c).ExecuteNonQuery();

                new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS Tickets(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        BookingId INTEGER NOT NULL,
                        TicketNumber TEXT NOT NULL UNIQUE,
                        GuestNo INTEGER NOT NULL)", c).ExecuteNonQuery();
            }
        }

        public static EventInfo GetEvent(int eventId)
        {
            using (var c = Open())
            using (var cmd = new SQLiteCommand(
                "SELECT e.EventId, e.EventName, e.Venue, e.EventDate, e.StartTime, e.Rating, e.Price, " +
                SeatsLeftSql + " AS SeatsAvailable FROM Events e WHERE e.EventId = @id", c))
            {
                cmd.Parameters.AddWithValue("@id", eventId);
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new EventInfo
                    {
                        Id = Convert.ToInt32(r["EventId"]),
                        Name = Convert.ToString(r["EventName"]),
                        Venue = Convert.ToString(r["Venue"]),
                        DateText = MakeDateText(r["EventDate"], r["StartTime"]),
                        Rating = (double)Dec(r["Rating"]),
                        PricePerGuest = Dec(r["Price"]),
                        SeatsAvailable = Convert.ToInt32(r["SeatsAvailable"])
                    };
                }
            }
        }

        public static BookingResult CreateBooking(int eventId, string visitorId, string fullName, string email, int guests)
        {
            var res = new BookingResult();
            if (guests < 1) { res.Error = "At least 1 guest is required."; return res; }

            using (var c = Open())
            using (var tx = c.BeginTransaction())
            {
                // One statement: it only inserts the booking if enough seats are still left,
                // and the price comes from the Events table (not from the screen).
                int rows;
                using (var ins = new SQLiteCommand(
                    @"INSERT INTO Bookings(EventId,VisitorId,FullName,Email,Guests,PricePerGuest,TotalPaid,Status,CreatedAt)
                      SELECT e.EventId, @v, @n, @m, @g, e.Price, e.Price * @g, 'Active', @d
                      FROM Events e
                      WHERE e.EventId = @e AND e.Status = 'Active' AND " + SeatsLeftSql + " >= @g", c, tx))
                {
                    ins.Parameters.AddWithValue("@e", eventId);
                    ins.Parameters.AddWithValue("@v", visitorId ?? "");
                    ins.Parameters.AddWithValue("@n", fullName);
                    ins.Parameters.AddWithValue("@m", email);
                    ins.Parameters.AddWithValue("@g", guests);
                    ins.Parameters.AddWithValue("@d", DateTime.Now.ToString("s"));
                    rows = ins.ExecuteNonQuery();
                }

                if (rows == 0)
                {
                    tx.Rollback();
                    var ev = GetEvent(eventId);
                    res.Error = (ev == null || ev.SeatsAvailable <= 0)
                        ? "Sorry, this event is sold out."
                        : "Only " + ev.SeatsAvailable + " seat(s) left for this event.";
                    return res;
                }

                long bookingId = c.LastInsertRowId;

                using (var get = new SQLiteCommand("SELECT PricePerGuest, TotalPaid FROM Bookings WHERE Id = @id", c, tx))
                {
                    get.Parameters.AddWithValue("@id", bookingId);
                    using (var r = get.ExecuteReader())
                    {
                        r.Read();
                        res.PricePerGuest = Dec(r["PricePerGuest"]);
                        res.Total = Dec(r["TotalPaid"]);
                    }
                }

                // one unique ticket number per guest
                for (int i = 1; i <= guests; i++)
                {
                    string ticketNo = string.Format("BH-{0:D6}-{1:D2}-{2}", bookingId, i,
                                      Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant());
                    using (var t = new SQLiteCommand(
                        "INSERT INTO Tickets(BookingId,TicketNumber,GuestNo) VALUES(@b,@n,@i)", c, tx))
                    {
                        t.Parameters.AddWithValue("@b", bookingId);
                        t.Parameters.AddWithValue("@n", ticketNo);
                        t.Parameters.AddWithValue("@i", i);
                        t.ExecuteNonQuery();
                    }
                    res.TicketNumbers.Add(ticketNo);
                }

                tx.Commit();
                res.Success = true;
                res.BookingId = (int)bookingId;
                return res;
            }
        }

        // number of active events that still have seats
        public static int GetToursAvailable()
        {
            using (var c = Open())
            using (var cmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM Events e WHERE e.Status = 'Active' AND " + SeatsLeftSql + " > 0", c))
                return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // admin: active bookings and total revenue
        public static void GetAdminStats(out int activeBookings, out decimal revenue)
        {
            using (var c = Open())
            using (var cmd = new SQLiteCommand(
                "SELECT COUNT(*), COALESCE(SUM(TotalPaid),0) FROM Bookings WHERE Status = 'Active'", c))
            using (var r = cmd.ExecuteReader())
            {
                r.Read();
                activeBookings = Convert.ToInt32(r[0]);
                revenue = Dec(r[1]);
            }
        }

        // visitor: their number of bookings and what they have paid
        public static void GetVisitorStats(string visitorId, out int bookings, out decimal spent)
        {
            using (var c = Open())
            using (var cmd = new SQLiteCommand(
                "SELECT COUNT(*), COALESCE(SUM(TotalPaid),0) FROM Bookings WHERE VisitorId = @v AND Status = 'Active'", c))
            {
                cmd.Parameters.AddWithValue("@v", visitorId ?? "");
                using (var r = cmd.ExecuteReader())
                {
                    r.Read();
                    bookings = Convert.ToInt32(r[0]);
                    spent = Dec(r[1]);
                }
            }
        }
        public static DataTable GetVisitorBookings(string visitorId)
        {
            var dt = new DataTable();
            string sql = @"SELECT b.Id AS [Booking ID],
                          e.EventName AS [Event Name],
                          e.Venue AS [Venue],
                          b.Guests AS [Guests Booked],
                          b.PricePerGuest AS [Price Per Guest],
                          b.TotalPaid AS [Total Paid],
                          b.CreatedAt AS [Date Booked]
                   FROM Bookings b
                   INNER JOIN Events e ON b.EventId = e.EventId
                   WHERE b.VisitorId = @v
                   ORDER BY b.CreatedAt DESC";

            using (var c = Open())
            using (var cmd = new SQLiteCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@v", visitorId ?? "");
                using (var da = new SQLiteDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }


    }


}
