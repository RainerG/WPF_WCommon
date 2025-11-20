using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows;

using NS_Utilities;
using NS_AppConfig;
using NS_UserOut;

namespace NS_Trace
{
    /***************************************************************************
    SPECIFICATION: Global types 
    CREATED:       19.03.2016
    LAST CHANGE:   19.03.2016
    ***************************************************************************/
    public class TraceType
    {
        public Color  color;
        public int    level;
        public string text;

        public TraceType()
        :this("",0,Color.Black)
        {
        }

        public TraceType( string a_Text, int a_Level, Color a_Color )
        {
            text  = a_Text;
            level = a_Level;
            color = a_Color;
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
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public static bool Running  { get { return m_Running;   } }
        public static int  MaxLevel { set { m_MaxLevel = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       19.03.2016
        LAST CHANGE:   19.03.2016
        ***************************************************************************/
        private static List<TraceType> m_TraceLog;
        private static bool            m_Running;
        private static int             m_MaxLevel;
        private static UserTrace       m_Inst;
        private        TraceOutput     m_Out;

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public UserTrace()
        {
            m_TraceLog = new List<TraceType>();
            m_Out      = new TraceOutput();
            m_MaxLevel = 10;
            m_Inst     = null;
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
        CREATED:       20.03.2016
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {

            }
            else
            {

            }

            m_Out.Serialize( ref a_Conf );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.03.2016
        LAST CHANGE:   19.03.2016
        ***************************************************************************/
        public static void ResetLog()
        {
            m_TraceLog.Clear();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public static void Log( string text )            { Log( text, 0, Color.Black ); }
        public static void Log( string text, int level ) { Log( text, level, Color.Black ); }
        public static void Log( string text, int level, Color color )
        {
            if ( ! m_Running ) return;
            if ( level > m_MaxLevel ) return;
            m_TraceLog.Add( new TraceType( text,level,color ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public static void Start()              { m_Running = true;  }
        public static void Stop ()              { m_Running = false; }
        public static void Enable( bool enab)   { m_Running = enab;  }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public bool ShowTrace()
        {
            if (m_TraceLog.Count == 0) return false;

            foreach( TraceType tt in m_TraceLog )
            {
                if (tt.level > m_MaxLevel) continue;

                string space = "";
                for (int i=0; i<tt.level; i++) space += "    ";
                string txt = tt.text.Replace("\n","");
                m_Out.ShowOutput( space + txt, tt.color, false, true );
            }

            m_TraceLog.Clear();

            m_Out.Refresh();
            return true;
        }
    }
}
