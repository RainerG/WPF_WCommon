namespace NS_UserCombo
{
    partial class ComboFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComboFilter));
            this.label1 = new System.Windows.Forms.Label();
            this.cbInverted = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbFilters = new System.Windows.Forms.TextBox();
            this.cbRegEx = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filterwords separated by space (case sensitive):";
            // 
            // cbInverted
            // 
            this.cbInverted.AutoSize = true;
            this.cbInverted.Location = new System.Drawing.Point(12, 51);
            this.cbInverted.Name = "cbInverted";
            this.cbInverted.Size = new System.Drawing.Size(75, 17);
            this.cbInverted.TabIndex = 2;
            this.cbInverted.Text = "Invert filter";
            this.cbInverted.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(248, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 21);
            this.button1.TabIndex = 3;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbFilters
            // 
            this.tbFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilters.Location = new System.Drawing.Point(12, 26);
            this.tbFilters.Name = "tbFilters";
            this.tbFilters.Size = new System.Drawing.Size(229, 20);
            this.tbFilters.TabIndex = 4;
            // 
            // cbRegEx
            // 
            this.cbRegEx.AutoSize = true;
            this.cbRegEx.Location = new System.Drawing.Point(112, 51);
            this.cbRegEx.Name = "cbRegEx";
            this.cbRegEx.Size = new System.Drawing.Size(70, 17);
            this.cbRegEx.TabIndex = 5;
            this.cbRegEx.Text = "Reg.Exp.";
            this.cbRegEx.UseVisualStyleBackColor = true;
            // 
            // ComboFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 78);
            this.Controls.Add(this.cbRegEx);
            this.Controls.Add(this.tbFilters);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbInverted);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(2000, 117);
            this.MinimumSize = new System.Drawing.Size(272, 117);
            this.Name = "ComboFilter";
            this.Text = "Combo Filter Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ComboFilter_FormClosing);
            this.Load += new System.EventHandler(this.ComboFilter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbInverted;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbFilters;
        private System.Windows.Forms.CheckBox cbRegEx;
    }
}