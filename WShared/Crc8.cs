using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NS_WUtilities
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       16.05.2019
    LAST CHANGE:   16.05.2019
    ***************************************************************************/
    public class Crc8
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       16.05.2019
        LAST CHANGE:   16.05.2019
        ***************************************************************************/
        private const byte poly1 = 0xd5;
        private const uint poly2 = 0x1d;
        private byte[]     table;
        private byte       start = 0;


        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       16.05.2019
        LAST CHANGE:   16.05.2019
        ***************************************************************************/
        public Crc8()
        {
            table = new byte[256];

            for( int i = 0; i < 256; ++i )
            {
                int temp = i;
                for( int j = 0; j < 8; ++j )
                {
                    if( ( temp & 0x80 ) != 0 )
                    {
                        temp = ( temp << 1 ) ^ poly1;
                    }
                    else
                    {
                        temp <<= 1;
                    }
                }
                table[i] = (byte)temp;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.05.2019
        LAST CHANGE:   16.05.2019
        ***************************************************************************/
        public byte Compute( List<byte> a_Mem, int a_Idx, int a_NrBts )
        {
            byte crc = 0;
            int  idx = a_Idx;

            for ( int i = 0; i < a_NrBts; i++ )
            {
                byte b = a_Mem[idx++];
                crc = table[crc ^ b];
            }

            return crc;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.05.2019
        LAST CHANGE:   16.05.2019
        ***************************************************************************/
        public byte Compute2( List<byte> a_Mem, int a_Idx, int a_NrBts )
        {
            start = 0xff;

            for ( int j=0; j < a_NrBts; j++ )
            {
                byte b   = a_Mem[a_Idx+j];

                start ^= b;

                for ( int i=0; i<8; i++ )
                {
                    if ( (start & (byte)0x80) == 0 ) start <<= 1;
                    else                             start = (byte)((uint)(start << 1) ^ poly2);
                }
            }

            start ^= 0xff;
            return start;
        }

    }

}
