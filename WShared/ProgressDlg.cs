using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using NS_AppConfig;
using NS_WUtilities;

namespace NS_ProgressDlg
{
    /***************************************************************************
    SPECIFICATION: Globals
    CREATED:       31.03.2016
    LAST CHANGE:   31.03.2016
    ***************************************************************************/
    public partial class ProgressDlg:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       30.06.2015
        LAST CHANGE:   09.02.2023
        ***************************************************************************/
        public int  MaxProgr  { set { m_MaxNr   = value; } }
        public int  RelProgr  { set { m_CurrNr  = value; } }
        public bool Running   { set { m_Running = value; } get { return m_Running; } }
        public bool Aborted   { get { return m_Aborted; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       30.06.2015
        LAST CHANGE:   06.12.2018
        ***************************************************************************/
        private int         m_MaxNr;
        private int         m_CurrNr;
        private bool        m_Running;
        private bool        m_Aborted;
        private DateTime    m_StartTime;
        //private UserTimer   m_UpdtTimer;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       30.06.2015
        LAST CHANGE:   06.12.2018
        ***************************************************************************/
        public ProgressDlg()
        {
            InitializeComponent();
            this.TopMost = true;
            m_Running    = false;
            Clear();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.06.2015
        LAST CHANGE:   06.12.2018
        ***************************************************************************/
        public void Clear()
        {
            richTextBoxOut.Clear();
            usrPrgrssBar.ShowVal( 0 );
            Application.DoEvents();
            ClearInt();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.03.2016
        LAST CHANGE:   30.03.2016
        ***************************************************************************/
        public void DisableAbort()
        {
            btnAbort.Enabled = false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.06.2015
        LAST CHANGE:   30.06.2015
        ***************************************************************************/
        public void IncProgr()
        {
            m_CurrNr++;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.06.2015
        LAST CHANGE:   01.07.2015
        ***************************************************************************/
        private delegate void dl_ShowOutput   ( string a_Text, Color a_Color, bool a_Bold );
        public void ShowOutput( string a_Text, Color a_Color ) { ShowOutput( a_Text, a_Color, false ); }
        public void ShowOutput( string a_Text, Color a_Color, bool a_Bold )
        {
            if ( this.InvokeRequired )
            {
                dl_ShowOutput d = new dl_ShowOutput(ShowOutput);
                this.Invoke(d, new object[] { a_Text, a_Color, a_Bold });
            }
            else
            {
                Font fnt = new Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //Font fnt = richTextBoxOut.SelectionFont;

                if (a_Bold) fnt = new Font(fnt, FontStyle.Bold );

                richTextBoxOut.SelectionColor = a_Color;
                richTextBoxOut.SelectionFont  = fnt;
                richTextBoxOut.SelectionStart = richTextBoxOut.Text.Length;
                richTextBoxOut.SelectedText   = a_Text;
                richTextBoxOut.ScrollToCaret();
                Update();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.12.2016
        LAST CHANGE:   11.12.2016
        ***************************************************************************/
        private void ProgressDlg_Load( object sender, EventArgs e )
        {
            //m_UpdtTimer.m_eExpiredHandler += UpdateTimerHandler;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.07.2015
        LAST CHANGE:   11.12.2016
        ***************************************************************************/
        private void ProgressDlg_FormClosing( object sender, FormClosingEventArgs e )
        {
            e.Cancel = true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.12.2016
        LAST CHANGE:   09.02.2023
        ***************************************************************************/
        public delegate void dl_UpdateProgress( );
        //public event dl_UpdateProgress ev_UpdateProgress;
        public void UpdateProgress( )
        {
            if (this.InvokeRequired)
            {
                dl_UpdateProgress d = new dl_UpdateProgress(UpdateProgress);
                this.Invoke( d, new object[]{} );
            }
            else
            {
                int pcv = m_CurrNr * 100 / m_MaxNr;
                usrPrgrssBar.ShowVal( pcv );

                Application.DoEvents();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.06.2015
        LAST CHANGE:   25.03.2022
        ***************************************************************************/
        private delegate void dl_Stop ();
        public void Stop()
        {
            try
            {
                if ( this.InvokeRequired )
                {
                    dl_Stop d = new dl_Stop(Stop);
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    ClearInt();
                    Application.DoEvents();
                    m_Running = false;
                    Hide();
                }
            }
            catch (System.Exception)
            {
                // do nothing
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.06.2015
        LAST CHANGE:   07.12.2018
        ***************************************************************************/
        private delegate void dl_Start( string a_Text );
        public void Start() { Start(""); }
        public void Start( string a_Text )
        {
            try
            {
                if ( this.InvokeRequired )
                {
                    dl_Start d = new dl_Start( Start );
                    this.Invoke( d, new object[]{a_Text});
                }
                else
                {
                    Clear();
                    m_Running   = true;
                    m_StartTime = DateTime.Now;
                    this.Text   = a_Text;
                    this.Show();
                    this.BringToFront();
                    Application.DoEvents();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show( ex.Message, "Exception in Start of ProgressDlg");
                // do nothing
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.07.2015
        LAST CHANGE:   01.07.2015
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                a_Conf.DeserializeDialog( this );
            }
            else
            {
                a_Conf.SerializeDialog( this );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       31.03.2016
        LAST CHANGE:   31.03.2016
        ***************************************************************************/
        private delegate void dl_Refresh();
        public void Refresh()
        {
            try
            {
                if( this.InvokeRequired )
                {
                    dl_Refresh d = new dl_Refresh( Refresh );
                    this.Invoke( d, new object[]{} );
                }
                else
                {
                    this.Update();
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Exception in Refresh" );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.07.2015
        LAST CHANGE:   02.07.2015
        ***************************************************************************/
        private void btnAbort_Click( object sender, EventArgs e )
        {
            Application.DoEvents();
            while ( m_Running ) Stop();
            m_Aborted = true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.03.2016
        LAST CHANGE:   30.03.2016
        ***************************************************************************/
        public void CloseExt()
        {
            ClearInt();
            while ( m_Running ) Stop();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.12.2018
        LAST CHANGE:   06.12.2018
        ***************************************************************************/
        public void ClearInt()
        {
            m_CurrNr  = 0;
            m_MaxNr   = 0;
            m_Aborted = false;
            m_Running = false;
        }

    } // class

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
