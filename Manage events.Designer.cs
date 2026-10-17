namespace Tourist
{
    partial class Manage_events
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlEvent = new System.Windows.Forms.Panel();
            this.lblNameValidation = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblNumberofSeats = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSeatperRow = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblChoose = new System.Windows.Forms.Label();
            this.btnChoose = new System.Windows.Forms.Button();
            this.picEvent = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtVenue = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtRating = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEventName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblVenue = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblSeatperRow = new System.Windows.Forms.Label();
            this.lblRows = new System.Windows.Forms.Label();
            this.lblEventPhoto = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.pnlEvent.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEvent)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(23, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(739, 146);
            this.panel2.TabIndex = 3;
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(559, 54);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(136, 36);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Back to events";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(30, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(458, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Active events appear on visitor dashboard as soon as you save them";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(25, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 50);
            this.label3.TabIndex = 2;
            this.label3.Text = "Add new event";
            // 
            // pnlEvent
            // 
            this.pnlEvent.BackColor = System.Drawing.Color.White;
            this.pnlEvent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEvent.Controls.Add(this.lblEventPhoto);
            this.pnlEvent.Controls.Add(this.lblRows);
            this.pnlEvent.Controls.Add(this.lblSeatperRow);
            this.pnlEvent.Controls.Add(this.lblStatus);
            this.pnlEvent.Controls.Add(this.lblDescription);
            this.pnlEvent.Controls.Add(this.lblVenue);
            this.pnlEvent.Controls.Add(this.lblPrice);
            this.pnlEvent.Controls.Add(this.lblRating);
            this.pnlEvent.Controls.Add(this.lblStartTime);
            this.pnlEvent.Controls.Add(this.lblDate);
            this.pnlEvent.Controls.Add(this.lblCategory);
            this.pnlEvent.Controls.Add(this.lblNameValidation);
            this.pnlEvent.Controls.Add(this.cmbStatus);
            this.pnlEvent.Controls.Add(this.btnCancel);
            this.pnlEvent.Controls.Add(this.btnSave);
            this.pnlEvent.Controls.Add(this.panel1);
            this.pnlEvent.Controls.Add(this.label18);
            this.pnlEvent.Controls.Add(this.label6);
            this.pnlEvent.Controls.Add(this.txtRow);
            this.pnlEvent.Controls.Add(this.label7);
            this.pnlEvent.Controls.Add(this.txtSeatperRow);
            this.pnlEvent.Controls.Add(this.label17);
            this.pnlEvent.Controls.Add(this.label5);
            this.pnlEvent.Controls.Add(this.lblChoose);
            this.pnlEvent.Controls.Add(this.btnChoose);
            this.pnlEvent.Controls.Add(this.picEvent);
            this.pnlEvent.Controls.Add(this.label1);
            this.pnlEvent.Controls.Add(this.txtDescription);
            this.pnlEvent.Controls.Add(this.label15);
            this.pnlEvent.Controls.Add(this.txtVenue);
            this.pnlEvent.Controls.Add(this.label16);
            this.pnlEvent.Controls.Add(this.txtDate);
            this.pnlEvent.Controls.Add(this.label13);
            this.pnlEvent.Controls.Add(this.txtStartTime);
            this.pnlEvent.Controls.Add(this.label14);
            this.pnlEvent.Controls.Add(this.cmbCategory);
            this.pnlEvent.Controls.Add(this.label12);
            this.pnlEvent.Controls.Add(this.txtRating);
            this.pnlEvent.Controls.Add(this.label11);
            this.pnlEvent.Controls.Add(this.txtPrice);
            this.pnlEvent.Controls.Add(this.label8);
            this.pnlEvent.Controls.Add(this.txtEventName);
            this.pnlEvent.Controls.Add(this.label9);
            this.pnlEvent.Controls.Add(this.label10);
            this.pnlEvent.Location = new System.Drawing.Point(23, 196);
            this.pnlEvent.Name = "pnlEvent";
            this.pnlEvent.Size = new System.Drawing.Size(739, 1030);
            this.pnlEvent.TabIndex = 6;
            // 
            // lblNameValidation
            // 
            this.lblNameValidation.AutoSize = true;
            this.lblNameValidation.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameValidation.ForeColor = System.Drawing.Color.Red;
            this.lblNameValidation.Location = new System.Drawing.Point(32, 130);
            this.lblNameValidation.Name = "lblNameValidation";
            this.lblNameValidation.Size = new System.Drawing.Size(0, 17);
            this.lblNameValidation.TabIndex = 33;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(513, 825);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(181, 24);
            this.cmbStatus.TabIndex = 32;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.Location = new System.Drawing.Point(187, 952);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(137, 51);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DarkSlateGray;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(33, 952);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(137, 51);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "Save event";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(220)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblNumberofSeats);
            this.panel1.Location = new System.Drawing.Point(33, 885);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 46);
            this.panel1.TabIndex = 29;
            // 
            // lblNumberofSeats
            // 
            this.lblNumberofSeats.AutoSize = true;
            this.lblNumberofSeats.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberofSeats.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblNumberofSeats.Location = new System.Drawing.Point(12, 12);
            this.lblNumberofSeats.Name = "lblNumberofSeats";
            this.lblNumberofSeats.Size = new System.Drawing.Size(151, 17);
            this.lblNumberofSeats.TabIndex = 30;
            this.lblNumberofSeats.Text = "32 seats will be created";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label18.Location = new System.Drawing.Point(31, 747);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(162, 23);
            this.label18.TabIndex = 28;
            this.label18.Text = "Seats and Visibility";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label6.Location = new System.Drawing.Point(510, 790);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 17);
            this.label6.TabIndex = 26;
            this.label6.Text = "Status";
            // 
            // txtRow
            // 
            this.txtRow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRow.Location = new System.Drawing.Point(33, 810);
            this.txtRow.Multiline = true;
            this.txtRow.Name = "txtRow";
            this.txtRow.Size = new System.Drawing.Size(181, 39);
            this.txtRow.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label7.Location = new System.Drawing.Point(31, 791);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "Rows";
            // 
            // txtSeatperRow
            // 
            this.txtSeatperRow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSeatperRow.Location = new System.Drawing.Point(276, 810);
            this.txtSeatperRow.Multiline = true;
            this.txtSeatperRow.Name = "txtSeatperRow";
            this.txtSeatperRow.Size = new System.Drawing.Size(181, 39);
            this.txtSeatperRow.TabIndex = 23;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label17.Location = new System.Drawing.Point(273, 791);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(85, 17);
            this.label17.TabIndex = 22;
            this.label17.Text = "Seat per row";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label5.Location = new System.Drawing.Point(273, 592);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(187, 17);
            this.label5.TabIndex = 21;
            this.label5.Text = "JPG, PNG or WebP, up to 5MB.";
            // 
            // lblChoose
            // 
            this.lblChoose.AutoSize = true;
            this.lblChoose.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChoose.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblChoose.Location = new System.Drawing.Point(372, 567);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(92, 17);
            this.lblChoose.TabIndex = 20;
            this.lblChoose.Text = "No file chosen";
            // 
            // btnChoose
            // 
            this.btnChoose.BackColor = System.Drawing.Color.Silver;
            this.btnChoose.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChoose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnChoose.Location = new System.Drawing.Point(270, 561);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(96, 28);
            this.btnChoose.TabIndex = 19;
            this.btnChoose.Text = "Choose file";
            this.btnChoose.UseVisualStyleBackColor = false;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // picEvent
            // 
            this.picEvent.BackColor = System.Drawing.Color.Silver;
            this.picEvent.Location = new System.Drawing.Point(34, 561);
            this.picEvent.Name = "picEvent";
            this.picEvent.Size = new System.Drawing.Size(210, 159);
            this.picEvent.TabIndex = 18;
            this.picEvent.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label1.Location = new System.Drawing.Point(31, 530);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Event photo";
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Location = new System.Drawing.Point(33, 333);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(662, 175);
            this.txtDescription.TabIndex = 16;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label15.Location = new System.Drawing.Point(30, 313);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(79, 17);
            this.label15.TabIndex = 15;
            this.label15.Text = "Description";
            // 
            // txtVenue
            // 
            this.txtVenue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVenue.Location = new System.Drawing.Point(513, 253);
            this.txtVenue.Multiline = true;
            this.txtVenue.Name = "txtVenue";
            this.txtVenue.Size = new System.Drawing.Size(181, 39);
            this.txtVenue.TabIndex = 14;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label16.Location = new System.Drawing.Point(510, 232);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(46, 17);
            this.label16.TabIndex = 13;
            this.label16.Text = "Venue";
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.Location = new System.Drawing.Point(33, 252);
            this.txtDate.Multiline = true;
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(181, 39);
            this.txtDate.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label13.Location = new System.Drawing.Point(31, 233);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 17);
            this.label13.TabIndex = 11;
            this.label13.Text = "Date";
            // 
            // txtStartTime
            // 
            this.txtStartTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartTime.Location = new System.Drawing.Point(276, 252);
            this.txtStartTime.Multiline = true;
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(181, 39);
            this.txtStartTime.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label14.Location = new System.Drawing.Point(273, 233);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 17);
            this.label14.TabIndex = 9;
            this.label14.Text = "Start Time";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(33, 182);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(181, 24);
            this.cmbCategory.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label12.Location = new System.Drawing.Point(31, 147);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 17);
            this.label12.TabIndex = 7;
            this.label12.Text = "Category";
            // 
            // txtRating
            // 
            this.txtRating.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRating.Location = new System.Drawing.Point(276, 167);
            this.txtRating.Multiline = true;
            this.txtRating.Name = "txtRating";
            this.txtRating.Size = new System.Drawing.Size(181, 39);
            this.txtRating.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label11.Location = new System.Drawing.Point(273, 147);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 17);
            this.label11.TabIndex = 5;
            this.label11.Text = "Rating(0 to 5)";
            // 
            // txtPrice
            // 
            this.txtPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrice.Location = new System.Drawing.Point(513, 167);
            this.txtPrice.Multiline = true;
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(181, 39);
            this.txtPrice.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label8.Location = new System.Drawing.Point(510, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "Price per seat(ZAR)";
            // 
            // txtEventName
            // 
            this.txtEventName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEventName.Location = new System.Drawing.Point(33, 88);
            this.txtEventName.Multiline = true;
            this.txtEventName.Name = "txtEventName";
            this.txtEventName.Size = new System.Drawing.Size(661, 39);
            this.txtEventName.TabIndex = 2;
            this.txtEventName.TextChanged += new System.EventHandler(this.txtEventName_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label9.Location = new System.Drawing.Point(30, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "Event name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label10.Location = new System.Drawing.Point(30, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 23);
            this.label10.TabIndex = 0;
            this.label10.Text = "Event details";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(768, 4);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(24, 1244);
            this.vScrollBar1.TabIndex = 7;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategory.ForeColor = System.Drawing.Color.Red;
            this.lblCategory.Location = new System.Drawing.Point(36, 209);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(0, 17);
            this.lblCategory.TabIndex = 34;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.Red;
            this.lblDate.Location = new System.Drawing.Point(32, 294);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 17);
            this.lblDate.TabIndex = 35;
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartTime.ForeColor = System.Drawing.Color.Red;
            this.lblStartTime.Location = new System.Drawing.Point(273, 294);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(0, 17);
            this.lblStartTime.TabIndex = 36;
            // 
            // lblRating
            // 
            this.lblRating.AutoSize = true;
            this.lblRating.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRating.ForeColor = System.Drawing.Color.Red;
            this.lblRating.Location = new System.Drawing.Point(273, 209);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(0, 17);
            this.lblRating.TabIndex = 37;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.Color.Red;
            this.lblPrice.Location = new System.Drawing.Point(510, 209);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(0, 17);
            this.lblPrice.TabIndex = 38;
            // 
            // lblVenue
            // 
            this.lblVenue.AutoSize = true;
            this.lblVenue.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVenue.ForeColor = System.Drawing.Color.Red;
            this.lblVenue.Location = new System.Drawing.Point(510, 294);
            this.lblVenue.Name = "lblVenue";
            this.lblVenue.Size = new System.Drawing.Size(0, 17);
            this.lblVenue.TabIndex = 39;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.Red;
            this.lblDescription.Location = new System.Drawing.Point(30, 511);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(0, 17);
            this.lblDescription.TabIndex = 40;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(510, 852);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            this.lblStatus.TabIndex = 41;
            // 
            // lblSeatperRow
            // 
            this.lblSeatperRow.AutoSize = true;
            this.lblSeatperRow.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeatperRow.ForeColor = System.Drawing.Color.Red;
            this.lblSeatperRow.Location = new System.Drawing.Point(273, 852);
            this.lblSeatperRow.Name = "lblSeatperRow";
            this.lblSeatperRow.Size = new System.Drawing.Size(0, 17);
            this.lblSeatperRow.TabIndex = 42;
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRows.ForeColor = System.Drawing.Color.Red;
            this.lblRows.Location = new System.Drawing.Point(31, 852);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(0, 17);
            this.lblRows.TabIndex = 43;
            // 
            // lblEventPhoto
            // 
            this.lblEventPhoto.AutoSize = true;
            this.lblEventPhoto.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventPhoto.ForeColor = System.Drawing.Color.Red;
            this.lblEventPhoto.Location = new System.Drawing.Point(31, 723);
            this.lblEventPhoto.Name = "lblEventPhoto";
            this.lblEventPhoto.Size = new System.Drawing.Size(0, 17);
            this.lblEventPhoto.TabIndex = 44;
            // 
            // Manage_events
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.pnlEvent);
            this.Controls.Add(this.panel2);
            this.Name = "Manage_events";
            this.Size = new System.Drawing.Size(645, 1082);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlEvent.ResumeLayout(false);
            this.pnlEvent.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEvent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlEvent;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtVenue;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtRating;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEventName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.PictureBox picEvent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRow;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSeatperRow;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblChoose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNumberofSeats;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Label lblNameValidation;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblVenue;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblEventPhoto;
        private System.Windows.Forms.Label lblRows;
        private System.Windows.Forms.Label lblSeatperRow;
        private System.Windows.Forms.Label lblStatus;
    }
}
