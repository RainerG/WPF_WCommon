namespace NS_UserOut
{
    partial class PreferencesUOList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
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
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesUOList));
            this.btnFactDflt = new System.Windows.Forms.Button();
            this.btnFont = new System.Windows.Forms.Button();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.userColorSel = new NS_UserColor.UserColorSelect();
            this.SuspendLayout();
            // 
            // btnFactDflt
            // 
            this.btnFactDflt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFactDflt.Location = new System.Drawing.Point(3, 3);
            this.btnFactDflt.Name = "btnFactDflt";
            this.btnFactDflt.Size = new System.Drawing.Size(87, 22);
            this.btnFactDflt.TabIndex = 1;
            this.btnFactDflt.Text = "Factory Default";
            this.btnFactDflt.UseVisualStyleBackColor = true;
            this.btnFactDflt.Click += new System.EventHandler(this.btnFactDflt_Click);
            // 
            // btnFont
            // 
            this.btnFont.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnFont.Location = new System.Drawing.Point(3, 29);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(46, 22);
            this.btnFont.TabIndex = 2;
            this.btnFont.Text = "Font...";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // userColorSel
            // 
            this.userColorSel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userColorSel.Location = new System.Drawing.Point(12, 31);
            this.userColorSel.Name = "userColorSel";
            this.userColorSel.Size = new System.Drawing.Size(143, 159);
            this.userColorSel.TabIndex = 0;
            // 
            // PreferencesUOList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 283);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.btnFactDflt);
            this.Controls.Add(this.userColorSel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(333, 326);
            this.MinimumSize = new System.Drawing.Size(333, 326);
            this.Name = "PreferencesUOList";
            this.Text = "Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreferencesUOList_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private NS_UserColor.UserColorSelect userColorSel;
        private System.Windows.Forms.Button btnFactDflt;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.FontDialog fontDialog;
    }
}