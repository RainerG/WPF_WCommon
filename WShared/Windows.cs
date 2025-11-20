//using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

//using NS_UserList;
using NS_UserOut;

namespace NS_Utilities
{
    /***************************************************************************
    SPECIFICATION: Windows convenience functions
    CREATED:       05.02.2020
    LAST CHANGE:   05.02.2020
    ***************************************************************************/
    public class Windows
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       05.02.2020
        LAST CHANGE:   28.04.2021
        ***************************************************************************/
        public List<Window> Wins { get { return m_Wins; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       05.02.2020
        LAST CHANGE:   28.04.2025
        ***************************************************************************/
        private Form          m_Parent;
        private List<Window>  m_Wins;
        private Point         m_Loc;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       05.02.2020
        LAST CHANGE:   05.02.2020
        ***************************************************************************/
        public Windows( Form a_Parent )
        {
            m_Parent = a_Parent;
            m_Loc    = a_Parent.Location;
            m_Wins   = new List<Window>();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.02.2020
        LAST CHANGE:   28.04.2021
        ***************************************************************************/
        public void Rearrange()
        {
            RearrangeWins( m_Parent );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.02.2020
        LAST CHANGE:   05.02.2020
        ***************************************************************************/
        public void LocationChanged( bool a_StickWnds )
        {
            Point loc = m_Parent.Location;
            Point dif = new Point(loc.X - m_Loc.X, loc.Y - m_Loc.Y);
            m_Loc = loc;

            if ( ! a_StickWnds ) return;

            foreach ( Window dlg in m_Wins ) Utils.MoveLocation( dlg.Cntrl, dif );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.02.2020
        LAST CHANGE:   05.02.2020
        ***************************************************************************/
        public void AllWinsToFront()
        {
            foreach ( Window dlg in m_Wins ) dlg.Cntrl.BringToFront();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.11.2019
        LAST CHANGE:   26.06.2025
        ***************************************************************************/
        const int XOFF = 7;

        public void RearrangeWins( Form a_Dlg )
        {
            FormWindowState ws = a_Dlg.WindowState;

            if ( ws == FormWindowState.Maximized )
            {
                a_Dlg.WindowState = FormWindowState.Normal;
                a_Dlg.Width = a_Dlg.MinimumSize.Width;
            }

            List<Window> wns  = m_Wins.FindAll( w => w.Cntrl.Visible );

            List<Window> wins = new List<Window>( wns );
            //wins.AddRange( wns );

            foreach ( Window w in wins )
            {
                Control ctl = w.Cntrl;

                if ( ((Form)ctl).WindowState == FormWindowState.Maximized ) ((Form)ctl).WindowState = FormWindowState.Normal;


                if ( ctl.Name.Contains( "List" ) || ctl.Text.Contains("List") )
                {
                     ((UserOutList)(ctl)).SetOptimalWidth();
                }

                if ( ctl.Name.Contains( "Text") || ctl.Text.Contains("Text") )
                {
                     ((UserOutText)(ctl)).SetOptimalWidth();
                }
            }

            //foreach( Window w in wns ) wins.Add( new Window(w) );

            Rectangle srct = Screen.PrimaryScreen.WorkingArea;
            int swdth = srct.Width;
            int shght = srct.Height;

            Point loc = a_Dlg.Location;

            // on first or second screen
            int orgx = 0;
            int orgy = 0;
            if ( loc.X + XOFF >= srct.Width )
            {
                orgx  = srct.Width;
                Screen scn = Screen.AllScreens[1];
                srct  = scn.WorkingArea;
                swdth = srct.Width;
                shght = srct.Height;
            }

            // size of main dialog
            int dwdth = a_Dlg.Width;    
            int dhght = a_Dlg.Height;

            // starting y coordinates for left and right side
            int outyl = srct.Y + dhght;
            int outyr = srct.Y;

            a_Dlg.Location = new Point( srct.X, srct.Y );

            // sort all windows by width and push the smaller to left side unless they are defined as left.
            wins.Sort( new WindowComparer() );

            List<Window> rgtwins = wins.FindAll( w => !w.Left );
            List<Window> lftwins = wins.FindAll( w =>  w.Left );

            int rgts = rgtwins.Count ;
            int lfts = lftwins.Count ;

            // push the rest of the rights to left if more than half of the wins
            for ( int i = lfts; i < wins.Count; i++ )
            {
                if ( i < wins.Count / 2 ) wins[i].Left = true;
                else                      wins[i].Left = false;
            }

            rgtwins = wins.FindAll( w => !w.Left );
            lftwins = wins.FindAll( w =>  w.Left );

            rgts = rgtwins.Count ;
            lfts = lftwins.Count ;

            List<Window> lftwnfx = wins.FindAll( w => w.Left &&  w.FixHght );
            List<Window> lftwnvr = wins.FindAll( w => w.Left && !w.FixHght );

            foreach( Window wf in lftwnfx ) { if ( dwdth < wf.Cntrl.MinimumSize.Width ) dwdth = wf.Cntrl.Width; }

            List<Window> rgtwnfx = wins.FindAll( w => !w.Left &&  w.FixHght );
            List<Window> rgtwnvr = wins.FindAll( w => !w.Left && !w.FixHght );


            int lr = 1;
            int wl,wr,hl,hr;

            switch( rgtwins.Count + lftwnvr.Count )
            {
                case 1: 
                    wl   = dwdth;
                    wr   = swdth - dwdth;
                    if (lfts == 0) rgts = 1;
                    break;

                default:
                    if ( lftwnvr.Count > 0)
                    {
                        wl = srct.Width / 2;
                        wr = wl;
                    }
                    else
                    {
                        wl = dwdth;
                        wr = swdth - dwdth;
                    }
                    rgts = rgtwins.Count;
                    lfts = lftwnvr.Count;
                    break;                    
            }

            if ( rgts == 0 && lftwnfx.Count == 0) return;

            hr = rgts == 0 ? 0 : shght / rgts;
            hl = 0; 

            // all fix heigths on left side first
            int wlmx = dwdth;
            foreach( Window w in lftwnfx )
            {
                hl = w.Cntrl.Height;
                w.Cntrl.Location = new Point( orgx, outyl );
                if ( ! w.FixWdth ) w.Cntrl.Size = new Size ( dwdth, hl );
                outyl += hl;

                wl = w.Cntrl.Size.Width;
                if (wlmx < wl) wlmx = wl;
            }

            if (lftwnvr.Count > 0) hl = ( srct.Height - outyl + srct.Y ) / lftwnvr.Count;

            foreach( Window w in lftwnvr )
            {
                wl = w.Cntrl.Size.Width < srct.Width / 2 ? w.Cntrl.Size.Width : srct.Width;

                if (wlmx < wl) wlmx = wl;

                w.Cntrl.Location = new Point( orgx, outyl );
                w.Cntrl.Size     = new Size ( wl, hl );
                outyl += hl;
            }

            // all fix heigths on right side first
            foreach( Window w in rgtwnfx )
            {
                wr = w.Cntrl.Size.Width < (srct.Width - wlmx) ? w.Cntrl.Size.Width : srct.Width - wlmx;

                hr = w.Cntrl.Height;
                w.Cntrl.Location = new Point( orgx + wlmx, outyr );
                if ( ! w.FixWdth ) w.Cntrl.Size = new Size ( wr, hr );
                outyr += hr;
            }

            if ( rgtwnvr.Count > 0 ) hr = ( srct.Height - outyr ) / rgtwnvr.Count;

            foreach( Window w in rgtwnvr )
            {
                wr = w.Cntrl.Size.Width < (srct.Width - wlmx) ? w.Cntrl.Size.Width : srct.Width - wlmx;

                w.Cntrl.Location = new Point ( orgx + wlmx, outyr );
                w.Cntrl.Size     = new Size  ( wr, hr );
                outyr += hr;
            }

            a_Dlg.BringToFront();
            AllWinsToFront();
        }


    } // class

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       28.04.2021
    LAST CHANGE:   28.04.2021
    ***************************************************************************/
    public class Window
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       28.04.2021
        LAST CHANGE:   28.04.2025
        ***************************************************************************/
        public Control Cntrl   { get { return m_Cntrl; } }
        public bool    Left    { get { return m_Left; } set { m_Left = value; } }
        public bool    FixHght { get { return m_FixHeight; } }
        public bool    FixWdth { get { return m_FixWidth;  } }
        public bool    Sizable { get { return !m_FixWidth && !m_FixHeight; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       28.04.2021
        LAST CHANGE:   28.04.2025
        ***************************************************************************/
        private Control m_Cntrl    ;
        private bool    m_Left     ;
        private bool    m_FixWidth ;
        private bool    m_FixHeight;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       28.04.2021
        LAST CHANGE:   28.04.2025
        ***************************************************************************/
        public Window( Control a_Cntrl, bool a_Left = false )
        {
            m_Cntrl         = a_Cntrl;
            m_Left          = a_Left;
            CtorSub();
        }

        public Window( Window a_Src )
        {
            m_Cntrl     = a_Src.Cntrl;
            m_Left      = a_Src.Left;
            CtorSub();
        }

        private void CtorSub()
        {
            m_FixWidth  = ( ( m_Cntrl.MaximumSize.Width  - m_Cntrl.MinimumSize.Width ) == 0 ); 
            m_FixHeight = ( ( m_Cntrl.MaximumSize.Height - m_Cntrl.MinimumSize.Height) == 0 );
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       27.06.2025
    LAST CHANGE:   27.06.2025
    ***************************************************************************/
    public class WindowComparer: IComparer<Window>
    {
        public int Compare( Window x, Window y )
        {
            if ( x.Cntrl.Width < y.Cntrl.Width ) return -1;
            if ( x.Cntrl.Width > y.Cntrl.Width ) return 1;
            return 0; // X == y
        }
    }


} // namespace

