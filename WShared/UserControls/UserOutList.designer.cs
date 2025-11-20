namespace NS_UserOut
{
    partial class UserOutList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserOutList));
            this.menuStripBase = new System.Windows.Forms.MenuStrip();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoArrangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.autoCArrangeByOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoHArrangeByOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoscrollDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideLinesWithoutKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideLinesWithKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markLinesWithKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markItemsWithKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.wordDisjunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regularExpressionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAsCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.export2XLToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.prefsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFilterErase = new System.Windows.Forms.Button();
            this.textBoxNrLines = new System.Windows.Forms.TextBox();
            this.userComboBoxFilter = new NS_UserCombo.UserComboBox();
            this.userListViewOutp = new NS_UserList.UserListView();
            this.pictBoxFilt = new System.Windows.Forms.PictureBox();
            this.setoptWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxFilt)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripBase
            // 
            this.menuStripBase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.autoArrangeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStripBase.Location = new System.Drawing.Point(0, 0);
            this.menuStripBase.Name = "menuStripBase";
            this.menuStripBase.Size = new System.Drawing.Size(572, 39);
            this.menuStripBase.TabIndex = 1;
            this.menuStripBase.Text = "menuStrip1";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::Shared.Properties.Resources.loschen;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(46, 35);
            this.clearToolStripMenuItem.Text = "&Clear";
            this.clearToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // autoArrangeToolStripMenuItem
            // 
            this.autoArrangeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.columnsToolStripMenuItem,
            this.headersToolStripMenuItem,
            this.setoptWidthToolStripMenuItem,
            this.toolStripSeparator4,
            this.autoCArrangeByOpenToolStripMenuItem,
            this.autoHArrangeByOpenToolStripMenuItem});
            this.autoArrangeToolStripMenuItem.Image = global::Shared.Properties.Resources.roboter;
            this.autoArrangeToolStripMenuItem.Name = "autoArrangeToolStripMenuItem";
            this.autoArrangeToolStripMenuItem.Size = new System.Drawing.Size(88, 35);
            this.autoArrangeToolStripMenuItem.Text = "&Auto arrange";
            this.autoArrangeToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // columnsToolStripMenuItem
            // 
            this.columnsToolStripMenuItem.Name = "columnsToolStripMenuItem";
            this.columnsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.columnsToolStripMenuItem.Text = "&Columns";
            this.columnsToolStripMenuItem.Click += new System.EventHandler(this.columnsToolStripMenuItem_Click);
            // 
            // headersToolStripMenuItem
            // 
            this.headersToolStripMenuItem.Name = "headersToolStripMenuItem";
            this.headersToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.headersToolStripMenuItem.Text = "&Headers";
            this.headersToolStripMenuItem.Click += new System.EventHandler(this.headersToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(198, 6);
            // 
            // autoCArrangeByOpenToolStripMenuItem
            // 
            this.autoCArrangeByOpenToolStripMenuItem.CheckOnClick = true;
            this.autoCArrangeByOpenToolStripMenuItem.Name = "autoCArrangeByOpenToolStripMenuItem";
            this.autoCArrangeByOpenToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.autoCArrangeByOpenToolStripMenuItem.Text = "Auto C arrange by open";
            this.autoCArrangeByOpenToolStripMenuItem.Click += new System.EventHandler(this.autoCArrangeByOpenToolStripMenuItem_Click);
            // 
            // autoHArrangeByOpenToolStripMenuItem
            // 
            this.autoHArrangeByOpenToolStripMenuItem.Checked = true;
            this.autoHArrangeByOpenToolStripMenuItem.CheckOnClick = true;
            this.autoHArrangeByOpenToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoHArrangeByOpenToolStripMenuItem.Name = "autoHArrangeByOpenToolStripMenuItem";
            this.autoHArrangeByOpenToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.autoHArrangeByOpenToolStripMenuItem.Text = "Auto H arrange by open";
            this.autoHArrangeByOpenToolStripMenuItem.Click += new System.EventHandler(this.autoHArrangeByOpenToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoscrollDownToolStripMenuItem,
            this.filterToolStripMenuItem,
            this.toolStripSeparator3,
            this.exportAsCSVToolStripMenuItem,
            this.export2XLToolStripMenuItem1,
            this.toolStripSeparator1,
            this.prefsToolStripMenuItem});
            this.optionsToolStripMenuItem.Image = global::Shared.Properties.Resources.icons8_zahnrad_48;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 35);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // autoscrollDownToolStripMenuItem
            // 
            this.autoscrollDownToolStripMenuItem.Checked = true;
            this.autoscrollDownToolStripMenuItem.CheckOnClick = true;
            this.autoscrollDownToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoscrollDownToolStripMenuItem.Name = "autoscrollDownToolStripMenuItem";
            this.autoscrollDownToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.autoscrollDownToolStripMenuItem.Text = "Auto &scroll down ";
            this.autoscrollDownToolStripMenuItem.Click += new System.EventHandler(this.autoscrollDownToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideLinesWithoutKeyToolStripMenuItem,
            this.hideLinesWithKeyToolStripMenuItem,
            this.markLinesWithKeyToolStripMenuItem,
            this.markItemsWithKeyToolStripMenuItem,
            this.toolStripSeparator2,
            this.wordDisjunctionToolStripMenuItem,
            this.regularExpressionsToolStripMenuItem});
            this.filterToolStripMenuItem.Image = global::Shared.Properties.Resources.filter;
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.filterToolStripMenuItem.Text = "Filter ...";
            // 
            // hideLinesWithoutKeyToolStripMenuItem
            // 
            this.hideLinesWithoutKeyToolStripMenuItem.Checked = true;
            this.hideLinesWithoutKeyToolStripMenuItem.CheckOnClick = true;
            this.hideLinesWithoutKeyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hideLinesWithoutKeyToolStripMenuItem.Name = "hideLinesWithoutKeyToolStripMenuItem";
            this.hideLinesWithoutKeyToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.hideLinesWithoutKeyToolStripMenuItem.Text = "Hide lines without key";
            this.hideLinesWithoutKeyToolStripMenuItem.Click += new System.EventHandler(this.hideLinesWithoutKeyToolStripMenuItem_Click);
            // 
            // hideLinesWithKeyToolStripMenuItem
            // 
            this.hideLinesWithKeyToolStripMenuItem.Name = "hideLinesWithKeyToolStripMenuItem";
            this.hideLinesWithKeyToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.hideLinesWithKeyToolStripMenuItem.Text = "Hide lines with key";
            this.hideLinesWithKeyToolStripMenuItem.Click += new System.EventHandler(this.hideLinesWithKeyToolStripMenuItem_Click);
            // 
            // markLinesWithKeyToolStripMenuItem
            // 
            this.markLinesWithKeyToolStripMenuItem.CheckOnClick = true;
            this.markLinesWithKeyToolStripMenuItem.Name = "markLinesWithKeyToolStripMenuItem";
            this.markLinesWithKeyToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.markLinesWithKeyToolStripMenuItem.Text = "Mark lines with key";
            this.markLinesWithKeyToolStripMenuItem.Click += new System.EventHandler(this.markLinesWithKeyToolStripMenuItem_Click);
            // 
            // markItemsWithKeyToolStripMenuItem
            // 
            this.markItemsWithKeyToolStripMenuItem.Name = "markItemsWithKeyToolStripMenuItem";
            this.markItemsWithKeyToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.markItemsWithKeyToolStripMenuItem.Text = "Mark items with key";
            this.markItemsWithKeyToolStripMenuItem.Click += new System.EventHandler(this.markItemsWithKeyToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(188, 6);
            // 
            // wordDisjunctionToolStripMenuItem
            // 
            this.wordDisjunctionToolStripMenuItem.Checked = true;
            this.wordDisjunctionToolStripMenuItem.CheckOnClick = true;
            this.wordDisjunctionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wordDisjunctionToolStripMenuItem.Name = "wordDisjunctionToolStripMenuItem";
            this.wordDisjunctionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.wordDisjunctionToolStripMenuItem.Text = "Word disjunction";
            // 
            // regularExpressionsToolStripMenuItem
            // 
            this.regularExpressionsToolStripMenuItem.Checked = true;
            this.regularExpressionsToolStripMenuItem.CheckOnClick = true;
            this.regularExpressionsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.regularExpressionsToolStripMenuItem.Name = "regularExpressionsToolStripMenuItem";
            this.regularExpressionsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.regularExpressionsToolStripMenuItem.Text = "Regular expressions";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(164, 6);
            // 
            // exportAsCSVToolStripMenuItem
            // 
            this.exportAsCSVToolStripMenuItem.Image = global::Shared.Properties.Resources.csv_datei;
            this.exportAsCSVToolStripMenuItem.Name = "exportAsCSVToolStripMenuItem";
            this.exportAsCSVToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.exportAsCSVToolStripMenuItem.Text = "Export as &CSV";
            this.exportAsCSVToolStripMenuItem.Click += new System.EventHandler(this.exportAsCSVToolStripMenuItem_Click);
            // 
            // export2XLToolStripMenuItem1
            // 
            this.export2XLToolStripMenuItem1.Image = global::Shared.Properties.Resources.excel;
            this.export2XLToolStripMenuItem1.Name = "export2XLToolStripMenuItem1";
            this.export2XLToolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.export2XLToolStripMenuItem1.Text = "&Export to EXCEL";
            this.export2XLToolStripMenuItem1.Click += new System.EventHandler(this.export2XLToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
            // 
            // prefsToolStripMenuItem
            // 
            this.prefsToolStripMenuItem.Image = global::Shared.Properties.Resources.icons8_support_48;
            this.prefsToolStripMenuItem.Name = "prefsToolStripMenuItem";
            this.prefsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.prefsToolStripMenuItem.Text = "Preferences ...";
            this.prefsToolStripMenuItem.Click += new System.EventHandler(this.prefsToolStripMenuItem_Click);
            // 
            // btnFilterErase
            // 
            this.btnFilterErase.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilterErase.Location = new System.Drawing.Point(346, 11);
            this.btnFilterErase.Name = "btnFilterErase";
            this.btnFilterErase.Size = new System.Drawing.Size(20, 20);
            this.btnFilterErase.TabIndex = 4;
            this.btnFilterErase.Text = "X";
            this.btnFilterErase.UseVisualStyleBackColor = true;
            this.btnFilterErase.Click += new System.EventHandler(this.btnFilterErase_Click);
            // 
            // textBoxNrLines
            // 
            this.textBoxNrLines.Location = new System.Drawing.Point(256, 11);
            this.textBoxNrLines.Name = "textBoxNrLines";
            this.textBoxNrLines.ReadOnly = true;
            this.textBoxNrLines.Size = new System.Drawing.Size(48, 20);
            this.textBoxNrLines.TabIndex = 5;
            this.textBoxNrLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // userComboBoxFilter
            // 
            this.userComboBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userComboBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.userComboBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.userComboBoxFilter.FormattingEnabled = true;
            this.userComboBoxFilter.Location = new System.Drawing.Point(373, 11);
            this.userComboBoxFilter.MaxDropDownItems = 53;
            this.userComboBoxFilter.Name = "userComboBoxFilter";
            this.userComboBoxFilter.ReadOnly = false;
            this.userComboBoxFilter.Size = new System.Drawing.Size(187, 21);
            this.userComboBoxFilter.Sorted = true;
            this.userComboBoxFilter.TabIndex = 2;
            this.userComboBoxFilter.Txt = "";
            this.userComboBoxFilter.TextChanged += new System.EventHandler(this.userComboBoxFilter_TextChanged);
            // 
            // userListViewOutp
            // 
            this.userListViewOutp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userListViewOutp.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userListViewOutp.ForeColor = System.Drawing.Color.SeaGreen;
            this.userListViewOutp.FullRowSelect = true;
            this.userListViewOutp.HideSelection = false;
            this.userListViewOutp.Location = new System.Drawing.Point(13, 42);
            this.userListViewOutp.Name = "userListViewOutp";
            this.userListViewOutp.Size = new System.Drawing.Size(547, 453);
            this.userListViewOutp.TabIndex = 0;
            this.userListViewOutp.UseCompatibleStateImageBehavior = false;
            this.userListViewOutp.SelectedIndexChanged += new System.EventHandler(this.userListViewOutp_SelectedIndexChanged);
            // 
            // pictBoxFilt
            // 
            this.pictBoxFilt.Image = global::Shared.Properties.Resources.FilterIcon;
            this.pictBoxFilt.Location = new System.Drawing.Point(308, 4);
            this.pictBoxFilt.Name = "pictBoxFilt";
            this.pictBoxFilt.Size = new System.Drawing.Size(31, 33);
            this.pictBoxFilt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictBoxFilt.TabIndex = 6;
            this.pictBoxFilt.TabStop = false;
            this.pictBoxFilt.Click += new System.EventHandler(this.pictBoxFilt_Click);
            // 
            // setoptWidthToolStripMenuItem
            // 
            this.setoptWidthToolStripMenuItem.Name = "setoptWidthToolStripMenuItem";
            this.setoptWidthToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.setoptWidthToolStripMenuItem.Text = "Set &opt. width";
            this.setoptWidthToolStripMenuItem.Click += new System.EventHandler(this.setoptWidthToolStripMenuItem_Click);
            // 
            // UserOutList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 507);
            this.Controls.Add(this.pictBoxFilt);
            this.Controls.Add(this.textBoxNrLines);
            this.Controls.Add(this.btnFilterErase);
            this.Controls.Add(this.userComboBoxFilter);
            this.Controls.Add(this.userListViewOutp);
            this.Controls.Add(this.menuStripBase);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripBase;
            this.MinimumSize = new System.Drawing.Size(447, 150);
            this.Name = "UserOutList";
            this.Text = "List Output";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserOutList_FormClosing);
            this.Load += new System.EventHandler(this.UserOutList_Load);
            this.VisibleChanged += new System.EventHandler(this.UserOutList_VisibleChanged);
            this.menuStripBase.ResumeLayout(false);
            this.menuStripBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxFilt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected NS_UserList.UserListView userListViewOutp;
        protected System.Windows.Forms.MenuStrip menuStripBase;
        protected System.Windows.Forms.ToolStripMenuItem autoArrangeToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem columnsToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem headersToolStripMenuItem;
        protected NS_UserCombo.UserComboBox userComboBoxFilter;
        protected System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem export2XLToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem prefsToolStripMenuItem;
        private System.Windows.Forms.Button btnFilterErase;
        private System.Windows.Forms.ToolStripMenuItem autoscrollDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideLinesWithoutKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markLinesWithKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markItemsWithKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem wordDisjunctionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideLinesWithKeyToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxNrLines;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exportAsCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem autoCArrangeByOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoHArrangeByOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regularExpressionsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictBoxFilt;
        private System.Windows.Forms.ToolStripMenuItem setoptWidthToolStripMenuItem;
    }
}