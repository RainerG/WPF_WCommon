namespace NS_UserColor
{
    partial class ColorSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSelector));
            this.userColSel = new NS_UserColor.UserColorSelect();
            this.SuspendLayout();
            // 
            // userColSel
            // 
            this.userColSel.Location = new System.Drawing.Point(0, 0);
            this.userColSel.Name = "userColSel";
            this.userColSel.Size = new System.Drawing.Size(99, 111);
            this.userColSel.TabIndex = 0;
            // 
            // ColorSelector
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.userColSel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ColorSelector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorSelector_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private NS_UserColor.UserColorSelect userColSel;

    }
}