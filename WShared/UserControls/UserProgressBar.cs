using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace NS_ProgrssBar
{
    /***************************************************************************
    SPECIFICATION: Extended progress bar
    CREATED:       09.02.2023
    LAST CHANGE:   09.02.2023
    ***************************************************************************/
    public partial class UserProgressBar:UserControl
    {
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.12.2024
        LAST CHANGE:   04.02.2025
        ***************************************************************************/
        public int Val { get { return progressBar.Value; } set { progressBar.Value = value; } } 
        public int Max { get { return m_ProgMax; }         set { m_ProgMax = value; ShowProgr( 0 ); } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       09.02.2023
        LAST CHANGE:   09.02.2023
        ***************************************************************************/

        private DateTime m_PrgSTime;
        private LongTP   m_TP;
        private uint     m_PctMem;
        private TimeSpan m_TS;
        private TimeSpan m_ET;

        private int m_ProgMax;
        private int m_PercMem;

        public UserProgressBar()
        {
            InitializeComponent();
        }

        /***************************************************************************
        SPECIFICATION: Value between 0 and 100.
        CREATED:       09.02.2023
        LAST CHANGE:   09.02.2023
        ***************************************************************************/
        private delegate void dl_ShowVal( UInt32 a_Percent );
        public void ShowVal( int a_Percent ) { ShowVal( (uint) a_Percent ); }
        public void ShowVal( UInt32 a_Percent )
        {
            try
            {
                if ( this.InvokeRequired )
                {
                    dl_ShowVal d = new dl_ShowVal(ShowVal);
                    this.Invoke( d, new object[]{ a_Percent } );
                }
                else
                {
                    if (a_Percent > 100) return;
                    if (a_Percent == 0)
                    {
                        m_PrgSTime = DateTime.Now; 
                        m_TP       = new LongTP();
                        m_PctMem   = 0;
                        m_TS       = new TimeSpan();
                        m_ET       = new TimeSpan();
                    }
                    else
                    {
                        if ( a_Percent != m_PctMem )
                        {
                            m_PctMem = a_Percent;
                            DateTime now = DateTime.Now;
                            m_TS = now - m_PrgSTime;
                            long tks = m_TS.Ticks;
                            long est = tks * 100L / a_Percent;
                            m_ET = TimeSpan.FromTicks( est );
                        }
                    }
    
                    //Debug.WriteLine( "ShowProg: " + a_Percent.ToString() );

                    progressBar.Value = (int)a_Percent;
                    if (a_Percent == 0)
                    {
                        labelPB0.Visible = false;
                        labelPB1.Visible = false;
                        labelPB2.Visible = false;
                    }
                    else
                    {
                        labelPB0.Visible = true;
                        labelPB1.Visible = true;
                        labelPB2.Visible = true;
                        labelPB0.Text    = string.Format ( "{0:00}:{1:00}", m_TS.Minutes, m_TS.Seconds );
                        labelPB1.Text    = string.Format ( "{0}%"         , a_Percent );
                        labelPB2.Text    = string.Format ( "{0:00}:{1:00}", m_ET.Minutes, m_ET.Seconds );
                    }
                }
            }
            catch( Exception ex )
            {
                // do nothing
            }
        }

        /***************************************************************************
        SPECIFICATION: Shows value related to m_ProgMax (Max). Set Max before !
        CREATED:       04.02.2025
        LAST CHANGE:   04.02.2025
        ***************************************************************************/
        public void ShowProgr( int a_Percent )
        {
            if (a_Percent == 0)
            {
                m_PercMem = 0;
                Val = 0;
                ShowVal( 0 );
                Update();
                return;
            }

            int perc = a_Percent * 100 / m_ProgMax;
            if ( perc - m_PercMem >= 2 || m_ProgMax < 100 )
            {
                ShowVal( perc );
                Update();
                m_PercMem = perc;
            }
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       16.01.2023
    LAST CHANGE:   16.01.2023
    ***************************************************************************/
    public class LongTP
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       16.01.2023
        LAST CHANGE:   16.01.2023
        ***************************************************************************/
        private const int LEN = 5;

        private long[] vals;
        private int    idx;
        private bool   init;    
        private long   sum;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       16.01.2023
        LAST CHANGE:   16.01.2023
        ***************************************************************************/
        public LongTP()
        {
            vals = new long[LEN];
            Init();
        }

        public void Init()
        {
            idx  = 0;
            sum  = 0;
            init = true;
        }

        public void IncIdx( )
        {
            if ( ++idx >= LEN) idx = 0;
        }

        public long Filter( long val )
        {
            if (init)
            {
                init = false;
                for( int i=0; i<LEN; i++)
                {
                    vals[i] = val;
                    sum    += val;
                }
            }

            vals[idx] = val;
            sum += val;
            IncIdx();
            sum -= vals[idx];

            return sum / LEN;
        }
    }

} // namespace
