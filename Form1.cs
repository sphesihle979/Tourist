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
    public partial class Tourist : Form
    {
        public Tourist()
        {
            InitializeComponent();
            SaveButtonColors();
            Db.EnsureCreated();
            LoadTopEvents();
           // <-- added: show the top 5 when the app opens
        }


        private void ClearUserControls()
        {
            // Remove only dynamically added UserControls.
            // Keep the original dashboard labels, panels and other controls.

            for (int i = pnlMainOne.Controls.Count - 1; i >= 0; i--)
            {
                Control control = pnlMainOne.Controls[i];

                if (control is UserControl)
                {
                    pnlMainOne.Controls.RemoveAt(i);
                    control.Dispose();
                }
            }
        }

        private void ShowDashboard()
        {
            // Remove the Browse or Booking UserControl
            ClearUserControls();

            // Show the original dashboard controls again
            foreach (Control control in pnlMainOne.Controls)
            {
                control.Visible = true;
            }

            pnlMainOne.BringToFront();

            // Refresh dashboard information
            RefreshVisitorStats();
            LoadTopEvents();
        }

        private void ShowBrowse()
        {
            // Remove the previous UserControl
            ClearUserControls();

            // Hide the dashboard content, not the sidebar
            foreach (Control control in pnlMainOne.Controls)
            {
                control.Visible = false;
            }

            // Create the browsing UserControl
            uc_browse_event browseControl = new uc_browse_event();

            // Return to the dashboard when requested
            browseControl.BackToDashboard += (s, e) =>
            {
                ShowDashboard();
                HighlightButton(btnHome);
            };

            // Open the selected event in Booking
            browseControl.BookNowRequested += eventId =>
            {
                ShowBooking(eventId);
            };

            browseControl.Dock = DockStyle.Fill;
            pnlMainOne.Controls.Add(browseControl);
            browseControl.BringToFront();
        }

        private void ShowBooking(int eventId)
        {
            ClearUserControls();

            // Hide the dashboard content
            foreach (Control control in pnlMainOne.Controls)
            {
                control.Visible = false;
            }

            // Open the Booking UserControl
            Booking bookingControl = new Booking();

            bookingControl.BackRequested += () =>
            {
                ShowBrowse();
                HighlightButton(btnExploreTours);
            };

            bookingControl.Dock = DockStyle.Fill;
            pnlMainOne.Controls.Add(bookingControl);
            bookingControl.BringToFront();

            // Load the selected event
            bookingControl.LoadEvent(eventId);
        }



        private void btnExploreTours_Click(object sender, EventArgs e)
        {
            HighlightButton(btnExploreTours);
            ShowBrowse();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HighlightButton(btnHome);
            ShowDashboard();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HighlightButton(btnExploreTours);
            ShowBrowse();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HighlightButton(button4);
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // ---------- top rated events ----------

        public void LoadTopEvents()
        {
            flpTopEvents.SuspendLayout();

            while (flpTopEvents.Controls.Count > 0)
            {
                Control old = flpTopEvents.Controls[0];
                flpTopEvents.Controls.RemoveAt(0);
                old.Dispose();
            }

            foreach (DataRow row in Db.GetTopRatedEvents(5).Rows)
                flpTopEvents.Controls.Add(MakeCard(row));

            flpTopEvents.ResumeLayout();

            
        }

        private Control MakeCard(DataRow row)
        {
            int id = Convert.ToInt32(row["EventId"]);

            int seatsLeft = row.Table.Columns.Contains("SeatsAvailable") && row["SeatsAvailable"] != DBNull.Value
                ? Convert.ToInt32(row["SeatsAvailable"])
                : Convert.ToInt32(row["TotalSeats"]);

            var card = new Panel { Width = 240, Height = 316, Margin = new Padding(10), BackColor = Color.White };

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
                Width = 224,
                Height = 24
            };

            decimal priceValue;
            decimal.TryParse(Convert.ToString(row["Price"], CultureInfo.InvariantCulture),
                             NumberStyles.Any, CultureInfo.InvariantCulture, out priceValue);

            // CRUCIAL: Pass the numeric price through your global custom Zar method explicitly
            string formattedPrice = Money.Zar(priceValue);

            string seatsText = seatsLeft <= 0 ? "SOLD OUT"
                             : seatsLeft + (seatsLeft == 1 ? " seat left" : " seats left");

            var lblInfo = new Label
            {
                Text = row["Category"] + "  |  " + row["EventDate"] + "\n" +
                       row["Venue"] + "\n" +
                       formattedPrice + "  |  Rating " + row["Rating"] + "\n" + // Uses the explicit Rand value
                       seatsText,
                Left = 8,
                Top = 184,
                Width = 224,
                Height = 84
            };
            if (seatsLeft <= 0) lblInfo.ForeColor = Color.Firebrick;

            var btnBook = new Button { Text = seatsLeft <= 0 ? "Sold out" : "Book now", Left = 8, Top = 274, Width = 224, Height = 32 };
            btnBook.Click += (a, b) => ShowBooking(id);

            card.Controls.AddRange(new Control[] { pic, lblName, lblInfo, btnBook });
            return card;
        }


        private void HighlightButton(Button selectedButton)
        {
            Button[] sidebarButtons =
     {
        btnHome,
        btnExploreTours,
        btnMyBookings,
        btnMyProfile,
        btnContactUs,
        button4,
        button6
    };

            foreach (Button btn in sidebarButtons)
            {
                // Restore the original color
                if (btn.Tag is Color originalColor)
                {
                    btn.BackColor = originalColor;
                }
            }

            // Brighten the selected button using its original color
            if (selectedButton.Tag is Color selectedColor)
            {
                selectedButton.BackColor =
                    ControlPaint.Light(selectedColor, 0.25f);
            }
        }

        private void SaveButtonColors()
        {
            Button[] sidebarButtons =
            {
                  btnHome,
                  btnExploreTours,
                  btnMyBookings,
                  btnMyProfile,
                  btnContactUs,
                  button4,
                  button6
            };

            foreach (Button btn in sidebarButtons)
            {
                btn.Tag = btn.BackColor;
            }
        }

        private void Tourist_Load(object sender, EventArgs e)
        {
            BookingService.BookingCompleted += RefreshAll;
            
            RefreshAll();
        }

        private void Tourist_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            BookingService.BookingCompleted -= RefreshAll;
        }

        void RefreshAll()
        {
            RefreshVisitorStats();
            LoadTopEvents();
        }

        void RefreshVisitorStats()
        {
            lblToursAvailable.Text = BookingService.GetToursAvailable().ToString();

            int bookings; decimal spent;
            BookingService.GetVisitorStats(AppSession.VisitorId, out bookings, out spent);
            lblTotalBookings.Text = bookings.ToString();
            lblTotalBooked.Text = Money.Zar(spent);
        }

        private void btnMyBookings_Click(object sender, EventArgs e)
        {
            HighlightButton(btnMyBookings);
            ShowMyBookings();
        }

        private void ShowMyBookings()
        {
            // Remove previous user controls dynamically active on main panel deck
            ClearUserControls();

            // Hide background static elements inside pnlMainOne container workspace
            foreach (Control control in pnlMainOne.Controls)
            {
                control.Visible = false;
            }

            // Create the booking lookup history control screen instance
            MyBookings historyControl = new MyBookings();
            historyControl.Dock = DockStyle.Fill;

            // Add layout control to panel hierarchy layer
            pnlMainOne.Controls.Add(historyControl);
            historyControl.BringToFront();

            // Feed current login session details right into target tracking table grid matrix
            historyControl.LoadHistory(AppSession.VisitorId);
        }

        private void ShowPage(UserControl page)
        {
            ClearUserControls();

            // Hide the original dashboard controls
            foreach (Control control in pnlMainOne.Controls)
            {
                control.Visible = false;
            }

            // Display the requested page
            page.Dock = DockStyle.Fill;
            pnlMainOne.Controls.Add(page);
            page.BringToFront();
        }

        private void btnContactUs_Click(object sender, EventArgs e)
        {
            HighlightButton(btnContactUs);
            ShowPage(new ContactUs());
        }

        private void btnMyProfile_Click(object sender, EventArgs e)
        {
            HighlightButton(btnMyProfile);
            ShowPage(new MyProfile());
        }
    }
}
