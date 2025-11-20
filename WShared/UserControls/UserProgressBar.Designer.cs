namespace NS_ProgrssBar
{
    partial class UserProgressBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelPB2 = new System.Windows.Forms.Label();
            this.labelPB1 = new System.Windows.Forms.Label();
            this.labelPB0 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelPB2
            // 
            this.labelPB2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPB2.AutoSize = true;
            this.labelPB2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.labelPB2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelPB2.Location = new System.Drawing.Point(436, 4);
            this.labelPB2.Name = "labelPB2";
            this.labelPB2.Size = new System.Drawing.Size(34, 13);
            this.labelPB2.TabIndex = 23;
            this.labelPB2.Text = "00:00";
            this.labelPB2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelPB1
            // 
            this.labelPB1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelPB1.AutoSize = true;
            this.labelPB1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.labelPB1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelPB1.Location = new System.Drawing.Point(227, 4);
            this.labelPB1.Name = "labelPB1";
            this.labelPB1.Size = new System.Drawing.Size(21, 13);
            this.labelPB1.TabIndex = 22;
            this.labelPB1.Text = "0%";
            this.labelPB1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelPB0
            // 
            this.labelPB0.AutoSize = true;
            this.labelPB0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.labelPB0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelPB0.Location = new System.Drawing.Point(4, 4);
            this.labelPB0.Name = "labelPB0";
            this.labelPB0.Size = new System.Drawing.Size(34, 13);
            this.labelPB0.TabIndex = 21;
            this.labelPB0.Text = "00:00";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 1);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(474, 20);
            this.progressBar.TabIndex = 20;
            // 
            // UserProgressBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.labelPB2);
            this.Controls.Add(this.labelPB1);
            this.Controls.Add(this.labelPB0);
            this.Controls.Add(this.progressBar);
            this.Name = "UserProgressBar";
            this.Size = new System.Drawing.Size(474, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPB2;
        private System.Windows.Forms.Label labelPB1;
        private System.Windows.Forms.Label labelPB0;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}
