using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NS_Utilities;

namespace NS_Utilities
{
    /***************************************************************************
    SPECIFICATION: For managing one or more network management telegrams
    CREATED:       19.08.2019
    LAST CHANGE:   23.07.2021
    ***************************************************************************/
    public class NwMngemnt
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       19.08.2019
        LAST CHANGE:   26.08.2023
        ***************************************************************************/
        public bool Enabled { get { return Enabld(); } }

        public List<string> Name   { set { EnterNames ( value ); } }
        public List<string> TxID   { set { EnterTxIDs ( value ); } }
        public List<string> Tlg    { set { EnterTlgs  ( value ); } }
        public List<string> FD     { set { EnterFDs   ( value ); } }
        public List<string> DLC    { set { EnterDLCs  ( value ); } }
        public List<string> Intvl  { set { EnterIntvls( value ); } }
        public List<string> Vlan   { set { EnterVlans ( value ); } }
        public List<string> IpAdr  { set { EnterIpAdrs( value ); } }
        public List<string> IpTx   { set { EnterIpTxs ( value ); } }
        public List<string> IpRx   { set { EnterIpRxs ( value ); } }
        public List<string> Port   { set { EnterPorts ( value ); } }
        public List<string> PrtRx  { set { EnterPrtRxs( value ); } }
        public List<string> PrtTx  { set { EnterPrtTxs( value ); } }
        public List<string> MacTx  { set { EnterMacTx ( value ); } }
        public string       Ifce   { get { return m_Ifce.Length < 2 ? "" : m_Ifce.ToLower().Substring(0,2); } }
        public string       Intfce { set { m_Ifce = value; } get { return m_Ifce; } }
        public List<NwMgmnt> Tlgs  { get { return m_NwMgmnts; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       19.08.2019
        LAST CHANGE:   29.03.2024
        ***************************************************************************/
        private List<NwMgmnt> m_NwMgmnts;
        //private NwMgmnt       m_TstPrsnt;
        private int           m_PllIdx;
        private int           m_PllSqIdx;
        private string        m_Ifce = "";

        private static int recurse;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       19.08.2019
        LAST CHANGE:   29.03.2024
        ***************************************************************************/
        public NwMngemnt()
        {
            m_NwMgmnts         = new List<NwMgmnt>();
            //m_TstPrsnt         = new NwMgmnt();
            //m_TstPrsnt.Name    = TSTPRSNTNM;
            //m_TstPrsnt.Comment = TSTPRSNTNM;
            m_PllIdx           = 0; 
            m_PllSqIdx         = 0; 
            recurse            = 0;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.04.2021
        LAST CHANGE:   01.04.2021
        ***************************************************************************/
        public void Copy( NwMngemnt a_Src )
        {
            m_NwMgmnts.Clear();
            m_NwMgmnts.AddRange( a_Src.m_NwMgmnts );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.08.2019
        LAST CHANGE:   01.04.2022
        ***************************************************************************/
        private void EnterNames  ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].Name  = a_Data[i]; }
        private void EnterTxIDs  ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].TxID  = a_Data[i]; }
        private void EnterTlgs   ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].sTlg  = a_Data[i]; }
        private void EnterFDs    ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].FD    = a_Data[i]; }
        private void EnterDLCs   ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].sDLC  = a_Data[i]; }
        private void EnterIntvls ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].Intvl = a_Data[i]; }
        private void EnterVlans  ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].Vlan  = a_Data[i]; }
        private void EnterIfce   ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].Ifce  = a_Data[i]; }
        private void EnterIpAdrs ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].IpAdr = a_Data[i]; }
        private void EnterIpTxs  ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].IpTx  = a_Data[i]; }
        private void EnterIpRxs  ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].IpRx  = a_Data[i]; }
        private void EnterPorts  ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].Port  = a_Data[i]; }
        private void EnterPrtRxs ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].PrtRx = a_Data[i]; }
        private void EnterPrtTxs ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].PrtTx = a_Data[i]; }
        private void EnterMacTx  ( List<string> a_Data ) {  ExtendArr( a_Data.Count );  for (int i=0; i<a_Data.Count; i++) m_NwMgmnts[i].MacTx = a_Data[i]; }

        private void ExtendArr( int a_Cnt )
        {
            int ncnt = m_NwMgmnts.Count;
            if ( a_Cnt > ncnt )
            {
                for ( int i=0; i < a_Cnt-ncnt; i++ )
                {
                    NwMgmnt nwm = new NwMgmnt();
                    m_NwMgmnts.Add(nwm);
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.10.2019
        LAST CHANGE:   04.02.2023
        ***************************************************************************/
        private bool Enabld()
        {
            lock( this )
            {
                try
                {
                    int cnt = m_NwMgmnts.Count;
                    if ( cnt == 0 ) return false;
                    if ( m_NwMgmnts    == null ) return false;
                    if ( m_NwMgmnts[0] == null ) return false;
                    if ( null != m_NwMgmnts.Find( tg => tg.Uds ) ) cnt--;

                    return cnt > 0;
                }
                catch( Exception )
                {
                    return false; 
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.08.2019
        LAST CHANGE:   31.01.2021
        ***************************************************************************/
        public void StartTmrs()
        {
            foreach( NwMgmnt nwm in m_NwMgmnts )
            {
                if( ! nwm.Enab )       continue;

                if( nwm.iIntvl == 0 )  nwm.Once = true;
                else                   nwm.Tmr.Start( nwm.iIntvl );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.08.2019
        LAST CHANGE:   19.08.2019
        ***************************************************************************/
        public void StopTmrs()
        {
            foreach( NwMgmnt nwm in m_NwMgmnts ) nwm.Tmr.Stop( );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.08.2019
        LAST CHANGE:   19.04.2024
        ***************************************************************************/
        public NwMgmnt Poll( string a_IgnIfce = "none" )
        {
            try
            {
                if (m_NwMgmnts.Count == 0 ) { return null; }

                int cnt = 0;
                lock(this) cnt = m_NwMgmnts.Count;
                int idx = 0;
                NwMgmnt nwm = null;

                do
                {
                    lock(this)
                    {
                        if ( ++m_PllIdx >= cnt ) m_PllIdx = 0; 
                        nwm = m_NwMgmnts[ m_PllIdx ];
                        if ( nwm == null ) return null;
                    }

                    if ( ! nwm.Enab ) continue;

                    if ( nwm.Ifce == a_IgnIfce && a_IgnIfce != "none" ) return null;  

                    if ( nwm.Once ) 
                    {
                        nwm.Once = false;
                        return nwm;
                    }

                    if ( nwm.Tmr.Expired() )
                    {
                        nwm.Tmr.Restart();
                        return nwm;
                    }

                } while( ++idx < cnt );
            }
            catch( Exception )
            {
                // do nothing 
            }

            return null;  // none found or none expired
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.02.2024
        LAST CHANGE:   22.02.2024
        ***************************************************************************/
        public NwMgmnt PollSeq( string a_Ifce = "et" )
        {
            try
            {
                int cnt = 0;
                lock(this) cnt = m_NwMgmnts.Count;

                if ( cnt == 0 ) { return null; }

                int idx = 0;
                NwMgmnt nwm = null;

                do
                {
                    lock(this)
                    {
                        nwm = m_NwMgmnts[ m_PllSqIdx ];
                        if ( nwm == null ) return null;
                    }

                    if ( ! nwm.Enab ) { IncPllIdx(cnt); continue; }

                    if ( nwm.Once ) 
                    {
                        nwm.Once = false;
                        IncPllIdx(cnt);
                        return nwm;
                    }

                    switch ( nwm.Ifce )
                    {
                        case "et":
                        case "ve": 
                            if (a_Ifce == "et") break;
                            IncPllIdx(cnt);
                            continue;

                        case "ca": 
                            if (a_Ifce == "ca") break;
                            IncPllIdx(cnt);
                            continue;

                        default  : 
                            IncPllIdx(cnt);
                            continue;
                    }

                    //if ( nwm.Name == m_TstPrsnt.Name && nwm.Comment == m_TstPrsnt.Comment && ! m_TstPrsnt.Enab ) { IncPllIdx(cnt); return null; }

                    if ( nwm.Tmr.Expired() )
                    { 
                        nwm.Tmr.Restart();
                        IncPllIdx(cnt);
                        if ( nwm.Sent ) continue;
                    }
                    else if ( ! nwm.Tmr.running )
                    {
                        nwm.Tmr.Restart();
                        IncPllIdx(cnt);
                    }

                    if ( ! nwm.Sent )
                    {
                        nwm.Sent = true;
                        return nwm;
                    }

                } while( ++idx < cnt );
            }
            catch( Exception )
            {
                // do nothing 
            }

            return null;  // none found or none expired
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.02.2024
        LAST CHANGE:   22.02.2024
        ***************************************************************************/
        private void IncPllIdx( int a_Max )
        {
            if ( ++m_PllSqIdx >= a_Max ) m_PllSqIdx = 0; 
            m_NwMgmnts[ m_PllSqIdx ].Sent = false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.07.2021
        LAST CHANGE:   23.07.2021
        ***************************************************************************/
        //public NwMgmnt PollTP()
        //{
        //    if ( m_TstPrsnt.Tmr.Expired() )
        //    {
        //        m_TstPrsnt.Tmr.Restart();
        //        return m_TstPrsnt;
        //    }

        //    return null;
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.09.2019
        LAST CHANGE:   27.09.2019
        ***************************************************************************/
        public NwMgmnt FirstTlg()
        {
            if (m_NwMgmnts.Count == 0 ) return null;

            return m_NwMgmnts[0];
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.10.2019
        LAST CHANGE:   29.03.2024
        ***************************************************************************/
        public void EnableTlg( string a_Name, bool a_Enab )
        {
            NwMgmnt nwm = m_NwMgmnts.Find( nw => nw.Name == a_Name );

            //switch( a_Name )
            //{
            //    case TSTPRSNTNM:
            //        nwm = m_TstPrsnt;
            //        AddTlg( nwm );
            //        break;

            //    default:
            //        nwm = m_NwMgmnts.Find( nw => nw.Name == a_Name );
            //        break;
            //}
            
            if (nwm == null) return;

            nwm.Enab = a_Enab;
            if (a_Enab) nwm.Tmr.Start( nwm.iIntvl );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.10.2019
        LAST CHANGE:   29.03.2024
        ***************************************************************************/
        public void AddTlg( NwMgmnt a_Tlg, bool a_Force = false )
        {
            if ( ! a_Force )
            {
                NwMgmnt tlg = m_NwMgmnts.Find( tg => ( tg.Comment == a_Tlg.Comment && tg.Name == a_Tlg.Name && tg.SeqNr == a_Tlg.SeqNr ) );
                if (tlg != null) m_NwMgmnts.Remove( tlg );
            }

            //if ( a_Tlg.Comment == TSTPRSNTNM ) m_TstPrsnt = a_Tlg;
            m_NwMgmnts.Add( a_Tlg );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.08.2019
        LAST CHANGE:   20.08.2019
        ***************************************************************************/
        public bool Running()
        {
            foreach( NwMgmnt nwm in m_NwMgmnts )
            {
                if ( nwm.Tmr.Running() ) return true;
            }
            return false;
        }
    }


    /***************************************************************************
    SPECIFICATION: Network management parameters
    CREATED:       02.07.2019
    LAST CHANGE:   02.07.2019
    ***************************************************************************/
    public class NwMgmnt
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       02.07.2019
        LAST CHANGE:   30.03.2022
        ***************************************************************************/
        public uint   iTxID   { get { return Utils.Str2UInt(TxID); } }
        public bool   bFD     { get { return Utils.Str2Bool(FD); } set { FD = value.ToString(); } }
        public int    iIntvl  { get { return Intvl == "none" ? 1000 : Utils.Str2Int(Intvl); } } 
        public string sTlg    { set { SetTlgs(value); } }
        public List<byte> Tlg { get { return GetTlg(); }  }

        public int    iPort   { get { return (int)Utils.Str2UInt(Port);  } }
        public int    iPrtRx  { get { return (int)Utils.Str2UInt(PrtRx); } }
        public int    iPrtTx  { get { return (int)Utils.Str2UInt(PrtTx); } }
        public int    iVlan   { get { return (int)Utils.Str2UInt(Vlan);  } }
        public string sIpAdr  { get { return IpAdr; } }
        public bool   Enab    { get { return m_Enab; } set { m_Enab = value; } }
        public string IpPrtTx { set { SetIpPortTx(value); } }
        public string IpPrtRx { set { SetIpPortRx(value); } }
        public string sPrtcl  { get { return Protcl.ToLower(); }   set { Protcl = value; } }
        public int    iDLC    { get { return Utils.Str2Int(Dlc); } set { Dlc = value.ToString(); } }
        public string sDLC    { get { return Dlc; } set { Dlc = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       02.07.2019
        LAST CHANGE:   28.05.2025
        ***************************************************************************/
        public  string  IpTx   ;
        public  string  IpRx   ;
        public  string  TxID   ;
        public  string  FD     ;
        private string  Dlc    ; 
        public  string  Intvl  ;
        public  string  Ifce   ;
        public  string  Name   ;
        public  string  Comment;
        public  int     SeqNr  ;
        public  int     Chan   ;

        public string   IpAdr ;
        public string   Port  ;
        public string   PrtRx ;
        public string   PrtTx ;
        public string   MacTx ;
        public string   Vlan  ;
        private string  Protcl;

        public List<byte> Data;

        public UserTimer Tmr ;
        public bool      Once;
        public bool      Uds ;
        public bool      Sent;

        private bool             m_Enab;
        private List<List<byte>> Tlgs;
        private int              Tidx;

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       02.07.2019
        LAST CHANGE:   07.05.2024
        ***************************************************************************/
        public NwMgmnt()
        {
            TxID    = "none";
            FD      = "none";
            Dlc     = "8";
            Intvl   = "none";
            Ifce    = "none";
            Name    = "NWM" ;
            Comment = ""    ;
            IpRx    = "none";
            IpTx    = "none";
            MacTx   = "none";
            IpAdr   = "none";
            Port    = "0";
            PrtRx   = "0";
            PrtTx   = "0";
            Vlan    = "0";
            Protcl  = "UDP";
            SeqNr   = -1;
            Tidx    = 0;
            Once    = false;
            Uds     = false;
            Sent    = false;
            m_Enab  = true;
            Tlgs    = new List<List<byte>>();
            Data    = new List<byte>();
            Tmr     = new UserTimer("NwMgt");
        }

        public NwMgmnt( NwMgmnt a_Src )
            :this()
        {
            TxID    = a_Src.TxID   ;
            FD      = a_Src.FD     ;
            Dlc     = a_Src.Dlc    ;
            Intvl   = a_Src.Intvl  ;
            Ifce    = a_Src.Ifce   ;
            Name    = a_Src.Name   ;
            Comment = a_Src.Comment;
            IpRx    = a_Src.IpRx   ;
            IpTx    = a_Src.IpTx   ;
            MacTx   = a_Src.MacTx  ;
            IpAdr   = a_Src.IpAdr  ;
            Port    = a_Src.Port   ;
            PrtRx   = a_Src.PrtRx  ;
            PrtTx   = a_Src.PrtTx  ;
            Vlan    = a_Src.Vlan   ;
            Protcl  = a_Src.Protcl ;
            SeqNr   = a_Src.SeqNr  ;
            Tidx    = a_Src.Tidx   ;
            Once    = a_Src.Once   ;
            Uds     = a_Src.Uds    ;
            Sent    = a_Src.Sent   ;
            m_Enab  = a_Src.m_Enab ;

            Tlgs.Clear(); Tlgs.AddRange( a_Src.Tlgs );
            Data.Clear(); Data.AddRange( a_Src.Data );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.08.2019
        LAST CHANGE:   16.03.2021
        ***************************************************************************/
        private List<byte> GetTlg()
        {
            if (Tlgs.Count == 0) Tlgs.Add( new List<byte>() );

            lock( this )
            {
                if ( Tidx >= Tlgs.Count ) Tidx = 0;
                return Tlgs[Tidx++];
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.08.2019
        LAST CHANGE:   20.08.2019
        ***************************************************************************/
        private void SetTlgs( string a_Data )
        {
            Tlgs.Clear();
            List<string> tlgs = Utils.SplitExt( a_Data, "," );

            foreach ( string tlg in tlgs )
            {
                List<byte> t = Utils.HexStr2ByteList( tlg );
                Tlgs.Add( t );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.10.2019
        LAST CHANGE:   02.10.2019
        ***************************************************************************/
        public void AddTlg( List<byte> a_Tlg )
        {
            Tlgs.Clear();
            Tlgs.Add( a_Tlg );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.04.2021
        LAST CHANGE:   12.02.2025
        ***************************************************************************/
        public bool SetIpPortTx( string a_IpPrt )
        {
            List<string> segs = Utils.ParseIpPort( a_IpPrt );
            IpTx  = segs[0];
            PrtTx = segs.Count>1 ? segs[1] : "0";
            return true;
        }
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.04.2021
        LAST CHANGE:   12.02.2025
        ***************************************************************************/
        public bool SetIpPortRx( string a_IpPrt )
        {
            List<string> segs = Utils.ParseIpPort( a_IpPrt );
            IpRx  = segs[0];
            PrtRx = segs.Count>1 ? segs[1] : "0";
            return true;
        }

    }

} // namespace
