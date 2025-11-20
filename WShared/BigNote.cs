using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NS_Utilities;

namespace NS_Utilities
{
    /***************************************************************************
    SPECIFICATION: Just a big warning sign
    CREATED:       26.11.2019
    LAST CHANGE:   26.11.2019
    ***************************************************************************/
    public partial class BigNote:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       26.11.2019
        LAST CHANGE:   26.11.2019
        ***************************************************************************/
        public UserRichTextBox RchTxtBox { get { return userRichTB; } }
        public UserTimer       ExitTimer { get { return m_ExitTimer; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       26.11.2019
        LAST CHANGE:   26.11.2019
        ***************************************************************************/
        private UserTimer m_ExitTimer;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       26.11.2019
        LAST CHANGE:   26.11.2019
        ***************************************************************************/
        public BigNote()
        {
            InitializeComponent();
            m_ExitTimer = new UserTimer();
        }

        /***************************************************************************
        SPECIFICATION: Exit timer handler
        CREATED:       26.11.2019
        LAST CHANGE:   21.08.2025
        ***************************************************************************/
        private void ExitTimeoutHandler( int Time )
        {
            Environment.Exit(1);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.11.2019
        LAST CHANGE:   21.08.2025
        ***************************************************************************/
        private void BigNote_Load(object sender,EventArgs e )
        {
            m_ExitTimer.m_eExpiredHandler += ExitTimeoutHandler;
            BringToFront();
            TopMost = true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.11.2019
        LAST CHANGE:   26.11.2019
        ***************************************************************************/
        private void BigNote_FormClosing(object sender,FormClosingEventArgs e)
        {
            m_ExitTimer .m_eExpiredHandler -= ExitTimeoutHandler;
        }
    }
}
