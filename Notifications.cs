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
    public partial class Notifications : UserControl
    {
        public Notifications()
        {
            InitializeComponent();

            
        }

        private async void Notifications_Load(object sender, EventArgs e)
        {
            await LoadNotificationDataAsync();
        }

        private async Task LoadNotificationDataAsync()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // Fetch messages from your database service
                var messages = await Task.Run(() => ContactService.GetAllMessages());

                int totalCount = messages.Count;
                int unreadCount = messages.Count(m => !m.IsRead);
                int readCount = messages.Count(m => m.IsRead);

                // Update your top counter labels/cards
                lblTotal.Text = totalCount.ToString();
                lblUnread.Text = unreadCount.ToString();
                lblRead.Text = readCount.ToString();

                // Bind data to the DataGridView (the "Recent messages" area)
                dgvMessages.DataSource = messages;

                // Optional styling for the grid
                if (dgvMessages.Columns["Id"] != null) dgvMessages.Columns["Id"].Visible = false;
                dgvMessages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load notifications: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // Optional: Double-clicking a message in the grid marks it as read and views full details
        private async void dgvMessages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedMessage = (ContactMessage)dgvMessages.Rows[e.RowIndex].DataBoundItem;

                // Mark as read in DB if it was unread
                if (!selectedMessage.IsRead)
                {
                    await Task.Run(() => ContactService.MarkAsRead(selectedMessage.Id));
                    await LoadNotificationDataAsync(); // Refresh counts and grid
                }

                // Show full message details popup or panel
                MessageBox.Show($"From: {selectedMessage.Name} ({selectedMessage.Email})\nSubject: {selectedMessage.Subject}\n\n{selectedMessage.Message}",
                                "Message Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
    }
}
