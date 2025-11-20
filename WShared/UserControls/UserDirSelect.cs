using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NS_Utilities;
using NS_AppConfig;

namespace NS_UserCombo
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       27.09.2015
    LAST CHANGE:   27.09.2015
    ***************************************************************************/
    public partial class UserDirSelect:UserControl
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       27.09.2015
        LAST CHANGE:   27.09.2015
        ***************************************************************************/
        public string       Text  { get { return fileComboUDS.Text; } set { fileComboUDS.Text = value; } }
        public FileComboBox Combo { get { return fileComboUDS; } }

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       27.09.2015
        LAST CHANGE:   27.09.2015
        ***************************************************************************/
        public UserDirSelect()
        {
            InitializeComponent();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.09.2015
        LAST CHANGE:   27.09.2015
        ***************************************************************************/
        public void Serialize(ref AppSettings a_Conf)
        {
            if( a_Conf.IsReading )
            {
            }
            else
            {
            }

            fileComboUDS.Serialize( ref a_Conf );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.09.2015
        LAST CHANGE:   27.09.2015
        ***************************************************************************/
        private void btnUDSBrowse_Click(object sender,EventArgs e)
        {
            fileComboUDS.BrowseFolder();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.09.2015
        LAST CHANGE:   27.09.2015
        ***************************************************************************/
        private void UserDirSelect_Leave(object sender,EventArgs e)
        {
            fileComboUDS.AddTextEntry();
        }
    }
}
