using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist
{
    public partial class MyBookings : UserControl
    {
        public MyBookings()
        {
            InitializeComponent();
        }

        public void LoadHistory(string visitorId)
        {
            // Fetch rows out of the database mapping layer
            DataTable dt = BookingService.GetVisitorBookings(visitorId);

            // Bind the source directly onto the data table grid UI layout
            dgvBookings.DataSource = dt;

            // Visual Polish: Format pricing columns to South African Rand formatting styles
            if (dgvBookings.Columns.Contains("Price Per Guest"))
                dgvBookings.Columns["Price Per Guest"].DefaultCellStyle.Format = "\"R \"#,##0.00";

            if (dgvBookings.Columns.Contains("Total Paid"))
                dgvBookings.Columns["Total Paid"].DefaultCellStyle.Format = "\"R \"#,##0.00";

            // Auto-size columns to cleanly stretch out and fill up all available layout space
            dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void MyBookings_Load(object sender, EventArgs e)
        {

        }
    }
}
