namespace NS_UserCombo
{
    partial class UserFileSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserFileSelect));
            this.btnUFSBrowse = new System.Windows.Forms.Button();
            this.btnUFSCreateEdit = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.fileComboUFS = new NS_UserCombo.FileComboBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUFSBrowse
            // 
            resources.ApplyResources(this.btnUFSBrowse, "btnUFSBrowse");
            this.btnUFSBrowse.Name = "btnUFSBrowse";
            this.btnUFSBrowse.UseVisualStyleBackColor = true;
            this.btnUFSBrowse.Click += new System.EventHandler(this.btnUFSBrowse_Click);
            // 
            // btnUFSCreateEdit
            // 
            resources.ApplyResources(this.btnUFSCreateEdit, "btnUFSCreateEdit");
            this.btnUFSCreateEdit.Name = "btnUFSCreateEdit";
            this.btnUFSCreateEdit.UseVisualStyleBackColor = true;
            this.btnUFSCreateEdit.Click += new System.EventHandler(this.btnUFSCreateEdit_Click);
            // 
            // groupBox
            // 
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Controls.Add(this.btnUFSCreateEdit);
            this.groupBox.Controls.Add(this.fileComboUFS);
            this.groupBox.Controls.Add(this.btnUFSBrowse);
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // fileComboUFS
            // 
            this.fileComboUFS.AllowDrop = true;
            resources.ApplyResources(this.fileComboUFS, "fileComboUFS");
            this.fileComboUFS.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.fileComboUFS.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.fileComboUFS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fileComboUFS.FormattingEnabled = true;
            this.fileComboUFS.Name = "fileComboUFS";
            this.fileComboUFS.ReadOnly = false;
            this.fileComboUFS.Txt = "";
            // 
            // UserFileSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox);
            this.Name = "UserFileSelect";
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUFSBrowse;
        private System.Windows.Forms.Button btnUFSCreateEdit;
        private NS_UserCombo.FileComboBox fileComboUFS;
        private System.Windows.Forms.GroupBox groupBox;
    }
}
