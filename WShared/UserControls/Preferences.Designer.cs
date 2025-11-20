namespace NS_UserOut
{
    partial class Preferences
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCommon = new System.Windows.Forms.TabPage();
            this.textBoxBCol2 = new System.Windows.Forms.TextBox();
            this.textBoxBCol1 = new System.Windows.Forms.TextBox();
            this.labelChldResp = new System.Windows.Forms.Label();
            this.labelBCol1 = new System.Windows.Forms.Label();
            this.textBoxFCol2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFCol1 = new System.Windows.Forms.TextBox();
            this.labelSeparator1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPageCommon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageCommon);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(374, 176);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageCommon
            // 
            this.tabPageCommon.AutoScroll = true;
            this.tabPageCommon.Controls.Add(this.textBoxBCol2);
            this.tabPageCommon.Controls.Add(this.textBoxBCol1);
            this.tabPageCommon.Controls.Add(this.labelChldResp);
            this.tabPageCommon.Controls.Add(this.labelBCol1);
            this.tabPageCommon.Controls.Add(this.textBoxFCol2);
            this.tabPageCommon.Controls.Add(this.label3);
            this.tabPageCommon.Controls.Add(this.textBoxFCol1);
            this.tabPageCommon.Controls.Add(this.labelSeparator1);
            this.tabPageCommon.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tabPageCommon.Location = new System.Drawing.Point(4, 22);
            this.tabPageCommon.Name = "tabPageCommon";
            this.tabPageCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCommon.Size = new System.Drawing.Size(366, 150);
            this.tabPageCommon.TabIndex = 0;
            this.tabPageCommon.Text = "Colors";
            this.tabPageCommon.UseVisualStyleBackColor = true;
            // 
            // textBoxBCol2
            // 
            this.textBoxBCol2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBCol2.BackColor = System.Drawing.Color.LightGreen;
            this.textBoxBCol2.Location = new System.Drawing.Point(148, 51);
            this.textBoxBCol2.Name = "textBoxBCol2";
            this.textBoxBCol2.ReadOnly = true;
            this.textBoxBCol2.Size = new System.Drawing.Size(177, 20);
            this.textBoxBCol2.TabIndex = 31;
            this.textBoxBCol2.Click += new System.EventHandler(this.textBoxColBack2_Click);
            // 
            // textBoxBCol1
            // 
            this.textBoxBCol1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBCol1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.textBoxBCol1.Location = new System.Drawing.Point(148, 25);
            this.textBoxBCol1.Name = "textBoxBCol1";
            this.textBoxBCol1.ReadOnly = true;
            this.textBoxBCol1.Size = new System.Drawing.Size(177, 20);
            this.textBoxBCol1.TabIndex = 30;
            this.textBoxBCol1.Click += new System.EventHandler(this.textBoxColBack1_Click);
            // 
            // labelChldResp
            // 
            this.labelChldResp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelChldResp.AutoSize = true;
            this.labelChldResp.Location = new System.Drawing.Point(42, 54);
            this.labelChldResp.Name = "labelChldResp";
            this.labelChldResp.Size = new System.Drawing.Size(67, 13);
            this.labelChldResp.TabIndex = 29;
            this.labelChldResp.Text = "Back color 2";
            // 
            // labelBCol1
            // 
            this.labelBCol1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelBCol1.AutoSize = true;
            this.labelBCol1.Location = new System.Drawing.Point(42, 28);
            this.labelBCol1.Name = "labelBCol1";
            this.labelBCol1.Size = new System.Drawing.Size(67, 13);
            this.labelBCol1.TabIndex = 28;
            this.labelBCol1.Text = "Back color 1";
            // 
            // textBoxFCol2
            // 
            this.textBoxFCol2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFCol2.BackColor = System.Drawing.Color.SeaGreen;
            this.textBoxFCol2.Location = new System.Drawing.Point(148, 103);
            this.textBoxFCol2.Name = "textBoxFCol2";
            this.textBoxFCol2.ReadOnly = true;
            this.textBoxFCol2.Size = new System.Drawing.Size(177, 20);
            this.textBoxFCol2.TabIndex = 25;
            this.textBoxFCol2.Click += new System.EventHandler(this.textBoxColFore2_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Fore color 2";
            // 
            // textBoxFCol1
            // 
            this.textBoxFCol1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFCol1.BackColor = System.Drawing.Color.Blue;
            this.textBoxFCol1.Location = new System.Drawing.Point(148, 77);
            this.textBoxFCol1.Name = "textBoxFCol1";
            this.textBoxFCol1.ReadOnly = true;
            this.textBoxFCol1.Size = new System.Drawing.Size(177, 20);
            this.textBoxFCol1.TabIndex = 23;
            this.textBoxFCol1.Click += new System.EventHandler(this.textBoxColFore1_Click);
            // 
            // labelSeparator1
            // 
            this.labelSeparator1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelSeparator1.AutoSize = true;
            this.labelSeparator1.Location = new System.Drawing.Point(42, 80);
            this.labelSeparator1.Name = "labelSeparator1";
            this.labelSeparator1.Size = new System.Drawing.Size(63, 13);
            this.labelSeparator1.TabIndex = 22;
            this.labelSeparator1.Text = "Fore color 1";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.Location = new System.Drawing.Point(12, 194);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(331, 194);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(55, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(398, 226);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tabControl);
            this.MaximumSize = new System.Drawing.Size(5000, 465);
            this.MinimumSize = new System.Drawing.Size(414, 200);
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.tabControl.ResumeLayout(false);
            this.tabPageCommon.ResumeLayout(false);
            this.tabPageCommon.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageCommon;
        private System.Windows.Forms.TextBox textBoxBCol2;
        private System.Windows.Forms.TextBox textBoxBCol1;
        private System.Windows.Forms.Label labelChldResp;
        private System.Windows.Forms.Label labelBCol1;
        private System.Windows.Forms.TextBox textBoxFCol2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFCol1;
        private System.Windows.Forms.Label labelSeparator1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}