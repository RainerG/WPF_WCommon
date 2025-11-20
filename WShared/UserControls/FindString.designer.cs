using NS_UserCombo;

namespace NS_UserOut
{
    partial class FindString
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindString));
            this.buttonFind = new System.Windows.Forms.Button();
            this.checkBoxReverse = new System.Windows.Forms.CheckBox();
            this.checkBoxWholeWord = new System.Windows.Forms.CheckBox();
            this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
            this.comboBoxFind = new NS_UserCombo.UserComboBox();
            this.checkBoxRegEx = new System.Windows.Forms.CheckBox();
            this.checkBoxWrap = new System.Windows.Forms.CheckBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFind.Location = new System.Drawing.Point(174, 108);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 40);
            this.buttonFind.TabIndex = 1;
            this.buttonFind.Text = "&Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            this.buttonFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.buttonFind_KeyPress);
            // 
            // checkBoxReverse
            // 
            this.checkBoxReverse.AutoSize = true;
            this.checkBoxReverse.Checked = true;
            this.checkBoxReverse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReverse.Location = new System.Drawing.Point(12, 39);
            this.checkBoxReverse.Name = "checkBoxReverse";
            this.checkBoxReverse.Size = new System.Drawing.Size(66, 17);
            this.checkBoxReverse.TabIndex = 2;
            this.checkBoxReverse.Text = "&Reverse";
            this.checkBoxReverse.UseVisualStyleBackColor = true;
            // 
            // checkBoxWholeWord
            // 
            this.checkBoxWholeWord.AutoSize = true;
            this.checkBoxWholeWord.Location = new System.Drawing.Point(12, 85);
            this.checkBoxWholeWord.Name = "checkBoxWholeWord";
            this.checkBoxWholeWord.Size = new System.Drawing.Size(83, 17);
            this.checkBoxWholeWord.TabIndex = 3;
            this.checkBoxWholeWord.Text = "Whole word";
            this.checkBoxWholeWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxMatchCase
            // 
            this.checkBoxMatchCase.AutoSize = true;
            this.checkBoxMatchCase.Location = new System.Drawing.Point(12, 108);
            this.checkBoxMatchCase.Name = "checkBoxMatchCase";
            this.checkBoxMatchCase.Size = new System.Drawing.Size(82, 17);
            this.checkBoxMatchCase.TabIndex = 4;
            this.checkBoxMatchCase.Text = "Match case";
            this.checkBoxMatchCase.UseVisualStyleBackColor = true;
            // 
            // comboBoxFind
            // 
            this.comboBoxFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxFind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxFind.FormattingEnabled = true;
            this.comboBoxFind.Location = new System.Drawing.Point(12, 12);
            this.comboBoxFind.Name = "comboBoxFind";
            this.comboBoxFind.ReadOnly = false;
            this.comboBoxFind.Size = new System.Drawing.Size(237, 21);
            this.comboBoxFind.TabIndex = 0;
            this.comboBoxFind.Text = "Error";
            this.comboBoxFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxFind_KeyDown);
            this.comboBoxFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxFind_KeyPress);
            // 
            // checkBoxRegEx
            // 
            this.checkBoxRegEx.AutoSize = true;
            this.checkBoxRegEx.Location = new System.Drawing.Point(12, 131);
            this.checkBoxRegEx.Name = "checkBoxRegEx";
            this.checkBoxRegEx.Size = new System.Drawing.Size(121, 17);
            this.checkBoxRegEx.TabIndex = 5;
            this.checkBoxRegEx.Text = "Regular expressions";
            this.checkBoxRegEx.UseVisualStyleBackColor = true;
            // 
            // checkBoxWrap
            // 
            this.checkBoxWrap.AutoSize = true;
            this.checkBoxWrap.Checked = true;
            this.checkBoxWrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWrap.Location = new System.Drawing.Point(12, 62);
            this.checkBoxWrap.Name = "checkBoxWrap";
            this.checkBoxWrap.Size = new System.Drawing.Size(88, 17);
            this.checkBoxWrap.TabIndex = 6;
            this.checkBoxWrap.Text = "&Wrap around";
            this.checkBoxWrap.UseVisualStyleBackColor = true;
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbResult.ForeColor = System.Drawing.SystemColors.Highlight;
            this.tbResult.Location = new System.Drawing.Point(164, 40);
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.Size = new System.Drawing.Size(85, 20);
            this.tbResult.TabIndex = 7;
            this.tbResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FindString
            // 
            this.AcceptButton = this.buttonFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 162);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.checkBoxWrap);
            this.Controls.Add(this.checkBoxRegEx);
            this.Controls.Add(this.checkBoxMatchCase);
            this.Controls.Add(this.checkBoxWholeWord);
            this.Controls.Add(this.checkBoxReverse);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.comboBoxFind);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 201);
            this.MinimumSize = new System.Drawing.Size(279, 201);
            this.Name = "FindString";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Find String";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindString_FormClosing);
            this.Load += new System.EventHandler(this.FindString_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        UserComboBox comboBoxFind;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.CheckBox checkBoxReverse;
        private System.Windows.Forms.CheckBox checkBoxWholeWord;
        private System.Windows.Forms.CheckBox checkBoxMatchCase;
        private System.Windows.Forms.CheckBox checkBoxRegEx;
        private System.Windows.Forms.CheckBox checkBoxWrap;
        private System.Windows.Forms.TextBox tbResult;
    }
}