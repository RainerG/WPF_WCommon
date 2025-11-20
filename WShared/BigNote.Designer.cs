namespace NS_Utilities
{
    partial class BigNote
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
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BigNote));
            this.userRichTB = new NS_Utilities.UserRichTextBox();
            this.SuspendLayout();
            // 
            // userRichTB
            // 
            this.userRichTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userRichTB.BackColor = System.Drawing.SystemColors.Window;
            this.userRichTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userRichTB.Location = new System.Drawing.Point(13, 13);
            this.userRichTB.Name = "userRichTB";
            this.userRichTB.ReadOnly = true;
            this.userRichTB.Size = new System.Drawing.Size(829, 253);
            this.userRichTB.TabIndex = 0;
            this.userRichTB.Text = "";
            // 
            // BigNote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 278);
            this.Controls.Add(this.userRichTB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BigNote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warning";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BigNote_FormClosing);
            this.Load += new System.EventHandler(this.BigNote_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private NS_Utilities.UserRichTextBox userRichTB;
    }
}