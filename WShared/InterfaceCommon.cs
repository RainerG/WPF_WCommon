using System; 
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Drawing;
using System.Diagnostics;

using NS_Trace;
using NS_AppConfig;
using NS_UserCombo;

namespace NS_Utilities
{
    /***************************************************************************
    SPECIFICATION: Global enums
    CREATED:       ?
    LAST CHANGE:   24.04.2024
    ***************************************************************************/
    public enum QIndex
    {
        QI_UDSCAN,
        QI_XCPCAN,
        QI_UDSETH,
        QI_XCPETH
    }

    public enum ERROR_CODES
    {
        ERR_DISCONNECT,
        ERR_CONNECT,
        ERR_RECEIVE,
        ERR_SEND,
        ERR_NROF
    }

    public enum PROT_IDS
    {
        PROT_XBL,
        PROT_UDS,
        PROT_XAPP,
        PROT_NROF
    }

    public enum ETHPROT : byte
    {
        EP_ICMP   = 1,
        EP_IGMPV3 = 2,
        EP_TCP    = 6,
        EP_UDP    = 17,
        EP_ICMPV6 = 58,
        EP_NROF   = 0xff
    }

    public enum TCPFLGS : byte
    {
        TF_CWR = 0x80,   
        TF_ECE = 0x40,  
        TF_URG = 0x20,  
        TF_ACK = 0x10,  
        TF_PSH = 0x08,   
        TF_RST = 0x04,   
        TF_SYN = 0x02,   
        TF_FIN = 0x01   
    };

    public enum TCPSTATES
    {
        TS_INIT,
        //TS_WAITARP,
        TS_WAITSYNACK,
        //TS_WAITPSHACK,
        TS_WAITRSPACK,
        //TS_WAITACK1,
        //TS_WAITACK2,
        TS_WAITFINACK,
        TS_ESTAB
    }


    /***************************************************************************
    SPECIFICATION: 
    CREATED:       ?
    LAST CHANGE:   06.01.2025
    ***************************************************************************/
    public class InterfaceParamsCommon
    {
        public  bool   Active { get { return bActive; } set { bActive = value; } }

        public  string  Name;
        private bool    bActive;
        public  int     iTimeout;
        public  string  sCommand;
        public  Color   tColCmd;
        public  Color   tColRsp;
        public  Color   tColEvt;
        public  Color   tColCon;
        public  Color   tColDis;

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       ?
        LAST CHANGE:   06.01.2025
        ***************************************************************************/
        public InterfaceParamsCommon()
        {
            bActive     = false;
            iTimeout    = 3000;
            Name        = "";
            sCommand    = "";
            tColCmd     = Color.White;
            tColRsp     = Color.White;
            tColEvt     = Color.White;
            tColCon     = Color.White;
            tColDis     = Color.White;
        }

        public InterfaceParamsCommon( string a_sName, string a_sCmd, Color a_tClCmd, Color a_tClRsp, Color a_tClEvt, Color a_tClCon, Color a_tClDis, int a_iTmo, bool a_bEnab )
        {
            bActive  = a_bEnab  ;
            Name     = a_sName  ;
            iTimeout = a_iTmo   ;
            sCommand = a_sCmd   ;
            tColCmd  = a_tClCmd ;
            tColRsp  = a_tClRsp ;
            tColEvt  = a_tClEvt ;
            tColCon  = a_tClCon ;
            tColDis  = a_tClDis ;
        }

        public void Set( InterfaceParamsCommon a_Params )
        {
            bActive  = a_Params.bActive  ;
            Name     = a_Params.Name     ;
            iTimeout = a_Params.iTimeout ;
            sCommand = a_Params.sCommand ;
            tColCmd  = a_Params.tColCmd  ;
            tColRsp  = a_Params.tColRsp  ;
            tColEvt  = a_Params.tColEvt  ;
            tColCon  = a_Params.tColCon  ;
            tColDis  = a_Params.tColDis  ;
        }
    }


    public class InterfaceCommon
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       26.04.2013
        LAST CHANGE:   06.01.2025
        ***************************************************************************/
        protected const long  TIMEOUTCNT  = 100000;
        protected const int   JOINTO      = 200;
        protected const uint  TRACELVL    = (uint)TrcLvl.TL_Protocol;
        protected const uint  TRACELVL_PL = (uint)TrcLvl.TL_ProtocolPl;
        protected       Color TRACECOL    = Color.CornflowerBlue;

        public string sCmdCount { get { return m_iCmdCount.ToString(); } }

        public int    iCmdCount  { get { return m_iCmdCount; } set { m_iCmdCount = value; } }
        public string sResponse  { get { return m_sResponse; } set { m_sResponse = value; } }
        public string sExpected  { get { return m_sExpected; } set { m_sExpected = value; } }
        public string Parent     { get { return m_Parent;    } set { m_Parent    = value; } }

        public ConcurrentQueue<string> tResponses  { get { return m_Responses; } }
        public ConcurrentQueue<string> tEvents     { get { return m_Events; } }

        public int        NrResponses  { get { return m_Responses.Count; } }
        public int        NrEvents     { get { return m_Events   .Count; } }
        public int        NrObjectsUC  { get { return m_ObjRespUC.Count; } }
        public int        NrObjectsXC  { get { return m_ObjRespXC.Count; } }
        public int        NrObjectsUE  { get { return m_ObjRespUE.Count; } }
        public int        NrObjectsXE  { get { return m_ObjRespXE.Count; } }
        public int        iIndex       { get { return m_iIfceIdx; } set { m_iIfceIdx = value; } }
        public int        Variant      { get { return m_Variant;  } set { m_Variant = value;  } }
        public List<byte> TcpOpts      { get { return m_TcpOpts;  } set { m_TcpOpts = value;  } }
        public string     sIndex       { get { return (m_iIfceIdx +1).ToString(); } }   
        public long       WriteDelay   { set { m_WriteDelay = value; } }

        public int        VChan1       { get { return m_VChan1; } set { m_VChan1 = value; } }
        public int        VChan2       { get { return m_VChan2; } set { m_VChan2 = value; } }
        public int        HwChan1      { get { return m_HwChan1; }  set { m_HwChan1 = value; } }
        public int        HwChan2      { get { return m_HwChan2; }  set { m_HwChan2 = value; } }

        public EthParams EthPrms  { set { AddEthPrms( value ); } get { return m_EthPrms; } }

        public UdsData  RData     { get { return m_RxTlg; } }  
        public UdsData  TData     { get { return m_TxTlg; } }  
        public UdsData  Data      { get { return m_Data; } }  
        public bool     Run       { set { m_bThreadRunning = value; } } 
        public bool     InclZero  { get { return m_InclZero; }  set { m_InclZero = value; } } 
        public bool     Connected { get { return m_bConnected && m_bThreadRunning; } set { m_bConnected = value; } }
        public bool     CnnctedRB { get { return m_bCnnctedRB; } set { m_bCnnctedRB = value; } }
        //public bool     CnnctedTP { get { return m_bCnnctedTP; } set { m_bCnnctedTP = value; } }
        public bool     BootLdr   { get { return m_BootLd; }     set { m_BootLd = value; } }   

        public bool     Multicast { get { return m_bMulticast; }  set { m_bMulticast = value; } }   

