namespace Tourist
{
    partial class MyBookings
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
            this.dgvBookings = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBookings
            // 
            this.dgvBookings.BackgroundColor = System.Drawing.Color.White;
            this.dgvBookings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookings.Location = new System.Drawing.Point(30, 81);
            this.dgvBookings.Name = "dgvBookings";
            this.dgvBookings.RowHeadersWidth = 51;
            this.dgvBookings.RowTemplate.Height = 24;
            this.dgvBookings.Size = new System.Drawing.Size(681, 402);
            this.dgvBookings.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "My Bookings";
            // 
            // MyBookings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvBookings);
            this.Name = "MyBookings";
            this.Size = new System.Drawing.Size(749, 610);
            this.Load += new System.EventHandler(this.MyBookings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBookings;
        private System.Windows.Forms.Label label1;
    }
}
