namespace Tourist
{
    partial class uc_browse_event
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
            this.flpEvents = new System.Windows.Forms.FlowLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbOther = new System.Windows.Forms.ComboBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flpEvents
            // 
            this.flpEvents.AutoScroll = true;
            this.flpEvents.BackColor = System.Drawing.SystemColors.Control;
            this.flpEvents.Location = new System.Drawing.Point(0, 90);
            this.flpEvents.Name = "flpEvents";
            this.flpEvents.Size = new System.Drawing.Size(794, 561);
            this.flpEvents.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Location = new System.Drawing.Point(15, 50);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 24);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // cmbOther
            // 
            this.cmbOther.FormattingEnabled = true;
            this.cmbOther.Location = new System.Drawing.Point(455, 49);
            this.cmbOther.Name = "cmbOther";
            this.cmbOther.Size = new System.Drawing.Size(121, 24);
            this.cmbOther.TabIndex = 0;
            this.cmbOther.SelectedIndexChanged += new System.EventHandler(this.cmbOther_SelectedIndexChanged);
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(304, 49);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(121, 24);
            this.cmbCategory.TabIndex = 1;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 41);
            this.label3.TabIndex = 3;
            this.label3.Text = "Explore Tours";
            // 
            // uc_browse_event
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbOther);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.flpEvents);
            this.Name = "uc_browse_event";
            this.Size = new System.Drawing.Size(728, 588);
            this.Load += new System.EventHandler(this.uc_browse_event_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpEvents;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbOther;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label3;
    }
}
