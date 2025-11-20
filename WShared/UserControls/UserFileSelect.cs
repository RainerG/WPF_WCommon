using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NS_UserCombo
{
    /***************************************************************************
    SPECIFICATION: Global types
    CREATED:       01.10.2015
    LAST CHANGE:   01.10.2015
    ***************************************************************************/
    public enum UF_MODES
    {
        UF_READ,
        UF_READEDIT,
        UF_WRITE,
        UF_WRITECRTE,
        UF_NROF
    }

    public partial class UserFileSelect:UserControl
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       09.03.2016
        LAST CHANGE:   09.03.2016
        ***************************************************************************/
        public string Name { set { groupBox.Text = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       01.10.2015
        LAST CHANGE:   01.10.2015
        ***************************************************************************/
        private UF_MODES m_Mode;
        //private string   m_Exts;

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.10.2015
        LAST CHANGE:   01.10.2015
        ***************************************************************************/
        public UserFileSelect( )
        {
            InitializeComponent();

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.10.2015
        LAST CHANGE:   01.10.2015
        ***************************************************************************/
        public void SetRWMode( UF_MODES a_Mode )
        {
            m_Mode = a_Mode;

            switch( a_Mode )
            {
                case UF_MODES.UF_READEDIT:
                    btnUFSCreateEdit.Text = "&Edit";
                    break;

                case UF_MODES.UF_READ:
                case UF_MODES.UF_WRITE:
                    btnUFSCreateEdit.Hide();
                    break;

                case UF_MODES.UF_WRITECRTE:
                    btnUFSCreateEdit.Text = "&Create";
                    break;

            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.10.2015
        LAST CHANGE:   01.10.2015
        ***************************************************************************/
        public void SetExtensions( string a_Extens )
        {

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.10.2015
        LAST CHANGE:   01.10.2015
        ***************************************************************************/
        private void btnUFSCreateEdit_Click( object sender, EventArgs e )
        {
            switch(m_Mode)
            {
                case UF_MODES.UF_READEDIT:
                    fileComboUFS.Edit();
                    break;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.10.2015
        LAST CHANGE:   01.10.2015
        ***************************************************************************/
        private void btnUFSBrowse_Click( object sender, EventArgs e )
        {
            switch(m_Mode)
            {
                case UF_MODES.UF_READ:
                case UF_MODES.UF_READEDIT:
                    fileComboUFS.BrowseFileRead();
                    break;
            }
        }
    }
}
