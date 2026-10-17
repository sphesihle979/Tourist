using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Emit;
using Label = System.Windows.Forms.Label;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist
{
    public partial class Booking : UserControl
    {
        EventInfo _event;
        bool _busy;

        public event Action BackRequested;   // the host form listens to this to show the tours again

        public Booking()
        {
            InitializeComponent();
            
            nudGuests.Minimum = 1;      // minimum 1 guest
            nudGuests.Maximum = 1000;   // high on purpose; we warn when it is above the seats left
            nudGuests.Value = 1;

            SetPlaceholder(txtCardNumber, "1234 1234 1234 1234");
            SetPlaceholder(txtMonth, "MM");
            SetPlaceholder(txtYear, "YY");
            SetPlaceholder(txtCVC, "CVC");
            SetPlaceholder(txtFullName, "Full name");
            SetPlaceholder(txtEmailAddress, "Email address");
            SetupValidation();
            nudGuests.ValueChanged += nudGuests_ValueChanged;
            btnPayNow.Click += btnPayNow_Click;
            btnBack.Click += delegate { if (BackRequested != null) BackRequested(); };
        }

        // Called when "Book now" is clicked on an event card
        public void LoadEvent(int eventId)
        {
            _event = BookingService.GetEvent(eventId);
            if (_event == null)
            {
                MessageBox.Show("This event could not be found.", "Blue Horizon",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetBookingEnabled(false);
                return;
            }

            lblEventName.Text = _event.Name;
            lblVenue.Text = _event.Venue;
            lblDate.Text = _event.DateText; 
            lblRating.Text = "\u2605 " + _event.Rating.ToString("0.0", CultureInfo.InvariantCulture);
            lblPrice.Text = "Price per guest: " + Money.Zar(_event.PricePerGuest);

            nudGuests.Value = 1;
            UpdateTotals();

            if (_event.SeatsAvailable <= 0)
            {
                SetBookingEnabled(false);
                btnPayNow.Text = "Sold out";
                MessageBox.Show("Sorry, \"" + _event.Name + "\" is sold out. Booking is not available.",
                                "Sold out", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SetBookingEnabled(true);
                btnPayNow.Text = "Pay now";
            }
        }

        // blocks everything on the booking and payment side (Back to tours still works)
        void SetBookingEnabled(bool enabled)
        {
            txtFullName.Enabled = enabled;
            txtEmailAddress.Enabled = enabled;
            nudGuests.Enabled = enabled;
            txtCardNumber.Enabled = enabled;
            txtMonth.Enabled = enabled;
            txtYear.Enabled = enabled;
            txtCVC.Enabled = enabled;
            btnPayNow.Enabled = enabled;
        }

        void nudGuests_ValueChanged(object sender, EventArgs e)
        {
            if (_event == null) return;

            int seats = _event.SeatsAvailable;
            int wanted = (int)nudGuests.Value;

            if (seats > 0 && wanted > seats)
            {
                nudGuests.Value = seats;
                MessageBox.Show("Sorry, we only have " + seats + " seat" + (seats == 1 ? "" : "s") +
                                " left for this event. You asked for " + wanted +
                                ", so the number of guests has been set to " + seats + ".",
                                "Not enough seats", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdateTotals();
        }

        // label10 and label15 always show the same total
        void UpdateTotals()
        {
            if (_event == null) return;
            decimal total = _event.PricePerGuest * (int)nudGuests.Value;
            lblTotal.Text = Money.Zar(total);
            lblTotalDue.Text = Money.Zar(total);
        }

        async void btnPayNow_Click(object sender, EventArgs e)
        {

            
            if (_busy || _event == null) return;

            if (!ValidateAll()) return;   // red messages show under the boxes that are wrong

            string name = txtFullName.Text.Trim();
            string email = txtEmailAddress.Text.Trim();
            int guests = (int)nudGuests.Value;

            _busy = true;
            btnPayNow.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                // The card is only validated, NOT charged and NOT stored.
                // For real payments, call a gateway (PayFast, Yoco, Paystack) here first.

                var result = await Task.Run(() => BookingService.CreateBooking(
                    _event.Id, AppSession.VisitorId, name, email, guests));

                if (!result.Success)
                {
                    MessageBox.Show(result.Error, "Booking not completed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadEvent(_event.Id);
                    return;
                }

                var ev = _event;
                var pdfs = await Task.Run(() =>
                {
                    var list = new List<byte[]>();
                    for (int i = 0; i < result.TicketNumbers.Count; i++)
                        list.Add(TicketPdfService.CreateTicket(ev, name, result.TicketNumbers[i],
                                                               i + 1, result.TicketNumbers.Count, result.PricePerGuest));
                    return list;
                });

                string folder = SaveCopies(result.TicketNumbers, pdfs);

                string message;
                try
                {
                    await EmailService.SendTicketsAsync(email, name, ev, result.TicketNumbers, pdfs);
                    message = "Payment successful!\n\n" + guests + " ticket" + (guests == 1 ? "" : "s") +
                              " sent to " + email + ".\nTotal paid: " + Money.Zar(result.Total);
                }
                catch (Exception ex)
                {
                    message = "Your booking is confirmed (total " + Money.Zar(result.Total) + "), " +
                              "but the email could not be sent:\n" + ex.Message +
                              "\n\nYour tickets were saved here:\n" + folder;
                }

                BookingService.NotifyBookingCompleted();   // updates dashboards and event cards

                MessageBox.Show(message, "Booking complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (BackRequested != null) BackRequested();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message, "Blue Horizon",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _busy = false;
                if (_event != null && _event.SeatsAvailable > 0) btnPayNow.Enabled = true;
            }
        }

        string SaveCopies(List<string> ticketNos, List<byte[]> pdfs)
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                         "Blue Horizon Tickets");
            try
            {
                Directory.CreateDirectory(folder);
                for (int i = 0; i < pdfs.Count; i++)
                    File.WriteAllBytes(Path.Combine(folder, "Ticket-" + ticketNos[i] + ".pdf"), pdfs[i]);
            }
            catch { }
            return folder;
        }
        // ---------- validation (regex) with red messages while typing ----------
        static readonly Regex NameRx = new Regex(@"^\p{L}[\p{L} '.\-]{1,59}$", RegexOptions.Compiled);
        static readonly Regex EmailRx = new Regex(@"^[A-Za-z0-9._%+\-]+@[A-Za-z0-9\-]+(\.[A-Za-z0-9\-]+)*\.[A-Za-z]{2,}$", RegexOptions.Compiled);
        static readonly Regex CardRx = new Regex(@"^\d{13,19}$", RegexOptions.Compiled);
        static readonly Regex MonthRx = new Regex(@"^(0?[1-9]|1[0-2])$", RegexOptions.Compiled);
        static readonly Regex YearRx = new Regex(@"^(\d{2}|\d{4})$", RegexOptions.Compiled);
        static readonly Regex CvcRx = new Regex(@"^\d{3,4}$", RegexOptions.Compiled);

        Label lblNameErr, lblEmailErr, lblCardErr, lblExpiryErr, lblCvcErr;
        Color _normalBack;

        string CheckName()
        {
            return NameRx.IsMatch(Real(txtFullName)) ? null
                : "Enter your full name (letters only, at least 2).";
        }

        string CheckEmail()
        {
            return EmailRx.IsMatch(Real(txtEmailAddress)) ? null
                : "Enter a valid email, like name@example.com.";
        }

        string CheckCard()
        {
            string card = Real(txtCardNumber).Replace(" ", "").Replace("-", "");
            if (!CardRx.IsMatch(card)) return "Card number must be 13 to 19 digits.";
            if (!PassesLuhn(card)) return "This card number is not valid.";
            return null;
        }

        string CheckExpiry()
        {
            string m = Real(txtMonth), y = Real(txtYear);
            if (!MonthRx.IsMatch(m)) return "Month must be 1 to 12.";
            if (!YearRx.IsMatch(y)) return "Year must be 2 or 4 digits.";

            int month = int.Parse(m), year = int.Parse(y);
            if (year < 100) year += 2000;
            if (year > DateTime.Today.Year + 20) return "That year is too far ahead.";
            if (new DateTime(year, month, 1).AddMonths(1) <= DateTime.Today) return "This card has expired.";
            return null;
        }

        string CheckCvc()
        {
            return CvcRx.IsMatch(Real(txtCVC)) ? null : "CVC must be 3 or 4 digits.";
        }

        // shows or clears the red message and the light red box colour
        void ShowError(Label lbl, string message, params TextBox[] boxes)
        {
            bool bad = message != null;
            lbl.Text = bad ? message : "";
            lbl.Visible = bad;
            foreach (var b in boxes) b.BackColor = bad ? Color.MistyRose : _normalBack;
        }

        // small red label placed under a text box
        Label MakeErrorLabel(Control box)
        {
            var l = new Label
            {
                AutoSize = true,
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8f),
                Visible = false,
                Location = new Point(box.Left, box.Bottom + 1)
            };
            box.Parent.Controls.Add(l);
            l.BringToFront();
            return l;
        }

        // While typing: red as soon as the text is not valid, gone as soon as it is valid.
        void Watch(TextBox box, Label lbl, Func<string> check)
        {
            box.TextChanged += (s, e) =>
            {
                if (Real(box).Length == 0)
                    ShowError(lbl, null, box);          // empty: stay quiet until they leave the box or press Pay now
                else
                    ShowError(lbl, check(), box);
            };
            box.Leave += (s, e) => ShowError(lbl, check(), box);
        }

        private void txtCardNumber_Enter(object sender, EventArgs e)
        {
            if (txtCardNumber.Text =="1234 1234 1234 1234")
            {
                txtCardNumber.Text = "";
                txtCardNumber.ForeColor = Color.Black;
            }
        }

        private void txtCardNumber_Leave(object sender, EventArgs e)
        {
            if (txtCardNumber.Text == "")
            {
                txtCardNumber.Text = "1234 1234 1234 1234";
                txtCardNumber.ForeColor = Color.Gray;
            }
        }

        

        private void txtMonth_Leave(object sender, EventArgs e)
        {
            if (txtMonth.Text == "")
            {
                txtMonth.Text = "MM";
                txtMonth.ForeColor = Color.Gray;
            }
        }

        private void txtMonth_Enter_1(object sender, EventArgs e)
        {
            if (txtMonth.Text == "MM")
            {
                txtMonth.Text = "";
                txtMonth.ForeColor = Color.Black;
            }
        }

        private void txtYear_Enter(object sender, EventArgs e)
        {
            if (txtYear.Text == "YY")
            {
                txtYear.Text = "";
                txtYear.ForeColor = Color.Black;
            }
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {
            if (txtYear.Text == "")
            {
                txtYear.Text = "YY";
                txtYear.ForeColor = Color.Gray;
            }
        }

        private void txtCVC_Enter(object sender, EventArgs e)
        {
            if (txtCVC.Text == "CVC")
            {
                txtCVC.Text = "";
                txtCVC.ForeColor = Color.Black;
            }
        }

        private void txtCVC_Leave(object sender, EventArgs e)
        {
            if (txtCVC.Text == "")
            {
                txtCVC.Text = "CVC";
                txtCVC.ForeColor = Color.Gray;
            }
        }

        private void txtFullName_Enter(object sender, EventArgs e)
        {
            if (txtFullName.Text == "Full name")
            {
                txtFullName.Text = "";
                txtFullName.ForeColor = Color.Black;
            }
        }

        private void txtFullName_Leave(object sender, EventArgs e)
        {
            if (txtFullName.Text == "")
            {
                txtFullName.Text = "Full name";
                txtFullName.ForeColor = Color.Gray;
            }
        }

        private void txtEmailAddress_Enter(object sender, EventArgs e)
        {
            if (txtEmailAddress.Text == "Email address")
            {
                txtEmailAddress.Text = "";
                txtEmailAddress.ForeColor = Color.Black;
            }
        }

        private void txtEmailAddress_Leave(object sender, EventArgs e)
        {
            if (txtEmailAddress.Text == "")
            {
                txtEmailAddress.Text = "Email address";
                txtEmailAddress.ForeColor = Color.Gray;
            }
        }

        void SetPlaceholder(TextBox box, string text)
        {
            box.Tag = text;
            box.Text = text;
            box.ForeColor = Color.Gray;
        }

        // the text the user really typed ("" if the box only shows its placeholder)
        string Real(TextBox box)
        {
            return box.Text == (string)box.Tag ? "" : box.Text.Trim();
        }

        // only allow digits (plus any extra characters given) while typing
        void OnlyDigits(TextBox box, string extra = "")
        {
            box.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && extra.IndexOf(e.KeyChar) < 0)
                    e.Handled = true;
            };
        }

        void SetupValidation()
        {
            _normalBack = txtFullName.BackColor;

            lblNameErr = MakeErrorLabel(txtFullName);
            lblEmailErr = MakeErrorLabel(txtEmailAddress);
            lblCardErr = MakeErrorLabel(txtCardNumber);
            lblExpiryErr = MakeErrorLabel(txtMonth);
            lblCvcErr = MakeErrorLabel(txtCVC);

            Watch(txtFullName, lblNameErr, CheckName);
            Watch(txtEmailAddress, lblEmailErr, CheckEmail);
            Watch(txtCardNumber, lblCardErr, CheckCard);
            Watch(txtCVC, lblCvcErr, CheckCvc);

            // expiry uses two boxes (month and year) that share one message
            EventHandler expiryTyping = (s, e) =>
            {
                if (Real(txtMonth).Length == 0 && Real(txtYear).Length == 0)
                    ShowError(lblExpiryErr, null, txtMonth, txtYear);
                else if (Real(txtYear).Length == 0)
                    ShowError(lblExpiryErr, MonthRx.IsMatch(Real(txtMonth)) ? null : "Month must be 1 to 12.", txtMonth);
                else
                    ShowError(lblExpiryErr, CheckExpiry(), txtMonth, txtYear);
            }; txtMonth.TextChanged += expiryTyping;
            txtYear.TextChanged += expiryTyping;
            txtYear.Leave += (s, e) => ShowError(lblExpiryErr, CheckExpiry(), txtMonth, txtYear);

            OnlyDigits(txtMonth);
            OnlyDigits(txtYear);
            OnlyDigits(txtCVC);
            OnlyDigits(txtCardNumber, " -");

            txtCardNumber.MaxLength = 23;
            txtMonth.MaxLength = 2;
            txtYear.MaxLength = 4;
            txtCVC.MaxLength = 4;
        }

        // checks everything, shows every red message, and returns true only if all are fine
        bool ValidateAll()
        {
            string n = CheckName();
            string m = CheckEmail();
            string c = CheckCard();
            string x = CheckExpiry();
            string v = CheckCvc();

            ShowError(lblNameErr, n, txtFullName);
            ShowError(lblEmailErr, m, txtEmailAddress);
            ShowError(lblCardErr, c, txtCardNumber);
            ShowError(lblExpiryErr, x, txtMonth, txtYear);
            ShowError(lblCvcErr, v, txtCVC);

            return n == null && m == null && c == null && x == null && v == null;
        }

        static bool PassesLuhn(string digits)
        {
            if (digits.Length < 13 || digits.Length > 19) return false;
            int sum = 0; bool alt = false;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(digits[i])) return false;
                int d = digits[i] - '0';
                if (alt) { d *= 2; if (d > 9) d -= 9; }
                sum += d; alt = !alt;
            }
            return sum % 10 == 0;
        }


    }
}
