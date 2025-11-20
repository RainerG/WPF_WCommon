namespace NS_WordExcel
{
    partial class WordExcelExport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WordExcelExport));
            this.buttonAbort = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.usrPrgrssBar = new NS_ProgrssBar.UserProgressBar();
            this.SuspendLayout();
            // 
            // buttonAbort
            // 
            this.buttonAbort.Location = new System.Drawing.Point(266, 9);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(45, 23);
            this.buttonAbort.TabIndex = 3;
            this.buttonAbort.Text = "&Abort";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // textBox
            // 
            this.textBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.textBox.Location = new System.Drawing.Point(13, 11);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(238, 20);
            this.textBox.TabIndex = 4;
            this.textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // usrPrgrssBar
            // 
            this.usrPrgrssBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usrPrgrssBar.Location = new System.Drawing.Point(13, 43);
            this.usrPrgrssBar.Name = "usrPrgrssBar";
            this.usrPrgrssBar.Size = new System.Drawing.Size(297, 21);
            this.usrPrgrssBar.TabIndex = 5;
            // 
            // WordExcelExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 76);
            this.Controls.Add(this.usrPrgrssBar);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.buttonAbort);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WordExcelExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Be patient !";
            this.TopMost = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WordExcelExport_FormClosing);
            this.Load += new System.EventHandler(this.WordExcelExport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.TextBox textBox;
        private NS_ProgrssBar.UserProgressBar usrPrgrssBar;
    }
}