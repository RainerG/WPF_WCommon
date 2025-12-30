using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NS_AppConfig;
using NS_WUtilities;

namespace NS_UserColor
{
    public partial class UserColorPicker:UserControl
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       21.05.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public Color  color  { get { return textBoxColor.BackColor; } set { textBoxColor.BackColor = value; } } 
        public bool   bold   { get { return checkBoxBold.Checked;   } set { checkBoxBold.Checked   = value; } } 
        public bool   ital   { get { return checkBoxItal.Checked;   } set { checkBoxItal.Checked   = value; } } 
        public string text   { get { return labelCol.Text;          } set { labelCol.Text          = value; } }  


        /***************************************************************************
        SPECIFICATION: C'tors 
        CREATED:       21.05.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public UserColorPicker()
        {
            InitializeComponent();
            this.textBoxColor.Click += new System.EventHandler( this.textBoxColor_Click );
        }

        public UserColorPicker( bool a_ShowBold )
            :this()
        {
            if ( ! a_ShowBold )
            {
                checkBoxBold.Hide();
                checkBoxItal.Hide();
            }
        }

        public UserColorPicker( string a_Text, Color a_Col, bool a_Bold, bool a_Ital, bool a_HasBold, Size a_Size, int X, int Y )
            :this()
        {
            textBoxColor.BackColor = a_Col;
            checkBoxBold.Checked   = a_Bold;
            checkBoxItal.Checked   = a_Ital;
            labelCol.Text          = a_Text;
            Size                   = a_Size;
            Location               = new Point(X,Y);

            if ( ! a_HasBold )
            {
                checkBoxBold.Hide();
                checkBoxItal.Hide();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.05.2016
        LAST CHANGE:   18.11.2024
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                textBoxColor.BackColor = a_Conf.Deserialize<Color>();
                checkBoxBold.Checked   = a_Conf.Deserialize<bool>();
                checkBoxItal.Checked   = a_Conf.Deserialize<bool>();
            }
            else
            {
                a_Conf.Serialize( textBoxColor.BackColor );
                a_Conf.Serialize( checkBoxBold.Checked   );
                a_Conf.Serialize( checkBoxItal.Checked   );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.05.2016
        LAST CHANGE:   21.05.2016
        ***************************************************************************/
        private void textBoxColor_Click ( object sender, EventArgs e ) 
        { 
            textBoxColor.BackColor = WUtils.PickColor( textBoxColor.BackColor );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public void ShowBoxes( bool a_Show )
        {
            if ( a_Show )
            {
                checkBoxBold.Show();
                checkBoxItal.Show();
            }
            else
            {
                checkBoxBold.Hide();
                checkBoxItal.Hide();
            }
        }

    } // class
} // namespace
