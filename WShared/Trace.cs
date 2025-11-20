using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
        {
            color = Color.Black;
            level = 0;
            text  = "";
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       19.03.2016
    LAST CHANGE:   19.03.2016
    ***************************************************************************/
    public class Trace
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       19.03.2016
        LAST CHANGE:   19.03.2016
        ***************************************************************************/
        private static List<TraceType> m_TraceLog;

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.03.2016
        LAST CHANGE:   19.03.2016
        ***************************************************************************/
        public static void ResetLog()
        {
            m_TraceLog.Clear();
        }
    }
}
