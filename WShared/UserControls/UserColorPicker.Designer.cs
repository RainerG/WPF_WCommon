namespace NS_UserColor
{
    partial class UserColorPicker
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxColor = new System.Windows.Forms.TextBox();
            this.labelCol = new System.Windows.Forms.Label();
            this.checkBoxBold = new System.Windows.Forms.CheckBox();
            this.checkBoxItal = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxColor
            // 
            this.textBoxColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxColor.BackColor = System.Drawing.Color.Red;
            this.textBoxColor.Location = new System.Drawing.Point(85, 3);
            this.textBoxColor.Name = "textBoxColor";
            this.textBoxColor.ReadOnly = true;
            this.textBoxColor.Size = new System.Drawing.Size(194, 20);
            this.textBoxColor.TabIndex = 42;
            // 
            // labelCol
            // 
            this.labelCol.AutoSize = true;
            this.labelCol.Location = new System.Drawing.Point(3, 7);
            this.labelCol.Name = "labelCol";
            this.labelCol.Size = new System.Drawing.Size(35, 13);
            this.labelCol.TabIndex = 43;
            this.labelCol.Text = "label1";
            // 
            // checkBoxBold
            // 
            this.checkBoxBold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxBold.AutoSize = true;
            this.checkBoxBold.Location = new System.Drawing.Point(293, 6);
            this.checkBoxBold.Name = "checkBoxBold";
            this.checkBoxBold.Size = new System.Drawing.Size(15, 14);
            this.checkBoxBold.TabIndex = 44;
            this.checkBoxBold.UseVisualStyleBackColor = true;
            // 
            // checkBoxItal
            // 
            this.checkBoxItal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxItal.AutoSize = true;
            this.checkBoxItal.Location = new System.Drawing.Point(322, 6);
            this.checkBoxItal.Name = "checkBoxItal";
            this.checkBoxItal.Size = new System.Drawing.Size(15, 14);
            this.checkBoxItal.TabIndex = 45;
            this.checkBoxItal.UseVisualStyleBackColor = true;
            // 
            // UserColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxItal);
            this.Controls.Add(this.checkBoxBold);
            this.Controls.Add(this.labelCol);
            this.Controls.Add(this.textBoxColor);
            this.Name = "UserColorPicker";
            this.Size = new System.Drawing.Size(350, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxColor;
        private System.Windows.Forms.Label labelCol;
        private System.Windows.Forms.CheckBox checkBoxBold;
        private System.Windows.Forms.CheckBox checkBoxItal;
    }
}
