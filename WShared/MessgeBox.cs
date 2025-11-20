using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NS_Utilities
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       06.05.2021
    LAST CHANGE:   07.05.2025
    ***************************************************************************/
    public partial class MessgeBox:Form
    {
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.05.2021
        LAST CHANGE:   07.05.2025
        ***************************************************************************/
        private static MessgeBox instance;
        public static  MessgeBox Instance //property of this class. Creates an instance if it is not created yet
        {
            get
            {
                if( instance == null )
                    instance = new MessgeBox();
                return instance;
            }
        }

        static MessageBoxButtons m_Btns;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       06.05.2021
        LAST CHANGE:   25.06.2024
        ***************************************************************************/
        public MessgeBox()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = btnOK;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.05.2021
        LAST CHANGE:   07.05.2025
        ***************************************************************************/
        public static DialogResult Show( string a_Msg, string a_Title ) { return Show( a_Msg, a_Title, MessageBoxButtons.OK ); }
        public static DialogResult Show( string a_Msg, string a_Title, MessageBoxButtons a_Btns )
        {
            m_Btns = a_Btns;

            Instance.rtbOutput.Clear();
            Instance.rtbOutput.Text = "";
            Instance.rtbOutput.AppendText( a_Msg );
            Instance.Text = a_Title;

            Instance.btnOK  .Text    = "&Ok";
            Instance.btnNo  .Text    = "&No";
            Instance.btnCncl.Text    = "&Cancel";
            Instance.btnOK  .Enabled = true;
            Instance.btnNo  .Enabled = true;
            Instance.btnCncl.Enabled = true;

            switch( a_Btns )
            {

                case MessageBoxButtons.YesNo:
                    Instance.btnOK  .Text    = "&Yes";
                    Instance.btnCncl.Text    = "&No";
                    Instance.btnOK  .Enabled = true;
                    Instance.btnCncl.Enabled = true;
                    Instance.btnNo  .Enabled = false;
                    Instance.btnNo  .Visible = false;
                    break;

                case MessageBoxButtons.YesNoCancel:
                    Instance.btnOK  .Text    = "&Yes";
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    Instance.btnOK  .Text    = "&Retry";
                    Instance.btnNo  .Text    = "&Abort";
                    Instance.btnCncl.Text    = "&Ignore";
                    break;

                case MessageBoxButtons.OK:
                default:
                    Instance.btnNo  .Enabled = false;
                    Instance.btnCncl.Enabled = false;
                    break;
            }

            return Instance.ShowDialog();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.05.2021
        LAST CHANGE:   16.05.2025
        ***************************************************************************/
        private void btnOK_Click( object sender, EventArgs e )
        {
            switch( m_Btns )
            {
                case      MessageBoxButtons.YesNo           : this.DialogResult = DialogResult.Yes  ; break;
                case      MessageBoxButtons.AbortRetryIgnore:
                case      MessageBoxButtons.RetryCancel     : this.DialogResult = DialogResult.Retry; break;
                default:                                      this.DialogResult = DialogResult.OK   ; break;
            }
            Instance.Close();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.05.2021
        LAST CHANGE:   16.05.2025
        ***************************************************************************/
        private void btnCncl_Click( object sender, EventArgs e )
        {
            switch( m_Btns )
            {
                case      MessageBoxButtons.YesNo           : this.DialogResult = DialogResult.No    ; break;
                case      MessageBoxButtons.AbortRetryIgnore:
                case      MessageBoxButtons.RetryCancel     : this.DialogResult = DialogResult.Ignore; break;
                default:                                      this.DialogResult = DialogResult.Cancel; break;
            }

            Instance.Close();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.02.2023
        LAST CHANGE:   07.05.2025
        ***************************************************************************/
        private void btnNo_Click( object sender, EventArgs e )
        {
            switch( m_Btns )
            {
                case      MessageBoxButtons.AbortRetryIgnore:
                case      MessageBoxButtons.RetryCancel:    this.DialogResult = DialogResult.Abort; break;
                default:                                    this.DialogResult = DialogResult.No;    break;
            }

            Instance.Close();
        }
    }
}
