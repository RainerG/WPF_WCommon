using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NS_Utilities;
using NS_AppConfig;
using NS_UserTabControl;

namespace NS_UserOut
{
    public partial class Preferences:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       06.09.2015
        MODIFIED:      06.09.2015
        ***************************************************************************/
        public Color ColBack1    { get { return textBoxBCol1.BackColor; } }
        public Color ColBack2    { get { return textBoxBCol2.BackColor; } }
        public Color ColFore1    { get { return textBoxFCol1.BackColor; } }
        public Color ColFore2    { get { return textBoxFCol2.BackColor; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       18.04.2013
        LAST CHANGE:   30.04.2013
        ***************************************************************************/

        
        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       18.04.2013
        LAST CHANGE:   07.09.2015
        ***************************************************************************/
        public Preferences ( )
        {
            InitializeComponent();

            textBoxFCol1.BackColor = Color.Blue;
            textBoxFCol2.BackColor = Color.FromArgb(  0,117,  0);
            textBoxBCol1.BackColor = Color.FromArgb(222,240,254);
            textBoxBCol2.BackColor = Color.FromArgb(236,255,236);
        }


        /***************************************************************************
        SPECIFICATION: Color selection handlers
        CREATED:       18.04.2013
        LAST CHANGE:   17.01.2014
        ***************************************************************************/
        private void textBoxColBack1_Click   ( object sender, EventArgs e ) { textBoxBCol1.BackColor   = Utils.PickColor( textBoxBCol1.BackColor );  }
        private void textBoxColBack2_Click   ( object sender, EventArgs e ) { textBoxBCol2.BackColor   = Utils.PickColor( textBoxBCol2.BackColor );  }
        private void textBoxColFore1_Click   ( object sender, EventArgs e ) { textBoxFCol1.BackColor   = Utils.PickColor( textBoxFCol1.BackColor );  }
        private void textBoxColFore2_Click   ( object sender, EventArgs e ) { textBoxFCol2.BackColor   = Utils.PickColor( textBoxFCol2.BackColor );  }

        private void buttonOK_Click ( object sender, EventArgs e )
        {
            Close();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.04.2013
        LAST CHANGE:   18.04.2013
        ***************************************************************************/
        private void buttonCancel_Click ( object sender, EventArgs e )
        {
            Close();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.04.2013
        LAST CHANGE:   29.01.2014
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if ( a_Conf.IsReading )
            {
                a_Conf.DeserializeDialog( this );
                tabControl.SelectedIndex = (int)a_Conf.Deserialize<int>();

                textBoxBCol1  .BackColor  = (Color) a_Conf.Deserialize<Color>();
                textBoxBCol2  .BackColor  = (Color) a_Conf.Deserialize<Color>();
                textBoxFCol1  .BackColor  = (Color) a_Conf.Deserialize<Color>();
                textBoxFCol2  .BackColor  = (Color) a_Conf.Deserialize<Color>();
            }
            else
            {
                a_Conf.SerializeDialog( this );
                a_Conf.Serialize( tabControl.SelectedIndex );

                a_Conf.Serialize( textBoxBCol1  .BackColor );
                a_Conf.Serialize( textBoxBCol2  .BackColor );
                a_Conf.Serialize( textBoxFCol1  .BackColor );
                a_Conf.Serialize( textBoxFCol2  .BackColor );
            }
        }

    }
}
