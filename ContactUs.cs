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
    public partial class ContactUs : UserControl
    {
        public ContactUs()
        {
            InitializeComponent();
        }

        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmailAddress.Text.Trim();
            string subject = cmbSubject.SelectedItem?.ToString() ?? "General Inquiry";
            string message = txtMessage.Text.Trim();

            // Basic validation
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email, and Message).",
                                "Incomplete Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSendMessage.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                // Save message to database/backend service
                bool success = await Task.Run(() => ContactService.SaveMessage(name, email, subject, message));

                if (success)
                {
                    MessageBox.Show("Thank you! Your message has been sent successfully.",
                                    "Message Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear inputs
                    txtName.Clear();
                    txtEmailAddress.Clear();
                    txtMessage.Clear();
                    if (cmbSubject.Items.Count > 0) cmbSubject.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Could not send your message. Please try again later.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnSendMessage.Enabled = true;
            }
        }
    }
}
