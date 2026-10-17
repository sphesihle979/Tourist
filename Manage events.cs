using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist
{
    public partial class Manage_events : UserControl
    {
        public event EventHandler Finished;
        public event EventHandler BackToDashboard;

        public int EventId { get; private set; }      // 0 = new, otherwise editing

        private string imageFileName = "";            // saved in the database (name only)
        private string pickedFullPath = ""; // a newly chosen photo, copied on Save
        private string originalDate = "";      // <-- add
        private bool fixingPrice;

        private static string ImagesFolder
        {
            get { return Db.ImagesFolder; }
        }

        public Manage_events()
        {
            InitializeComponent();
            DoubleBuffered = true;
            picEvent.SizeMode = PictureBoxSizeMode.Zoom;

            if (cmbCategory.Items.Count == 0)
                cmbCategory.Items.AddRange(new object[] { "Adventure", "Culture", "Wildlife", "Food & Wine", "Beach", "Music" });
            if (cmbStatus.Items.Count == 0)
                cmbStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            if (cmbStatus.SelectedIndex < 0) cmbStatus.SelectedIndex = 0;

            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            txtEventName.MaxLength = 60;
            txtVenue.MaxLength = 60;
            txtDescription.MaxLength = 500;
            txtDate.MaxLength = 8;
            txtStartTime.MaxLength = 5;
            txtRow.MaxLength = 3;
            txtSeatperRow.MaxLength = 3;
            txtRating.MaxLength = 3;
            txtPrice.MaxLength = 11;

            // stop pasting into the restricted boxes
            foreach (var t in new[] { txtDate, txtStartTime, txtRating, txtPrice, txtRow, txtSeatperRow })
                t.ShortcutsEnabled = false;

            txtDate.KeyPress += (s, e) => MaskedKey(txtDate, e, '/', new[] { 2, 5 }, 8);
            txtStartTime.KeyPress += (s, e) => MaskedKey(txtStartTime, e, ':', new[] { 2 }, 5);
            txtRating.KeyPress += txtRating_KeyPress;
            txtPrice.KeyPress += txtPrice_KeyPress;
            txtPrice.TextChanged += txtPrice_TextChanged;
            txtRow.KeyPress += DigitsOnly_KeyPress;
            txtSeatperRow.KeyPress += DigitsOnly_KeyPress;

            txtRow.TextChanged += (s, e) => UpdateSeatCount();
            txtSeatperRow.TextChanged += (s, e) => UpdateSeatCount();
            UpdateSeatCount();
        }

        private void UpdateSeatCount()
        {
            
            int row, seats;
            int.TryParse(txtRow.Text, out row);
            int.TryParse(txtSeatperRow.Text, out seats);
            lblNumberofSeats.Text = (row * seats + " seats created").ToString();

            txtEventName.TextChanged += (s, e) => HideError(lblNameValidation);
            cmbCategory.SelectedIndexChanged += (s, e) => HideError(lblCategory);
            txtRating.TextChanged += (s, e) => HideError(lblRating);
            txtPrice.TextChanged += (s, e) => HideError(lblPrice);
            txtDate.TextChanged += (s, e) => { HideError(lblDate); HideError(lblStartTime); };
            txtStartTime.TextChanged += (s, e) => HideError(lblStartTime);
            txtVenue.TextChanged += (s, e) => HideError(lblVenue);
            txtRow.TextChanged += (s, e) => HideError(lblRows);
            txtSeatperRow.TextChanged += (s, e) => HideError(lblSeatperRow);
            txtDescription.TextChanged += (s, e) => HideError(lblDescription);
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            BackToDashboard?.Invoke(this, EventArgs.Empty);
        }

        // ---------- typing checks ----------

        private static bool AtEnd(TextBox t)
        {
            if (t.SelectionStart + t.SelectionLength != t.TextLength) return false;
            if (t.SelectionLength > 0) t.Text = t.Text.Substring(0, t.SelectionStart);   // typing over a selection
            t.SelectionStart = t.TextLength;
            return true;
        }

        // digits only, and inserts the separator (/ or :) by itself
        private static void MaskedKey(TextBox t, KeyPressEventArgs e, char sep, int[] sepAt, int maxLen)
        {
            if (char.IsControl(e.KeyChar)) return;           // backspace etc.
            e.Handled = true;                                // blocked unless accepted below

            bool digit = e.KeyChar >= '0' && e.KeyChar <= '9';
            if (!digit && e.KeyChar != sep) return;
            if (!AtEnd(t)) return;
            if (t.TextLength >= maxLen) return;

            if (e.KeyChar == sep)
            {
                if (Array.IndexOf(sepAt, t.TextLength) < 0) return;
                t.AppendText(sep.ToString());
            }
            else
            {
                if (Array.IndexOf(sepAt, t.TextLength) >= 0) t.AppendText(sep.ToString());
                t.AppendText(e.KeyChar.ToString());
            }
        }

        private static string Prospective(TextBox t, char c)
        {
            return t.Text.Remove(t.SelectionStart, t.SelectionLength).Insert(t.SelectionStart, c.ToString());
        }

        private void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !(e.KeyChar >= '0' && e.KeyChar <= '9');
        }

        // 0 to 5 only, one decimal: 4.5 is fine, 5.5 and 6 are blocked
        private void txtRating_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (e.KeyChar == ',') e.KeyChar = '.';
            e.Handled = !Regex.IsMatch(Prospective(txtRating, e.KeyChar), @"^(5(\.0?)?|[0-4](\.\d?)?)$");
        }

        // digits and one decimal point, 2 decimals at most
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (e.KeyChar == ',') e.KeyChar = '.';
            e.Handled = !Regex.IsMatch(Prospective(txtPrice, e.KeyChar), @"^R?\d{1,7}(\.\d{0,2})?$");
        }

        // puts the R in front automatically
        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (fixingPrice) return;
            fixingPrice = true;

            string t = txtPrice.Text;
            if (t == "R") txtPrice.Text = "";
            else if (t != "" && !t.StartsWith("R"))
            {
                txtPrice.Text = "R" + t;
                txtPrice.SelectionStart = txtPrice.TextLength;
            }
            fixingPrice = false;
        }

        // checks on Save 

       
        private void ClearErrors()
        {
            foreach (var l in new[] { lblNameValidation, lblCategory, lblRating, lblPrice, lblDate,
                              lblStartTime, lblVenue, lblRows, lblSeatperRow ,lblEventPhoto })
            {
                l.Text = "";
                l.Visible = false;
            }
        }

        private static void Flag(Label l, string message, Control field, ref Control firstBad)
        {
            l.Text = message;
            l.Visible = true;
            if (firstBad == null) firstBad = field;
        }
        private static bool TryParseDate(string text, out DateTime date)
        {
            date = DateTime.MinValue;
            var m = Regex.Match(text, @"^(\d{2})/(\d{2})/(\d{2})$");
            if (!m.Success) return false;
            try
            {
                date = new DateTime(2000 + int.Parse(m.Groups[3].Value),
                                    int.Parse(m.Groups[2].Value),
                                    int.Parse(m.Groups[1].Value));
                return true;
            }
            catch (ArgumentOutOfRangeException) { return false; }      // e.g. 31/02/26
        }

        private static bool TryParseTime(string text, out TimeSpan time)
        {
            time = TimeSpan.Zero;
            var m = Regex.Match(text, @"^(\d{2}):(\d{2})$");
            if (!m.Success) return false;
            int h = int.Parse(m.Groups[1].Value), min = int.Parse(m.Groups[2].Value);
            if (h > 23 || min > 59) return false;
            time = new TimeSpan(h, min, 0);
            return true;
        }
        private static void HideError(Label l)
        {
            l.Text = "";
            l.Visible = false;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            ClearErrors();
            Control firstBad = null;

            string name = txtEventName.Text.Trim();
            if (name.Length == 0)
                Flag(lblNameValidation, "Please enter the event name.", txtEventName, ref firstBad);
            else if (name.Length < 3)
                Flag(lblNameValidation, "The event name must be at least 3 characters.", txtEventName, ref firstBad);

            if (cmbCategory.SelectedIndex < 0)
                Flag(lblCategory, "Please choose a category.", cmbCategory, ref firstBad);

            double rating;
            if (!double.TryParse(txtRating.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rating)
                || rating < 0 || rating > 5)
                Flag(lblRating, "Please enter a rating from 0 to 5.", txtRating, ref firstBad);

            decimal price;
            if (!decimal.TryParse(txtPrice.Text.Replace("R", "").Trim(), NumberStyles.AllowDecimalPoint,
                                  CultureInfo.InvariantCulture, out price))
                Flag(lblPrice, "Please enter the price.", txtPrice, ref firstBad);

            DateTime date;
            bool dateOk = TryParseDate(txtDate.Text, out date);
            bool dateUnchanged = false;
            if (!dateOk)
            {
                Flag(lblDate, "Use day/month/year, for example 22/10/26.", txtDate, ref firstBad);
            }
            else
            {
                dateUnchanged = EventId != 0 &&
                                date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) == originalDate;
                if (date.Date < DateTime.Today && !dateUnchanged)
                    Flag(lblDate, "That date has already passed.", txtDate, ref firstBad);
            }

            TimeSpan time;
            if (!TryParseTime(txtStartTime.Text, out time))
            {
                Flag(lblStartTime, "Use 24-hour time, for example 12:00.", txtStartTime, ref firstBad);
            }
            else if (dateOk && date.Date == DateTime.Today && date.Date.Add(time) <= DateTime.Now && !dateUnchanged)
            {
                Flag(lblStartTime, "That start time has already passed today.", txtStartTime, ref firstBad);
            }

            if (txtVenue.Text.Trim().Length < 2)
                Flag(lblVenue, "Please enter the venue.", txtVenue, ref firstBad);

            int rows, seatsPerRow;
            if (!int.TryParse(txtRow.Text, out rows) || rows < 1 || rows > 100)
                Flag(lblRows, "Rows must be a whole number from 1 to 100.", txtRow, ref firstBad);

            if (!int.TryParse(txtSeatperRow.Text, out seatsPerRow) || seatsPerRow < 1 || seatsPerRow > 100)
                Flag(lblSeatperRow, "Seats per row must be a whole number from 1 to 100.", txtSeatperRow, ref firstBad);

            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                Flag(lblDescription, "Please fill in description.", txtDescription, ref firstBad);

            }

            bool hasPhoto = pickedFullPath != "" || imageFileName != "";   // new pick, or one already saved
            if (!hasPhoto)
                Flag(lblEventPhoto, "Please choose a photo for the event.", btnChoose, ref firstBad);
            else if (pickedFullPath != "" && !File.Exists(pickedFullPath))
                Flag(lblEventPhoto, "That photo can no longer be found. Please choose it again.", btnChoose, ref firstBad);

           
            if (firstBad != null)
            {
                firstBad.Focus();
                return;
            }
            // ---- photo: copy the chosen file into the Images folder ----
            if (pickedFullPath != "")
            {
                Directory.CreateDirectory(ImagesFolder);
                string newName = Guid.NewGuid().ToString("N") + Path.GetExtension(pickedFullPath);
                File.Copy(pickedFullPath, Path.Combine(ImagesFolder, newName));
                imageFileName = newName;
            }

            // ---- insert or update ----
            string sql = EventId == 0
                ? @"INSERT INTO Events (EventName, Category, Rating, EventDate, StartTime, Status, Venue, Price,
                                    Description, ImagePath, SeatRows, SeatsPerRow)
                VALUES (@name, @cat, @rating, @date, @time, @status, @venue, @price,
                        @desc, @img, @rows, @spr)"
                : @"UPDATE Events SET EventName=@name, Category=@cat, Rating=@rating, EventDate=@date,
                                  StartTime=@time, Status=@status, Venue=@venue, Price=@price,
                                  Description=@desc, ImagePath=@img, SeatRows=@rows, SeatsPerRow=@spr
                WHERE EventId=@id";

            using (var con = new SQLiteConnection(Db.ConnectionString))
            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@cat", cmbCategory.Text);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@time", txtStartTime.Text.Trim());
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@venue", txtVenue.Text.Trim());
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@desc", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@img", imageFileName);
                cmd.Parameters.AddWithValue("@rows", rows);
                cmd.Parameters.AddWithValue("@spr", seatsPerRow);
                if (EventId != 0) cmd.Parameters.AddWithValue("@id", EventId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.RestoreDirectory = true;
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (ofd.ShowDialog() != DialogResult.OK) return;

                if (new FileInfo(ofd.FileName).Length > 5 * 1024 * 1024)
                {
                    lblEventPhoto.Text = "The photo must be smaller than 5 MB.";
                    lblEventPhoto.Visible = true;
                    return;
                }

                pickedFullPath = ofd.FileName;
                ShowPicture(pickedFullPath);
                lblChoose.Text = Path.GetFileName(pickedFullPath);
                HideError(lblEventPhoto);
            }
        }

        private static string S(object o)
        {
            return (o == null || o == DBNull.Value) ? "" : Convert.ToString(o, CultureInfo.InvariantCulture);
        }

        public void LoadEvent(int id)
        {
            EventId = id;

            using (var con = new SQLiteConnection(Db.ConnectionString))
            using (var cmd = new SQLiteCommand("SELECT * FROM Events WHERE EventId = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;

                    txtEventName.Text = S(r["EventName"]);
                    cmbCategory.SelectedIndex = cmbCategory.FindStringExact(S(r["Category"]));
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(S(r["Status"]));
                    txtRating.Text = S(r["Rating"]);
                    txtStartTime.Text = S(r["StartTime"]);
                    txtVenue.Text = S(r["Venue"]);
                    txtDescription.Text = S(r["Description"]);
                    txtRow.Text = S(r["SeatRows"]);
                    txtSeatperRow.Text = S(r["SeatsPerRow"]);

                    string p = S(r["Price"]);
                    txtPrice.Text = p == "" ? "" : "R" + p;

                    DateTime d;
                    if (DateTime.TryParseExact(S(r["EventDate"]), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out d))
                    {
                        txtDate.Text = d.ToString("dd/MM/yy", CultureInfo.InvariantCulture);
                        originalDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    imageFileName = S(r["ImagePath"]);
                }
            }

            if (imageFileName != "")
            {
                string path = Path.Combine(ImagesFolder, imageFileName);
                if (File.Exists(path))
                {
                    ShowPicture(path);
                    lblChoose.Text = imageFileName;
                }
            }
        }
        private void ShowPicture(string path)
        {
            try
            {
                var old = picEvent.Image;
                picEvent.Image = null;
                if (old != null) old.Dispose();

                using (var img = Image.FromFile(path))
                {
                    double scale = Math.Min(1.0, 800.0 / Math.Max(img.Width, img.Height));
                    int w = Math.Max(1, (int)(img.Width * scale));
                    int h = Math.Max(1, (int)(img.Height * scale));
                    picEvent.Image = new Bitmap(img, w, h);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("That file could not be opened as a picture.");
            }
        }

        private void txtEventName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEventName.Text))
            {
                lblNameValidation.Visible = false;
                return;

            }
            if (!Regex.IsMatch(txtEventName.Text, @"^[\p{L}\d]{3,}(?:['-][\p{L}\d]+)*[\s]+[\p{L}\d]{3,}.*$"))
            {
                lblNameValidation.Text = "The event name must be at least 3 characters.";
                lblNameValidation.Visible = true;
                return;
            }
            else
            {
                lblNameValidation.Visible = false;

            }
        }

       
    }
}