        public UserTimer RspTimer { get { return m_RespTimer; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       26.04.2013
        LAST CHANGE:   06.01.2025
        ***************************************************************************/
        protected       bool  m_bConnected;
        protected       bool  m_bCnnctedRB;
        protected       bool  m_bCnnctedTP;

        public delegate void  dl_ErrorHandler   ( int a_iIfceIdx, ERROR_CODES a_eErr, string a_Msg, string a_Title );
        public          event dl_ErrorHandler   m_eErrorHandler;

        public delegate void  dl_TimeoutHandler ( int a_iIfceIdx, int a_iTime );
        public          event dl_TimeoutHandler m_eTimeoutHandler;

        public delegate void  dl_OnTargetClose  ( );
        public          event dl_OnTargetClose  m_eTargetClose;

        private  string  m_Parent;  // for debugging only 

        protected Memory mem;

        protected Thread     m_RecThread;
        protected string     m_sResponse;
        protected string     m_sExpected;
        protected int        m_iCmdCount;
        protected int        m_iIfceIdx;
        protected bool       m_InclZero;
        protected bool       m_bMulticast;
        protected int        m_HwChan1;
        protected int        m_HwChan2;
        protected int        m_VChan1 ;
        protected int        m_VChan2 ;
        protected EthParams  m_EthPrms;
        protected int        m_Variant; // in order to handle different protocol variants
        protected List<byte> m_TcpOpts; 
        protected long       m_WriteDelay;

        protected static List<EthParams> m_EthPrmsLst = new List<EthParams>();
        protected        ConnectParms    m_ConnPrms;

        protected static bool m_bThreadRunning = false;
        //protected ManualResetEvent m_ThrdTerm;

        private   bool      m_BootLd;
        
        protected ConcurrentQueue<string>  m_Responses;
        protected ConcurrentQueue<string>  m_Events;
        protected ConcurrentQueue<byte>    m_RawResp1;
        protected ConcurrentQueue<byte>    m_RawResp2;
        // Must exist only once
        protected static ConcurrentQueue<object> m_ObjRespUC;   //UDSCAN
        protected static ConcurrentQueue<object> m_ObjRespXC;   //XCPCAN
        protected static ConcurrentQueue<object> m_ObjRespUE;   //UDSETH
        protected static ConcurrentQueue<object> m_ObjRespXE;   //XCPETH
        
        protected CriticalSection    m_CritSect ;
        protected UserTimer          m_RespTimer;

        protected UdsData m_TxTlg;
        protected UdsData m_RxTlg;
        protected UdsData m_Data ;

        /***************************************************************************
        SPECIFICATION: Interface for deriving class
        CREATED:       10.02.2021
        LAST CHANGE:   06.07.2025
        ***************************************************************************/
        public virtual bool Connect( ConnectParms a_CP )                   { return false; }
        public virtual void Disconnect( bool a_Force = false )             { }
        public virtual bool SendMessageT( List<byte> a_Msg, int a_Timeout = 0 ) { return false; }
        public virtual bool SendMessageU( List<byte> a_Msg, int a_Timeout = 0 ) { return false; }

        public virtual void InitComm( bool a_Force = false )  { }
        public virtual void OnClosing() { }
        public virtual void OnLoad()    { }
        public virtual void OnRespTimeout() { }

        //public virtual void Serialize( ref AppSettings a_Conf ) { }

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       10.08.2015
        LAST CHANGE:   06.01.2025
        ***************************************************************************/
        private void CtorSub()
        {
            mem              = new Memory();
            m_RecThread      = null;
            //m_bActive         = false;
            m_iCmdCount      = 0;
            m_VChan1         = 1;   // Vector CAN channel assignment
            m_VChan2         = 2;
            m_HwChan1        = 1;
            m_HwChan2        = 2;
            //m_iTimeout        = 3000;
            m_Responses      = new ConcurrentQueue<string>();
            m_Events         = new ConcurrentQueue<string>();
            m_RawResp1       = new ConcurrentQueue<byte>();
            m_RawResp2       = new ConcurrentQueue<byte>();
            m_ObjRespUC      = new ConcurrentQueue<object>();
            m_ObjRespXC      = new ConcurrentQueue<object>();
            m_ObjRespUE      = new ConcurrentQueue<object>();
            m_ObjRespXE      = new ConcurrentQueue<object>();
            m_InclZero       = false;
            m_CritSect       = new CriticalSection();
            m_bConnected     = false;
            m_EthPrms        = new EthParams();
            //m_EthPrmsLst     = new List<EthParams>();
            m_ConnPrms       = null;
            m_WriteDelay     = -1;

            m_RespTimer = new UserTimer("Interface timeout timer");
            m_RespTimer.m_eExpiredHandler += TimeoutHandler;

            m_TxTlg = new UdsData();
            m_RxTlg = new UdsData();
            m_Data  = new UdsData();

            Init();
        }


        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       26.04.2013
        LAST CHANGE:   12.05.2023
        ***************************************************************************/
        public InterfaceCommon ( int a_iIfceIdx )
        {
            CtorSub();
            m_iIfceIdx = a_iIfceIdx;
        }

        public InterfaceCommon ( )
        {
            CtorSub();
            m_iIfceIdx = 0;
        }

        public InterfaceCommon( InterfaceCommon a_Src )
        {
            CtorSub();
            Copy( a_Src );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.04.2024
        LAST CHANGE:   06.01.2025
        ***************************************************************************/
        public void Copy( InterfaceCommon a_Src )
        {
            m_iIfceIdx = a_Src.m_iIfceIdx;
            m_Parent   = a_Src.Parent;

            m_TxTlg.Copy( a_Src.TData );
            m_RxTlg.Copy( a_Src.RData );
            m_Data .Copy( a_Src.Data  );

            m_EthPrms.Copy( a_Src.m_EthPrms );

            m_sResponse   = a_Src.m_sResponse ;
            m_sExpected   = a_Src.m_sExpected ;
            m_iCmdCount   = a_Src.m_iCmdCount ;
            m_iIfceIdx    = a_Src.m_iIfceIdx  ;
            m_InclZero    = a_Src.m_InclZero  ;
            m_bMulticast  = a_Src.m_bMulticast;
            m_HwChan1     = a_Src.m_HwChan1   ;
            m_HwChan2     = a_Src.m_HwChan2   ;
            m_VChan1      = a_Src.m_VChan1    ;
            m_VChan2      = a_Src.m_VChan2    ;
            m_Variant     = a_Src.m_Variant   ;
            m_TcpOpts     = a_Src.m_TcpOpts   ;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   ?
        ***************************************************************************/
        public void Init ()
        {
            m_iCmdCount = 0;

            m_sResponse = "";
            m_sExpected = "";
            
            // replacement for clear command
            m_Responses = new ConcurrentQueue<string>();
            m_Events    = new ConcurrentQueue<string>();
        }

        public void AddResponse()
        {
            m_sResponse = m_sResponse.Replace("\r","");
            string[] rsp = m_sResponse.Split("\n".ToCharArray());

            foreach ( string r in rsp )
            {
                UserTrace.Log( "Enqueueing " + r, TRACELVL_PL, TRACECOL );
                lock (this) {  m_Responses.Enqueue( r ); }
                //m_Responses.Enqueue( r );
            }
            m_sResponse = "";
        }

        public void AddResponse( string a_sRsp )
        {
            lock (this) { m_Responses.Enqueue( a_sRsp ); }
            //m_Responses.Enqueue( a_sRsp );
        }

        public void AddEvent( string a_sEvt )
        {
            lock (this) { m_Events.Enqueue( a_sEvt ); }
            //m_Events.Enqueue( a_sEvt );
        }

        public string GetResponse()
        {
            string rsp;
            lock (this) { m_Responses.TryDequeue( out rsp ); }
            UserTrace.Log("Dequeued response: " + rsp , TRACELVL_PL, TRACECOL );
            return rsp;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.11.2015
        LAST CHANGE:   05.11.2022
        ***************************************************************************/
        public virtual bool Clear( bool a_Full = false )
        {
            Trace("Clear");

            m_RxTlg.Clear();
            m_TxTlg.Clear();
            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.05.2023
        LAST CHANGE:   13.05.2023
        ***************************************************************************/
        public void AddEthPrms( EthParams a_Prms )
        {
            m_EthPrms.Copy( a_Prms );
            EnterEthPrms();
        }

        public void EnterEthPrms()
        {
            EthParams pms = m_EthPrmsLst.Find( p => p.Prot == m_EthPrms.Prot );
            if ( pms == null ) m_EthPrmsLst.Add( new EthParams( m_EthPrms ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.08.2015
        LAST CHANGE:   17.11.2015
        ***************************************************************************/
        public void ClearRx() {  m_RxTlg.Clear(); }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.06.2016
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        public void AddRawResp( object a_Data, QIndex a_QIdx )
        {
            Trace( "AddRawResp(" + a_QIdx.ToString() + ") (object)" );
            switch( a_QIdx )
            {
                case QIndex.QI_UDSCAN: m_ObjRespUC.Enqueue( a_Data ); break;
                case QIndex.QI_XCPCAN: m_ObjRespXC.Enqueue( a_Data ); break;
                case QIndex.QI_UDSETH: m_ObjRespUE.Enqueue( a_Data ); break;
                case QIndex.QI_XCPETH: m_ObjRespXE.Enqueue( a_Data ); break;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.08.2015
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        public object GetRawResp( QIndex a_QIdx )
        {
            object ret = null;
            bool   ok  = false;

            switch( a_QIdx )
            {
                case QIndex.QI_UDSCAN: ok = m_ObjRespUC.TryDequeue( out ret ); break;
                case QIndex.QI_XCPCAN: ok = m_ObjRespXC.TryDequeue( out ret ); break;
                case QIndex.QI_UDSETH: ok = m_ObjRespUE.TryDequeue( out ret ); break;
                case QIndex.QI_XCPETH: ok = m_ObjRespXE.TryDequeue( out ret ); break;
            }
            if( !ok ) return null;

            Trace( "GetRawResp(" + a_QIdx.ToString() + ") (object)"  );
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.09.2013
        LAST CHANGE:   19.12.2013
        ***************************************************************************/
        public string ShowResponse ()
        {
            string rsp;
            m_Responses.TryPeek( out rsp );
            return rsp;
        }

        public string GetEvent()
        {
            string rsp;
            lock (this) { m_Events.TryDequeue( out rsp ); }
            //m_Events.TryDequeue( out rsp );
            UserTrace.Log( "Dequeued event: " + rsp, TRACELVL_PL, TRACECOL );
            return rsp;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.09.2013
        LAST CHANGE:   19.12.2013
        ***************************************************************************/
        public string ShowEvent ()
        {
            string rsp;
            lock (this) { m_Events.TryPeek( out rsp ); }
            //m_Events.TryPeek( out rsp );
            return rsp;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.09.2013
        LAST CHANGE:   18.06.2016
        ***************************************************************************/
        private void TimeoutHandler( int a_iTime )
        {
            if (m_eTimeoutHandler != null) m_eTimeoutHandler( m_iIfceIdx, a_iTime );       
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.03.2024
        LAST CHANGE:   02.04.2024
        ***************************************************************************/
        protected void OnTargClosedHdlr( )
        {
            if (m_eTargetClose != null) m_eTargetClose();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.09.2013
        LAST CHANGE:   06.12.2024
        ***************************************************************************/
        public bool Flush ( )
        {
            bool   ret = false;
            string trc = "";

            while( m_Responses .Count > 0 ) { trc = "RS"; m_Responses   = new ConcurrentQueue<string>(); Application.DoEvents(); ret = true; }
            while( m_Events    .Count > 0 ) { trc = "ES"; m_Events      = new ConcurrentQueue<string>(); Application.DoEvents(); ret = true; }
            while( m_RawResp1  .Count > 0 ) { trc = "R1"; m_RawResp1    = new ConcurrentQueue<byte>();   Application.DoEvents(); ret = true; }
            while( m_RawResp2  .Count > 0 ) { trc = "R2"; m_RawResp2    = new ConcurrentQueue<byte>();   Application.DoEvents(); ret = true; }
            while( m_ObjRespUC .Count > 0 ) { trc = "UC"; m_ObjRespUC   = new ConcurrentQueue<object>(); Application.DoEvents(); ret = true; }
            while( m_ObjRespXC .Count > 0 ) { trc = "XC"; m_ObjRespXC   = new ConcurrentQueue<object>(); Application.DoEvents(); ret = true; }
            while( m_ObjRespUE .Count > 0 ) { trc = "UE"; m_ObjRespUE   = new ConcurrentQueue<object>(); Application.DoEvents(); ret = true; }
            while( m_ObjRespXE .Count > 0 ) { trc = "XE"; m_ObjRespXE   = new ConcurrentQueue<object>(); Application.DoEvents(); ret = true; }

            if (ret) Trace( "Flush " + trc );

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.12.2013
        LAST CHANGE:   13.12.2013
        ***************************************************************************/
        public void SetRespTimeout( int a_iTimeout )
        {
            m_RespTimer.Start( a_iTimeout );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.04.2021
        LAST CHANGE:   01.04.2021
        ***************************************************************************/
        protected void ShowError( int a_IfceIdx, ERROR_CODES a_ErrCd, string a_ErrMsg, string a_Title )
        {
            Trace("ShowError");

            if ( m_eErrorHandler != null ) m_eErrorHandler( a_IfceIdx, a_ErrCd, a_ErrMsg, a_Title );
            else                           MessageBox.Show( a_ErrMsg, a_Title );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.02.2017
        LAST CHANGE:   07.08.2020
        ***************************************************************************/
        protected void Trace( string a_Func ) { UserTrace.Trace( this, a_Func, Color.Purple ); } 

    }  // class

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       15.03.2017
    LAST CHANGE:   18.11.2024
    ***************************************************************************/
    public class CanParams
    {
        private const string DEFDR  = "500000";
        private const string DEFCFG = DEFDR + ",127,32,32";

        public uint   iRxID    { get { return Utils.Str2CanID(RxID); } }
        public uint   iTxID    { get { return Utils.Str2CanID(TxID); } }
        public uint   iBaud    { get { return iAbr[0]; } }
                               
        public uint   iDLC     { get { return Utils.Str2UInt(DLC );  } }
        public string DLC      { get { return Dlc == "none" ? "8"     : Dlc;  } set { Dlc = value; } }
        public bool   bFD      { get { return Fd == "none" ?  false   : Utils.Str2Bool(Fd); } set { Fd = value.ToString(); } }
        public bool   FrmErrDt { get { return FrmErrDet; } set { FrmErrDet = value; } }
        public long iFdLat     { get { return Utils.Str2Long( FdLt ); } }
        public string FD       { get { return Fd == "none" ? "false"  : Fd; } set { Fd = value; } }
        public string CH       { get { return Ch == "none" ? "1" : Ch; } set { Ch = value; } }
        public int  iCH        { get { return Utils.Str2Int(CH); } }  
        public PROT_IDS  Prot  { get { return Prt; } set { Prt = value; } }
        public string ABR      { get { return Abr == "none" ? DEFDR + ",127,32,32" : Abr; } set { SetAbr( value ); } }
        public string DBR      { get { return Dbr == "none" ? DEFDR + ",127,32,32" : Dbr; } set { SetDbr( value ); } }
        public List<uint> iABR { get { return iAbr; } }
        public List<uint> iDBR { get { return iDbr; } }

        public  string     Name;
        public  string     RxID;
        public  string     TxID;
        private string     Dlc;
        private string     Fd;
        private string     FdLt;
        private string     Abr;
        private string     Dbr;
        private string     Ch;
        private PROT_IDS   Prt;
        private bool       FrmErrDet;

        private List<uint> iAbr;
        private List<uint> iDbr;

        public CanParams()
        {
            Name = "none";
            RxID = "0";
            TxID = "0";
            Dlc  = "8";
            Fd   = "none";
            FdLt = "15000";
            Ch   = "1";
            Abr  = DEFCFG; 
            Dbr  = DEFCFG; 
            Prt  = PROT_IDS.PROT_NROF;

            FrmErrDet = true;

            iAbr = new List<uint>();
            iDbr = new List<uint>();

            List<uint> br = new List<uint> { 500000, 127, 32, 32 };
            //br.Add(500000);
            //br.Add(127);
            //br.Add(32);
            //br.Add(32);
            iAbr.AddRange(br);
            iDbr.AddRange(br);
        }
        public CanParams( string a_Name )
            :this()
        {
            Name = a_Name;
        }

        public void Copy( CanParams a_Src, bool a_OvrWrt )
        {
            RxID = a_Src.RxID;
            TxID = a_Src.TxID;

            // copy data rates only
            iAbr[0] = a_Src.iABR[0];
            iDbr[0] = a_Src.iDBR[0];
            
            if ( a_OvrWrt )
            {
                // copy everything
                iAbr.Clear(); iAbr.AddRange( a_Src.iABR );
                iDbr.Clear(); iDbr.AddRange( a_Src.iDBR );
                Prt      = a_Src.Prt;
                CH       = a_Src.CH;
                FD       = a_Src.FD;
                Dlc      = a_Src.Dlc;
                FrmErrDt = a_Src.FrmErrDet;
            }
        }

        private void SetAbr( string a_Abr ) { Abr = a_Abr; SetBr( a_Abr, ref iAbr ); }
        private void SetDbr( string a_Dbr ) { Dbr = a_Dbr; SetBr( a_Dbr, ref iDbr ); }

        private void SetBr( string a_Br, ref List<uint> a_iBr )
        {
            List<uint> ret = new List<uint>();

            List<string> segs = Utils.SplitExt( a_Br,"," );

            if ( segs.Count < 2 ) return;

            a_iBr.Clear();

            foreach ( string seg in segs )
            {
                a_iBr.Add( Utils.Str2UInt( seg ) ); 
            }
        }

        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                Name    = a_Conf.Deserialize<string>();
                RxID    = a_Conf.Deserialize<string>();
                TxID    = a_Conf.Deserialize<string>();
                iAbr[0] = a_Conf.Deserialize<uint>();
                iDbr[0] = a_Conf.Deserialize<uint>();
                Dlc     = a_Conf.Deserialize<string>();
            }
            else
            {
                a_Conf.Serialize( Name );
                a_Conf.Serialize( RxID );
                a_Conf.Serialize( TxID );
                a_Conf.Serialize( iAbr[0] );
                a_Conf.Serialize( iDbr[0] );
                a_Conf.Serialize( Dlc );
            }
        }
    }  // CanParams 

    
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       15.03.2017
    LAST CHANGE:   26.10.2022
    ***************************************************************************/
    public class EthParams: InterfaceParamsCommon
    {
        public int      iRxPort { get { return (ushort)Utils.Str2Int( RxPort ); } set { RxPort = value.ToString(); } } 
        public int      iTxPort { get { return (ushort)Utils.Str2Int( TxPort ); } set { TxPort = value.ToString(); } } 
        public ushort   iVlan   { get { return (ushort)Utils.Str2UInt( Vlan ) ; } set { Vlan   = value.ToString(); } } 
        public string   Vlan    { get { return mVlan; } set { mVlan = value; } }
        public byte     iDoipV  { get { return (byte)  Utils.Str2Int( DoIP ); } } 
        public PROT_IDS Prot    { get { return Prt; } set { Prt = value; } }
        public string   Protcl  { get { return Prtcl.ToLower(); } set { Prtcl = value; } }
        public bool     IsIp6   { get {  return IsIP6(); } }

        public List<ushort> tRxAddr6 { get { return GetIp6( RxAddr ); } }
        public List<ushort> tTxAddr6 { get { return GetIp6( TxAddr ); } }
        public List<byte>   tRxAddr  { get { return GetIp ( RxAddr ); } }
        public List<byte>   tTxAddr  { get { return GetIp ( TxAddr ); } }
        public List<byte>   tRxMac   { get { return GetMac( RxMac ); } }
        public List<byte>   tTxMac   { get { return GetMac( TxMac ); } }

        public  string   RxMac  ;
        public  string   RxAddr ;
        public  string   RxPort ;
        public  string   TxMac  ;
        public  string   TxAddr ;
        public  string   TxPort ;
        private string   mVlan  ;
        public  string   DoIP   ;
        private PROT_IDS Prt    ; 
        private string   Prtcl  ;

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       19.10.2020
        LAST CHANGE:   06.01.2025
        ***************************************************************************/
        public EthParams()
        {
            Name    = "none";
            RxMac   = "none";
            RxAddr  = "none";
            RxPort  = "0";
            TxMac   = "none";
            TxAddr  = "none";
            TxPort  = "0";      
            DoIP    = "0";
            mVlan   = "0";
            Prtcl   = "none";
            Prot    = PROT_IDS.PROT_NROF;
        }

        public EthParams( string a_Name )
            :this()
        {
            Name = a_Name;
        }

        public EthParams( EthParams a_Src )
        {
            Name   = a_Src.Name   ;
            RxMac  = a_Src.RxMac  ;
            RxAddr = a_Src.RxAddr ;
            RxPort = a_Src.RxPort ;
            TxMac  = a_Src.TxMac  ;
            TxAddr = a_Src.TxAddr ;
            TxPort = a_Src.TxPort ;
            DoIP   = a_Src.DoIP   ;
            mVlan  = a_Src.mVlan  ;
            Prtcl  = a_Src.Prtcl  ;
            Prot   = a_Src.Prot   ;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.03.2017
        LAST CHANGE:   06.01.2025
        ***************************************************************************/

        public void Copy( EthParams a_Src, bool a_OvrWrt = true )
        {
            Name   = a_Src.Name   ;
            RxAddr = a_Src.RxAddr ;
            RxPort = a_Src.RxPort ;
            TxAddr = a_Src.TxAddr ;
            TxPort = a_Src.TxPort ;

            if ( a_OvrWrt ) 
            {
                RxMac = a_Src.RxMac;
                TxMac = a_Src.TxMac;
                Vlan  = a_Src.Vlan ;
                Prot  = a_Src.Prot ; 
                DoIP  = a_Src.DoIP ;
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.10.2020
        LAST CHANGE:   14.10.2022
        ***************************************************************************/
        private List<byte> GetIp( string a_Ip )
        {
            List<byte> ret = new List<byte>();

            if ( a_Ip.Contains(":") ) return ret;

            List<string> ips = Utils.SplitExt( a_Ip, "." );
            foreach( string ip in ips )
            {
                ret.Add( Utils.Str2Byte( ip ) );
            }

            return ret; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2022
        LAST CHANGE:   22.05.2023
        ***************************************************************************/
        private List<ushort> GetIp6( string a_Ip )
        {
            List<ushort> ret = new List<ushort>();

            if ( a_Ip.Contains(".") ) return ret;

            return Utils.IPv6Adr2Lst( a_Ip );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.11.2021
        LAST CHANGE:   21.07.2025
        ***************************************************************************/
        private List<byte> GetMac( string a_Ip )
        {
            List<byte> ret = new List<byte>();

            List<string> ips = Utils.SplitExt( a_Ip, ":- " );
            foreach( string ip in ips )
            {
                ret.Add( Utils.Str2Byte( ip, 16 ) );
            }

            return ret; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.10.2022
        LAST CHANGE:   26.10.2022
        ***************************************************************************/
        private bool IsIP6()
        {
            if ( tRxAddr6.Count > 0 ) return true;
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.03.2017
        LAST CHANGE:   30.06.2021
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                Name   = a_Conf.Deserialize<string>();
                RxAddr = a_Conf.Deserialize<string>();
                RxPort = a_Conf.Deserialize<string>();
                TxAddr = a_Conf.Deserialize<string>();
                TxPort = a_Conf.Deserialize<string>();
            }
            else
            {
                a_Conf.Serialize( Name   );
                a_Conf.Serialize( RxAddr );
                a_Conf.Serialize( RxPort );
                a_Conf.Serialize( TxAddr );
                a_Conf.Serialize( TxPort );
            }
        }
    }


    /***************************************************************************
    SPECIFICATION: 
    CREATED:       15.03.2017
    LAST CHANGE:   02.07.2019
    ***************************************************************************/
    public class IntfceParms
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       04.06.2019
        LAST CHANGE:   08.05.2024
        ***************************************************************************/
        public EthParams Eth { get { return m_EthPars; } }
        public CanParams Can { get { return m_CanPars; } }
        public NwMngemnt RBS { get { return m_NwMgmnt; } }
        
        public List<EthParams> Eths { get { return m_EthParams; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       15.03.2017
        LAST CHANGE:   12.01.2021
        ***************************************************************************/
        public const string IFPRM_XAPP = "XCP APP";
        public const string IFPRM_XBL  = "XCP BL";
        public const string IFPRM_UDS  = "UDS";

        private List<CanParams>  m_CanParams;
        private List<EthParams>  m_EthParams;

        private EthParams m_EthPars;
        private CanParams m_CanPars;
        private NwMngemnt m_NwMgmnt;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       15.03.2017
        LAST CHANGE:   27.04.2021
        ***************************************************************************/
        public IntfceParms()
        {
            m_CanParams = new List<CanParams>();
            m_CanParams.Add( new CanParams(IFPRM_XAPP) );
            m_CanParams.Add( new CanParams(IFPRM_XBL)  );
            m_CanParams.Add( new CanParams(IFPRM_UDS)  );

            m_CanParams[0].Prot = PROT_IDS.PROT_XAPP;
            m_CanParams[1].Prot = PROT_IDS.PROT_XBL ;
            m_CanParams[2].Prot = PROT_IDS.PROT_UDS ;

            m_EthParams = new List<EthParams>();
            m_EthParams.Add( new EthParams(IFPRM_XBL)  );
            m_EthParams.Add( new EthParams(IFPRM_UDS)  );
            m_EthParams.Add( new EthParams(IFPRM_XAPP) );

            m_EthParams[0].Prot = PROT_IDS.PROT_XBL ;
            m_EthParams[1].Prot = PROT_IDS.PROT_UDS ;
            m_EthParams[2].Prot = PROT_IDS.PROT_XAPP;

            GetCan(IFPRM_UDS);
            GetEth(IFPRM_UDS);

            m_NwMgmnt = new NwMngemnt();
        }

        public void Copy( IntfceParms a_Src )
        {
            if (a_Src.m_CanParams.Count > 0 && a_Src.m_CanParams != m_CanParams) { m_CanParams.Clear(); m_CanParams.AddRange( a_Src.m_CanParams ); }
            if (a_Src.m_EthParams.Count > 0 && a_Src.m_EthParams != m_EthParams) { m_EthParams.Clear(); m_EthParams.AddRange( a_Src.m_EthParams ); }

            //m_NwMgmnt.Copy( a_Src.m_NwMgmnt );
        }

        public void CopySel( IntfceParms a_Src, bool a_OvrWrt )
        {
            int cancnt = m_CanParams.Count > a_Src.m_CanParams.Count ? a_Src.m_CanParams.Count : m_CanParams.Count;
            int ethcnt = m_EthParams.Count > a_Src.m_EthParams.Count ? a_Src.m_EthParams.Count : m_EthParams.Count;
            for ( int i=0; i<cancnt; i++ ) m_CanParams[i].Copy( a_Src.m_CanParams[i], a_OvrWrt );
            for ( int i=0; i<ethcnt; i++ ) m_EthParams[i].Copy( a_Src.m_EthParams[i], a_OvrWrt );
        }

        public CanParams GetCan( string a_PrmName )
        {
            m_CanPars = m_CanParams.Find( p => p.Name == a_PrmName );
            return ( m_CanPars );
        }

        public EthParams GetEth( string a_PrmName )
        {
            m_EthPars = m_EthParams.Find( p => p.Name == a_PrmName );
            return ( m_EthPars );
        }

        public void FillEthCombo( ref UserComboBox a_Cmb )
        {
            a_Cmb.Clear();
            foreach( EthParams p in m_EthParams ) a_Cmb.Items.Add( p.Name );
        }

        public void FillCanCombo( ref UserComboBox a_Cmb )
        {
            a_Cmb.Clear();
            foreach( CanParams p in m_CanParams ) a_Cmb.Items.Add( p.Name );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2019
        LAST CHANGE:   19.09.2019
        ***************************************************************************/
        public void CanA2CanB()
        {
            CanParams cna = GetCan( IFPRM_XAPP );
            CanParams cnb = GetCan( IFPRM_XBL  );

            if (cnb.RxID == "none" && cna.RxID != "none") cnb.Copy( cna, true );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.05.2019
        LAST CHANGE:   18.11.2024
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                int 
                idx = a_Conf.Deserialize<int>();
                m_CanParams.Clear();
                for (int i=0; i<idx; i++) 
                {
                    CanParams pms = new CanParams();
                    pms.Serialize( ref a_Conf );
                    m_CanParams.Add( pms );
                }

                idx = a_Conf.Deserialize<int>();
                m_EthParams.Clear();
                for (int i=0; i<idx; i++) 
                {
                    EthParams pms = new EthParams();
                    pms.Serialize( ref a_Conf );
                    m_EthParams.Add( pms );
                }
            }
            else
            {
                a_Conf.Serialize( m_CanParams.Count ); foreach ( CanParams pms in m_CanParams ) pms.Serialize( ref a_Conf );
                a_Conf.Serialize( m_EthParams.Count ); foreach ( EthParams pms in m_EthParams ) pms.Serialize( ref a_Conf );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.01.2024
        LAST CHANGE:   16.01.2024
        ***************************************************************************/
        public void SetFrmErrDet( bool a_Enab )
        {
            m_CanPars.FrmErrDt = a_Enab;

            foreach( CanParams p in m_CanParams )
            {
                p.FrmErrDt = a_Enab;
            }
        }

    }  // class IntfceParms

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       24.07.2019
    LAST CHANGE:   27.02.2023
    ***************************************************************************/
    public class FlashParms
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       24.07.2019
        LAST CHANGE:   16.10.2023
        ***************************************************************************/
        public bool bIgnBlks { get { return Utils.Str2Bool(IgnBlks); } set { IgnBlks = value.ToString(); } }
        public bool bIgnPInf { get { return Utils.Str2Bool(IgnPInf); } set { IgnPInf = value.ToString(); } }
        public bool bOmtSnK  { get { return Utils.Str2Bool(OmtSnK) ; } set { OmtSnK  = value.ToString(); } }
        public bool bOmtBlnk { get { return Utils.Str2Bool(OmtBlnk); } set { OmtBlnk = value.ToString(); } }
        public bool bBytOrdr { get { return BytOrdr.ToLower() == "lsb"; } } 
        //public int  iDelay   { get { return Utils.Str2Int (Delay)  ; } set { Delay   = value.ToString(); } }
        public int  iPngDly  { get { return Utils.Str2Int (PngDly) ; } set { PngDly  = value.ToString(); } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       24.07.2019
        LAST CHANGE:   10.11.2025
        ***************************************************************************/
        public string  IgnBlks ;  // ignore block info when flashing
        public string  IgnPInf ;  // ignore PGM Info when flashing
        public string  OmtSnK  ;  // Omit seed n key when flashing
        public string  OmtBlnk ;  // Omit blanking
        public string  PngDly  ;  // Delay after PING uds command
        public string  BytOrdr ;  // Byte order in XCP protocol (for HA projects) (overruling CONNECT response)
        public string  BlkIDs  ;  // Permitted XCP info block IDs
        public int     BlkIdSet;  // Set of XCP block IDs
        public bool    ShtUpld ;  // SHORT_UPLOAD instead of UPLOAD
        public bool    DltMode ;  // Connect procedure similar XCPDLT (less efficient)

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       24.07.2019
        LAST CHANGE:   10.11.2025
        ***************************************************************************/
        public FlashParms()
        {
            IgnBlks  = "false";
            IgnPInf  = "false";
            OmtSnK   = "false";
            OmtBlnk  = "false";
            ShtUpld  = false;
            DltMode  = false;
            BytOrdr  = "none";
            BlkIDs   = "none";
            BlkIdSet = 0;
            PngDly   = "500";

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.07.2019
        LAST CHANGE:   10.11.2025
        ***************************************************************************/
        public void Copy( FlashParms a_Src ) { CopySel( a_Src, true ); }
        public void CopySel( FlashParms a_Src, bool a_OvrWrt )
        {
            OmtBlnk = a_Src.OmtBlnk;

            if ( a_OvrWrt )
            {
                IgnBlks  = a_Src.IgnBlks;
                IgnPInf  = a_Src.IgnPInf;
                OmtSnK   = a_Src.OmtSnK ;
                BytOrdr  = a_Src.BytOrdr;
                BlkIDs   = a_Src.BlkIDs ;
                BlkIdSet = a_Src.BlkIdSet;
                PngDly   = a_Src.PngDly ;
                DltMode  = a_Src.DltMode;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.07.2019
        LAST CHANGE:   16.10.2023
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {

            if( a_Conf.IsReading )
            {
                if ( a_Conf.DbVersion > 317 )
                {
                    OmtBlnk = a_Conf.Deserialize<string>( );
                    return;  
                }
                IgnBlks = a_Conf.Deserialize<string>( );
                OmtSnK  = a_Conf.Deserialize<string>( );
                OmtBlnk = a_Conf.Deserialize<string>( );
            }
            else
            {
                a_Conf.Serialize( OmtBlnk );
                return; 
                a_Conf.Serialize( IgnBlks );
                a_Conf.Serialize( OmtSnK  );
            }
        }

    } // class

    /***************************************************************************
    SPECIFICATION: Ethernet telegram class
    CREATED:       13.10.2020
    LAST CHANGE:   24.10.2022
    ***************************************************************************/
    public class EthMsg
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       13.10.2020
        LAST CHANGE:   02.01.2025
        ***************************************************************************/
        public  bool   IsIP6 { get { return Dst.IsIp6(); } }

        public  EthAddrs  Src   { get { return m_Src; } set { m_Src = value; } } 
        public  EthAddrs  Dst   { get { return m_Dst; } set { m_Dst = value; } }

        public static int StSeqNr { get { return m_StSeqNr; } set { m_StSeqNr = value; } }
        public static int StAckNr { get { return m_StAckNr; } set { m_StAckNr = value; } } 


        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       13.10.2020
        LAST CHANGE:   02.01.2025
        ***************************************************************************/
        private Memory  mem ;

        public  ushort  Vlan   ;
        public  ushort  IpType ;

        public  byte    Version  ;
        public  byte    HdrLen   ;
        public  byte    Service  ;
        public  ushort  TotLen   ;
        public  ushort  Ident    ;
        //public  byte    CtlFlgs  ;
        public  ushort  FragOffs ;
        public  byte    Tme2Live ;
        public  byte    Protcl   ;
        public  ushort  IpCrc    ;

        public  ushort     Tag     ;
        public  ushort     EthType ;
        public  List<byte> TlgData ;
        public  List<byte> Data    ;
        private EthAddrs   m_Src   ;
        private EthAddrs   m_Dst   ;

        //IP6 Header
        public byte        TrfcClss;
        public uint        FlowLbl ;
        public ushort      PyldLen ;
        public byte        NxtHdr  ;
        public byte        HopLimit;

        // TCP specific
        public  int        SeqNr   ;
        public  int        AckNr   ;
        public  byte       DatOffs ;
        public  byte       Flags   ;
        public  ushort     Window  ;
        public  ushort     PrtCrc  ;
        public  ushort     UrgPtr  ;
        public  List<byte> Opts    ; 

        public  static ushort StIdent   = 0;
        public  static ushort StSrcPrt  = 0;
        private static int    m_StSeqNr = 0;
        private static int    m_StAckNr = 0;

        // UDP(XCP) specific
        public        ushort UdpLen ;

        // IGMP specific
        public byte   MemRep;

        // ICMPv6 specific
        public byte         PrtType;
        public byte         Code   ;
        public EthAddrs     TargIP6;
        public byte         OptType;
        public byte         OptLen ;

        // ARP specific
        public ushort     HwType    ;
        public ushort     ProtType  ;
        public byte       HwSize    ;
        public byte       ProtSize  ;
        public ushort     OpCdReq   ;
        public List<byte> SenderMAC ;
        public List<byte> SenderIP  ;
        public List<byte> TargetMAC ;
        public List<byte> TargetIP  ;


        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       19.10.2020
        LAST CHANGE:   10.01.2025
        ***************************************************************************/
        public EthMsg ()
            :this( ETHPROT.EP_TCP, false )
        {
        }

        public EthMsg( ETHPROT a_Prot )
            :this( a_Prot, false )
        {
        }

        public EthMsg( ETHPROT a_Prot, bool a_Full, bool a_IP6 = false )
        {
            mem = new Memory();
            mem.AssignMem( TlgData );

            if ( a_IP6 )
            {
                switch( a_Prot )
                {
                    case ETHPROT.EP_TCP: 
                    case ETHPROT.EP_ICMPV6:
                        Vlan     = 2; 
                        EthType  = 0x86DD; 
                        Version  = 6;
                        break;
                }

                IpType = 0x86dd; // IP6
            }
            else
            {
                HdrLen = 5;

                switch( a_Prot )
                {
                    case ETHPROT.EP_UDP:
                        if (a_Full) { Vlan =  0; EthType = 0;      }
                        else        { Vlan = 73; EthType = 0x8100; }
                        break;

                    case ETHPROT.EP_TCP: 
                        Vlan = 73; EthType = 0x8100; 
                        break;

                    case ETHPROT.EP_IGMPV3:
                        HdrLen = 6;
                        break;
                }

                IpType  = 0x0800;  // IP4

                Version  = 4;
                Service  = 0;
                TotLen   = 287;
                Protcl   = (byte)a_Prot;
                IpCrc    = 0;

                Ident    = 8091;
                FragOffs = 0x4000;  // don't fragment
                Tme2Live = 0x80;
                AckNr    = 0;
                Flags    = (byte)TCPFLGS.TF_PSH;

                Window   = 0xFFFF;
            }

            DatOffs  = 0x50;  // header length
            HopLimit = 255;

            Opts  = new List<byte>();

            TlgData  = new List<byte>();
            Data     = new List<byte>(); 
            Src      = new EthAddrs();
            Dst      = new EthAddrs();

            SenderMAC = new List<byte>();
            SenderIP  = new List<byte>();
            TargetMAC = new List<byte>();
            TargetIP  = new List<byte>();
            TargIP6   = new EthAddrs();
        }

        public EthMsg( EthMsg a_Msg )
            : this( (ETHPROT)a_Msg.Protcl, false, a_Msg.IsIP6 )
        {
            Copy( a_Msg, true );
        }

        public EthMsg( List<byte> a_Data, ETHPROT a_Prot, bool a_FullFrm, bool a_IP6 = false )
            : this( a_Prot, a_FullFrm, a_IP6 )
        {
            List<byte> data = new List<byte>( a_Data );
            mem.AssignMem( data );
            int ix = 0;

            if ( a_IP6 ) Version = 6;

            if ( a_FullFrm )
            {
                if ( data.Count < 12 ) return;

                data.RemoveRange( 0, 12 );  // discard the MAC addresses
                EthType = mem.GetMem2( ref ix );

                if ( EthType == 0x8100 )
                {
                    Vlan = mem.GetMem2( ref ix );
                    //data.RemoveRange( 0, 2 );   
                }

                data.RemoveRange( 0, 2 );   
            }

            TlgData.AddRange( data );
        }

        public EthMsg( List<byte> a_Data )
            : this( a_Data, ETHPROT.EP_TCP, false )
        {
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.11.2021
        LAST CHANGE:   17.11.2021
        ***************************************************************************/
        public void Clear()
        {
            Src.Mac.Clear();
            Dst.Mac.Clear();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.11.2020
        LAST CHANGE:   10.01.2025
        ***************************************************************************/
        public void Copy( EthMsg a_Src, bool a_Data = false )
        {
            if( a_Src == null ) return;

            Tag     = a_Src.Tag    ;
            EthType = a_Src.EthType;

            IpType   = a_Src.IpType  ;
            Version  = a_Src.Version ;
            //HdrLen   = a_Src.HdrLen  ;
            Service  = a_Src.Service ;
            TotLen   = a_Src.TotLen  ;
            Ident    = a_Src.Ident   ;
            FragOffs = a_Src.FragOffs;
            Tme2Live = a_Src.Tme2Live;
            Protcl   = a_Src.Protcl  ;
            PrtCrc   = a_Src.PrtCrc  ;
            IpCrc    = a_Src.IpCrc   ;

            AckNr    = a_Src.AckNr   ;
            SeqNr    = a_Src.SeqNr   ;
            DatOffs  = a_Src.DatOffs ;
            Window   = a_Src.Window  ;
            Flags    = a_Src.Flags   ;
            UrgPtr   = a_Src.UrgPtr  ;
            Vlan     = a_Src.Vlan    ;

            Opts.Clear();
            Opts.AddRange(a_Src.Opts);

            // IP6 related
            TrfcClss = a_Src.TrfcClss;
            FlowLbl  = a_Src.FlowLbl ;
            PyldLen  = a_Src.PyldLen ;
            NxtHdr   = a_Src.NxtHdr  ;
            HopLimit = a_Src.HopLimit;

            m_Src  .Copy( a_Src.Src );
            m_Dst  .Copy( a_Src.Dst );

            if ( ! a_Data ) return;

            TlgData.Clear();
            TlgData .AddRange( a_Src.TlgData );

            Data.Clear();
            Data.AddRange( a_Src.Data );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.12.2020
        LAST CHANGE:   02.11.2022
        ***************************************************************************/
        public void CopyNrs( EthMsg a_Src )
        {
            SeqNr = a_Src.SeqNr;
            AckNr = a_Src.AckNr;

            int ix = 0;

            if ( IsIP6 ) ix = 48;
            else
            {
                ix = 8;
                mem.U16ToByteList( Ident, ref TlgData, ref ix );
                ix = 28;
            }
            mem.I32ToByteList( SeqNr, ref TlgData, ref ix );
            mem.I32ToByteList( AckNr, ref TlgData, ref ix );
            //EnterTcpCRCs();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.01.2021
        LAST CHANGE:   28.03.2024
        ***************************************************************************/
        public void CopyAddrs( EthMsg a_Src )
        {
            m_Src.Copy( a_Src.Src );
            m_Dst.Copy( a_Src.Dst );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.01.2021
        LAST CHANGE:   01.02.2021
        ***************************************************************************/
        public void CreateArp( bool a_Req )
        {
            mem.VarToByteList( Vlan          , ref TlgData );  // Vlan
            mem.VarToByteList( (ushort)0x0806, ref TlgData );  // Type (ARP)
            mem.VarToByteList( (ushort)0x0001, ref TlgData );  // HW Type (Ethernet)
            mem.VarToByteList( (ushort)0x0800, ref TlgData );  // Protocol Type (IP4)
            mem.VarToByteList( (byte)  0x06  , ref TlgData );  // HW Size
            mem.VarToByteList( (byte)  0x04  , ref TlgData );  // Prot. Size
            if (a_Req) mem.VarToByteList( (ushort)0x0001, ref TlgData );  // Opcode (Request)
            else       mem.VarToByteList( (ushort)0x0002, ref TlgData );  // Opcode (Response)
            TlgData.AddRange( Src.Mac );
            TlgData.AddRange( Src.Ip  ); // Sender IP
            TlgData.AddRange( mem.CreateByteList( 0, 6 ) );  // Empty target MACAddress
            TlgData.AddRange( Dst.Ip ); // Target IP
            //TlgData.AddRange( mem.CreateByteList( 0, 14 ) ); // padding
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.10.2022
        LAST CHANGE:   02.05.2024
        ***************************************************************************/
        public void CreateICMPv6( byte a_Type = 135 ) // type == NDP
        {
            mem.VarToByteList( Vlan          , ref TlgData );  // Vlan
            mem.VarToByteList( (ushort)0x86dd, ref TlgData );  // Type (ARP)
            Version  = 6;
            NxtHdr   = 58; // ICMPv6
            PyldLen  = 32;
            //PyldLen  = 24;
            CreateHdr6();
            EnterIPs  ();

            byte tp = 0;

            mem.VarToByteList( a_Type      , ref TlgData );  // type
            mem.VarToByteList( (byte)0     , ref TlgData );  // code

            switch( a_Type )
            {
                case 135: tp = 1; break;
                case 136: tp = 2; break;

                case 129: 
                    mem.VarToByteList( (uint)  0x00000000, ref TlgData );  // identifier & sequence
                    mem.VarToByteList( (ushort)0x0000    , ref TlgData );  // identifier & sequence
            
                    EnterICMPv6CRC();
                    return;
            }

            mem.VarToByteList( (ushort)0   , ref TlgData );  // crc
            mem.VarToByteList( 0x60000000  , ref TlgData );  // reserved (value is crucial, otherwise XCP Upload is interrupted by weired Neigb.Solit.Tlg)
            mem.VarToByteList( TargIP6.Ip6 , ref TlgData );
            mem.VarToByteList( tp          , ref TlgData );  // type src / target link layer address
            mem.VarToByteList( (byte)1     , ref TlgData );  // length (8 bytes)
            mem.VarToByteList( TargetMAC   , ref TlgData );

            EnterICMPv6CRC();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.10.2020
        LAST CHANGE:   01.02.2021
        ***************************************************************************/
        private void CreateHdr()
        {
            mem.VarToByteList( (byte)((Version << 4) | HdrLen), ref TlgData );

            mem.VarToByteList( Service , ref TlgData );
            mem.VarToByteList( TotLen  , ref TlgData );
            mem.VarToByteList( Ident   , ref TlgData );
            mem.VarToByteList( FragOffs, ref TlgData );
            mem.VarToByteList( Tme2Live, ref TlgData );
            mem.VarToByteList( Protcl  , ref TlgData );
            mem.VarToByteList( IpCrc   , ref TlgData );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2022
        LAST CHANGE:   14.10.2022
        ***************************************************************************/
        private void CreateHdr6()
        {
            uint hlp = (uint)(Version  << 28);
            hlp     |= (uint)(TrfcClss << 24);
            hlp     |= (FlowLbl & 0xFFFFF);
            mem.VarToByteList( hlp, ref TlgData );

            mem.VarToByteList( PyldLen , ref TlgData );
            mem.VarToByteList( NxtHdr  , ref TlgData );
            mem.VarToByteList( HopLimit, ref TlgData );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.01.2021
        LAST CHANGE:   21.10.2022
        ***************************************************************************/
        public void CreateTcpHdr( bool a_IncID )
        {
            if ( a_IncID ) Ident = EthMsg.StIdent++;

            if ( Dst.IsIp6() )
            {
                IpType   = 0x86dd;  // ip6
                Version  = 6;
                NxtHdr   = 6;
                TrfcClss = 0;
                FlowLbl  = 0;
                HopLimit = 255;
            }
            else
            {
                IpType  = 0x0800;  // ip4
                Version = 4;
                //if (Vlan != 0) Utils.VarToByteList( 0x8100, ref TlgData );
            }
                
            if (Vlan != 0) Utils.VarToByteList( Vlan, ref TlgData );
           
            Utils.VarToByteList( IpType, ref TlgData );

            EthType = 0x8100;
            if ( Dst.IsIp6() ) CreateHdr6();
            else               CreateHdr ();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.01.2021
        LAST CHANGE:   29.01.2021
        ***************************************************************************/
        public void CreateUdpHdr()
        {
            Ident = 8091; //EthMsg.StIdent++;

            if (Vlan != 0) Utils.VarToByteList( Vlan, ref TlgData );

            if ( Dst.IsIp6() )
            {
                IpType   = 0x86dd;  // ip6
                Version  = 6;
                NxtHdr   = 17;
                TrfcClss = 0;
                FlowLbl  = 0;
                HopLimit = 64;
            }
            else
            {
                IpType  = 0x0800;  // ip4
                Version = 4;
            }

            Utils.VarToByteList( IpType, ref TlgData );

            EthType = 0x8100;
            if ( Dst.IsIp6() ) CreateHdr6();
            else               CreateHdr ();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.10.2020
        LAST CHANGE:   07.07.2023
        ***************************************************************************/
        public void CreateTcpFtr( List<byte> a_Opts = null )
        {
            Utils.VarToByteList( SeqNr  , ref TlgData );
            Utils.VarToByteList( AckNr  , ref TlgData );
            Utils.VarToByteList( DatOffs, ref TlgData );
            Utils.VarToByteList( Flags  , ref TlgData );
            Utils.VarToByteList( Window , ref TlgData );
            Utils.VarToByteList( PrtCrc , ref TlgData );
            Utils.VarToByteList( UrgPtr , ref TlgData );

            if (a_Opts != null ) TlgData.AddRange( a_Opts );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.01.2021
        LAST CHANGE:   19.01.2021
        ***************************************************************************/
        public void CreateUdpFtr( int a_UdpLen )
        {
            UdpLen = (ushort)a_UdpLen;
            Utils.VarToByteList( UdpLen, ref TlgData );
            Utils.VarToByteList( PrtCrc, ref TlgData );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.10.2020
        LAST CHANGE:   02.11.2022
        ***************************************************************************/
        public void EnterAddr()
        {
            EnterIPs();

            mem.VarToByteList( Src.Port, ref TlgData );
            mem.VarToByteList( Dst.Port, ref TlgData );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.04.2022
        LAST CHANGE:   17.10.2022
        ***************************************************************************/
        public void EnterIPs()
        {
            if ( Src.Ip6.Count > 0 )
            {
                mem.VarToByteList( Src.Ip6 , ref TlgData );
                mem.VarToByteList( Dst.Ip6 , ref TlgData );
            }
            else
            {
                mem.VarToByteList( Src.Ip  , ref TlgData );
                mem.VarToByteList( Dst.Ip  , ref TlgData );
            }
        }

        /***************************************************************************
        SPECIFICATION: Ethernet frame CRCs
        CREATED:       24.10.2020
        LAST CHANGE:   11.08.2022
        ***************************************************************************/
        public class EtherFrame
        {
            public static ushort TcpCrc( List<byte> a_Data, bool a_CRC, bool a_Udp ) { return TcpCrc( a_Data, 0, a_Data.Count, a_Udp ); } 
            public static ushort TcpCrc( List<byte> a_Data, int a_Idx, int a_Len, bool a_Udp )
            {
                bool   zero = false;
                ulong  sum = 0;
                int    idx = a_Idx;
                Memory mem = new Memory();
                mem.AssignMem( a_Data );

                if( a_Data.Count % 2 != 0 )
                {
                    a_Data.Add( 0 );
                    zero = true;
                }

                while ( idx < ( a_Idx + a_Len ) )
                {
                    int ix = idx;
                    ushort d = mem.GetMem2( ref idx );
                    if (ix == idx) break;
                    sum += d;
                }

                uint ret = 0;

                for( int i = 0; i < 4; i++ )
                {
                    ret += (uint)(sum & 0xffff);
                    sum >>= 16;
                    if ( sum == 0 ) break;
                }

                if ( (ret & 0xffff0000) != 0 )
                {  // regarding a glitch in all algo descriptions in the WWW
                    uint hlp = ret & 0xffff;
                    hlp += ret >> 16;
                    ret = hlp;
                }

                ret = (ushort)~ret;

                if (ret == 0) ret = 0xffff;

                if (zero) a_Data.RemoveAt(a_Data.Count-1);
                return (ushort)ret;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.10.2020
        LAST CHANGE:   13.01.2021
        ***************************************************************************/
        public void EnterTcpCRCs()
        {
            int ioff = 4;
            if ( Vlan == 0 ) ioff = 2;

            int hdsz = 20 + ioff;
            int cidx = 10 + ioff;

            // Set IP CRC to 0
            int idx = cidx;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            // Set TCP CRC to 0
            idx = cidx + 26;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            // Enter IP CRC
            IpCrc = EtherFrame.TcpCrc( TlgData, ioff, 20, false );

            idx = cidx;
            Utils.U16ToByteList( IpCrc, ref TlgData, ref idx );

            ushort tcplen = (ushort)( TotLen - hdsz + ioff ); 

            // Pseudo header 
            List<byte> hlp = new List<byte>();
            Utils.VarToByteList( Dst.Ip , ref hlp );
            Utils.VarToByteList( Src.Ip , ref hlp );
            Utils.VarToByteList( (byte)0, ref hlp );
            Utils.VarToByteList( Protcl , ref hlp );
            Utils.VarToByteList( tcplen , ref hlp );

            // preserve the IP header ( VLAN - Dest.IP )
            byte[] mem = new byte[hdsz];
            TlgData.CopyTo( 0, mem, 0, hdsz );
            TlgData.RemoveRange( 0, hdsz );

            // Add TCP header incl. payload to the pseudo header 
            hlp.AddRange( TlgData );

            PrtCrc = EtherFrame.TcpCrc( hlp, true, false );

            // Restore message
            TlgData.InsertRange( 0, mem );

            // Enter CRC
            idx = cidx + 26;
            Utils.U16ToByteList( PrtCrc, ref TlgData, ref idx );

            //Data.RemoveRange( 0, 12 );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.10.2022
        LAST CHANGE:   21.11.2022
        ***************************************************************************/
        public void EnterTcp6CRC( int a_IdxOff = 0 )
        {
            int ioff = a_IdxOff;
            if ( Vlan == 0 ) ioff -= 2;

            int hdsz =   44 + ioff;
            int cidx =   60 + ioff;

            // Set TCP CRC to 0
            int idx = cidx;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            uint tcplen = (uint)( TlgData.Count - hdsz ); 

            // Pseudo header 
            List<byte> hlp = new List<byte>();
            mem.VarToByteList( Src.Ip6      , ref hlp );
            mem.VarToByteList( Dst.Ip6      , ref hlp );
            mem.VarToByteList( tcplen       , ref hlp );
            mem.VarToByteList( (uint)NxtHdr , ref hlp );

            // preserve the IP header ( VLAN - Dest.IP )
            byte[] mm = new byte[hdsz];
            TlgData.CopyTo( 0, mm, 0, hdsz );
            TlgData.RemoveRange( 0, hdsz );

            // Add TCP header incl. payload to the pseudo header 
            hlp.AddRange( TlgData );

            PrtCrc = EtherFrame.TcpCrc( hlp, true, false );

            // Restore message
            TlgData.InsertRange( 0, mm );

            // Enter CRC
            idx = cidx;
            mem.U16ToByteList( PrtCrc, ref TlgData, ref idx );

            //Data.RemoveRange( 0, 12 );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.11.2022
        LAST CHANGE:   28.04.2023
        ***************************************************************************/
        public void EnterUdp6CRC( int a_IdxOff = 0 )
        {
            try
            {
                int ioff = a_IdxOff;
                if ( Vlan == 0 ) ioff -= 4;

                int hdsz = 44 + ioff;
                int cidx = 50 + ioff;

                // Set UDP CRC to 0
                int idx = cidx;
                Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

                uint udplen = (uint)( TlgData.Count - hdsz ); 

                // Pseudo header 
                List<byte> hlp = new List<byte>();
                mem.VarToByteList( Src.Ip6      , ref hlp );
                mem.VarToByteList( Dst.Ip6      , ref hlp );
                mem.VarToByteList( udplen       , ref hlp );
                mem.VarToByteList( (uint)NxtHdr , ref hlp );

                // preserve the IP header ( VLAN - Dest.IP )
                byte[] mm = new byte[hdsz];
                TlgData.CopyTo( 0, mm, 0, hdsz );
                TlgData.RemoveRange( 0, hdsz );

                // Add TCP header incl. payload to the pseudo header 
                hlp.AddRange( TlgData );

                PrtCrc = EtherFrame.TcpCrc( hlp, true, false );

                // Restore message
                TlgData.InsertRange( 0, mm );

                // Enter CRC
                idx = cidx;
                mem.U16ToByteList( PrtCrc, ref TlgData, ref idx );

                //Data.RemoveRange( 0, 12 );
            }
            catch( Exception )
            {
                // do nothing
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.10.2022
        LAST CHANGE:   13.02.2024
        ***************************************************************************/
        public void EnterICMPv6CRC( int a_IdxOff = 0 )
        {
            int ioff = a_IdxOff;
            if ( Vlan == 0 ) ioff -= 2;

            int hdsz = 44 + ioff;
            int cidx = 46 + ioff;

            // Set TCP CRC to 0
            int idx = cidx;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            uint tcplen = (uint)( TlgData.Count - hdsz );

            // Pseudo header 
            List<byte> hlp = new List<byte>();
            mem.VarToByteList( Src.Ip6, ref hlp );
            mem.VarToByteList( Dst.Ip6, ref hlp );
            mem.VarToByteList( tcplen, ref hlp );
            mem.VarToByteList( (uint)NxtHdr, ref hlp );

            // preserve the IP header ( VLAN - Dest.IP )
            byte[] mm = new byte[hdsz];
            if ( TlgData.Count < hdsz ) hdsz = TlgData.Count;
            TlgData.CopyTo( 0, mm, 0, hdsz );
            TlgData.RemoveRange( 0, hdsz );

            // Add TCP header incl. payload to the pseudo header 
            hlp.AddRange( TlgData );

            PrtCrc = EtherFrame.TcpCrc( hlp, true, false );

            // Restore message
            TlgData.InsertRange( 0, mm );

            // Enter CRC
            idx = cidx;
            mem.U16ToByteList( PrtCrc, ref TlgData, ref idx );

            //Data.RemoveRange( 0, 12 );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.01.2021
        LAST CHANGE:   13.01.2021
        ***************************************************************************/
        public void EnterUdpCRCs( int a_IdxOff )
        {
            int OFFS = a_IdxOff;
            int CIDX = 10 + OFFS;
            int HDSZ = 20 + OFFS;

            // Set IP CRC to 0
            int idx = CIDX;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            // Set TCP CRC to 0
            idx = CIDX + 16;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            // Enter IP CRC
            IpCrc = EtherFrame.TcpCrc( TlgData, OFFS, 20, false );
            //IpCrc = 0;

            idx = CIDX;
            Utils.U16ToByteList( IpCrc, ref TlgData, ref idx );

            ushort tcplen = (ushort)(TotLen - HDSZ + OFFS ); 

            // Pseudo header 
            List<byte> hlp = new List<byte>();
            Utils.VarToByteList( Src.Ip , ref hlp );
            Utils.VarToByteList( Dst.Ip , ref hlp );
            Utils.VarToByteList( (byte)0, ref hlp );
            Utils.VarToByteList( Protcl , ref hlp );
            Utils.VarToByteList( tcplen , ref hlp );

            int hlen = hlp.Count;

            // preserve the IP header ( IPType - Dest.IP )
            byte[] mem = new byte[HDSZ];
            TlgData.CopyTo( 0, mem, 0, HDSZ );
            TlgData.RemoveRange( 0, HDSZ );
            hlp.AddRange( TlgData );   // add complete telegram without IP header to the pseudo header
   
            PrtCrc = EtherFrame.TcpCrc( hlp, false, true );
            //PrtCrc = 0;

            // Restore payload
            TlgData.InsertRange(0,mem);

            // Enter CRC
            idx = CIDX + 16;
            Utils.U16ToByteList( PrtCrc, ref TlgData, ref idx );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.04.2022
        LAST CHANGE:   10.05.2022
        ***************************************************************************/
        public void EnterIgmpCRCs( int a_IdxOff )
        {
#if false
            int ioff = 4;
            if ( Vlan == 0 ) ioff = 2;

            int OFFS = ioff;
            int CIDX = 10 + OFFS;
            int HDSZ = 20 + OFFS;

            // Set IP CRC to 0
            int idx = CIDX;

            if ( a_IdxOff > TlgData.Count-1 ) return;

            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            // Set TCP CRC to 0
            idx = CIDX + 16;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            // Enter IP CRC
            IpCrc = EtherFrame.TcpCrc( TlgData, 0, 20 );

            idx = CIDX;
            Utils.U16ToByteList( IpCrc, ref TlgData, ref idx );

            ushort tcplen = (ushort)(TotLen - HDSZ + OFFS ); 

            // Pseudo header 
            List<byte> hlp = new List<byte>();
            Utils.VarToByteList( Src.Ip , ref hlp );
            Utils.VarToByteList( Dst.Ip , ref hlp );
            Utils.VarToByteList( (byte)0, ref hlp );
            Utils.VarToByteList( Protcl , ref hlp );
            Utils.VarToByteList( tcplen , ref hlp );

            int hlen = hlp.Count;

            // preserve the IP header ( IPType - Dest.IP )
            byte[] mem = new byte[HDSZ];
            TlgData.CopyTo( 0, mem, 0, HDSZ );
            TlgData.RemoveRange( 0, HDSZ );
            hlp.AddRange( TlgData );

            int mod = hlp.Count % 2;
            for (int i=0; i<mod; i++) hlp.Add(0);

            PrtCrc = EtherFrame.TcpCrc( hlp );

            // Restore payload
            TlgData.InsertRange(0,mem);

            // Enter CRC
            idx = CIDX + 16;
            Utils.U16ToByteList( PrtCrc, ref TlgData, ref idx );
#else
            int ioff = 4;
            if ( Vlan == 0 ) ioff = 0;

            int OFFS = ioff;
            int CIDX = 10 + OFFS;
            int HDSZ = 20 + OFFS;
            int IGIX = 24 + OFFS;

            // Set IP CRC to 0
            int idx = CIDX;

            if ( a_IdxOff > TlgData.Count-1 ) return;

            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            // Set IGMP CRC to 0
            idx = CIDX + 16;
            Utils.U16ToByteList( (ushort)0, ref TlgData, ref idx );

            idx = IGIX;
            PrtCrc = EtherFrame.TcpCrc( TlgData, idx, TlgData.Count - idx, true );

            // Enter PrtCRC
            idx = CIDX + 16;
            Utils.U16ToByteList( PrtCrc, ref TlgData, ref idx );

            IpCrc  = EtherFrame.TcpCrc( TlgData, ioff, TlgData.Count, true );

            // Enter IP CRC
            idx = CIDX;
            Utils.U16ToByteList( IpCrc, ref TlgData, ref idx );
#endif
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.10.2020
        LAST CHANGE:   01.06.2023
        ***************************************************************************/
        public void EnterTcpPayload( List<byte> a_Payld, int a_OptSz = 0 )
        {
            if ( a_Payld == null ) TlgData.AddRange( Data );
            else
            {
                Data.Clear();
                Data.AddRange   ( a_Payld );
                TlgData.AddRange( a_Payld );
            }

            if ( IsIP6 )
            {
                EnterPldLen6();
                EnterTcp6CRC();
            }
            else
            {
                EnterTotLen( a_OptSz );
                EnterTcpCRCs();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.01.2021
        LAST CHANGE:   21.11.2022
        ***************************************************************************/
        public void EnterUdpPayload( List<byte> a_Payld )
        {
            Utils.VarToByteList( a_Payld, ref TlgData );
            if ( IsIP6 )
            {
                EnterPldLen6();
                EnterUdp6CRC();
            }
            else
            {
                EnterTotLen( );
                EnterUdpCRCs( 4 );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.04.2022
        LAST CHANGE:   11.05.2022
        ***************************************************************************/
        public void EnterIgmpPayload( List<byte> a_Payld )
        {
            Utils.VarToByteList( a_Payld, ref TlgData );
            EnterTotLen( );
            EnterIgmpCRCs( 4 );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.11.2020
        LAST CHANGE:   31.10.2022
        ***************************************************************************/
        public void EnterTcpOpts( int a_Var )
        {
            int cntin = TlgData.Count;

            switch ( a_Var )
            {
                case 0:
                    Utils.VarToByteList( (ushort)0x0204 , ref TlgData );
                    Utils.VarToByteList( (ushort)0x05b4 , ref TlgData );
                    Utils.VarToByteList( (byte)  0x01   , ref TlgData );
                    Utils.VarToByteList( (ushort)0x0303 , ref TlgData );
                    Utils.VarToByteList( (byte)  0x03   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x01   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x01   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x08   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x0a   , ref TlgData );
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    break;

                case 1:
                    Utils.VarToByteList( (ushort)0x0101 , ref TlgData );
                    Utils.VarToByteList( (ushort)0x080a , ref TlgData );
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData );
                    Utils.VarToByteList( (ushort)0x029C , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0203 , ref TlgData ); 
                    break;

                case 2:
                    Utils.VarToByteList( (ushort)0x0101 , ref TlgData );
                    Utils.VarToByteList( (ushort)0x080a , ref TlgData );
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData );
                    Utils.VarToByteList( (ushort)0x029C , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0203 , ref TlgData ); 
                    break;

                case 3:
                    Utils.VarToByteList( (ushort)0x0204 , ref TlgData );
                    Utils.VarToByteList( (ushort)0x05a0 , ref TlgData );
                    Utils.VarToByteList( (byte)  0x01   , ref TlgData );
                    Utils.VarToByteList( (ushort)0x0303 , ref TlgData );
                    Utils.VarToByteList( (byte)  0x03   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x01   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x01   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x08   , ref TlgData );
                    Utils.VarToByteList( (byte)  0x0a   , ref TlgData );
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    Utils.VarToByteList( (ushort)0x0000 , ref TlgData ); 
                    break;

            }

            int cntout = TlgData.Count; // add the options length to header length
            int hl = cntout - cntin;
            hl += DatOffs >> 2;
            DatOffs = (byte)(hl << 2);

            int idx = cntin - 8;
            TlgData[idx] = DatOffs;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.11.2020
        LAST CHANGE:   13.06.2023
        ***************************************************************************/
        public void EnterTotLen ( int a_OptSz = 0 )
        {
            // enter total length
            int idx = 6;

            switch( (ETHPROT)Protcl )
            {
                case ETHPROT.EP_UDP:
                case ETHPROT.EP_TCP:
                    TotLen = (ushort)(TlgData.Count-4);
                    break;

                case ETHPROT.EP_IGMPV3:
                    TotLen = (ushort)TlgData.Count;
                    break;
            }

            mem.U16ToByteList( TotLen, ref TlgData, ref idx );

            switch( (ETHPROT)Protcl )
            {
                case ETHPROT.EP_TCP: 
                    // enter data offset
                    int datlen = 20 + a_OptSz;
                    datlen <<= 2;
                    if (TlgData.Count <= 36) break;
                    TlgData[36] = (byte) datlen;
                    break;

                case ETHPROT.EP_UDP:
                    ushort len = (ushort)(TotLen - 20);
                    idx = 28;
                    mem.U16ToByteList( len, ref TlgData, ref idx );
                    break;

                case ETHPROT.EP_IGMPV3:
                    len = (ushort)(TotLen - 6);
                    idx = 6;
                    mem.U16ToByteList( len, ref TlgData, ref idx );
                    break;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.10.2022
        LAST CHANGE:   22.11.2022
        ***************************************************************************/
        public void EnterPldLen6 ()
        {
            // enter IP6 payload length
            ushort pl = (ushort)(TlgData.Count - 40 - 4);
            int ix = 8;
            mem.U16ToByteList( pl, ref TlgData, ref ix );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2023
        LAST CHANGE:   11.08.2023
        ***************************************************************************/
        public List<ushort> CreateNrpAddr()
        {
            List<ushort> ret = Utils.IPv6Adr2Lst("ff02::1:ff00:55");
            if (Dst.Ip6.Count >= 8) ret[7] = Dst.Ip6[7];
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2023
        LAST CHANGE:   22.05.2023
        ***************************************************************************/
        public List<byte> CreateNrpMac()
        {
            List<byte> ret = new List<byte>();
            Utils.Mac2Lst( "33:33:ff:00:00:55", ref ret );
            if ( Dst.Ip6.Count >= 8 )
            {
                int i=3;
                ret[i++] = (byte)(Dst.Ip6[6] &  0xff);
                ret[i++] = (byte)(Dst.Ip6[7] >> 8);
                ret[i++] = (byte)(Dst.Ip6[7] &  0xff);
            }
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2021
        LAST CHANGE:   01.02.2021
        ***************************************************************************/
        public void EnterPadding()
        {
            TlgData.Add(0);
            TlgData.Add(0);
        }

        /***************************************************************************
        SPECIFICATION: Extraction of TCP/IP info
        CREATED:       19.10.2020
        LAST CHANGE:   02.01.2025
        ***************************************************************************/
        public void Parse( )
        {
            Vlan      = 0;
            int  ix   = 0;
            int  offs = ix + 2;
            uint hlp  = 0;

            try
            {
                switch( EthType )
                {
                    case 0x8100:
                        Vlan   = mem.GetMem2( ref ix );
                        IpType = mem.GetMem2( ref ix );
                        switch( IpType )
                        {
                            case 0x800: offs += 4; break;

                            case 0x806:  // ARP 
                                HwType   = mem.GetMem2( ref ix );
                                ProtType = mem.GetMem2( ref ix );
                                HwSize   = mem.GetMem1( ref ix );
                                ProtSize = mem.GetMem1( ref ix );
                                OpCdReq  = mem.GetMem2( ref ix );
                                mem.ByteListToByteList( SenderMAC, mem.Mem, ref ix, 6 ); 
                                mem.ByteListToByteList( SenderIP , mem.Mem, ref ix, 4 ); 
                                mem.ByteListToByteList( TargetMAC, mem.Mem, ref ix, 6 ); 
                                mem.ByteListToByteList( TargetIP , mem.Mem, ref ix, 4 ); 
                                return;

                            case 0x86dd: // IP6
                                hlp      = mem.GetMem4( ref ix );
                                Version  = (byte)(hlp >> 28);
                                TrfcClss = (byte)((hlp >> 20) & 0xFF);
                                FlowLbl  = hlp & 0xFFFFF;
                                PyldLen  = mem.GetMem2( ref ix );
                                NxtHdr   = mem.GetMem1( ref ix ); 
                                HopLimit = mem.GetMem1( ref ix );
                                Src.CopyIP6( mem.Mem, ref ix );
                                Dst.CopyIP6( mem.Mem, ref ix );
                                Protcl = NxtHdr;
                                break;
                        }
                        break;

                    case 0x86dd: // IP6
                        hlp      = mem.GetMem4( ref ix );
                        Version  = (byte)(hlp >> 28);
                        TrfcClss = (byte)((hlp >> 20) & 0xFF);
                        FlowLbl  = hlp & 0xFFFFF;
                        PyldLen  = mem.GetMem2( ref ix );
                        NxtHdr   = mem.GetMem1( ref ix ); 
                        HopLimit = mem.GetMem1( ref ix );
                        Src.CopyIP6( mem.Mem, ref ix );
                        Dst.CopyIP6( mem.Mem, ref ix );
                        Protcl = NxtHdr;
                        break;

                    case 0x800:
                        break;

                    case 0x806: // ARP
                        break;
                        
                    default:
                        EthType = mem.GetMem2( ref ix );
                        switch( EthType )
                        {
                            case 0x8100:
                            case 0x81:
                                Vlan = mem.GetMem2( ref ix );
                                offs += 4;
                                break;

                            case 0x806:
                                break;

                            default:
                                EthType = 0;
                                Vlan    = 0;

                                switch( (ETHPROT)Protcl )
                                {
                                    case ETHPROT.EP_TCP:
                                        offs += 2;
                                        break;

                                    case ETHPROT.EP_UDP:
                                        break;

                                    case ETHPROT.EP_IGMPV3:
                                        break;
                                }
                                break;
                        }
                        break;
                }

                if ( EthType != 0x86dd && IpType != 0x86dd )
                {
                    ix = offs;

                    TotLen   = mem.GetMem2( ref ix );
                    Ident    = mem.GetMem2( ref ix );
                    FragOffs = mem.GetMem2( ref ix );
                    Tme2Live = mem.GetMem1( ref ix );
                    Protcl   = mem.GetMem1( ref ix );
                    IpCrc    = mem.GetMem2( ref ix );
                    Src.CopyIP( mem.Mem, ref ix );
                    Dst.CopyIP( mem.Mem, ref ix ); 
                }

                switch( (ETHPROT)Protcl )
                {
                    case ETHPROT.EP_TCP:
                        Src.Port = mem.GetMem2( ref ix );
                        Dst.Port = mem.GetMem2( ref ix );
                        SeqNr    = (int)mem.GetMem4( ref ix );
                        AckNr    = (int)mem.GetMem4( ref ix );
                        DatOffs  = mem.GetMem1( ref ix ); 
                        Flags    = mem.GetMem1( ref ix );
                        Window   = mem.GetMem2( ref ix );
                        PrtCrc   = mem.GetMem2( ref ix );
                        UrgPtr   = mem.GetMem2( ref ix );

                        int hdlen = (DatOffs >> 2);
                        int optln = hdlen - 20;
                        Opts.Clear(); 
                        Opts.AddRange( mem.GetMemList( ref ix, optln ) );

                        int seqlen = IsIP6 ? PyldLen - (DatOffs >> 2) : TotLen - ix - 8 + optln;
                        if ( seqlen > 0 )
                        {
                            StAckNr = SeqNr + seqlen;
                            StSeqNr = AckNr;
                        }
                        break;

                    case ETHPROT.EP_UDP:
                        Src.Port = mem.GetMem2( ref ix );
                        Dst.Port = mem.GetMem2( ref ix );
                        UdpLen   = mem.GetMem2( ref ix );
                        PrtCrc   = mem.GetMem2( ref ix );
                        break;

                    case ETHPROT.EP_IGMPV3:
                        ix+=4;      // Options
                        MemRep   = mem.GetMem1( ref ix );
                        ix++;       // Reserved
                        PrtCrc   = mem.GetMem2( ref ix );
                        break;

                    case ETHPROT.EP_ICMPV6:
                        PrtType  = mem.GetMem1( ref ix );  // NDP etc.
                        Code     = mem.GetMem1( ref ix );  
                        PrtCrc   = mem.GetMem2( ref ix );
                        ix+=4;
                        TargIP6.CopyIP6 ( mem.Mem, ref ix );
                        OptType  = mem.GetMem1( ref ix );  
                        OptLen   = mem.GetMem1( ref ix );  
                        mem.ByteListToByteList( Src.Mac, mem.Mem, ref ix, 6 ); 
                        break;

                    default: return;
                }

                Data.AddRange( mem.GetMemList( ref ix ) );
            }
            catch( Exception ex)
            {
                MessageBox.Show( ex.Message, "Error in telegram parser" );
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.01.2021
        LAST CHANGE:   01.02.2021
        ***************************************************************************/
        public bool IsArpNdp( List<byte> a_TxAddr )
        {
            mem.AssignMem( ref TlgData );
            int ix = 0;

            ushort vlan  = mem.GetMem2( ref ix );
            ushort type  = mem.GetMem2( ref ix );

            switch ( type )
            {
                case 0x0806: // arp
                    ix += 14;
                    byte[] ip = mem.GetMemList( ref ix, 4 );
                    return mem.ByteListCompare( a_TxAddr, new List<byte>(ip) );

                case 0x86dd: // ndp
                    ix = 10;
                    byte nxthdr = mem.GetMem1( ref ix );
                    if ( nxthdr != 58 ) return false;
                    ix += 33;
                    byte ndp    = mem.GetMem1( ref ix );
                    if ( ndp < 132 || ndp > 137 )   return false;
                    return true;
            }
            return false;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.11.2020
        LAST CHANGE:   26.01.2021
        ***************************************************************************/
        public bool HasAck() { return (Flags & (byte)TCPFLGS.TF_ACK) != 0; }
        public bool HasSyn() { return (Flags & (byte)TCPFLGS.TF_SYN) != 0; }
        public bool HasPsh() { return (Flags & (byte)TCPFLGS.TF_PSH) != 0; }
        public bool HasRst() { return (Flags & (byte)TCPFLGS.TF_RST) != 0; }
        public bool HasFin() { return (Flags & (byte)TCPFLGS.TF_FIN) != 0; }
    } // class 

    /***************************************************************************
    SPECIFICATION: EthMsg sub class
    CREATED:       19.10.2020
    LAST CHANGE:   21.11.2022
    ***************************************************************************/
    public class EthAddrs
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       17.07.2025
        LAST CHANGE:   17.07.2025
        ***************************************************************************/
        public bool IsBrdcst { get { return Utils.ByteListCmp( Mac, Utils.HexStr2ByteList("FFFFFFFFFFFF") ); } }


        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       19.10.2020
        LAST CHANGE:   17.07.2025
        ***************************************************************************/
        private Memory mem;

        public List<byte>   Mac    ;
        public List<byte>   Ip     ;
        public List<ushort> Ip6    ;
        public List<ushort> MltiCst;
        public ushort       Port   ;
        
        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       19.10.2020
        LAST CHANGE:   21.11.2022
        ***************************************************************************/
        public EthAddrs()
        {
            mem  = new Memory();
            Mac  = new List<byte>();
            Ip   = new List<byte>();
            Ip6  = new List<ushort>();
            Port = 0;

            MltiCst = new List<ushort>();
            MltiCst.Add( 0xff02 );
            for (int i=0; i<6; i++) MltiCst.Add(0);
            MltiCst.Add( 1 );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2022
        LAST CHANGE:   14.10.2022
        ***************************************************************************/
        public bool IsIp6()
        {
            if ( Ip6.Count > 0 ) return true;
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.10.2023
        LAST CHANGE:   19.10.2023
        ***************************************************************************/
        public bool IsIpZero()
        {
            if ( IsIp6() )  foreach( ushort u in Ip6 ) if ( u != 0 ) return false;
            else            foreach( byte   b in Ip  ) if ( b != 0 ) return false;

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.11.2022
        LAST CHANGE:   17.10.2023
        ***************************************************************************/
        public bool IsIp6Multicast()
        {
            //for( int i=0; i<8; i++ )
            //{
            //    if ( Ip6[i] != MltiCst[i] ) return false;
            //}

            if ( Ip6[0] != 0xff02 ) return false;
            if ( Ip6[5] != 0x0001 ) return false;
            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.10.2020
        LAST CHANGE:   15.05.2023
        ***************************************************************************/
        public void Copy( EthAddrs a_Src, bool a_IncMac = true )
        {
            if ( a_IncMac ) CopyMAC( a_Src.Mac );
            CopyIP ( a_Src.Ip  );
            CopyIP6( a_Src.Ip6 );
            Port   = a_Src.Port;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.10.2020
        LAST CHANGE:   17.11.2020
        ***************************************************************************/
        public void CopyIP( List<byte> a_Pyld, ref int a_Idx )
        {
            try
            {
                mem.AssignMem( a_Pyld );

                Ip.Clear();
                Ip.AddRange( mem.GetMemList( ref a_Idx, 4 ) );
            }
            catch( Exception ex )
            {
                string exp = ex.Message;
                MessageBox.Show(exp, "Exception in CopyIP" );
            }
        }

        public void CopyIP( List<byte> a_SrcIp )
        {
            Ip.Clear();
            Ip.AddRange( a_SrcIp );
        }

        public void CopyIP( string a_SrcIP )
        {
            Ip.Clear();
            List<string> segs = Utils.SplitExt( a_SrcIP, "." );
            foreach( string s in segs )
            {
                Ip.Add( Utils.Str2Byte(s) );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2022
        LAST CHANGE:   28.10.2022
        ***************************************************************************/
        public void CopyIP6( List<byte> a_Pyld, ref int a_Idx )
        {
            try
            {
                mem.AssignMem( a_Pyld );

                Ip6.Clear();
                Ip6.AddRange( mem.GetMemList2( ref a_Idx, 16 ) );
            }
            catch( Exception ex )
            {
                string exp = ex.Message;
                MessageBox.Show(exp, "Exception in CopyIP6" );
            }
        }

        public void CopyIP6( List<ushort> a_SrcIp )
        {
            Ip6.Clear();
            Ip6.AddRange( a_SrcIp );
        }

        public void CopyIP6( string a_SrcIP )
        {
            Ip6 = Utils.IPv6Adr2Lst( a_SrcIP );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.01.2021
        LAST CHANGE:   28.10.2022
        ***************************************************************************/
        public void CopyMAC( List<byte> a_SrcMAC )
        {
            Mac.Clear();
            Mac.AddRange( a_SrcMAC );
        }

        public void CopyMAC( byte[] a_SrcMAC )
        {
            Mac.Clear();
            Mac.AddRange( a_SrcMAC );
        }

        public void CopyMAC( string a_SrcMac )
        {
            Utils.Mac2Lst( a_SrcMac, ref Mac );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.10.2020
        LAST CHANGE:   19.10.2020
        ***************************************************************************/
        public bool CompareIP( List<byte> a_Ip )
        {
            return mem.ByteListCompare( Ip, a_Ip );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2022
        LAST CHANGE:   14.10.2022
        ***************************************************************************/
        public bool CompareIP6( List<ushort> a_Ip )
        {
            if ( a_Ip.Count != Ip6.Count ) return false;

            for( int i = 0; i< a_Ip.Count; i++ )
            {
                if ( a_Ip[i] != Ip6[i] ) return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.12.2023
        LAST CHANGE:   01.12.2023
        ***************************************************************************/
        public bool CompareIP6( string a_Ip )
        {
            List<ushort> lst = Utils.IPv6Adr2Lst( a_Ip );

            if ( lst.Count != Ip6.Count ) return false;

            for( int i = 0; i< lst.Count; i++ )
            {
                if ( lst[i] != Ip6[i] ) return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.02.2021
        LAST CHANGE:   04.02.2021
        ***************************************************************************/
        public bool CompareMAC( List<byte> a_Mac )
        {
            return mem.ByteListCompare( Mac, a_Mac );
        }

        public bool CompareMAC( byte[] a_Mac )
        {
            List<byte> mac = new List<byte>(a_Mac);
            return mem.ByteListCompare( Mac, mac );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.02.2021
        LAST CHANGE:   14.10.2022
        ***************************************************************************/
        public string GetStr()
        {
            if (IsIp6()) return string.Format( "{0} - {1}:{2}", Utils.ShowMac( Mac.ToArray() ), Utils.UshortArr2Ip6(Ip6.ToArray()), Port );

            return string.Format( "{0} - {1}:{2}", Utils.ShowMac( Mac.ToArray() ), Utils.ByteArr2Ip(Ip.ToArray()), Port );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.11.2021
        LAST CHANGE:   15.11.2021
        ***************************************************************************/
        public bool HasMac()
        {
            if ( Mac.Count < 6 ) return false;
            return true;
        }

    } // class

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       05.11.2022
    LAST CHANGE:   13.12.2022
    ***************************************************************************/
    public class ConnectParms
    {
        public bool     uds      ;
        public bool     xcp      ;
        public bool     xcpbl    ;
        public bool     someip   ;
        public bool     restbus  ;
        public bool     force    ;
        public bool     connect  ;
        public string   ecu      ;
        public byte     doipv    ;
        public ushort   vlan     ;

        public static bool button  = false ;

        public IntfceParms ifceprms ;

        public ConnectParms()
        {
            uds      = false;
            xcp      = false;
            xcpbl    = false;
            someip   = false;
            force    = false;
            connect  = false;
            doipv    = 0;
            ecu      = "none";
            //button   = false;
        }

        public ConnectParms( bool a_Uds, string a_Ecu )
            :this()
        {
            uds = a_Uds;
            ecu = a_Ecu;
        }

        public ConnectParms( bool a_Uds, string a_Ecu, byte a_DoipV )
            :this()
        {
            uds   = a_Uds;
            ecu   = a_Ecu;
            doipv = a_DoipV;
        }

        public ConnectParms( bool a_Uds, bool a_Xcp, bool a_SomeIp, string a_Ecu, IntfceParms a_IfcPrms )
            :this()
        {
            uds      = a_Uds;
            xcp      = a_Xcp;
            someip   = a_SomeIp;
            ecu      = a_Ecu;
            ifceprms = a_IfcPrms;
        }

        public ConnectParms( ConnectParms a_Src )
        {
            if ( a_Src == null ) return;
            Copy( a_Src );
        }


        //************************************************* 24.06.2025 **************
        public void Copy( ConnectParms a_Src )
        {
            uds      = a_Src.uds      ;
            doipv    = a_Src.doipv    ;
            xcp      = a_Src.xcp      ;
            xcpbl    = a_Src.xcpbl    ;
            someip   = a_Src.someip   ;
            restbus  = a_Src.restbus  ;
            connect  = a_Src.connect  ;
            ecu      = a_Src.ecu      ;
            ifceprms = a_Src.ifceprms ;
            force    = a_Src.force    ;
        }
    }


}  // namespace
