namespace NS_UserCombo
{
    partial class UserDirSelect
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileComboUDS = new NS_UserCombo.FileComboBox();
            this.btnUDSBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileComboUDS
            // 
            this.fileComboUDS.AllowDrop = true;
            this.fileComboUDS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileComboUDS.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.fileComboUDS.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.fileComboUDS.FormattingEnabled = true;
            this.fileComboUDS.Location = new System.Drawing.Point(4, 4);
            this.fileComboUDS.Name = "fileComboUDS";
            this.fileComboUDS.ReadOnly = false;
            this.fileComboUDS.Size = new System.Drawing.Size(333, 21);
            this.fileComboUDS.TabIndex = 0;
            // 
            // btnUDSBrowse
            // 
            this.btnUDSBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUDSBrowse.Location = new System.Drawing.Point(343, 3);
            this.btnUDSBrowse.Name = "btnUDSBrowse";
            this.btnUDSBrowse.Size = new System.Drawing.Size(38, 23);
            this.btnUDSBrowse.TabIndex = 1;
            this.btnUDSBrowse.Text = "...";
            this.btnUDSBrowse.UseVisualStyleBackColor = true;
            this.btnUDSBrowse.Click += new System.EventHandler(this.btnUDSBrowse_Click);
            // 
            // UserDirSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.btnUDSBrowse);
            this.Controls.Add(this.fileComboUDS);
            this.Name = "UserDirSelect";
            this.Size = new System.Drawing.Size(383, 46);
            this.Leave += new System.EventHandler(this.UserDirSelect_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private NS_UserCombo.FileComboBox fileComboUDS;
        private System.Windows.Forms.Button btnUDSBrowse;
    }
}
