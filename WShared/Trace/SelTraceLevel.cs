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

namespace NS_Trace
{
    /***************************************************************************
    SPECIFICATION: Global types 
    CREATED:       21.03.2016
    LAST CHANGE:   23.08.2021
    ***************************************************************************/
    public enum TrcLvl : uint
    {
        TL_Function    = 0x00000001,
        TL_State       = 0x00000002,
        TL_SubState    = 0x00000004,
        TL_Timer       = 0x00000008,
        TL_SuperProt   = 0x00000010,
        TL_SuperProtPl = 0x00000020,
        TL_Protocol    = 0x00000040,
        TL_ProtocolPl  = 0x00000080,
        TL_ProtocolPl2 = 0x00000100,
        TL_Output      = 0x00000200,
        TL_Comment     = 0x00000400,
        TL_Max         = 0x000007FF
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       21.03.2016
    LAST CHANGE:   21.03.2016
    ***************************************************************************/
    public partial class SelTraceLevel:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       21.03.2016
        LAST CHANGE:   21.03.2016
        ***************************************************************************/
        public uint   TraceLvl   { get { return Utils.Hex2UInt(userCmbTraceLvl.Text); } }
        public string TraceLevel { get { return userCmbTraceLvl.Text; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       21.03.2016
        LAST CHANGE:   21.03.2016
        ***************************************************************************/
        private uint m_TraceLvl;
        private bool m_Chbx;

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2016
        LAST CHANGE:   23.08.2021
        ***************************************************************************/
        public SelTraceLevel()
        {
            InitializeComponent();

            checkBox1 .Text = "Functions";
            checkBox2 .Text = "States";
            checkBox3 .Text = "Sub states";
            checkBox4 .Text = "Timers";
            checkBox5 .Text = "Superior protocol";
            checkBox6 .Text = "Sup. prot. payload";
            checkBox7 .Text = "Lower protocol";
            checkBox8 .Text = "Lower prot. payload";
            checkBox9 .Text = "Lower prot. payload 2";
            checkBox10.Text = "Output" ;
            checkBox11.Text = "Comment";

            checkBox1 .Checked = true;
            checkBox2 .Checked = true;
            checkBox3 .Checked = true;
            checkBox4 .Checked = true;
            checkBox5 .Checked = true;
            checkBox6 .Checked = true;
            checkBox7 .Checked = true;
            checkBox8 .Checked = true;
            checkBox9 .Checked = true;
            checkBox10.Checked = true;
            checkBox11.Checked = true;

            m_TraceLvl = (uint)TrcLvl.TL_Max;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2016
        LAST CHANGE:   21.03.2016
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if ( a_Conf.IsReading )
            {
                Size sz = this.Size;
                a_Conf.DeserializeDialog( this );
                this.Size = sz;
            }
            else
            {
                a_Conf.SerializeDialog( this );
            }

            userCmbTraceLvl.Serialize( ref a_Conf );

            if ( a_Conf.IsReading )
            {
                m_TraceLvl = Utils.Hex2UInt( userCmbTraceLvl.Text );
                SetCheckBoxes();
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2016
        LAST CHANGE:   23.08.2021
        ***************************************************************************/
        private void CheckTraceLvl( uint mask, ref CheckBox chbx )
        {
            m_Chbx = true;

            if (chbx.Checked) m_TraceLvl |=  mask;
            else              m_TraceLvl &= ~mask;

            userCmbTraceLvl.Text = string.Format( "{0:X8}",m_TraceLvl );
        }

        private void checkBox1_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x001, ref checkBox1  ); }
        private void checkBox2_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x002, ref checkBox2  ); }
        private void checkBox3_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x004, ref checkBox3  ); }
        private void checkBox4_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x008, ref checkBox4  ); }
        private void checkBox5_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x010, ref checkBox5  ); }
        private void checkBox6_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x020, ref checkBox6  ); }
        private void checkBox7_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x040, ref checkBox7  ); }
        private void checkBox8_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x080, ref checkBox8  ); }
        private void checkBox9_CheckedChanged ( object sender, EventArgs e ) { CheckTraceLvl( 0x100, ref checkBox9  ); }
        private void checkBox10_CheckedChanged( object sender, EventArgs e ) { CheckTraceLvl( 0x200, ref checkBox10 ); }
        private void checkBox11_CheckedChanged( object sender, EventArgs e ) { CheckTraceLvl( 0x400, ref checkBox11 ); }

        private void SetCheckBoxes()
        {
            m_Chbx = true;
            checkBox1 .Checked = ( (m_TraceLvl & 0x001) != 0 );
            checkBox2 .Checked = ( (m_TraceLvl & 0x002) != 0 );
            checkBox3 .Checked = ( (m_TraceLvl & 0x004) != 0 );
            checkBox4 .Checked = ( (m_TraceLvl & 0x008) != 0 );
            checkBox5 .Checked = ( (m_TraceLvl & 0x010) != 0 );
            checkBox6 .Checked = ( (m_TraceLvl & 0x020) != 0 );
            checkBox7 .Checked = ( (m_TraceLvl & 0x040) != 0 );
            checkBox8 .Checked = ( (m_TraceLvl & 0x080) != 0 );
            checkBox9 .Checked = ( (m_TraceLvl & 0x100) != 0 );
            checkBox10.Checked = ( (m_TraceLvl & 0x200) != 0 );
            checkBox11.Checked = ( (m_TraceLvl & 0x400) != 0 );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2016
        LAST CHANGE:   23.08.2021
        ***************************************************************************/
        private void userCmbTraceLvl_TextChanged( object sender, EventArgs e )
        {
            if (m_Chbx) 
            {
                m_Chbx = false;
                return;
            }

            m_TraceLvl = (uint)Utils.Hex2UInt(userCmbTraceLvl.Text);
            if ( m_TraceLvl > (uint)TrcLvl.TL_Max ) 
            {
                m_TraceLvl = (uint)TrcLvl.TL_Max;
                userCmbTraceLvl.Text = string.Format("{0:X8}", TrcLvl.TL_Max );
            }
            SetCheckBoxes();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2016
        LAST CHANGE:   23.08.2021
        ***************************************************************************/
        private void btnSelAll_Click( object sender, EventArgs e )
        {
            m_TraceLvl = (uint)TrcLvl.TL_Max;
            SetCheckBoxes();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2016
        LAST CHANGE:   21.03.2016
        ***************************************************************************/
        private void btnClrAll_Click( object sender, EventArgs e )
        {
            m_TraceLvl = 0;
            SetCheckBoxes();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2016
        LAST CHANGE:   21.03.2016
        ***************************************************************************/
        private void SelTraceLevel_FormClosing( object sender, FormClosingEventArgs e )
        {
            userCmbTraceLvl.AddTextEntry();

            e.Cancel = true;
            this.Hide();
        }
    }
}
