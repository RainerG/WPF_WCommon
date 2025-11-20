using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NS_AppConfig;
using NS_Utilities;

namespace NS_UserTabControl 
{
#if false
    /***************************************************************************
    SPECIFICATION: UserTabPage class
    CREATED:       12.12.2013
    LAST CHANGE:   05.09.2016
    ***************************************************************************/
    public class UserTabPage : TabPage
    {
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.12.2013
        LAST CHANGE:   05.09.2016
        ***************************************************************************/
        public Color TabColor  { get { return m_TabColor;  } set { SetColor(value); } }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:      12.12.2013
        LAST CHANGE:  23.09.2016
        ***************************************************************************/
        private const int BORDER = 4;

        private Color m_TabColor;
        private Color m_TabColorActive;
        private Color m_TabColorInact;
        private bool  m_bColorChanged;
        
        private delegate void dl_Activate ( bool a_bActive );

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       12.12.2013
        LAST CHANGE:    9/23/2016
        ***************************************************************************/
        public UserTabPage()
            :base()
        {
            init();
        }

        public UserTabPage( string a_sTab )
            : base( a_sTab )
        {
            init();
        }

        private void init()
        {
            m_TabColor      = Color.Black;
            m_bColorChanged = false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       9/23/2016
        LAST CHANGE:   9/23/2016
        ***************************************************************************/

        public void SetColor( Color a_tCol )
        {
            m_TabColor      = a_tCol;
            m_bColorChanged = true;
            Invalidate();
        }

        public void SetActiveColor  ( Color a_tActCol ) { m_TabColorActive = a_tActCol; }
        public void SetInactiveColor( Color a_tActCol ) { m_TabColorInact  = a_tActCol; }

        public bool ColChanged()
        {
            bool ret = m_bColorChanged;
            m_bColorChanged = false;
            return ret;
        }

        public void Activate( bool a_bActive )
        {
            if ( this.InvokeRequired )
            {
                dl_Activate d = new dl_Activate( Activate );
                this.Invoke( d, new object[] { a_bActive } );
            }
            else 
            {
                if (a_bActive ) SetColor( m_TabColorActive );
                else            SetColor( m_TabColorInact );
                Invalidate();
            }
        }
    }

#endif

    /***************************************************************************
    SPECIFICATION: UserTabControl class
    CREATED:       12.12.2013
    LAST CHANGE:   12.12.2013
    ***************************************************************************/
    public class UserTabControl : TabControl
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       12.12.2013
        LAST CHANGE:   23.09.2016
        ***************************************************************************/
        private const int BORDER=4;

        //public List<UserTabPage> TabPages { get { return m_TabPages; } }
        //private List<UserTabPage> m_TabPages;


        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       12.12.2013
        LAST CHANGE:   07.11.2024
        ***************************************************************************/
        public UserTabControl()
        : base()
        {
            DrawMode  = TabDrawMode.Normal;
            DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.tabControl_DrawItem );       
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       9/23/2016
        LAST CHANGE:   9/23/2016
        ***************************************************************************/
        public void Adjust( Size sz )
        {
            Size = new Size( sz.Width-2*BORDER, sz.Height-2*BORDER );
            Location = new Point( BORDER, BORDER );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.12.2013
        LAST CHANGE:   07.11.2024
        ***************************************************************************/
        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tp = TabPages[e.Index];

            StringFormat sf = new StringFormat();
            sf.Alignment    = StringAlignment.Center;

            Font fnt = e.Font; // new Font( "Tahoma", 8.25f );

            Brush backBr = new SolidBrush ( Color.Transparent );
            Brush foreBr = new SolidBrush ( tp.Enabled ? Color.Black : Color.Gray );

            //e.Graphics.FillRectangle(backBr, e.Bounds);
            SizeF sz = e.Graphics.MeasureString( TabPages[e.Index].Text, fnt );

            e.Graphics.DrawString( tp.Text, fnt, foreBr, e.Bounds, sf );

            sf.Dispose();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.08.2015
        LAST CHANGE:   14.08.2015
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                 this.TabIndex = a_Conf.Deserialize<int>();

                 this.SelectTab( this.TabIndex );
            }
            else
            {
                a_Conf.Serialize( this.SelectedTab.TabIndex );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:      08.09.2016
        LAST CHANGE:  11.01.2017
        ***************************************************************************/
        public void SyncTabIndices()
        {
            for (int i=0; i<TabPages.Count; i++)
            {
                TabPage tp = TabPages[i];

                tp.TabIndex = i;
                Utils.InitCtrl(tp);
            }
        }
    }


}
