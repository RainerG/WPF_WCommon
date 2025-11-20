using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows;

using NS_Utilities;
using NS_AppConfig;
using NS_UserOut;
using NS_ProgressDlg;

namespace NS_Trace
{
    /***************************************************************************
    SPECIFICATION: Global types 
    CREATED:       19.03.2016
    LAST CHANGE:   13.04.2023
    ***************************************************************************/
    public enum  TraceReturn
    {
        TR_Success,
        TR_Abort,
        TR_NoLevels,
        TR_NrOf
    }

    public class TraceType
    {
        public Color    color;
        public uint     level;
        public string   text;
        public bool     stofln;
        public DateTime time;
        public TimeSpan dtime;
        public bool     pure;

        public TraceType()
        :this(DateTime.Now,new TimeSpan(),"",0,Color.Black,true,false)
        {
        }

        public TraceType( DateTime a_Time, TimeSpan a_DTime, string a_Text, uint a_Level, Color a_Color, bool a_StOfLn, bool a_Pure )
        {
            time   = a_Time;
            dtime  = a_DTime;
            text   = a_Text;
            level  = a_Level;
            color  = a_Color;
            stofln = a_StOfLn;
            pure   = a_Pure;
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       19.03.2016
    LAST CHANGE:   19.03.2016
    ***************************************************************************/
    public class UserTrace
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       20.03.2016
        LAST CHANGE:   14.01.2025
        ***************************************************************************/
        public static bool Running      { get { return m_Running; } set { m_Running = value; } }
        public static bool IsEmpty      { get { return (m_TraceLog.Count == 0); } }
        public static bool AutoIndent   { set { m_AutoIndt     = value; } }
        public static bool AutoClear    { set { m_AutoClear    = value; } }
        public static bool LineNumbers  { set { m_LineNumbers  = value; } }
        public static uint TrcLevel     { set { m_TrcLevel     = value; } }
        public static uint NrStopLines  { set { m_NrStopLines  = value; } }
        public static bool StpAftrLines { set { m_StpAftrLines = value; } }
        public static bool OnlyLstLines { set { m_OnlyLstLines = value; } }
        public static bool Pure         { set { m_Pure         = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       19.03.2016
        LAST CHANGE:   25.02.2021
        ***************************************************************************/
        public delegate void dl_TraceEnable ( bool a_Enab );
        public static event dl_TraceEnable m_eTraceEnable;

        private static ConcurrentQueue<TraceType> m_TraceLog;
        private static bool         m_Running;
        private static bool         m_AutoIndt;
        private static bool         m_AutoClear;
        private static bool         m_LineNumbers;
        private static bool         m_ForcedStop;
        private static bool         m_Showing;
        private static uint         m_TrcLevel;
        private static bool         m_StpAftrLines;
        private static bool         m_OnlyLstLines;
        private static uint         m_NrStopLines;    
        private static UserTrace    m_Inst;
        private static Mutex        m_Mutex;
        private static bool         m_StOfLn;
        private static bool         m_Pure;
        private static ProgressDlg  m_Progress;
        private static TraceOutput  m_Out;
        private static Thread       m_ProgThread;
        private static string       m_FuncMem;
        private static DateTime     m_LastTime;        
        
        // just flags
        public  static int          IdleCnt;
        private static uint         m_LineCnt;


        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       20.03.2016
        LAST CHANGE:   25.02.2021
        ***************************************************************************/
        public UserTrace()
        {
            m_TraceLog    = new ConcurrentQueue<TraceType>();
            m_Out         = new TraceOutput();
            m_Progress    = new ProgressDlg();
            m_Mutex       = new Mutex();
            m_FuncMem     = "";
            m_ForcedStop  = false;
            m_Showing     = false;
            m_Inst        = null;
            m_StOfLn      = true;
            m_Pure        = false;
            m_LineCnt     = 0;
            IdleCnt       = 0;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public static UserTrace GetInst()
        {
            if( m_Inst == null )
            {
                m_Inst = new UserTrace();
            }

            return m_Inst;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.09.2018
        LAST CHANGE:   19.09.2018
        ***************************************************************************/
        public static void DecIdleCnt()
        {
            if (IdleCnt > 0) IdleCnt--;
            else             IdleCnt = 0;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   12.03.2019
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
            }
            else
            {
            }

            m_Out.Serialize     ( ref a_Conf );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.03.2016
        LAST CHANGE:   19.03.2016
        ***************************************************************************/
        public static void ResetLog()
        {
            TraceType trty;
            foreach ( TraceType tt in m_TraceLog ) m_TraceLog.TryDequeue( out trty );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   13.04.2023
        ***************************************************************************/
        public static void Log( string text, TrcLvl level )                         { Log( text, (uint)level, Color.Black ); }
        public static void Log( string text, TrcLvl level, Color color )            { Log( text, (uint)level, color, true ); }
        public static void Log( string text, TrcLvl level, Color color, bool Lf )   { Log( text, (uint)level, color, Lf ); }

        public static void Log( string text )                               { Log( text, (uint)0, Color.Black ); }
        public static void Log( string text, uint level )                   { Log( text, level, Color.Black ); }
        public static void Log( string text, uint level, Color color )      { Log( text, level, color, true ); }
        public static void Log( string text, uint level, Color color, bool Lf )
        {
            if ( ! m_Running ) return;

            bool lf = Lf;

            if ( (level & m_TrcLevel) == 0 ) return;
            if ( text == "" ) return;
            if ( -1 != text.IndexOf("\n") ) lf = true;
            if ( lf )
            {
                text += "\n";
                if ( m_StpAftrLines && m_LineCnt++ >= m_NrStopLines )
                {
                    Stop();
                    m_eTraceEnable(false);
                    GetInst().ShowTrace();                    
                    return;
                }

                if ( m_OnlyLstLines && m_LineCnt++ >= m_NrStopLines )
                {
                    TraceType trty;
                    do
                    {
                        m_TraceLog.TryDequeue(out trty);
                    } while( m_TraceLog.Count > m_NrStopLines );
                }
            }

            // inserting the timestamp
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt - m_LastTime;
            m_LastTime  = dt;

            if (m_TraceLog == null) return;

            m_TraceLog.Enqueue( new TraceType( dt, ts, text, level, color, m_StOfLn, m_Pure ) );

            if ( lf ) m_StOfLn = true;
            else      m_StOfLn = false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   12.12.2016
        ***************************************************************************/
        public static void Start()              
        { 
            UserTimer timeout = new UserTimer("Forced stop timeout tmr");
            timeout.Start(3000);

            if (m_Showing)
            {
                m_ForcedStop = true;
                while ( m_ForcedStop )
                {
                    Thread.Sleep(0);
                    if (timeout.Expired()) break;
                }
            }

            m_Running = true;  
            m_LineCnt = 0;
            if (m_AutoClear) m_Out.Clear();
        }
        public static void Stop ()             { m_Running = false; m_Progress.Stop(); }
        public static void Enable( bool enab)  { if (enab) Start(); else Stop(); }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       31.03.2016
        LAST CHANGE:   12.12.2016
        ***************************************************************************/
        private void StartProgress( int cnt )
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(StartProgressThrd);

            m_ProgThread = new Thread( ts );
            m_ProgThread.Start( cnt );
        }

        private void StartProgressThrd( object cnt )
        {
            do
            {
                m_Progress.Start("Trace progress");
                Thread.Sleep(100);
            } while( ! m_Progress.Running );

            m_Progress.ShowOutput("Trace output in operation ...\n",Color.Blue);
            m_Progress.ShowOutput("Be patient !\n",Color.Orange);
            m_Progress.RelProgr = 0;
            m_Progress.MaxProgr = (int)cnt;
            //m_Progress.ShowProgress();

            while( m_Progress.Running )
            {
                m_Progress.UpdateProgress();
                Thread.Sleep(100);
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   17.05.2023
        ***************************************************************************/
        private const int MAXCNT = 5000;

        public TraceReturn ShowTrace( bool a_OpenOnly = false )
        {
            if (a_OpenOnly)
            {
                m_Out.Refresh();
                return TraceReturn.TR_Success;
            }

            int  cnt  = m_TraceLog.Count;

            if (cnt == 0) return TraceReturn.TR_NoLevels;

            if ( cnt > MAXCNT )
            {
                StartProgress( cnt );
            }

            lock(m_TraceLog)
            {
                int linenr  = 0;
                m_Showing   = true;

                foreach( TraceType tt in m_TraceLog )
                {
                    string txt = "";

                    if (tt.stofln && ! tt.pure )
                    {
                        if (m_LineNumbers) txt = string.Format("{0:000000}  ",linenr);
 
                        if (m_AutoIndt)
                        {
                            uint ident = Utils.Ld(tt.level);
                            for (uint i=0; i<ident; i++) txt += "  ";
                        }
                    }

                    txt += tt.text;

                    if (! tt.pure )
                    {
                        string ts = string.Format( "{0:00}:{1:00}:{2:00},{3:000} {4,6:0.000} ", tt.time.Hour,tt.time.Minute,tt.time.Second,tt.time.Millisecond, tt.dtime.TotalMilliseconds );
                        txt = ts + txt;
                    }

                    m_Out.ShowOutput( txt, tt.color, false );

                    if (m_ForcedStop) break;

                    if ( cnt > MAXCNT )
                    {
                        if (m_Progress != null) 
                        {
                            m_Progress.RelProgr = linenr;
                            if ( m_Progress.Aborted ) 
                            {
                                AbortThread();
                                return TraceReturn.TR_Abort;
                            }
                        }
                    }
                    linenr++;
                }

                ResetLog();
            }

            AbortThread();
            m_Out.Deselect();
            m_Out.Refresh();
            m_ForcedStop = false;
            m_Showing    = false;
            return TraceReturn.TR_Success;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.12.2018
        LAST CHANGE:   06.12.2018
        ***************************************************************************/
        private void AbortThread()
        {
            if (m_Progress   != null)   m_Progress.CloseExt();
            if (m_ProgThread != null)   
            {
                m_ProgThread.Abort();
                m_ProgThread = null;
            }
            ResetLog();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.03.2016
        LAST CHANGE:   24.03.2016
        ***************************************************************************/
        public static void Close()
        {
            m_ForcedStop = true;
            m_Progress.Stop();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.12.2016
        LAST CHANGE:   17.06.2025
        ***************************************************************************/
        public static void Trace( object a_Class, string a_Func, Color a_Col, bool a_OmitMult = false )
        {
            if( ! Running ) return;
            bool omit = false;

            if ( a_OmitMult )
            {
                if ( a_Func == m_FuncMem ) return;
                m_FuncMem = a_Func;
                omit      = true;
            }
            string clss = a_Class.ToString();
            List<string> segs = Utils.SplitExt(clss,".,");
            clss = segs[1];
            Log( clss +"."+ a_Func + (omit ? " ( Omitting further outputs )" : "" ), TrcLvl.TL_Function, a_Col );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.01.2018
        LAST CHANGE:   03.01.2018
        ***************************************************************************/
        public static void Trace( string a_Class, string a_Func, Color a_Col )
        {
            if( ! Running ) return;
            Log( a_Class +"."+ a_Func, TrcLvl.TL_Function, a_Col );
        }

    } // class
} // namespace
