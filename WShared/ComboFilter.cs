using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NS_AppConfig;
using NS_WUtilities;

namespace NS_UserCombo
{
    /***************************************************************************
    SPECIFICATION: ComboBox filter dialog
    CREATED:       07.11.2019
    LAST CHANGE:   07.11.2019
    ***************************************************************************/
    public partial class ComboFilter:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       07.11.2019
        LAST CHANGE:   06.02.2023
        ***************************************************************************/
        public string Filters { get { return tbFilters.Text; } }
        public bool   Inverse { get { return cbInverted.Checked; } }
        public bool   HasFilt { get { return tbFilters.Text != ""; } }
        public bool   RegEx   { get { return cbRegEx.Checked; } }

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       07.11.2019
        LAST CHANGE:   07.11.2019
        ***************************************************************************/
        public ComboFilter()
        {
            InitializeComponent();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.11.2019
        LAST CHANGE:   18.11.2024
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                a_Conf.DeserializeDialog( this );
                tbFilters.Text     = a_Conf.Deserialize<string>();
                cbInverted.Checked = a_Conf.Deserialize<bool>();
                if ( a_Conf.DbVersion < 360 ) return;
                cbRegEx.Checked = a_Conf.Deserialize<bool>();
            }
            else
            {
                a_Conf.SerializeDialog  ( this );
                a_Conf.Serialize( tbFilters.Text );
                a_Conf.Serialize( cbInverted.Checked );
                a_Conf.Serialize( cbRegEx.Checked );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.11.2019
        LAST CHANGE:   08.11.2019
        ***************************************************************************/
        private void ComboFilter_FormClosing(object sender,FormClosingEventArgs e)
        {

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.11.2019
        LAST CHANGE:   08.11.2019
        ***************************************************************************/
        private void ComboFilter_Load(object sender,EventArgs e)
        {

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.11.2019
        LAST CHANGE:   08.11.2019
        ***************************************************************************/
        private void button1_Click(object sender,EventArgs e)
        {
            tbFilters.Text = "";
        }

    }
}
