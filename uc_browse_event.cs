using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist
{
    public partial class uc_browse_event : UserControl
    {
        public event EventHandler BackToDashboard;
        public event Action<int> BookNowRequested;
        private DataTable allEvents;
        public uc_browse_event()
        {
            InitializeComponent();
            LoadEvents();
            VisibleChanged += (s, e) => { if (Visible) LoadEvents(); };   // refresh each time it is shown
                                                                          // Connect the search and filter controls
            
            // Load the sorting options
            cmbOther.Items.Clear();
            cmbOther.Items.Add("Default Order");
            cmbOther.Items.Add("Rating: High to Low");
            cmbOther.Items.Add("Rating: Low to High");
            cmbOther.Items.Add("Price: High to Low");
            cmbOther.Items.Add("Price: Low to High");
            cmbOther.SelectedIndex = 0;

            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            cmbOther.SelectedIndexChanged += cmbOther_SelectedIndexChanged;


            LoadEvents();

           
        }

        private void LoadEvents()
        {
            allEvents = Db.GetActiveEvents();

            if (allEvents == null)
                return;

            cmbCategory.SelectedIndexChanged -=
                cmbCategory_SelectedIndexChanged;

            try
            {
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All Categories");

                var categories = allEvents.AsEnumerable()
                    .Where(row => row["Category"] != DBNull.Value)
                    .Select(row => row["Category"].ToString())
                    .Where(category => !string.IsNullOrWhiteSpace(category))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(category => category);

                foreach (string category in categories)
                    cmbCategory.Items.Add(category);

                cmbCategory.SelectedIndex = 0;
            }
            finally
            {
                cmbCategory.SelectedIndexChanged +=
                    cmbCategory_SelectedIndexChanged;
            }

            ApplyFilters();

        }

        private Control MakeCard(DataRow row)
        {
            int id = Convert.ToInt32(row["EventId"]);

            // seats left: uses SeatsAvailable if the query provides it (see step 5), otherwise TotalSeats
            int seatsLeft = row.Table.Columns.Contains("SeatsAvailable") && row["SeatsAvailable"] != DBNull.Value
                ? Convert.ToInt32(row["SeatsAvailable"])
                : Convert.ToInt32(row["TotalSeats"]);

            var card = new Panel { Width = 260, Height = 316, Margin = new Padding(10), BackColor = Color.White };

            var pic = new PictureBox { Dock = DockStyle.Top, Height = 150, SizeMode = PictureBoxSizeMode.Zoom };
            string file = row["ImagePath"].ToString();
            string path = Path.Combine(Db.ImagesFolder, file);
            if (file != "" && File.Exists(path))
                using (var img = Image.FromFile(path))
                    pic.Image = new Bitmap(img);

            var lblName = new Label
            {
                Text = row["EventName"].ToString(),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Left = 8,
                Top = 158,
                Width = 244,
                Height = 24
            };

            string price = GetNumber(row, "Price").ToString("C", new CultureInfo("en-ZA"));
            string seatsText = seatsLeft <= 0 ? "SOLD OUT"
                             : seatsLeft + (seatsLeft == 1 ? " seat left" : " seats left");

            var lblInfo = new Label
            {
                Text = row["Category"] + "  |  " + row["EventDate"] + "\n" +
                       row["Venue"] + "\n" +
                       price + "  |  ★ " + row["Rating"] + "\n" +
                       seatsText,
                Left = 8,
                Top = 184,
                Width = 244,
                Height = 84
            };
            if (seatsLeft <= 0) lblInfo.ForeColor = Color.Firebrick;

            // stays clickable when sold out: the booking screen blocks payment and explains why
            var btnBook = new Button { Text = seatsLeft <= 0 ? "Sold out" : "Book now", Left = 8, Top = 274, Width = 244, Height = 32 };
            btnBook.Click += (s, e) => BookEvent(id);

            card.Controls.AddRange(new Control[] { pic, lblName, lblInfo, btnBook });
            return card;
        }



        private void BookEvent(int eventId)
        {
            if (BookNowRequested != null) BookNowRequested(eventId);
        }

        

        private void btnBack_Click(object sender, EventArgs e)
        {
            BackToDashboard?.Invoke(this, EventArgs.Empty);
        }

        private void uc_browse_event_Load(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            // Search through all active events
            if (allEvents == null)
                return;

            flpEvents.SuspendLayout();

            try
            {
                flpEvents.Controls.Clear();

                foreach (DataRow row in allEvents.Rows)
                {
                    string eventName = row["EventName"] == DBNull.Value
                        ? ""
                        : row["EventName"].ToString();

                    string venue = row["Venue"] == DBNull.Value
                        ? ""
                        : row["Venue"].ToString();

                    string category = row["Category"] == DBNull.Value
                        ? ""
                        : row["Category"].ToString();

                    // Show matching events as the user types
                    if (eventName.IndexOf(
                            searchText,
                            StringComparison.OrdinalIgnoreCase) >= 0
                        || venue.IndexOf(
                            searchText,
                            StringComparison.OrdinalIgnoreCase) >= 0
                        || category.IndexOf(
                            searchText,
                            StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        flpEvents.Controls.Add(MakeCard(row));
                    }
                }
            }
            finally
            {
                flpEvents.ResumeLayout(true);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (allEvents == null)
                return;

            string searchText = txtSearch.Text.Trim();

            string category = cmbCategory.SelectedItem == null
                ? "All Categories"
                : cmbCategory.SelectedItem.ToString();

            string sortOption = cmbOther.SelectedItem == null
                ? "Default Order"
                : cmbOther.SelectedItem.ToString();

            // Start with all active events
            List<DataRow> events = allEvents.AsEnumerable().ToList();

            // 1. Search by event name, venue or category
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                events = events.Where(row =>
                {
                    string name = row["EventName"] == DBNull.Value
                        ? "" : row["EventName"].ToString();

                    string venue = row["Venue"] == DBNull.Value
                        ? "" : row["Venue"].ToString();

                    string eventCategory = row["Category"] == DBNull.Value
                        ? "" : row["Category"].ToString();

                    return name.IndexOf(searchText,
                               StringComparison.OrdinalIgnoreCase) >= 0
                        || venue.IndexOf(searchText,
                               StringComparison.OrdinalIgnoreCase) >= 0
                        || eventCategory.IndexOf(searchText,
                               StringComparison.OrdinalIgnoreCase) >= 0;
                }).ToList();
            }

            // 2. Filter by category
            if (category != "All Categories")
            {
                events = events.Where(row =>
                    string.Equals(
                        row["Category"] == DBNull.Value
                            ? "" : row["Category"].ToString(),
                        category,
                        StringComparison.OrdinalIgnoreCase
                    )).ToList();
            }

            // 3. Sort by rating or price
            switch (sortOption)
            {
                case "Rating: High to Low":
                    events = events.OrderByDescending(
                        row => GetNumber(row, "Rating")).ToList();
                    break;

                case "Rating: Low to High":
                    events = events.OrderBy(
                        row => GetNumber(row, "Rating")).ToList();
                    break;

                case "Price: High to Low":
                    events = events.OrderByDescending(
                        row => GetNumber(row, "Price")).ToList();
                    break;

                case "Price: Low to High":
                    events = events.OrderBy(
                        row => GetNumber(row, "Price")).ToList();
                    break;
            }

            // 4. Display the filtered events
            flpEvents.SuspendLayout();

            try
            {
                flpEvents.Controls.Clear();

                foreach (DataRow row in events)
                {
                    flpEvents.Controls.Add(MakeCard(row));
                }
            }
            finally
            {
                flpEvents.ResumeLayout(true);
            }
        }

        private decimal GetNumber(DataRow row, string columnName)
        {
            if (row[columnName] == DBNull.Value)
                return 0m;

            decimal.TryParse(
                Convert.ToString(row[columnName]),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal result);

            return result;
        }

    }
}
