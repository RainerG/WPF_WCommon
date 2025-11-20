using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NS_Trace;

namespace NS_Utilities
{ 
    /***************************************************************************
    SPECIFICATION: Global types / shared by all UDS related protocol classes 
    CREATED:       12.08.2015
    LAST CHANGE:   26.01.2023
    ***************************************************************************/
    public enum ProtTyp
    {
        PT_RoutReq,
        PT_UDS
    }

    public class UdsData
    {
        public int TlgLen    { get { return Data.Count; } }

        public uint        TxNodeId;
        public uint        RxNodeId;
        public UInt16      CtrlWord;
        public int         NrBytes;
        public int         Length;
        public uint        DLC;
        public byte        Service;
        public byte        ReqService;
        public byte        DoipV;
        public byte        RspLen;
        public List<byte>  Data;
        public byte        TlgCnt;
        public byte        AckCd;
        public ushort      PrevMsg;
        //public string      Test;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       12.08.2015
        LAST CHANGE:   26.01.2023
        ***************************************************************************/
        public UdsData()
        {
            Data     = new List<byte>();
            CtrlWord = 1;
            Length   = 2;
            DLC      = 8;
            //Test     = "test";
        }

        public UdsData( UdsData a_Src )
            : this()
        {
            Copy( a_Src );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   26.01.2023
        ***************************************************************************/
        public void Copy( UdsData dat )
        {
            TxNodeId = dat.TxNodeId ;
            RxNodeId = dat.RxNodeId ;
            CtrlWord = dat.CtrlWord ;
            Length   = dat.Length   ;
            DoipV    = dat.DoipV    ;
            DLC      = dat.DLC      ;

            Data.Clear();
            Data.AddRange(dat.Data);
        }

        public void Clear()
        {
            Length  = 0;
            Data  .Clear();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   10.04.2024
        ***************************************************************************/
        public List<byte> CreateEthTlg( bool a_InclZero, bool a_RestBus = false )
        {
            List<byte> ret = new List<byte>();

            Length = Data.Count;

            if ( ! a_RestBus )
            {
                if (DoipV > 0)
                {
                    Length+=4;
                    Utils.U8ToByteList ( DoipV              , ref ret );
                    Utils.U8ToByteList ( (byte)(0xff-DoipV) , ref ret );
                    Utils.U16ToByteList( 0x8001             , ref ret );
                    Utils.I32ToByteList( Length             , ref ret );
                    Utils.U16ToByteList( (ushort)RxNodeId   , ref ret );
                    Utils.U16ToByteList( (ushort)TxNodeId   , ref ret );
                }                                   
                else                                
                {       
                    if ( TxNodeId > 0xff )
                    {
                        Utils.U32ToByteList( TxNodeId       , ref ret );
                        Utils.I32ToByteList( Length         , ref ret );
                    }
                    else
                    {
                        Length += 2;
                        Utils.I32ToByteList( Length         , ref ret );
                        Utils.U16ToByteList( CtrlWord       , ref ret );
                        Utils.U8ToByteList ( (byte)RxNodeId , ref ret );
                        Utils.U8ToByteList ( (byte)TxNodeId , ref ret );
                    }
                }
            }

            if ( Data.Count > 0 ) ret.AddRange( Data );

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   07.11.2022
        ***************************************************************************/
        public List<byte> CreateRoutReq()
        {
            List<byte> ret = new List<byte>();

            Length = 7;

            Utils.U8ToByteList ( DoipV              , ref ret );
            Utils.U8ToByteList ( (byte)(0xff-DoipV) , ref ret );
            Utils.U16ToByteList( 5                  , ref ret );
            Utils.I32ToByteList( Length             , ref ret );
            Utils.U16ToByteList( (ushort)RxNodeId   , ref ret );
            Utils.U16ToByteList( 0                  , ref ret );
            Utils.U8ToByteList ( 0                  , ref ret );
            Utils.U16ToByteList( 0                  , ref ret );

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   26.01.2023
        ***************************************************************************/
        public List<byte> CreateCanTlg()
        {
            List<byte> ret = new List<byte>();

            TlgCnt = 0;

            Length += Data.Count;

            int len = Length;

            if (TxNodeId != 0) 
            {
                Utils.U8ToByteList ( (byte)TxNodeId , ref ret );
                len++;
            }

            if (len > DLC)
            {
                ushort ln = (ushort)Length;
                ln &= 0x0FFF;
                ln |= 0x1000;
                Utils.U16ToByteList( ln, ref ret );
                TlgCnt ++;
            }
            else 
            {
                Utils.U8ToByteList ( (byte)Length, ref ret );
            }

            //Utils.U8ToByteList ( Service , ref ret );
            //SubCmd2ByteList    ( SubCmd  , ref ret );

            for (int i = ret.Count; i<DLC; i++)
            {
                if (Data.Count > 0) 
                {
                    ret.Add(Data[0]);
                    Data.RemoveAt(0);
                }
                else ret.Add(0);
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.08.2017
        LAST CHANGE:   10.01.2019
        ***************************************************************************/
        public List<byte> CreateConsecCanTlg()
        {
            byte hdr = (byte)(0x20 + TlgCnt++);

            List<byte> ret = new List<byte>();
            
            if (TxNodeId != 0) Utils.U8ToByteList( (byte)TxNodeId, ref ret );

            Utils.U8ToByteList ( hdr , ref ret );

            for (int i = ret.Count; i<DLC; i++)
            {
                if (Data.Count > 0) 
                {
                    ret.Add(Data[0]);
                    Data.RemoveAt(0);
                }
                else ret.Add(0);
            }

            if (Data.Count == 0) TlgCnt = 0;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.06.2017
        LAST CHANGE:   10.08.2020
        ***************************************************************************/
        private void SubCmd2ByteList( List<byte> a_Subcmd, ref List<byte> a_Data ) { SubCmd2ByteList( a_Subcmd, ref a_Data, false ); }
        private void SubCmd2ByteList( List<byte> a_Subcmd, ref List<byte> a_Data, bool a_InclZro )
        {
            if (a_Subcmd.Count > 1 || a_InclZro ) 
            {
                a_Data.AddRange(a_Subcmd);
                return;
            }

            if (a_Subcmd.Count == 1)
            {
                byte b = a_Subcmd[0];
                if (b != 0) a_Data.Add(b);
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   ?
        ***************************************************************************/
        private List<string> Refract( string a_Data )
        {
            List<string> sbts = new List<string>();

            string bt = "";

            foreach( char c in a_Data )
            {
                if ( Utils.IsHex(c) ) bt += c;
                if ( bt.Length >= 2 )
                {
                    sbts.Add(bt);
                    bt = "";
                }
            }

            return sbts;
        }

    } // UdsData

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       ?
    LAST CHANGE:   30.06.2021
    ***************************************************************************/
    public class UDS_GuiParms
    {
        public CanParams Can;
        public EthParams Eth;

        public string CanEth;
        public bool   RxExt;
        public bool   TxExt;
        public bool   RawTlg;
        public bool   TstPrsnt;
        public uint   TxNode;
        public uint   RxNode;
        public int    DoipV;
        public int    UdsBtchDly;

        public UDS_GuiParms()
        {
            CanEth     = "et";
            RxExt      = false;
            TxExt      = false;
            RawTlg     = false;
            TstPrsnt   = false;
            TxNode     = 0;
            RxNode     = 0;
            DoipV      = 0;
            Can        = new CanParams();
            Eth        = new EthParams();
            UdsBtchDly = 0;
        }
    }

    public class UDS_Params
    {
        public UDS_GuiParms GuiPrms;
        public UDS_Params()
        {
            GuiPrms = new UDS_GuiParms();
        }
    }

} // Namespace
