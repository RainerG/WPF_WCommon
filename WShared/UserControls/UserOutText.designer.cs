namespace NS_UserOut
{
    partial class UserOutText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserOutText));
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoScrollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAndHandOverToWordpadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSeInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSelPathInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSelpathInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openselPathInExplorerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSelToXLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSelTolistViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordDisjunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.caseSensitiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.markAllLinesContainingKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAllLinesWithoutKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAllFoundKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMarkedLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.leaveAllLinesContainigKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllLinesContainitKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteAllRightFromKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllLeftFromKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonDelMark = new System.Windows.Forms.Button();
            this.userComboBoxFilter = new NS_UserCombo.UserComboBox();
            this.userComboBoxMark = new NS_UserCombo.UserComboBox();
            this.picBoxMark = new System.Windows.Forms.PictureBox();
            this.setOptWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMark)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.CausesValidation = false;
            this.richTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox.Location = new System.Drawing.Point(12, 42);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(635, 189);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            this.richTextBox.WordWrap = false;
            this.richTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox_KeyDown);
            this.richTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox_KeyPress);
            this.richTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.richTextBox_MouseUp);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.filterToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(660, 47);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::Shared.Properties.Resources.loschen;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(46, 43);
            this.clearToolStripMenuItem.Text = "&Clear";
            this.clearToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findStringToolStripMenuItem,
            this.sortSelectionToolStripMenuItem,
            this.autoScrollToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveOutputToolStripMenuItem,
            this.saveAndHandOverToWordpadToolStripMenuItem,
            this.openInEditorToolStripMenuItem,
            this.openSeInEditorToolStripMenuItem,
            this.openSelPathInEditorToolStripMenuItem,
            this.openSelpathInExplorerToolStripMenuItem,
            this.openselPathInExplorerToolStripMenuItem1,
            this.setOptWidthToolStripMenuItem,
            this.exportSelToXLToolStripMenuItem,
            this.exportSelTolistViewToolStripMenuItem,
            this.toolStripSeparator2,
            this.preferencesToolStripMenuItem});
            this.optionsToolStripMenuItem.Image = global::Shared.Properties.Resources.icons8_zahnrad_48;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 43);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // findStringToolStripMenuItem
            // 
            this.findStringToolStripMenuItem.Image = global::Shared.Properties.Resources.finden;
            this.findStringToolStripMenuItem.Name = "findStringToolStripMenuItem";
            this.findStringToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.findStringToolStripMenuItem.Text = "&Find string ...";
            this.findStringToolStripMenuItem.Click += new System.EventHandler(this.findStringToolStripMenuItem_Click);
            // 
            // sortSelectionToolStripMenuItem
            // 
            this.sortSelectionToolStripMenuItem.Name = "sortSelectionToolStripMenuItem";
            this.sortSelectionToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.sortSelectionToolStripMenuItem.Text = "&Sort selection";
            this.sortSelectionToolStripMenuItem.Click += new System.EventHandler(this.sortSelectionToolStripMenuItem_Click);
            // 
            // autoScrollToolStripMenuItem
            // 
            this.autoScrollToolStripMenuItem.Checked = true;
            this.autoScrollToolStripMenuItem.CheckOnClick = true;
            this.autoScrollToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoScrollToolStripMenuItem.Name = "autoScrollToolStripMenuItem";
            this.autoScrollToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.autoScrollToolStripMenuItem.Text = "&Auto scroll";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(242, 6);
            // 
            // saveOutputToolStripMenuItem
            // 
            this.saveOutputToolStripMenuItem.Name = "saveOutputToolStripMenuItem";
            this.saveOutputToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.saveOutputToolStripMenuItem.Text = "Sa&ve output";
            this.saveOutputToolStripMenuItem.Click += new System.EventHandler(this.saveOutputToolStripMenuItem_Click);
            // 
            // saveAndHandOverToWordpadToolStripMenuItem
            // 
            this.saveAndHandOverToWordpadToolStripMenuItem.Name = "saveAndHandOverToWordpadToolStripMenuItem";
            this.saveAndHandOverToWordpadToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.saveAndHandOverToWordpadToolStripMenuItem.Text = "Open in Wordpad";
            this.saveAndHandOverToWordpadToolStripMenuItem.Click += new System.EventHandler(this.saveAndHandOverToWordpadToolStripMenuItem_Click);
            // 
            // openInEditorToolStripMenuItem
            // 
            this.openInEditorToolStripMenuItem.Name = "openInEditorToolStripMenuItem";
            this.openInEditorToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.openInEditorToolStripMenuItem.Text = "Open in editor";
            this.openInEditorToolStripMenuItem.Click += new System.EventHandler(this.openInEditorToolStripMenuItem_Click);
            // 
            // openSeInEditorToolStripMenuItem
            // 
            this.openSeInEditorToolStripMenuItem.Name = "openSeInEditorToolStripMenuItem";
            this.openSeInEditorToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.openSeInEditorToolStripMenuItem.Text = "Open sel. &in editor";
            this.openSeInEditorToolStripMenuItem.Click += new System.EventHandler(this.openSelInEditorToolStripMenuItem_Click);
            // 
            // openSelPathInEditorToolStripMenuItem
            // 
            this.openSelPathInEditorToolStripMenuItem.Name = "openSelPathInEditorToolStripMenuItem";
            this.openSelPathInEditorToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.openSelPathInEditorToolStripMenuItem.Text = "Open sel. path in editor";
            this.openSelPathInEditorToolStripMenuItem.Click += new System.EventHandler(this.openSelPathInEditorToolStripMenuItem_Click);
            // 
            // openSelpathInExplorerToolStripMenuItem
            // 
            this.openSelpathInExplorerToolStripMenuItem.Name = "openSelpathInExplorerToolStripMenuItem";
            this.openSelpathInExplorerToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.openSelpathInExplorerToolStripMenuItem.Text = "Open sel.path by Win.deflt app";
            this.openSelpathInExplorerToolStripMenuItem.Click += new System.EventHandler(this.openSelpathInExplorerToolStripMenuItem_Click);
            // 
            // openselPathInExplorerToolStripMenuItem1
            // 
            this.openselPathInExplorerToolStripMenuItem1.Name = "openselPathInExplorerToolStripMenuItem1";
            this.openselPathInExplorerToolStripMenuItem1.Size = new System.Drawing.Size(245, 30);
            this.openselPathInExplorerToolStripMenuItem1.Text = "Open.sel path in Explorer";
            this.openselPathInExplorerToolStripMenuItem1.Click += new System.EventHandler(this.openselPathInExplorerToolStripMenuItem1_Click);
            // 
            // exportSelToXLToolStripMenuItem
            // 
            this.exportSelToXLToolStripMenuItem.Image = global::Shared.Properties.Resources.csv_datei;
            this.exportSelToXLToolStripMenuItem.Name = "exportSelToXLToolStripMenuItem";
            this.exportSelToXLToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.exportSelToXLToolStripMenuItem.Text = "&Export sel. to CSV";
            this.exportSelToXLToolStripMenuItem.Click += new System.EventHandler(this.exportSelToXLToolStripMenuItem_Click);
            // 
            // exportSelTolistViewToolStripMenuItem
            // 
            this.exportSelTolistViewToolStripMenuItem.Name = "exportSelTolistViewToolStripMenuItem";
            this.exportSelTolistViewToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.exportSelTolistViewToolStripMenuItem.Text = "Export sel. to &list view";
            this.exportSelTolistViewToolStripMenuItem.Click += new System.EventHandler(this.exportSelTolistViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(242, 6);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Image = global::Shared.Properties.Resources.icons8_support_481;
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.preferencesToolStripMenuItem.Text = "&Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordDisjunctionToolStripMenuItem,
            this.caseSensitiveToolStripMenuItem,
            this.toolStripSeparator3,
            this.markAllLinesContainingKeyToolStripMenuItem,
            this.markAllLinesWithoutKeyToolStripMenuItem,
            this.markAllFoundKeysToolStripMenuItem,
            this.invertMarkToolStripMenuItem,
            this.deleteMarkedLinesToolStripMenuItem,
            this.jToolStripMenuItem,
            this.leaveAllLinesContainigKeyToolStripMenuItem,
            this.deleteAllLinesContainitKeyToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteAllRightFromKeyToolStripMenuItem,
            this.deleteAllLeftFromKeyToolStripMenuItem,
            this.toolStripMenuItem2,
            this.undoToolStripMenuItem});
            this.filterToolStripMenuItem.Image = global::Shared.Properties.Resources.filter;
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(45, 43);
            this.filterToolStripMenuItem.Text = "&Filter";
            this.filterToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // wordDisjunctionToolStripMenuItem
            // 
            this.wordDisjunctionToolStripMenuItem.CheckOnClick = true;
            this.wordDisjunctionToolStripMenuItem.Name = "wordDisjunctionToolStripMenuItem";
            this.wordDisjunctionToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.wordDisjunctionToolStripMenuItem.Text = "&Word disjunction";
            this.wordDisjunctionToolStripMenuItem.Click += new System.EventHandler(this.wordDisjunctionToolStripMenuItem_Click);
            // 
            // caseSensitiveToolStripMenuItem
            // 
            this.caseSensitiveToolStripMenuItem.CheckOnClick = true;
            this.caseSensitiveToolStripMenuItem.Name = "caseSensitiveToolStripMenuItem";
            this.caseSensitiveToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.caseSensitiveToolStripMenuItem.Text = "&Case sensitive";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(227, 6);
            // 
            // markAllLinesContainingKeyToolStripMenuItem
            // 
            this.markAllLinesContainingKeyToolStripMenuItem.Name = "markAllLinesContainingKeyToolStripMenuItem";
            this.markAllLinesContainingKeyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.markAllLinesContainingKeyToolStripMenuItem.Text = "Mark all lines containing key";
            this.markAllLinesContainingKeyToolStripMenuItem.Click += new System.EventHandler(this.markAllLinesContainingKeyToolStripMenuItem_Click);
            // 
            // markAllLinesWithoutKeyToolStripMenuItem
            // 
            this.markAllLinesWithoutKeyToolStripMenuItem.Name = "markAllLinesWithoutKeyToolStripMenuItem";
            this.markAllLinesWithoutKeyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.markAllLinesWithoutKeyToolStripMenuItem.Text = "Mark all lines without key";
            this.markAllLinesWithoutKeyToolStripMenuItem.Click += new System.EventHandler(this.markAllLinesWithoutKeyToolStripMenuItem_Click);
            // 
            // markAllFoundKeysToolStripMenuItem
            // 
            this.markAllFoundKeysToolStripMenuItem.Name = "markAllFoundKeysToolStripMenuItem";
            this.markAllFoundKeysToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.markAllFoundKeysToolStripMenuItem.Text = "Mark all found keys";
            this.markAllFoundKeysToolStripMenuItem.Click += new System.EventHandler(this.markAllFoundKeysToolStripMenuItem_Click);
            // 
            // invertMarkToolStripMenuItem
            // 
            this.invertMarkToolStripMenuItem.Name = "invertMarkToolStripMenuItem";
            this.invertMarkToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.invertMarkToolStripMenuItem.Text = "Invert mark";
            this.invertMarkToolStripMenuItem.Click += new System.EventHandler(this.invertMarkToolStripMenuItem_Click);
            // 
            // deleteMarkedLinesToolStripMenuItem
            // 
            this.deleteMarkedLinesToolStripMenuItem.Name = "deleteMarkedLinesToolStripMenuItem";
            this.deleteMarkedLinesToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.deleteMarkedLinesToolStripMenuItem.Text = "Delete marked lines";
            this.deleteMarkedLinesToolStripMenuItem.Click += new System.EventHandler(this.deleteMarkedLinesToolStripMenuItem_Click);
            // 
            // jToolStripMenuItem
            // 
            this.jToolStripMenuItem.Name = "jToolStripMenuItem";
            this.jToolStripMenuItem.Size = new System.Drawing.Size(227, 6);
            // 
            // leaveAllLinesContainigKeyToolStripMenuItem
            // 
            this.leaveAllLinesContainigKeyToolStripMenuItem.Name = "leaveAllLinesContainigKeyToolStripMenuItem";
            this.leaveAllLinesContainigKeyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.leaveAllLinesContainigKeyToolStripMenuItem.Text = "Leave all lines containig key";
            this.leaveAllLinesContainigKeyToolStripMenuItem.Click += new System.EventHandler(this.leaveAllLinesContainigKeyToolStripMenuItem_Click);
            // 
            // deleteAllLinesContainitKeyToolStripMenuItem
            // 
            this.deleteAllLinesContainitKeyToolStripMenuItem.Name = "deleteAllLinesContainitKeyToolStripMenuItem";
            this.deleteAllLinesContainitKeyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.deleteAllLinesContainitKeyToolStripMenuItem.Text = "Delete all lines containing key";
            this.deleteAllLinesContainitKeyToolStripMenuItem.Click += new System.EventHandler(this.deleteAllLinesContainitKeyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(227, 6);
            // 
            // deleteAllRightFromKeyToolStripMenuItem
            // 
            this.deleteAllRightFromKeyToolStripMenuItem.Name = "deleteAllRightFromKeyToolStripMenuItem";
            this.deleteAllRightFromKeyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.deleteAllRightFromKeyToolStripMenuItem.Text = "Delete all right from key";
            this.deleteAllRightFromKeyToolStripMenuItem.Click += new System.EventHandler(this.deleteAllRightFromKeyToolStripMenuItem_Click);
            // 
            // deleteAllLeftFromKeyToolStripMenuItem
            // 
            this.deleteAllLeftFromKeyToolStripMenuItem.Name = "deleteAllLeftFromKeyToolStripMenuItem";
            this.deleteAllLeftFromKeyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.deleteAllLeftFromKeyToolStripMenuItem.Text = "Delete all left from key";
            this.deleteAllLeftFromKeyToolStripMenuItem.Click += new System.EventHandler(this.deleteAllLeftFromKeyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(227, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // buttonDelMark
            // 
            this.buttonDelMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelMark.Location = new System.Drawing.Point(277, 9);
            this.buttonDelMark.Name = "buttonDelMark";
            this.buttonDelMark.Size = new System.Drawing.Size(25, 23);
            this.buttonDelMark.TabIndex = 4;
            this.buttonDelMark.Text = "X";
            this.buttonDelMark.UseVisualStyleBackColor = true;
            this.buttonDelMark.Click += new System.EventHandler(this.buttonDelMark_Click);
            // 
            // userComboBoxFilter
            // 
            this.userComboBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userComboBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.userComboBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.userComboBoxFilter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.userComboBoxFilter.FormattingEnabled = true;
            this.userComboBoxFilter.Location = new System.Drawing.Point(181, 10);
            this.userComboBoxFilter.MaxDropDownItems = 53;
            this.userComboBoxFilter.MaximumSize = new System.Drawing.Size(400, 0);
            this.userComboBoxFilter.Name = "userComboBoxFilter";
            this.userComboBoxFilter.ReadOnly = false;
            this.userComboBoxFilter.Size = new System.Drawing.Size(53, 21);
            this.userComboBoxFilter.Sorted = true;
            this.userComboBoxFilter.TabIndex = 6;
            this.userComboBoxFilter.Txt = "";
            // 
            // userComboBoxMark
            // 
            this.userComboBoxMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userComboBoxMark.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.userComboBoxMark.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.userComboBoxMark.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.userComboBoxMark.FormattingEnabled = true;
            this.userComboBoxMark.Location = new System.Drawing.Point(308, 10);
            this.userComboBoxMark.MaxDropDownItems = 53;
            this.userComboBoxMark.Name = "userComboBoxMark";
            this.userComboBoxMark.ReadOnly = false;
            this.userComboBoxMark.Size = new System.Drawing.Size(340, 21);
            this.userComboBoxMark.Sorted = true;
            this.userComboBoxMark.TabIndex = 3;
            this.userComboBoxMark.Txt = "";
            // 
            // picBoxMark
            // 
            this.picBoxMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxMark.Image = global::Shared.Properties.Resources.Mark;
            this.picBoxMark.Location = new System.Drawing.Point(241, 3);
            this.picBoxMark.Name = "picBoxMark";
            this.picBoxMark.Size = new System.Drawing.Size(36, 34);
            this.picBoxMark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxMark.TabIndex = 7;
            this.picBoxMark.TabStop = false;
            this.picBoxMark.Click += new System.EventHandler(this.picBoxMark_Click);
            // 
            // setOptWidthToolStripMenuItem
            // 
            this.setOptWidthToolStripMenuItem.Name = "setOptWidthToolStripMenuItem";
            this.setOptWidthToolStripMenuItem.Size = new System.Drawing.Size(245, 30);
            this.setOptWidthToolStripMenuItem.Text = "Set &opt. width";
            this.setOptWidthToolStripMenuItem.Click += new System.EventHandler(this.setOptWidthToolStripMenuItem_Click);
            // 
            // UserOutText
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(660, 243);
            this.Controls.Add(this.picBoxMark);
            this.Controls.Add(this.userComboBoxFilter);
            this.Controls.Add(this.buttonDelMark);
            this.Controls.Add(this.userComboBoxMark);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(652, 152);
            this.Name = "UserOutText";
            this.Text = "Text Output";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutText_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.UserOutText_VisibleChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMark)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findStringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSelToXLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveOutputToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSelTolistViewToolStripMenuItem;

        protected System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private NS_UserCombo.UserComboBox userComboBoxMark;
        private System.Windows.Forms.Button buttonDelMark;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private NS_UserCombo.UserComboBox userComboBoxFilter;
        private System.Windows.Forms.ToolStripMenuItem leaveAllLinesContainigKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAllLinesContainingKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAllLinesWithoutKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator jToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllLinesContainitKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteAllRightFromKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllLeftFromKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertMarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMarkedLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordDisjunctionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem autoScrollToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAndHandOverToWordpadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAllFoundKeysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSeInEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSelPathInEditorToolStripMenuItem;
        private System.Windows.Forms.PictureBox picBoxMark;
        private System.Windows.Forms.ToolStripMenuItem caseSensitiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSelpathInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openselPathInExplorerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setOptWidthToolStripMenuItem;
    }
}