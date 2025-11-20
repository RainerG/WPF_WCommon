namespace NS_UserOut
{
    partial class PreferencesUOText
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
            if ( disposing && ( components != null ) )
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesUOText));
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.tabPageColSel = new System.Windows.Forms.TabPage();
            this.btnFont = new System.Windows.Forms.Button();
            this.btnFactDflt = new System.Windows.Forms.Button();
            this.userColSel = new NS_UserColor.UserColorSelect();
            this.tabPageCommon = new System.Windows.Forms.TabPage();
            this.groupBoxMarker = new System.Windows.Forms.GroupBox();
            this.linkLabelRexHlp = new System.Windows.Forms.LinkLabel();
            this.cbMarkLtrs = new System.Windows.Forms.CheckBox();
            this.cbRegEx = new System.Windows.Forms.CheckBox();
            this.btnOutPthBrowse = new System.Windows.Forms.Button();
            this.fileComboOutpath = new NS_UserCombo.FileComboBox();
            this.userComboBoxPrefSepar = new NS_UserCombo.UserComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userTabControl = new NS_UserTabControl.UserTabControl();
            this.tabPageColSel.SuspendLayout();
            this.tabPageCommon.SuspendLayout();
            this.groupBoxMarker.SuspendLayout();
            this.userTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageColSel
            // 
            this.tabPageColSel.BackColor = System.Drawing.Color.LightGray;
            this.tabPageColSel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageColSel.Controls.Add(this.btnFont);
            this.tabPageColSel.Controls.Add(this.btnFactDflt);
            this.tabPageColSel.Controls.Add(this.userColSel);
            this.tabPageColSel.Location = new System.Drawing.Point(4, 22);
            this.tabPageColSel.Name = "tabPageColSel";
            this.tabPageColSel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageColSel.Size = new System.Drawing.Size(393, 282);
            this.tabPageColSel.TabIndex = 2;
            this.tabPageColSel.Text = "Colors / Font";
            // 
            // btnFont
            // 
            this.btnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFont.Location = new System.Drawing.Point(331, 254);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(57, 23);
            this.btnFont.TabIndex = 11;
            this.btnFont.Text = "Font...";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // btnFactDflt
            // 
            this.btnFactDflt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFactDflt.Location = new System.Drawing.Point(298, 1);
            this.btnFactDflt.Name = "btnFactDflt";
            this.btnFactDflt.Size = new System.Drawing.Size(90, 23);
            this.btnFactDflt.TabIndex = 1;
            this.btnFactDflt.Text = "Factory Default";
            this.btnFactDflt.UseVisualStyleBackColor = true;
            this.btnFactDflt.Click += new System.EventHandler(this.btnFactDflt_Click);
            // 
            // userColSel
            // 
            this.userColSel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userColSel.AutoSize = true;
            this.userColSel.BackColor = System.Drawing.Color.Transparent;
            this.userColSel.Location = new System.Drawing.Point(7, 7);
            this.userColSel.Name = "userColSel";
            this.userColSel.Size = new System.Drawing.Size(102, 120);
            this.userColSel.TabIndex = 0;
            // 
            // tabPageCommon
            // 
            this.tabPageCommon.BackColor = System.Drawing.Color.LightGray;
            this.tabPageCommon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageCommon.Controls.Add(this.groupBoxMarker);
            this.tabPageCommon.Controls.Add(this.btnOutPthBrowse);
            this.tabPageCommon.Controls.Add(this.fileComboOutpath);
            this.tabPageCommon.Controls.Add(this.userComboBoxPrefSepar);
            this.tabPageCommon.Controls.Add(this.label1);
            this.tabPageCommon.Controls.Add(this.label2);
            this.tabPageCommon.Location = new System.Drawing.Point(4, 22);
            this.tabPageCommon.Name = "tabPageCommon";
            this.tabPageCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCommon.Size = new System.Drawing.Size(393, 282);
            this.tabPageCommon.TabIndex = 3;
            this.tabPageCommon.Text = "Common";
            // 
            // groupBoxMarker
            // 
            this.groupBoxMarker.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBoxMarker.Controls.Add(this.linkLabelRexHlp);
            this.groupBoxMarker.Controls.Add(this.cbMarkLtrs);
            this.groupBoxMarker.Controls.Add(this.cbRegEx);
            this.groupBoxMarker.Location = new System.Drawing.Point(17, 106);
            this.groupBoxMarker.Name = "groupBoxMarker";
            this.groupBoxMarker.Size = new System.Drawing.Size(242, 93);
            this.groupBoxMarker.TabIndex = 9;
            this.groupBoxMarker.TabStop = false;
            this.groupBoxMarker.Text = "Filter / Marker";
            // 
            // linkLabelRexHlp
            // 
            this.linkLabelRexHlp.AutoSize = true;
            this.linkLabelRexHlp.Location = new System.Drawing.Point(163, 56);
            this.linkLabelRexHlp.Name = "linkLabelRexHlp";
            this.linkLabelRexHlp.Size = new System.Drawing.Size(61, 13);
            this.linkLabelRexHlp.TabIndex = 9;
            this.linkLabelRexHlp.TabStop = true;
            this.linkLabelRexHlp.Text = "Regex help";
            this.linkLabelRexHlp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRexHlp_LinkClicked);
            // 
            // cbMarkLtrs
            // 
            this.cbMarkLtrs.AutoSize = true;
            this.cbMarkLtrs.Location = new System.Drawing.Point(17, 33);
            this.cbMarkLtrs.Name = "cbMarkLtrs";
            this.cbMarkLtrs.Size = new System.Drawing.Size(121, 17);
            this.cbMarkLtrs.TabIndex = 7;
            this.cbMarkLtrs.Text = "Markers color letters";
            this.cbMarkLtrs.UseVisualStyleBackColor = true;
            this.cbMarkLtrs.CheckedChanged += new System.EventHandler(this.cbMarkLtrs_CheckedChanged);
            // 
            // cbRegEx
            // 
            this.cbRegEx.AutoSize = true;
            this.cbRegEx.Location = new System.Drawing.Point(17, 56);
            this.cbRegEx.Name = "cbRegEx";
            this.cbRegEx.Size = new System.Drawing.Size(124, 17);
            this.cbRegEx.TabIndex = 8;
            this.cbRegEx.Text = "Regular expressions ";
            this.cbRegEx.UseVisualStyleBackColor = true;
            // 
            // btnOutPthBrowse
            // 
            this.btnOutPthBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutPthBrowse.Location = new System.Drawing.Point(318, 24);
            this.btnOutPthBrowse.Name = "btnOutPthBrowse";
            this.btnOutPthBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnOutPthBrowse.TabIndex = 6;
            this.btnOutPthBrowse.Text = "...";
            this.btnOutPthBrowse.UseVisualStyleBackColor = true;
            this.btnOutPthBrowse.Click += new System.EventHandler(this.btnOutPthBrowse_Click);
            // 
            // fileComboOutpath
            // 
            this.fileComboOutpath.AllowDrop = true;
            this.fileComboOutpath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileComboOutpath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.fileComboOutpath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.fileComboOutpath.FormattingEnabled = true;
            this.fileComboOutpath.Location = new System.Drawing.Point(87, 25);
            this.fileComboOutpath.MaxDropDownItems = 53;
            this.fileComboOutpath.Name = "fileComboOutpath";
            this.fileComboOutpath.ReadOnly = false;
            this.fileComboOutpath.Size = new System.Drawing.Size(224, 21);
            this.fileComboOutpath.TabIndex = 5;
            this.fileComboOutpath.Txt = "";
            // 
            // userComboBoxPrefSepar
            // 
            this.userComboBoxPrefSepar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.userComboBoxPrefSepar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.userComboBoxPrefSepar.BackColor = System.Drawing.SystemColors.Window;
            this.userComboBoxPrefSepar.FormattingEnabled = true;
            this.userComboBoxPrefSepar.Location = new System.Drawing.Point(138, 59);
            this.userComboBoxPrefSepar.MaxDropDownItems = 53;
            this.userComboBoxPrefSepar.Name = "userComboBoxPrefSepar";
            this.userComboBoxPrefSepar.ReadOnly = false;
            this.userComboBoxPrefSepar.Size = new System.Drawing.Size(121, 21);
            this.userComboBoxPrefSepar.TabIndex = 4;
            this.userComboBoxPrefSepar.Text = "\" ,;\\t\"";
            this.userComboBoxPrefSepar.Txt = "\" ,;\\t\"";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Output path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Separators 4 XL export:";
            // 
            // userTabControl
            // 
            this.userTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userTabControl.Controls.Add(this.tabPageCommon);
            this.userTabControl.Controls.Add(this.tabPageColSel);
            this.userTabControl.Location = new System.Drawing.Point(12, 12);
            this.userTabControl.Name = "userTabControl";
            this.userTabControl.SelectedIndex = 0;
            this.userTabControl.Size = new System.Drawing.Size(401, 308);
            this.userTabControl.TabIndex = 6;
            this.userTabControl.TabStop = false;
            // 
            // PreferencesUOText
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(425, 332);
            this.Controls.Add(this.userTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(2000, 400);
            this.MinimumSize = new System.Drawing.Size(420, 366);
            this.Name = "PreferencesUOText";
            this.Text = "Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreferencesUOText_FormClosing);
            this.tabPageColSel.ResumeLayout(false);
            this.tabPageColSel.PerformLayout();
            this.tabPageCommon.ResumeLayout(false);
            this.tabPageCommon.PerformLayout();
            this.groupBoxMarker.ResumeLayout(false);
            this.groupBoxMarker.PerformLayout();
            this.userTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.TabPage tabPageColSel;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Button btnFactDflt;
        private NS_UserColor.UserColorSelect userColSel;
        private System.Windows.Forms.TabPage tabPageCommon;
        private System.Windows.Forms.GroupBox groupBoxMarker;
        private System.Windows.Forms.LinkLabel linkLabelRexHlp;
        private System.Windows.Forms.CheckBox cbMarkLtrs;
        private System.Windows.Forms.CheckBox cbRegEx;
        private System.Windows.Forms.Button btnOutPthBrowse;
        private NS_UserCombo.FileComboBox fileComboOutpath;
        private NS_UserCombo.UserComboBox userComboBoxPrefSepar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private NS_UserTabControl.UserTabControl userTabControl;
    }
}