using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_WUtilities
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       02.10.2019
    LAST CHANGE:   02.10.2019
    ***************************************************************************/
    public class Memory
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       02.10.2019
        LAST CHANGE:   02.10.2019
        ***************************************************************************/
        public List<Byte>        Mem         { get { return m_Mem;  } }
        private List<List<byte>> MemStck     { get { return m_MemStck;  } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       02.10.2019
        LAST CHANGE:   20.11.2019
        ***************************************************************************/
        protected List<Byte>     m_Mem;
        private List<List<byte>> m_MemStck;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       02.10.2019
        LAST CHANGE:   20.11.2019
        ***************************************************************************/
        public Memory()
        {
            m_Mem     = null;
            m_MemStck = new List<List<byte>>();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.04.2024
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        public void Copy( Memory a_Src )
        {
            //m_Mem.Clear();
            //m_Mem.AddRange( a_Src.Mem );
            m_Mem = a_Src.Mem;
            m_MemStck.Clear();
            foreach( List<byte> bl in a_Src.MemStck )
            {
                m_MemStck.Add( new List<byte>( bl ) );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.05.2015
        LAST CHANGE:   13.02.2020
        ***************************************************************************/
        public void AssignMem( ref List<Byte> a_Mem ) { m_Mem = a_Mem; }
        public void AssignMem( List<Byte> a_Mem )     { m_Mem = a_Mem; }
        public void AssignMem( byte[] a_Mem )         { m_Mem = new List<byte>( a_Mem ); }
        public void AssignMem( ref byte[] a_Mem )     { m_Mem = new List<byte>( a_Mem ); }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.09.2016
        LAST CHANGE:   26.09.2016
        ***************************************************************************/
        public byte[] GetSubMem( int a_Start, int a_End )
        {
            byte[] ba = new byte[a_End - a_Start];

            if (m_Mem.Count - a_Start <= ba.Length) return null;

            m_Mem.CopyTo( a_Start, ba, 0, a_End - a_Start );

            return ba;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.11.2018
        LAST CHANGE:   29.04.2020
        ***************************************************************************/
        public void SaveMem()
        {
            m_MemStck.Insert( 0, m_Mem );
        }

        public void RecallMem()
        {
            if ( m_MemStck.Count == 0 ) return;
            m_Mem = m_MemStck[0];
            m_MemStck.RemoveAt(0);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.04.2015
        LAST CHANGE:   28.04.2015
        ***************************************************************************/
        public UInt32 GetMem4( ref int idx )
        {
            if (m_Mem.Count <= idx+3 ) return 0;

            UInt32 ret = 0;
            for( int i = 0; i < 4; i++ )
            {
                ret += m_Mem[idx++];
                if (i<3) ret <<= 8;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.12.2015
        LAST CHANGE:   21.12.2015
        ***************************************************************************/
        public UInt64 GetMem8( ref int idx )
        {
            if (m_Mem.Count <= idx+7 ) return 0;

            UInt64 ret = 0;
            for( int i = 0; i < 8; i++ )
            {
                ret += m_Mem[idx++];
                if (i<7) ret <<= 8;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.04.2015
        LAST CHANGE:   28.04.2015
        ***************************************************************************/
        public UInt16 GetMem2( ref int idx )
        {
            if (m_Mem.Count <= idx+1 ) return 0;

            UInt16  ret   = m_Mem[idx++];
                    ret <<= 8;
                    ret  += m_Mem[idx++];

            return ret;
        } 

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.07.2015
        LAST CHANGE:   03.02.2016
        ***************************************************************************/
        public Byte GetMem1( ref int idx )
        {
            if (m_Mem.Count <= idx ) return 0;
            return m_Mem[idx++];
        } 

        public SByte GetMem1S( ref int idx )
        {
            if (m_Mem.Count <= idx ) return 0;
            return (SByte)m_Mem[idx++];
        } 

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.07.2015
        LAST CHANGE:   05.07.2015
        ***************************************************************************/
        public UInt32 GetMem3( ref int idx )
        {
            if (m_Mem.Count <= idx+2 ) return 0;

            UInt32 ret = 0;
            for( int i = 0; i < 3; i++ )
            {
                ret += m_Mem[idx++];
                if (i<2) ret <<= 8;
            }

            return ret;
        } 

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.05.2021
        LAST CHANGE:   25.05.2021
        ***************************************************************************/
        public UInt32 GetMem3Swp( ref int idx )
        {
            if (m_Mem.Count <= idx+2 ) return 0;

            UInt32 ret = 0;
            for( int i = 0; i < 3; i++ )
            {
                ret += m_Mem[idx++];
                if (i<2) ret <<= 8;
            }

            ret = SwapBytes32( ret );
            ret >>= 8;
            return ret;
        } 

        /***************************************************************************
        SPECIFICATION: Polymorphic calls
        CREATED:       12.09.2015
        LAST CHANGE:   11.06.2025
        ***************************************************************************/
        public void GetMem ( ref UInt64 var, ref int idx ) { var =          GetMem8    (ref idx); }
        public void GetMem ( ref UInt32 var, ref int idx ) { var =          GetMem4    (ref idx); }
        public void GetMem ( ref Int32  var, ref int idx ) { var = (Int32)  GetMem4    (ref idx); }
        public void GetMem ( ref UInt16 var, ref int idx ) { var =          GetMem2    (ref idx); }
        public void GetMem ( ref Int16  var, ref int idx ) { var = (Int16)  GetMem2    (ref idx); }
        public void GetMem ( ref byte   var, ref int idx ) { var =          GetMem1    (ref idx); }
        public void GetMem ( ref bool   var, ref int idx ) { var =          GetMem1    (ref idx) == 0 ? false : true; }
        public void GetMem ( ref sbyte  var, ref int idx ) { var =          GetMem1S   (ref idx); }
        public void GetMem ( ref float  var, ref int idx ) { var =          GetMemFloat(ref idx); }
        public void GetMem3   ( ref UInt32 var, ref int idx ) { var =       GetMem3    (ref idx); }
        public void GetMem3   ( ref Int32  var, ref int idx ) { var = (int) GetMem3    (ref idx); }
        public void GetMem3Swp( ref UInt32 var, ref int idx ) { var =       GetMem3Swp (ref idx); }
        public void GetMem3Swp( ref int    var, ref int idx ) { var = (int) GetMem3Swp (ref idx); }

        public void GetMemSwp ( ref byte    var, ref int idx ) { var =       GetMem1    (ref idx); }
        public void GetMemSwp ( ref bool    var, ref int idx ) { var =       GetMem1    (ref idx) == 0 ? false : true; }
        public void GetMemSwp ( ref sbyte   var, ref int idx ) { var =(sbyte)GetMem1    (ref idx); }
        public void GetMemSwp ( ref UInt64  var, ref int idx ) { var =       GetMem8Swp (ref idx); }
        public void GetMemSwp ( ref Int32   var, ref int idx ) { var =  (int)GetMem4Swp (ref idx); }
        public void GetMemSwp ( ref UInt32  var, ref int idx ) { var =       GetMem4Swp (ref idx); }
        public void GetMemSwp ( ref UInt16  var, ref int idx ) { var =       GetMem2Swp (ref idx); }
        public void GetMemSwp ( ref Int16   var, ref int idx ) { var =(Int16)GetMem2Swp (ref idx); }
        public void GetMemSwp ( ref float   var, ref int idx ) { var =       GetMemFloatSwp (ref idx); }
        public void GetMemSwp ( ref byte[]  var, ref int idx ) { GetMem( ref var, ref idx ); } 

        public void GetMem ( ref byte       var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref bool       var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref sbyte      var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref UInt64     var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref Int32      var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref UInt32     var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem3( ref UInt32     var, ref int idx, bool swp ) { if(swp) GetMem3Swp(ref var, ref idx); else GetMem3(ref var, ref idx); }
        public void GetMem3( ref Int32      var, ref int idx, bool swp ) { if(swp) GetMem3Swp(ref var, ref idx); else GetMem3(ref var, ref idx); }
        public void GetMem ( ref UInt16     var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref Int16      var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref float      var, ref int idx, bool swp ) { if(swp) GetMemSwp (ref var, ref idx); else GetMem (ref var, ref idx); }
        public void GetMem ( ref byte[]     var, ref int idx, bool swp ) { GetMem(ref var, ref idx); }
        public void GetMem ( ref short[]    var, ref int idx, bool swp ) { if(swp) GetMemSwp(ref var, ref idx); else GetMem(ref var, ref idx); }
        public void GetMem ( ref uint[]     var, ref int idx, bool swp ) { if(swp) GetMemSwp(ref var, ref idx); else GetMem(ref var, ref idx); }
        public void GetMem ( ref ulong[]    var, ref int idx, bool swp ) { if(swp) GetMemSwp(ref var, ref idx); else GetMem(ref var, ref idx); }
        public void GetMem ( ref float[]    var, ref int idx, bool swp ) { if(swp) GetMemSwp(ref var, ref idx); else GetMem(ref var, ref idx); }
        public void GetMem ( ref List<uint> var, ref int idx, bool swp ) { if(swp) GetMemSwp(ref var, ref idx); else GetMem(ref var, ref idx); }

        public void GetMem ( ref byte[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ ) var[i] = GetMem1( ref idx );
        }

        public void GetMem ( ref byte[,] var, ref int idx, int ln1, int ln2 ) 
        { 
            for( int i=0; i<ln1; i++ )
            {
                for ( int j=0; j<ln2; j++ ) var[i,j] = GetMem1( ref idx );
            }
        }

        public void GetMem ( ref char[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = (char)GetMem1( ref idx );
            }
        }

        public void GetMem ( ref ushort[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = GetMem2( ref idx );
            }
        }

        public void GetMem ( ref short[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = (short)GetMem2( ref idx );
            }
        }

        public void GetMem ( ref List<uint> var, ref int idx ) 
        { 
            for( int i=0; i<var.Count; i++ )
            {
                var[i] = GetMem4( ref idx );
            }
        }

        public void GetMem ( ref uint[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = GetMem4( ref idx );
            }
        }

        public void GetMem ( ref ulong[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = GetMem8( ref idx );
            }
        }

        public void GetMem ( ref int[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = (int)GetMem4( ref idx );
            }
        }

        public void GetMem ( ref int[] var, ref int idx, bool swp ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = (int)GetMem4Swp( ref idx );
            }
        }

        public void GetMem ( ref float[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = GetMemFloat( ref idx );
            }
        }

        public void GetMem ( ref string var, int len,  ref int idx ) 
        { 
            var = "";
            for( int i=0; i<len; i++ )
            {
                var += (char)GetMem1( ref idx );
            }
        }

        public void GetMemSwp ( ref float[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = GetMemFloatSwp( ref idx );
            }
        }

        public void GetMemSwp( ref List<uint> var, ref int idx ) 
        { 
            for( int i=0; i<var.Count; i++ )
            {
                var[i] = GetMem4Swp( ref idx );
            }
        }

        public void GetMemSwp( ref uint[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = GetMem4Swp( ref idx );
            }
        }

        public void GetMemSwp( ref ulong[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = GetMem8Swp( ref idx );
            }
        }

        public void GetMemSwp( ref short[] var, ref int idx ) 
        { 
            for( int i=0; i<var.Length; i++ )
            {
                var[i] = (short)GetMem2Swp( ref idx );
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   27.06.2022
        ***************************************************************************/
        public string GetMemAscii( ref int idx, int len )
        {
            int ix = idx;
            string ret = "";

            for ( int i=0; i<len; i++ ) 
            {
                if ( idx >= m_Mem.Count ) break;
                byte b = m_Mem[idx++];
                if ( b==0 )
                {
                    ret += " ";
                    continue;
                }
                ret += b < 0x20 || b > 0x7f ? '.' : (char)b;
            }
            idx = ix + len;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.08.2015
        LAST CHANGE:   02.11.2020
        ***************************************************************************/
        public float GetMemFloat( ref int idx )
        {
            if ( m_Mem.Count < 4 )       return 0;
            if ( m_Mem.Count - idx < 4 ) return 0;

            byte[] barr = new byte[4];

            m_Mem.Reverse(idx,4);
            m_Mem.CopyTo (idx,barr,0,4);
            m_Mem.Reverse(idx,4);

            idx += 4;

            float test = BitConverter.ToSingle( barr, 0 );

            return test;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.07.2019
        LAST CHANGE:   02.11.2020
        ***************************************************************************/
        public float GetMemFloatSwp( ref int idx )
        {
            if ( m_Mem.Count < 4 )       return 0;
            if ( m_Mem.Count - idx < 4 ) return 0;

            byte[] barr = new byte[4];

            m_Mem.CopyTo (idx,barr,0,4);

            idx += 4;

            float test = BitConverter.ToSingle( barr, 0 );

            return test;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.05.2015
        LAST CHANGE:   24.11.2016
        ***************************************************************************/
        public UInt16 SwapBytes16(UInt16 value)
        {
          return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public UInt64 SwapBytes64(UInt64 value)
        {
            UInt64 val = value;
            UInt64 swp = ((0x00000000000000FF) & (val >> 56)   |
                          (0x000000000000FF00) & (val >> 40)   |
                          (0x0000000000FF0000) & (val >> 24)   |
                          (0x00000000FF000000) & (val >>  8)   |
                          (0x000000FF00000000) & (val <<  8)   |
                          (0x0000FF0000000000) & (val << 24)   |
                          (0x00FF000000000000) & (val << 40)   |
                          (0xFF00000000000000) & (val << 56));  
            return swp;
        }

        public UInt32 SwapBytes32(UInt32 value)
        {
          return (value & 0x000000FFU) << 24 | 
                 (value & 0x0000FF00U) << 8  |
                 (value & 0x00FF0000U) >> 8  | 
                 (value & 0xFF000000U) >> 24;
        }

        public UInt64 GetMem8Swp( ref int idx )
        {
#if true
            UInt64 ret = GetMem8( ref idx );
            return SwapBytes64(ret);

            return SwapBytes64( ret );
#else
            uint ret1 = GetMem4( ref idx );
            uint ret2 = GetMem4( ref idx );

            UInt64 ret  = SwapBytes32(ret1) << 32;
                   ret |= SwapBytes32(ret2);

            return ret;
#endif
        }

        public UInt32 GetMem4Swp( ref int idx )
        {
            UInt32 ret = GetMem4( ref idx );
            return SwapBytes32(ret);
        }

        public UInt16 GetMem2Swp( ref int idx )
        {
            UInt16 ret = GetMem2( ref idx );
            return SwapBytes16(ret);
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.02.2021
        LAST CHANGE:   08.12.2023
        ***************************************************************************/
        public byte[] GetMemList( ref int idx )
        {
            int len = m_Mem.Count; 
            return GetMemList( ref idx, len - idx );
        }
        public byte[] GetMemList( ref int idx, int count )
        {
            if ( idx < 0 ) return new byte[0];

            int cnt = count;
            if ( (idx + cnt) > m_Mem.Count ) cnt = m_Mem.Count - idx;
            if ( cnt < 0 )                   cnt = 0;
            byte[] ret = new byte[cnt];

            if ( cnt > 0 ) m_Mem.CopyTo( idx, ret, 0, cnt);

            idx += cnt;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2022
        LAST CHANGE:   18.10.2022
        ***************************************************************************/
        public ushort[] GetMemList2( ref int idx )
        {
            int len = m_Mem.Count; 
            return GetMemList2( ref idx, len - idx );
        }
        public ushort[] GetMemList2( ref int idx, int count )
        {
            int cnt = count;
            if ( (idx + cnt) > m_Mem.Count ) cnt = m_Mem.Count - idx;
            if ( cnt < 0 )                   cnt = 0;
            byte[] ret = new byte[cnt];

            if ( cnt > 0 ) m_Mem.CopyTo( idx, ret, 0, cnt );

            idx += cnt;

            List<ushort> ret2 = new List<ushort>();

            int    i=0;
            ushort u=0;
            foreach( byte b in ret )
            {
                if ( i++ % 2 == 0 ) 
                {
                    u = b;
                    u <<= 8;
                }
                else
                {
                    u += b;
                    ret2.Add(u);
                }
            }
            
            return ret2.ToArray();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.02.2021
        LAST CHANGE:   01.02.2021
        ***************************************************************************/
        public byte[] GetMemListSwp( ref int idx, int count )
        {
            byte[] ret = new byte[count];

            if ( (idx + count) > m_Mem.Count ) return null;

            m_Mem.CopyTo( idx, ret, 0, count);
            List<byte> hlp = new List<byte>( ret );
            hlp.Reverse();

            idx += count;

            return hlp.ToArray();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.01.2023
        LAST CHANGE:   17.01.2023
        ***************************************************************************/
        public uint GetBitfldUint( List<uint> a_Data, ref int a_BitPos, int a_Len )
        {
            uint ret = 0;

            int datidx = a_BitPos / 32;
            int bitidx = a_BitPos % 32;

            if ( datidx >= a_Data.Count ) return 0;

            UInt64 dat   = a_Data[datidx];  dat <<= 32;
                   if ( datidx+1 < a_Data.Count ) dat  += a_Data[datidx+1]; 

            dat <<= bitidx;

            for ( int i=0; i<a_Len; i++ )
            {
                ret += (uint)((dat & 0x8000000000000000) == 0 ? 0 : 1);
                if ( i >= a_Len - 1) break;
                ret <<= 1;
                dat <<= 1;
            }

            a_BitPos += a_Len;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.02.2021
        LAST CHANGE:   24.01.2023
        ***************************************************************************/
        public bool ByteListCompare( List<byte> a_Lst1, string a_Lst2, int a_Idx )
        {
            List<byte> lst = HexStr2ByteList( a_Lst2 );
            return ByteListCompare( a_Lst1, lst, a_Idx, lst.Count );
        }
        public bool ByteListCompare( List<byte> a_Lst1, List<byte> a_Lst2 )            { return ByteListCompare( a_Lst1, a_Lst2, 0, a_Lst1.Count ); }
        public bool ByteListCompare( List<byte> a_Lst1, List<byte> a_Lst2, int a_Len ) { return ByteListCompare( a_Lst1, a_Lst2, 0, a_Len ); }
        public bool ByteListCompare( List<byte> a_Lst1, List<byte> a_Lst2, int a_Idx, int a_Len )
        {
            if ( a_Lst2.Count != a_Len )        return false;
            if ( a_Idx + a_Len > a_Lst1.Count ) return false;
            if ( a_Len         > a_Lst2.Count ) return false;

            for( int i=0; i < a_Len; i++ )
            {
                if ( a_Lst1[ i + a_Idx ] != a_Lst2[ i ] ) return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   01.09.2015
        ***************************************************************************/
        private byte ToByteListSub( UInt32 a_Wrd )   
        { 
            return (byte)( a_Wrd & 0x000000ff ); 
        }

        private byte ToByteListSub( UInt16 a_Wrd )   
        { 
            return (byte)( a_Wrd & 0x00ff ); 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       31.08.2015
        LAST CHANGE:   31.08.2015
        ***************************************************************************/
        public void U8ToByteList( byte a_Byte, ref List<byte> a_List )
        {
            a_List.Add( a_Byte );
        }

        public void U8ToByteList( byte a_Byte, byte[] a_List, ref int a_Idx )
        {
            a_List[ a_Idx++ ] = a_Byte;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   12.08.2015
        ***************************************************************************/
        public void U16ToByteList( UInt16 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub(  (UInt16)(a_Wrd >> 8)  ) );
            a_List.Add( ToByteListSub(           a_Wrd        ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.07.2020
        LAST CHANGE:   21.07.2020
        ***************************************************************************/
        public void U16ToByteList( UInt16 a_Wrd, byte[] a_List, ref int a_Idx )
        {
            a_List[ a_Idx++ ] = ToByteListSub(  (UInt16)(a_Wrd >> 8)  );
            a_List[ a_Idx++ ] = ToByteListSub(           a_Wrd        );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.09.2015
        LAST CHANGE:   21.09.2015
        ***************************************************************************/
        public void U16ToByteListSwp( UInt16 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub(           a_Wrd        ) );
            a_List.Add( ToByteListSub(  (UInt16)(a_Wrd >> 8)  ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.09.2015
        LAST CHANGE:   03.09.2015
        ***************************************************************************/
        public void U24ToByteList( UInt32 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub( a_Wrd >> 16 ) );
            a_List.Add( ToByteListSub( a_Wrd >>  8 ) );
            a_List.Add( ToByteListSub( a_Wrd       ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.03.2017
        LAST CHANGE:   05.03.2017
        ***************************************************************************/
        public void U64ToByteList( UInt64 a_DWrd, ref List<byte> a_List )
        {
            ulong dwrd = a_DWrd;
            int   idx  = a_List.Count;

            for (int i=0; i<8; i++)
            {
                byte bt = (byte)(dwrd & 0x00000000000000FF);
                a_List.Insert(idx,bt);
                dwrd >>= 8;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.05.2020
        LAST CHANGE:   04.05.2020
        ***************************************************************************/
        public void U64ToByteListSwp( UInt64 a_DWrd, ref List<byte> a_List )
        {
            ulong dwrd = a_DWrd;
            int   idx  = a_List.Count;

            for (int i=0; i<8; i++)
            {
                byte bt = (byte)(dwrd >> 64);
                a_List.Insert(idx,bt);
                dwrd <<= 8;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   20.10.2020
        ***************************************************************************/
        public void I32ToByteList( int  a_Wrd, ref List<byte> a_List ) { U32ToByteList( (uint) a_Wrd, ref a_List ); }
        public void U32ToByteList( uint a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub( a_Wrd >> 24 ) );
            a_List.Add( ToByteListSub( a_Wrd >> 16 ) );
            a_List.Add( ToByteListSub( a_Wrd >>  8 ) );
            a_List.Add( ToByteListSub( a_Wrd       ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.06.2016
        LAST CHANGE:   19.08.2022
        ***************************************************************************/
        public void I32ToByteList( int  a_Wrd, ref List<byte> a_List, ref int a_Idx ) { U32ToByteList( (uint) a_Wrd, ref a_List, ref a_Idx  ); }
        public void U32ToByteList( uint a_Wrd, ref List<byte> a_List, ref int a_Idx )
        {
            a_List[a_Idx++] = (byte)( a_Wrd >> 24 );
            a_List[a_Idx++] = (byte)( a_Wrd >> 16 );
            a_List[a_Idx++] = (byte)( a_Wrd >>  8 );
            a_List[a_Idx++] = (byte)( a_Wrd       );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.07.2020
        LAST CHANGE:   21.07.2020
        ***************************************************************************/
        public void U32ToByteList( uint a_Wrd, byte[] a_List, ref int a_Idx )
        {
            a_List[a_Idx++] = (byte)( a_Wrd >> 24 );
            a_List[a_Idx++] = (byte)( a_Wrd >> 16 );
            a_List[a_Idx++] = (byte)( a_Wrd >>  8 );
            a_List[a_Idx++] = (byte)( a_Wrd       );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.09.2015
        LAST CHANGE:   21.09.2015
        ***************************************************************************/
        public void U32ToByteListSwp( UInt32 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub( a_Wrd       ) );
            a_List.Add( ToByteListSub( a_Wrd >>  8 ) );
            a_List.Add( ToByteListSub( a_Wrd >> 16 ) );
            a_List.Add( ToByteListSub( a_Wrd >> 24 ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.01.2017
        LAST CHANGE:   31.03.2023
        ***************************************************************************/
        public void U16ToByteList( ushort a_Wrd, ref List<byte> a_List, ref int a_Idx )
        {
            if ( a_List.Count <= a_Idx + 2 ) return;

            a_List[a_Idx++] = (byte)( a_Wrd >>  8 );
            a_List[a_Idx++] = (byte)( a_Wrd       );
        }

        /***************************************************************************
        SPECIFICATION: a_BLst is copied into a_List at index a_Idx
        CREATED:       14.11.2019
        LAST CHANGE:   14.11.2019
        ***************************************************************************/
        public bool ByteListToByteList( List<byte> a_BLst, ref List<byte> a_List, ref int a_Idx )
        {
            if ( a_List.Count < a_Idx + a_BLst.Count ) return false;
            
            a_List.RemoveRange( a_Idx, a_BLst.Count );
            a_List.InsertRange( a_Idx, a_BLst );

            a_Idx += a_BLst.Count;

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.08.2022
        LAST CHANGE:   03.08.2022
        ***************************************************************************/
        public bool ByteListToByteList( List<byte> a_DstLst, List<byte> a_SrcLst, ref int a_Idx, int a_Cnt )
        {
            if ( a_SrcLst.Count < a_Idx + a_Cnt ) return false;
            
            a_DstLst.Clear();

            byte[] ba = new byte[a_Cnt];
            a_SrcLst.CopyTo( a_Idx, ba, 0, a_Cnt );

            a_DstLst.Clear();
            a_DstLst.AddRange( ba );

            a_Idx += a_Cnt;

            return true;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.01.2021
        LAST CHANGE:   28.01.2021
        ***************************************************************************/
        public List<byte> CreateByteList( byte a_Val, uint a_Length )
        {
            List<byte> ret = new List<byte>();
            for (int i=0; i<a_Length; i ++) ret.Add(a_Val);
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: Polimorphic calls
        CREATED:       21.09.2015
        LAST CHANGE:   18.01.2021
        ***************************************************************************/
        public void VarToByteList( string       a_Str, ref List<byte> a_List ) { StrToByteList ( a_Str, ref a_List); }
        public void VarToByteList( List<byte>   a_Str, ref List<byte> a_List ) { a_List.AddRange( a_Str ); }
        public void VarToByteList( List<ushort> a_Str, ref List<byte> a_List ) { U16ListToByteList( a_Str, ref a_List); }
        public void VarToByteList( byte         a_Var, ref List<byte> a_List ) { U8ToByteList  ( a_Var, ref a_List); }
        public void VarToByteList( UInt16       a_Var, ref List<byte> a_List ) { U16ToByteList ( a_Var, ref a_List); }
        public void VarToByteList( UInt32       a_Var, ref List<byte> a_List ) { U32ToByteList ( a_Var, ref a_List); }
        public void VarToByteList( int          a_Var, ref List<byte> a_List ) { I32ToByteList ( a_Var, ref a_List); }
        public void SwpToByteList( UInt16       a_Var, ref List<byte> a_List ) { U16ToByteListSwp ( a_Var, ref a_List); }
        public void SwpToByteList( UInt32       a_Var, ref List<byte> a_List ) { U32ToByteListSwp ( a_Var, ref a_List); }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.02.2017
        LAST CHANGE:   18.01.2018
        ***************************************************************************/
        public List<byte> HexStr2ByteList( string a_HexStr )
        {
            List<byte> ret = new List<byte>();

            string hexstr = a_HexStr.Replace("0x","");
                   hexstr = hexstr  .Replace(" " ,"");
                   hexstr = hexstr  .Replace("\r","");
                   hexstr = hexstr  .Replace("\n","");
            if (hexstr.Length == 1) hexstr = "0" + hexstr;

            while(hexstr.Length > 1)
            {
                string hex = hexstr.Substring(0,2);
                ret.Add( WUtils.Hex2Byte(hex) );
                hexstr = hexstr.Remove(0,2);
            }

            return ret;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.07.2020
        LAST CHANGE:   06.08.2021
        ***************************************************************************/
        public string ByteList2HexStr( List<byte> a_Data, bool a_Delim ) { return ByteList2HexStr( a_Data.ToArray(), a_Delim ); }
        public string ByteList2HexStr( List<byte> a_Data )               { return ByteList2HexStr( a_Data, false ); }
        public string ByteList2HexStr( byte[] a_Data )                   { return ByteList2HexStr( a_Data, false ); }
        public string ByteList2HexStr( byte[] a_Data, bool a_Delim )
        {
            string ret = "";

            string frmt = "{0:X2}";
            if (a_Delim) frmt = "{0:X2} ";

            foreach( byte b in a_Data )
            {
                ret += string.Format( frmt, b );
            }
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.09.2015
        LAST CHANGE:   16.09.2015
        ***************************************************************************/
        public void StrToByteList( string a_Str, ref List<byte> a_List )
        {
            foreach( char c in a_Str ) a_List.Add((byte)c);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        public List<byte> Str2ByteList( string a_Str )
        {
            List<byte> ret = new List<byte>();
            StrToByteList( a_Str, ref ret );
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.10.2022
        LAST CHANGE:   17.10.2022
        ***************************************************************************/
        public void U16ListToByteList( List<ushort> a_UList, ref List<byte> a_List )
        {
            foreach( ushort u in a_UList ) U16ToByteList( u, ref a_List );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.08.2015
        LAST CHANGE:   17.05.2017
        ***************************************************************************/
        public byte U8FromByteList( ref List<byte> a_List, ref int a_Idx ) { return a_List[a_Idx++]; }
        public byte U8FromByteList( ref List<byte> a_List ) 
        { 
            byte ret = a_List[0];
            a_List.RemoveAt(0);
            return ret;
        }
        public byte U8FromByteList( byte[] a_List, ref int a_Idx )
        {
            if ( a_List.Length <= a_Idx ) return 0;
            return a_List[a_Idx++]; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.09.2018
        LAST CHANGE:   18.09.2018
        ***************************************************************************/
        public UInt16 U16FromByteList( ref List<byte> a_List, bool a_Swp )
        {
            ushort ret = U16FromByteList( ref a_List );
            if (a_Swp) ret = SwapBytes16( ret );
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   17.05.2017
        ***************************************************************************/
        public UInt16 U16FromByteList( ref List<byte> a_List )
        {
            UInt16 ret = 0;

            if (a_List.Count < 2) return 0;

            ret += a_List[0]; a_List.RemoveAt(0); ret <<= 8;
            ret += a_List[0]; a_List.RemoveAt(0);

            return ret;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.01.2017
        LAST CHANGE:   23.11.2020
        ***************************************************************************/
        public UInt32 U24FromByteList( ref List<byte> a_List )
        {
            if (a_List.Count < 3) return 0;

            UInt32 ret = 0;

            for (int i=0; i<3; i++)
            {
                ret += a_List[0];
                a_List.RemoveAt(0);
                if (i<2) ret <<= 8;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.09.2018
        LAST CHANGE:   18.09.2018
        ***************************************************************************/
        public UInt32 U32FromByteList( ref List<byte> a_List, bool a_Swp )
        {
            uint ret = U32FromByteList( ref a_List );   
            if ( a_Swp ) ret = SwapBytes32(ret);
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: returns 4 bytes of top of the stack and reduces the stack
        CREATED:       12.08.2015
        LAST CHANGE:   17.05.2017
        ***************************************************************************/
        public UInt32 U32FromByteList( ref List<byte> a_List )
        {
            if (a_List.Count < 4) return 0;

            UInt32 ret = 0;

            for (int i=0; i<4; i++)
            {
                ret += a_List[0];
                a_List.RemoveAt(0);
                if (i<3) ret <<= 8;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.11.2022
        LAST CHANGE:   07.11.2022
        ***************************************************************************/
        public int I32FromByteList( ref List<byte> a_List )
        {
            return (int)U32FromByteList( ref a_List );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.01.2016
        LAST CHANGE:   07.11.2022
        ***************************************************************************/
        public UInt32 U32FromByteList( ref List<byte> a_List, ref uint a_Idx ) 
        {
            int idx = (int) a_Idx;
            return U32FromByteList( ref a_List, ref idx );
        }

        public UInt32 U32FromByteList( ref List<byte> a_List, ref int a_Idx ) 
        { 
            return U32FromByteList( a_List.ToArray(), ref a_Idx ); 
        }
        public UInt32 U32FromByteList( byte[] a_List, ref uint a_Idx )
        { 
            int idx = (int)a_Idx; 
            return U32FromByteList(a_List, ref idx); 
        }

        public UInt32 U32FromByteList( byte[] a_List, ref int a_Idx )
        {
            if (a_List.Length < 4) return 0;
            UInt32 ret = 0;

            for (int i=0; i<4; i++)
            {
                ret += a_List[a_Idx++];
                if (i<3) ret <<= 8;
            }

            return ret;
        }

        public UInt32 U32FromByteList( List<byte> a_List, int a_Idx = 0 )
        {
            if (a_List.Count < 4) return 0;

            UInt32 ret = 0;

            for (int i=0; i<4; i++)
            {
                ret += a_List[a_Idx+i];
                if (i<3) ret <<= 8;
            }

            return ret;
        }

        public int I32FromByteList( List<byte> a_List, int a_Idx = 0 )
        {
            return (int)U32FromByteList( a_List, a_Idx );
        }


        /***************************************************************************
        SPECIFICATION:  
        CREATED:       24.05.2016
        LAST CHANGE:   24.05.2016
        ***************************************************************************/
        public double DoubleFromByteList( ref List<byte> a_List, ref int a_Idx )
        {
            double ret = BitConverter.ToDouble( a_List.ToArray(), a_Idx );
            a_Idx += 8;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.05.2016
        LAST CHANGE:   24.05.2016
        ***************************************************************************/
        public float FloatFromByteList( ref List<byte> a_List, ref int a_Idx )
        {
            float ret = BitConverter.ToSingle( a_List.ToArray(), a_Idx );
            a_Idx += 4;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.06.2016
        LAST CHANGE:   28.11.2023
        ***************************************************************************/
        public List<byte> ListFromByteList( ref List<byte> a_List, int a_NrBytes )
        {
            List<byte> ret = new List<byte>();

            int len = a_NrBytes;

            if ( len < 0 )
            {
                ret.AddRange( a_List);
                return ret;
            }

            if ( a_List.Count < len ) len = a_List.Count;

            ret.AddRange( a_List.GetRange( 0 , len ) );
            a_List.RemoveRange( 0, len );

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.03.2017
        LAST CHANGE:   11.04.2019
        ***************************************************************************/
        public int FindStrInByteList( string a_Key, ref List<byte> a_List, int a_Idx )
        {
            int  stridx = 0;

            for ( int i = a_Idx; i < a_List.Count; i++ ) 
            {
                char cl = (char)a_List[i];
                char cs = a_Key[stridx];

                if (cl != cs) 
                {
                    stridx=0;
                    continue;
                }

                stridx++;

                if ( stridx >= a_Key.Length )  return i - a_Key.Length + 1;
            }

            return -1;
        }

        /***************************************************************************
        SPECIFICATION: returns index of sublist
        CREATED:       11.04.2019
        LAST CHANGE:   11.04.2019
        ***************************************************************************/
        public int FindBytListInBytLst( List<byte> a_MainLst, List<byte> a_SubLst )
        {
            int bidx = 0;

            for ( int i = 0; i < a_MainLst.Count; i++ )
            {
                if ( a_MainLst[i] != a_SubLst[bidx] ) 
                {
                    bidx=0;
                    continue;
                }

                bidx++;

                if (bidx >= a_SubLst.Count ) return i - a_SubLst.Count + 1;
            }

            return -1;
        }

    } // class
} // namespace
