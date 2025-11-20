using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

using NS_Trace;

namespace NS_Utilities  
{
    public class UserTimer
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       18.09.2018
        LAST CHANGE:   02.04.2024
        ***************************************************************************/
        public bool running { get { return Running() && ! Expired(); } }
        public int  time    { get { return m_iTime; } set { m_iTime = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       11.09.2013
        LAST CHANGE:   21.03.2016
        ***************************************************************************/
        private const uint  TRACELVL = (uint)TrcLvl.TL_Timer;
        private       Color TRACECOL = Color.Orange;

        public delegate void dl_ExpiredHandler( int iTime );
        public event dl_ExpiredHandler m_eExpiredHandler;

        private System.Threading.Timer m_Timer;
        private bool                   m_bExpired;
        private bool                   m_bRunning;
        private int                    m_iTime;
        private string                 m_Name;
        
        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       11.09.2013
        LAST CHANGE:   17.10.2022
        ***************************************************************************/
        public UserTimer()
        {
            TimerCallback tcb = new TimerCallback( Timer_Tick );
            m_Timer = new System.Threading.Timer( tcb );
            m_bExpired = false;
            m_bRunning = false;
            m_Name     = "Timer";
        }
    
        public UserTimer( string a_Name, int a_iTime = -1 )
            :this()
        {
            if ( a_iTime != -1 ) m_iTime = a_iTime;
            m_Name = a_Name;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.09.2013
        LAST CHANGE:   08.07.2019
        ***************************************************************************/
        private void Timer_Tick( Object tObj )
        {
            bool ret = false;

            lock(this) 
            {
                if ( ! m_bRunning ) ret = true;
                m_bExpired = true;
                m_bRunning = false;
            }

            if (ret) return;

            UserTrace.Log( string.Format("{1} expired ({0} ms)", m_iTime, m_Name), TRACELVL, TRACECOL );

            if ( m_eExpiredHandler != null ) m_eExpiredHandler( m_iTime );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.09.2013
        LAST CHANGE:   21.08.2019
        ***************************************************************************/
        public void Start( int a_iMilliSec )
        {
            lock(this)
            {
                if (a_iMilliSec == 0) return;

                m_bExpired = false;
                m_bRunning = true;
                m_iTime    = a_iMilliSec;
                m_Timer.Change( a_iMilliSec, Timeout.Infinite );
            }

            UserTrace.Log( string.Format("{1} started ({0} ms)", m_iTime, m_Name ),TRACELVL, TRACECOL );
        }   


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.09.2015
        LAST CHANGE:   05.06.2023
        ***************************************************************************/
        public void Restart( bool a_OnlyIfRunning = false )
        {
            if ( a_OnlyIfRunning )
            {
                if (! Running()) return;
            }

            lock(this)
            {
                m_bExpired = false;
                m_bRunning = true;
                m_Timer.Change( m_iTime, Timeout.Infinite );
            }

            UserTrace.Log( string.Format("{1} restarted ({0} ms)", m_iTime, m_Name ), TRACELVL, TRACECOL );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.09.2013
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public void Stop()
        {
            UserTrace.Log( string.Format("{1} stopped ({0} ms)", m_iTime, m_Name ), TRACELVL, TRACECOL );

            lock(this) 
            {
                m_bRunning = false;
                m_bExpired = false;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.09.2013
        LAST CHANGE:   22.11.2022
        ***************************************************************************/
        public bool Expired()
        {
            bool ret;

            lock(this) ret = m_bExpired;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.09.2013
        LAST CHANGE:   12.09.2013
        ***************************************************************************/
        public bool Running()
        {
            bool ret;

            lock( this ) ret = m_bRunning;

            return ret;
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       22.07.2020
    LAST CHANGE:   22.07.2020
    ***************************************************************************/
    public class UserTickTimer
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       22.07.2020
        LAST CHANGE:   22.07.2020
        ***************************************************************************/
        public long Ticks { set { m_Ticks = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       22.07.2020
        LAST CHANGE:   20.04.2023
        ***************************************************************************/
        private long      m_Ticks;
        private Stopwatch m_StpWtch;

        /***************************************************************************
        SPECIFICATION: C'tor
                       10000 ticks are 1 ms
        CREATED:       22.07.2020
        LAST CHANGE:   22.07.2020
        ***************************************************************************/
        public UserTickTimer( long a_NrTicks )
        {
            m_Ticks = a_NrTicks;

            m_StpWtch = new Stopwatch();
            m_StpWtch.Restart();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.07.2020
        LAST CHANGE:   14.04.2023
        ***************************************************************************/
        public bool Expired()
        {
            if ( m_Ticks == 0 )         return true;
            if ( m_Ticks <= Measure() ) return true;
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.07.2020
        LAST CHANGE:   18.04.2023
        ***************************************************************************/
        public long Measure()
        {
            return m_StpWtch.ElapsedTicks;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.07.2020
        LAST CHANGE:   20.04.2023
        ***************************************************************************/
        public void Restart()
        {
            m_StpWtch.Restart();
        }
    }
}
