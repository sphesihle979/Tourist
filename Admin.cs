using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Tourist
{
    public partial class Admin : Form
    {
        private UserControl current;
        private readonly List<Control> dashboardControls = new List<Control>();
        private readonly System.Windows.Forms.Timer statsTimer = new System.Windows.Forms.Timer { Interval = 3000 };

        public Admin()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Db.EnsureCreated();
            BookingService.EnsureSchema();
            SetupGrid(); // adds the Edit and Delete button columns, once
            dgvEvents.CellClick += dgvEvents_CellClick;
            LoadEvents();
            RefreshStats();
            statsTimer.Tick += statsTimer_Tick;
            statsTimer.Start();
            FormClosed += (a, b) => statsTimer.Dispose();
            // 1. controls you gave Tag = "dashboard" are the dashboard
            foreach (Control c in pnlMain.Controls)
                if (c.Tag != null && c.Tag.ToString() == "dashboard")
                    dashboardControls.Add(c);

            // 2. if none are tagged, treat everything on pnlMain (except the list panel) as dashboard
            if (dashboardControls.Count == 0)
                foreach (Control c in pnlMain.Controls)
                    if (c != pnlManageEvents)
                        dashboardControls.Add(c);

           
        }

        private void dgvEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string column = dgvEvents.Columns[e.ColumnIndex].Name;
            int id = Convert.ToInt32(dgvEvents.Rows[e.RowIndex].Cells["EventId"].Value);

            if (column == "colDelete") DeleteEvent(id);
            else if (column == "colEdit") ShowManageEvents(id);

            
        }

        // ---------- navigation ----------

        private void ClearUserControls()
        {
            for (int i = pnlMain.Controls.Count - 1; i >= 0; i--)
            {
                Control c = pnlMain.Controls[i];
                if (c is UserControl)
                {
                    pnlMain.Controls.RemoveAt(i);
                    c.Dispose();
                }
            }
        }

        private void ShowDashboard()
        {
            ClearUserControls();
            foreach (Control c in pnlMain.Controls)
                c.Visible = true;                  // banner, cards, the list: everything on pnlMain
            pnlMain.Visible = true;
            pnlMain.BringToFront();
            LoadEvents();
            RefreshStats();
        }

        private void ShowManageEvents(int eventId = 0)
        {
            ClearUserControls();
            foreach (Control c in pnlMain.Controls)
                c.Visible = false;                 // hide the dashboard content

            var uc = new Manage_events();
            if (eventId > 0) uc.LoadEvent(eventId);
            uc.Finished += (s, e) => BeginInvoke((Action)ShowDashboard);
            uc.BackToDashboard += (s, e) => BeginInvoke((Action)ShowDashboard);

            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(uc);
            uc.BringToFront();
        }


        // ---------- button clicks ----------

       
        
        private void btnBack_Click(object sender, EventArgs e) { ShowDashboard(); }  // on the list

        private void btnDash_Click(object sender, EventArgs e)
        {
            { ShowDashboard(); }
        }

        private void btnManageEvents_Click_1(object sender, EventArgs e)
        {
            { ShowManageEvents(); }
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            { ShowManageEvents(); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log out", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
            if (result==DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {

            }
        }
        private void SetupGrid()
        {
            dgvEvents.ReadOnly = true;
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvEvents.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "",
                Text = "Edit",
                UseColumnTextForButtonValue = true
            });
            dgvEvents.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colDelete",
                HeaderText = "",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            });
        } 

        private void LoadEvents()
        {
            string sql = "SELECT EventId, EventName, Category, Rating, EventDate, Status FROM Events ORDER BY EventDate";

            using (var da = new SQLiteDataAdapter(sql, Db.ConnectionString))
            {
                var table = new DataTable();
                da.Fill(table);
                dgvEvents.DataSource = table;
            }

            dgvEvents.Columns["EventId"].Visible = false;                  // the id isn't shown
            dgvEvents.Columns["colEdit"].DisplayIndex = dgvEvents.Columns.Count - 2;
            dgvEvents.Columns["colDelete"].DisplayIndex = dgvEvents.Columns.Count - 1;
        }

        private void DeleteEvent(int id)
        {
            // how many guests have already booked this event
            int booked;
            using (var chk = new SQLiteConnection(Db.ConnectionString))
            using (var q = new SQLiteCommand(
                "SELECT COALESCE(SUM(Guests),0) FROM Bookings WHERE EventId = @id AND Status = 'Active'", chk))
            {
                q.Parameters.AddWithValue("@id", id);
                chk.Open();
                booked = Convert.ToInt32(q.ExecuteScalar());
            }

            string warning = booked > 0
                ? "This event already has " + booked + " booked guest(s). Delete it anyway?"
                : "Delete this event?";

            if (MessageBox.Show(warning, "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            using (var con = new SQLiteConnection(Db.ConnectionString))
            using (var cmd = new SQLiteCommand("DELETE FROM Events WHERE EventId = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            LoadEvents();   // refresh the list
            RefreshStats();
        }

        private void statsTimer_Tick(object sender, EventArgs e)
        {
            if (!lblLiveNow.Visible) return;          // not on the dashboard right now
            RefreshStats();
        }

        private void RefreshStats()
        {
            try
            {
                lblTotalEvents.Text = Db.CountAllEvents().ToString();   // <-- add: all events
                lblLiveNow.Text = Db.CountLiveEvents().ToString();
                RefreshAdminStats();
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                // database busy for a moment, try again on the next tick
            }
        }

        private void btnAddEvent1_Click(object sender, EventArgs e)
        {
            { ShowManageEvents(); }
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            BookingService.BookingCompleted += RefreshAdminStats;
            RefreshAdminStats();
        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            BookingService.BookingCompleted -= RefreshAdminStats;
        }

        void RefreshAdminStats()
        {
            int active; decimal revenue;
            BookingService.GetAdminStats(out active, out revenue);
            lblActiveBookings.Text = active.ToString();
            lblTotalBooked.Text = Money.Zar(revenue);   // revenue in ZAR
        }

       
        private void btnNotifications_Click(object sender, EventArgs e)
        {
            ClearUserControls();
            foreach (Control c in pnlMain.Controls)
                c.Visible = false;                 // hide the dashboard content, don't remove it

            var uc = new Notifications { Dock = DockStyle.Fill };
            pnlMain.Controls.Add(uc);
            uc.BringToFront();

        }
    }


    
}

