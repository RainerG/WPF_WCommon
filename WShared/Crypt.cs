using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using NS_Utilities;

namespace NS_Crypt
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       10.07.2019
    LAST CHANGE:   10.07.2019
    ***************************************************************************/
    public static class Crypt
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       24.09.2019
        LAST CHANGE:   24.09.2019
        ***************************************************************************/
        public static byte[] Key { get { return m_Key; } }
        public static byte[] IV  { get { return m_IV; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       10.07.2019
        LAST CHANGE:   24.09.2019
        ***************************************************************************/
        //private static AesManaged m_Aes;
        private static RijndaelManaged  m_Aes = new RijndaelManaged();    
        private static byte[]           m_Key = { 0x3a,0x80,0x8b,0xb6,0xfc,0x2e,0x72,0xe2,0x8b,0x32,0x87,0x38,0x7e,0x18,0x65,0xde };
        private static byte[]           m_IV  = { 0x2a,0xfe,0xe9,0x46,0x2a,0x8e,0x9a,0xd6,0x70,0x96,0x20,0xa4,0x21,0x98,0x4a,0x57 };
        public const int AESKeySize = 128;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        //public Crypt()
        //{
            ////m_Aes  = new AesManaged();
            //m_Aes = new RijndaelManaged();
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        public static byte[] AesEncrypt( byte[] a_Data, List<byte> a_Key, List<Byte> a_IV )
        {
#if false
            byte[] encrypted;
            List<byte> ret;
            List<byte> key = a_Key; //Utils.HexStr2ByteList(a_Key);
            List<byte> iv  = a_IV;  //Utils.HexStr2ByteList(a_IV);
            
            m_Aes.Clear();

            m_Aes.Key = key.ToArray();
            m_Aes.IV  = iv .ToArray();

            //m_Aes.BlockSize = 8;
            //m_Aes.KeySize      = 16;
            //m_Aes.FeedbackSize = 8;
            //m_Aes.Padding = PaddingMode.Zeros;

            ICryptoTransform encryptor = m_Aes.CreateEncryptor( m_Aes.Key, m_Aes.IV );

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.FlushFinalBlock();
                    csEncrypt.Close();

                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(a_Data);
                        swEncrypt.Close();
                    }
                    encrypted = msEncrypt.ToArray();
                }
                msEncrypt.Close();
            }

            ret = new List<byte>(encrypted);
            
            return ret;
#else
            byte[] initVectorBytes  = a_IV  .ToArray();
            byte[] plainTextBytes   = a_Data.ToArray();
            byte[] keyBytes         = a_Key .ToArray();

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode            = CipherMode.ECB;
            symmetricKey.Padding         = PaddingMode.Zeros;

            try
            {
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();

                return cipherTextBytes;
            }
            catch( Exception ex )
            {
                return null;
            }
#endif
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        public static byte[] AesDecrypt( byte[] a_Data, List<byte> a_Key, List<Byte> a_IV )
        //public static string AesDecrypt( List<byte> a_Data, List<byte> a_Key, List<Byte> a_IV )
        {
#if true
            string decrypted = null;
            List<byte> key = a_Key; //Utils.HexStr2ByteList(a_Key);
            List<byte> iv  = a_IV;  //Utils.HexStr2ByteList(a_IV);
            
            m_Aes.Clear();

            m_Aes.Key = key.ToArray();
            m_Aes.IV  = iv .ToArray();

            m_Aes.BlockSize    = 0x80;
            m_Aes.KeySize      = 0x80;
            m_Aes.FeedbackSize = 0x80;
            m_Aes.Padding = PaddingMode.Zeros;
            m_Aes.Mode    = CipherMode.ECB;

            ICryptoTransform decryptor = m_Aes.CreateDecryptor( a_Key.ToArray(), a_IV.ToArray() );

            // Create the streams used for encryption.
            using (MemoryStream msDecrypt = new MemoryStream())
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write) )
                {
                    csDecrypt.Write( a_Data, 0, a_Data.Length );
                    csDecrypt.FlushFinalBlock();
                }
                return msDecrypt.ToArray();
            }

            return Utils.Str2ByteList( decrypted ).ToArray();
#else
            byte[] initVectorBytes = a_IV  .ToArray();
            byte[] cipherTextBytes = a_Data.ToArray();
            byte[] keyBytes        = a_Key .ToArray();

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode      = CipherMode .ECB;
            symmetricKey.Padding   = PaddingMode.None;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor ( keyBytes, initVectorBytes );
            //decryptor.InputBlockSize = a_Data.Length;
            MemoryStream memoryStream  = new MemoryStream( cipherTextBytes );
            CryptoStream cryptoStream  = new CryptoStream( memoryStream, decryptor, CryptoStreamMode.Read );

            StreamReader sr = new StreamReader(cryptoStream);
            string plaintext = sr.ReadToEnd();
            sr.Close();

            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] bts = enc.GetBytes(plaintext);


            //int remain = cipherTextBytes.Length % 16;
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            cryptoStream.Write( plainTextBytes, 0, plainTextBytes.Length );

            cryptoStream.Close();
            memoryStream.Close();


            //return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            return plainTextBytes;
#endif      
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        public static byte[] AesEncrypt_Str2Bytes( string a_Data, List<byte> a_Key, List<byte> a_IV )
        {
            byte[] data = Utils.Str2ByteList( a_Data ).ToArray();
            return AesEncrypt( data, a_Key, a_IV );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.07.2019
        LAST CHANGE:   11.07.2019
        ***************************************************************************/
        public static string AesEncrypt_Str2Str( string a_Data, List<byte> a_Key, List<byte> a_IV )
        {
            byte[] data = Utils.Str2ByteList( a_Data ).ToArray();
            byte[] ret = AesEncrypt( data, a_Key, a_IV );
            string str = Encoding.UTF8.GetString(ret);
            return str;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        public static string AesDecrypt_Bytes2Str( byte[] a_Data, List<byte> a_Key, List<byte> a_IV )
        {
            byte[] ret = AesDecrypt( a_Data, a_Key, a_IV );
            //foreach( byte b in data ) ret += (char)b;
            return Encoding.UTF8.GetString( ret, 0, ret.Length );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.07.2019
        LAST CHANGE:   11.07.2019
        ***************************************************************************/
        public static string AesDecrypt_Str2Str( string a_Data, List<byte> a_Key, List<byte> a_IV )
        {
            byte[] data = Encoding.UTF8.GetBytes(a_Data);
            byte[] ret = AesDecrypt( data, a_Key, a_IV );
            //foreach( byte b in data ) ret += (char)b;
            return Encoding.UTF8.GetString( ret, 0, ret.Length );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2019
        LAST CHANGE:   14.10.2019
        ***************************************************************************/
        public static List<byte> AesEncrypt_Bytes2Bytes( List<byte> a_Data, List<byte> a_Key, List<byte> a_IV )
        {
            byte[] ret = AesEncrypt( a_Data.ToArray(), a_Key, a_IV );
            return new List<byte>(ret);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2019
        LAST CHANGE:   14.10.2019
        ***************************************************************************/
        public static List<byte> AesDecrypt_Bytes2Bytes( List<byte> a_Data, List<byte> a_Key, List<byte> a_IV )
        {
            byte[] ret = AesDecrypt( a_Data.ToArray(), a_Key, a_IV );

            return new List<byte>(ret);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        public static List<byte> GetKey( byte a_Val, uint a_Length )
        {
            List<byte> ret = new List<byte>();
            for (int i=0; i<a_Length; i ++) ret.Add(a_Val);
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.12.2017
        LAST CHANGE:   15.12.2017
        ***************************************************************************/
        static public List<byte> ToyotaAes( string args )
        {
            //AesManaged aes = new AesManaged();

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName  = "aes.exe";
            info.Arguments = args;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = info;
            proc.Start();

            List<byte> ret = new List<byte>();

            while ( ! proc.StandardOutput.EndOfStream )
            {
                string line = proc.StandardOutput.ReadLine();

                while( line.Length > 0 )
                {
                  string bt = line.Substring(0,2);
                  ret.Add( Utils.Hex2Byte(bt) );
                  line = line.Remove(0,2);
                }
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: Internet
        CREATED:       15.07.2019
        LAST CHANGE:   15.07.2019
        ***************************************************************************/
        public static byte[] RandomByteArray( int length )
        {
            byte[] result = new byte[length];

            using( RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider() )
            {
                provider.GetBytes( result );
                return result;
            }

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.07.2019
        LAST CHANGE:   09.04.2021
        ***************************************************************************/
        public static bool AESEncryptFile( string srcFile, string dstFile, byte[] password )
        {
            byte[] salt = RandomByteArray( 16 );

            using( FileStream fs = new FileStream( dstFile, FileMode.Create ) )
            {
                var key = GenerateKey( password, salt );

                password = null;
                GC.Collect();

                using( Aes aes = new AesManaged() )
                {

                    aes.KeySize = AESKeySize;
                    aes.Key     = key.GetBytes(aes.KeySize / 8);
                    aes.IV      = key.GetBytes(aes.BlockSize / 8);
                    //aes.Key = m_Key;
                    //aes.IV  = m_IV;
                    aes.Padding = PaddingMode.ISO10126;
                    aes.Mode    = CipherMode.CBC;

                    fs.Write( salt, 0, salt.Length );

                    using( CryptoStream cs = new CryptoStream( fs, aes.CreateEncryptor(), CryptoStreamMode.Write ) )
                    {
                        File.SetAttributes( srcFile, FileAttributes.Normal );
                        using( FileStream fsIn = new FileStream( srcFile, FileMode.Open ) )
                        {
                            byte[] buffer = new byte[1];
                            int read;

                            key.Dispose();

                            try
                            {
                                while( ( read = fsIn.Read( buffer, 0, buffer.Length ) ) > 0 )
                                {

                                    cs.Write( buffer, 0, read );

                                }

                                cs.Close();
                                fs.Close();
                                fsIn.Close();

                                return true;
                            }
                            catch( Exception e )
                            {
                                MessageBox.Show( e.Message, "Error writing encrypted file" + dstFile );
                                return false;
                            }
                        }
                    }
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.07.2019
        LAST CHANGE:   16.07.2019
        ***************************************************************************/
        public static bool AESDecryptFile( string srcFile, string dstFile, ref MemoryStream a_MemStrm, byte[] password )
        {
            byte[] salt = new byte[16];

            using( FileStream fsIn = new FileStream( srcFile, FileMode.Open ) )
            {
                fsIn.Read( salt, 0, salt.Length );

                var key = GenerateKey( password, salt );

                password = null;
                GC.Collect();

                using( Aes aes = new AesManaged() )
                {
                    aes.KeySize = AESKeySize;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV  = key.GetBytes(aes.BlockSize / 8);
                    //aes.Key = m_Key;
                    //aes.IV  = m_IV;
                    aes.Padding = PaddingMode.ISO10126;
                    aes.Mode    = CipherMode.CBC;

                    using( CryptoStream cs = new CryptoStream( fsIn, aes.CreateDecryptor(), CryptoStreamMode.Read ) )
                    {
                        if ( a_MemStrm == null)
                        {
                            using( FileStream fsOut = new FileStream( dstFile, FileMode.Create ) )
                            {
                                byte[] buffer = new byte[1];
                                int read;

                                key.Dispose();

                                try
                                {
                                    while( ( read = cs.Read( buffer, 0, buffer.Length ) ) > 0 )
                                    {
                                        fsOut.Write( buffer, 0, buffer.Length );
                                    }

                                    fsOut.Close();

                                    return true;
                                }
                                catch( Exception e )
                                {
                                    MessageBox.Show( e.Message, "Error writing decrypted file" + dstFile );
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            byte[] buffer = new byte[1];
                            int read;

                            key.Dispose();

                            try
                            {
                                while( ( read = cs.Read( buffer, 0, buffer.Length ) ) > 0 )
                                {
                                    a_MemStrm.Write( buffer, 0, buffer.Length );
                                }

                                a_MemStrm.Position = 0;

                                return true;
                            }
                            catch( Exception e )
                            {
                                MessageBox.Show( e.Message, "Error writing decrypted file" + dstFile );
                                return false;
                            }
                        }
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                }

                fsIn.Close();
            }
        }

        public static byte[] AESEncryptBytes( byte[] clear, byte[] password, byte[] salt )
        {

            byte[] encrypted = null;

            var key = GenerateKey( password, salt );

            password = null;
            GC.Collect();

            using( Aes aes = new AesManaged() )
            {

                aes.KeySize = AESKeySize;
                aes.Key = key.GetBytes( aes.KeySize / 8 );
                aes.IV  = key.GetBytes( aes.BlockSize / 8 );
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                using( MemoryStream ms = new MemoryStream() )
                {

                    using( CryptoStream cs = new CryptoStream( ms, aes.CreateEncryptor(), CryptoStreamMode.Write ) )
                    {
                        cs.Write( clear, 0, clear.Length );
                        cs.Close();
                    }

                    encrypted = ms.ToArray();
                }

                key.Dispose();

            }

            return encrypted;

        }

        public static byte[] AESDecryptBytes( byte[] encrypted, byte[] password, byte[] salt )
        {

            byte[] decrypted = null;

            var key = GenerateKey( password, salt );

            password = null;
            GC.Collect();

            using( Aes aes = new AesManaged() )
            {

                aes.KeySize = AESKeySize;
                aes.Key = key.GetBytes( aes.KeySize / 8 );
                aes.IV = key.GetBytes( aes.BlockSize / 8 );
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                using( MemoryStream ms = new MemoryStream() )
                {

                    using( CryptoStream cs = new CryptoStream( ms, aes.CreateDecryptor(), CryptoStreamMode.Write ) )
                    {

                        cs.Write( encrypted, 0, encrypted.Length );
                        cs.Close();

                    }

                    decrypted = ms.ToArray();

                }

                key.Dispose();

            }

            return decrypted;

        }

        public static bool CheckPassword( byte[] password, byte[] salt, byte[] key )
        {

            using( Rfc2898DeriveBytes r = GenerateKey( password, salt ) )
            {

                byte[] newKey = r.GetBytes( AESKeySize / 8 );
                return newKey.SequenceEqual( key );

            }

        }

        public static Rfc2898DeriveBytes GenerateKey( byte[] password, byte[] salt )
        {
            return new Rfc2898DeriveBytes( password, salt, 1000 );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.06.2020
        LAST CHANGE:   15.06.2020
        ***************************************************************************/
        public static List<byte> GetAes128CmacHash( byte[] a_PyLd, int a_PldSz )
        {
            List<byte> ret = new List<byte>();
            return ret;
        }

    } // class


    /***************************************************************************
    SPECIFICATION: 
    CREATED:       10.07.2019
    LAST CHANGE:   10.07.2019
    ***************************************************************************/
    public static class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes  = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes   = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes              = password.GetBytes(keysize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode            = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes   = password.GetBytes(keysize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor ( keyBytes, initVectorBytes );
            MemoryStream memoryStream  = new MemoryStream( cipherTextBytes );
            CryptoStream cryptoStream  = new CryptoStream( memoryStream, decryptor, CryptoStreamMode.Read );

            byte[] plainTextBytes  = new byte[ cipherTextBytes.Length ];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

    } // class


    /***************************************************************************
    SPECIFICATION: 
    CREATED:       02.07.2020
    LAST CHANGE:   02.07.2020
    ***************************************************************************/
    public static class SeednKey
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       28.07.2020
        LAST CHANGE:   24.11.2022
        ***************************************************************************/
        public static string   ProdKey    { set { m_ProdKey = value; } }
        public static string   KeyVar     { set { m_KeyVar  = value; } }
        public static ushort[] HondaL6SKs { set { m_HaL6SKs = value; } }


        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       02.07.2020
        LAST CHANGE:   24.11.2022
        ***************************************************************************/
        private const uint NC_DEFAULT_SEED = 0xa548fd85;  
        private const int  NC_UDS_KEYMASK  = 0x245dacbe;

        private static readonly uint[] nc_uds_keymul = new uint[]
            { 
                0x7678, 0x9130, 0xd753, 0x750f, 0x72cb, 0x55f7, 0x13da, 0x786b, 
                0x372a, 0x4932, 0x0e7c, 0x3687, 0x3261, 0xa82c, 0x8935, 0xd00c, 
                0x1995, 0x4311, 0xb854, 0x0d8d, 0x9863, 0x1a21, 0xf753, 0xd6d3, 
                0xb15d, 0x7f3d, 0x6821, 0x791c, 0x26c5, 0x2e37, 0x0e69, 0x64a0 
            };

        private static ushort[] m_HaL6SKs  = { 0xECDD, 0x9AE8, 0x896B };

        private static string  m_ProdKey;
        private static string  m_KeyVar ;

        /*******************************/
        private static ulong croleft( ulong c, int b )
        {  
            ulong left  = c << b;
            ulong right = c >> ( 32 - b );  
            ulong croleftvalue=left|right;  
            return croleftvalue;  
        }  
 

        /*******************************/
        private static ushort croshortright ( ushort c, ushort b )
        {  
            ushort right = (ushort)(c >> b);
            ushort left  = (ushort)(c << ( 16 - b ));  
            ushort crorightvalue = (ushort)(left | right);  
            return crorightvalue;  
        }  

        /*******************************/
        private static ulong mulu32 ( ulong val1, ulong val2)
        {
            ulong x, y, z, p;
            x = (val1 & NC_UDS_KEYMASK) | ((~val1) & val2);
            y = ((croleft(val1,1)) & (croleft(val2,14))) | ((croleft(NC_UDS_KEYMASK,21)) & (~(croleft(val1,30))));
            z = (croleft(val1,17)) ^ (croleft(val2,4)) ^ (croleft(NC_UDS_KEYMASK,11));
            p = x ^ y ^ z;
            return p;
        }

        /*******************************/
        public static List<byte> ArsSwKey( List<byte> a_Seed )
        {
            ulong  temp;
            ushort index;
            ushort mult1;
            ushort mult2;

            ulong seed = Utils.U32FromByteList( a_Seed );

            if( seed == 0 )
            {
                seed = NC_DEFAULT_SEED;
            }
            else
            { }

            for ( index = 0x5D39, temp = 0x80000000; temp != 0; temp >>= 1 )
            {
                if( (temp & seed) != 0 )
                {
                    index = croshortright ( index, 1 );
                    if ( (temp & NC_UDS_KEYMASK) != 0 )
                    {
                        index ^= 0x74c9;
                    }
                }
            }

            mult1 = (ushort)( nc_uds_keymul[ (index >> 2) & ((1 << 5) - 1) ] ^ index );
            mult2 = (ushort)( nc_uds_keymul[ (index >> 8) & ((1 << 5) - 1) ] ^ index );
            temp =  ( ( (ulong) mult1 ) << 16 )| ( (ulong)mult2 );
            temp =  mulu32(seed,temp);

            List<byte> ret = new List<byte>();
            Utils.U32ToByteList( (uint)temp, ref ret );

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: Suzuki Key
        CREATED:       28.01.2017
        LAST CHANGE:   28.01.2017
        ***************************************************************************/
        static public ushort SuzukiKey( ushort a_Seed )
        {
            byte   seedh = (byte)(a_Seed >> 8);
            byte   seedl = (byte)(a_Seed & 0x00ff);
            byte[] ck    = new byte[6] {0xC6,0x29,0x54,0x62,0x34,0x99};
            byte   x,y,z;

            for (int i=0; i<3; i++)
            {
                x = (byte)~(seedl^ck[2*i]);
                y=x; y>>=7; x<<=1; x+=y;
                z=(byte)(x^seedh);

                x = (byte)~(z^ck[2*i+1]);
                y=x; y>>=4; x<<=4; x+=y;
                seedh = (byte)(x^seedl);
                seedl = z;
            }

            ushort key  = (ushort)(seedh << 8);
                   key += seedl;
            return key;
        }

        /***************************************************************************
        SPECIFICATION: Conti Key
        CREATED:       03.03.2017
        LAST CHANGE:   22.11.2017
        ***************************************************************************/
        private const uint ADASKEY = 0xADA5ADA5;

        static public List<byte> ContiKey( List<byte> a_SeedArr )
        {
            if ( a_SeedArr.Count < 8 ) return null;
            byte[] gKeyArray  = new byte[5];
  
            uint   Result;
            uint   SeedH, SeedL;
            //byte   k;

            SeedH  = (uint)(a_SeedArr[7] << 24);
            SeedH |= (uint)(a_SeedArr[6] << 16);
            SeedH |= (uint)(a_SeedArr[5] << 8 );
            SeedH |= (uint)(a_SeedArr[4]      );
            SeedL  = (uint)(a_SeedArr[3] << 24);
            SeedL |= (uint)(a_SeedArr[2] << 16);
            SeedL |= (uint)(a_SeedArr[1] << 8 );
            SeedL |= (uint)(a_SeedArr[0]      );

            SeedH >>= (int)(SeedL % 13);
            SeedL <<= (int)(SeedH % 17);
  
            SeedH ^= ADASKEY;
            SeedL ^= ADASKEY;
  
            SeedH >>= (int)(SeedL % 19);
            SeedL <<= (int)(SeedH % 23);
  
            Result = SeedH ^ SeedL;

            /* C4, C27, C20, C14, C9, C22, C21, C8 */
            gKeyArray[0] =  (byte)( ( ((Result & 0x00000008) >>  3) & 0x000000ff ) |
                                    ( ((Result & 0x04000000) >> 25) & 0x000000ff ) |
                                    ( ((Result & 0x00080000) >> 17) & 0x000000ff ) |
                                    ( ((Result & 0x00002000) >> 10) & 0x000000ff ) |
                                    ( ((Result & 0x00000100) >>  4) & 0x000000ff ) |
                                    ( ((Result & 0x00200000) >> 16) & 0x000000ff ) |
                                    ( ((Result & 0x00100000) >> 14) & 0x000000ff ) |
                                    ( ((Result & 0x00000080) >>  0) & 0x000000ff ) );
            /* C19, C32, C22, C8, C4, C5, C19, C31 */
            gKeyArray[1] =  (byte)( ( ((Result & 0x00040000) >> 18) & 0x000000ff ) |
                                    ( ((Result & 0x80000000) >> 30) & 0x000000ff ) |
                                    ( ((Result & 0x00200000) >> 19) & 0x000000ff ) |
                                    ( ((Result & 0x00000080) >>  4) & 0x000000ff ) |
                                    ( ((Result & 0x00000008) <<  1) & 0x000000ff ) |
                                    ( ((Result & 0x00000010) <<  1) & 0x000000ff ) |
                                    ( ((Result & 0x00040000) >> 12) & 0x000000ff ) |
                                    ( ((Result & 0x40000000) >> 13) & 0x000000ff ) );
            /* C4, C29, C11, C5, C30, C8, C25, C11 */
            gKeyArray[2]  = (byte)( ( ((Result & 0x00000008) >>  3) & 0x000000ff ) |
                                    ( ((Result & 0x10000000) >> 27) & 0x000000ff ) |
                                    ( ((Result & 0x00000400) >>  8) & 0x000000ff ) |
                                    ( ((Result & 0x00000010) >>  1) & 0x000000ff ) |
                                    ( ((Result & 0x20000000) >> 25) & 0x000000ff ) |
                                    ( ((Result & 0x00000080) >>  2) & 0x000000ff ) |
                                    ( ((Result & 0x01000000) >> 18) & 0x000000ff ) |
                                    ( ((Result & 0x00000400) >>  3) & 0x000000ff ) );
            /* C4, C11, C22, C10, C25, C13, C9, C30 */
            gKeyArray[3]  = (byte)( ( ((Result & 0x00000008) >>  3) & 0x000000ff ) |
                                    ( ((Result & 0x00000400) >>  9) & 0x000000ff ) |
                                    ( ((Result & 0x00200000) >> 19) & 0x000000ff ) |
                                    ( ((Result & 0x00000200) >>  6) & 0x000000ff ) |
                                    ( ((Result & 0x01000000) >> 20) & 0x000000ff ) |
                                    ( ((Result & 0x00001000) >>  7) & 0x000000ff ) |
                                    ( ((Result & 0x00000100) >>  2) & 0x000000ff ) |
                                    ( ((Result & 0x20000000) >> 22) & 0x000000ff ) );
            /* C4, C10, C32, C2, C12, C3, C26, C26 */
            gKeyArray[4]  = (byte)( ( ((Result & 0x00000008) >>  3) & 0x000000ff ) |
                                    ( ((Result & 0x00000200) >>  8) & 0x000000ff ) |
                                    ( ((Result & 0x80000000) >> 29) & 0x000000ff ) |
                                    ( ((Result & 0x00000002) <<  2) & 0x000000ff ) |
                                    ( ((Result & 0x00000800) >>  7) & 0x000000ff ) |
                                    ( ((Result & 0x00000004) <<  3) & 0x000000ff ) |
                                    ( ((Result & 0x02000000) >> 19) & 0x000000ff ) |
                                    ( ((Result & 0x02000000) >> 18) & 0x000000ff ) );

            List<byte> key = new List<byte>();

            foreach( byte k in gKeyArray ) key.Add(k);

            return key;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.12.2017
        LAST CHANGE:   11.07.2019
        ***************************************************************************/
        public static List<byte> ToyotaKey( List<byte> a_SeedArr )
        {
#if true
            string seed = Utils.Array2Str( a_SeedArr, a_SeedArr.Count, false );
                   seed = seed.Replace(" ","");
            string args = "";
            string arg1 = "";

            switch( m_KeyVar )
            {
                case "none": arg1 = "--encrypt"; break;
                case "L5":   arg1 = "--genmac";  break;
                default:     arg1 = "--encrypt"; break;
             }

            switch( m_ProdKey )
            {
                case "none":  args = string.Format("{1} --key 11111111111111111111111111111111 --message {0}", seed, arg1 );            break;
                default:      args = string.Format("{1} --key {2} --message {0}"                             , seed, arg1, m_ProdKey ); break;
            }

            List<byte> ret = Crypt.ToyotaAes( args );
#else
            byte[] ret = Crypt.AesEncrypt( a_SeedArr.ToArray(), Crypt.GetKey(0x01,32), Crypt.GetKey(0,32) );
#endif
            return new List<byte>(ret);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.03.2019
        LAST CHANGE:   01.07.2020
        ***************************************************************************/
        public static List<byte> ToyotaKey2( List<byte> a_SeedArr )
        {
          int        val;
          int        i, j = 0;
          int        b_FALSE = 0, b_TRUE = 1; 
          byte[]     key_array = {0,0,0,0,0,0};
          byte[]     Shift_TBL = { 1,2,3,3,2,1 }; 
          List<byte> CalcKey = new List<byte>();

          if ( a_SeedArr.Count < 6 ) return CalcKey;

          for(i = 0; i < 6; i++)
          {
              CalcKey.Add( a_SeedArr[i] );    
          } 
    
          for(i = 0; i < 6; i++)
          {    
              val = (a_SeedArr[i] >> Shift_TBL[i]) & 0x03; // Right_Shift( SEED[I] , Shift_TBL[I] ) & 0x03
              val++; // +1 
              val = (((a_SeedArr[i] << val)) | ((a_SeedArr[i] >> (8 - val)))); // Left_Rotate(...)
    
              j = a_SeedArr[i] & 0x07;
    
              if(j > 5)
              {
                    j = j-6; // "In case of J >= 6..." 
              }                       
              else
              { 
                /* do nothing */         
              }
      
              CalcKey[i] = (byte)(val + CalcKey[j]);    
          }
  
          return CalcKey;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.10.2019
        LAST CHANGE:   15.12.2021
        ***************************************************************************/
        public static List<byte> HondaKey1( List<byte> a_SeedArr, int a_KeyGrp )
        {
            ushort SK1 = 0;
            ushort SK2 = 0;
            ushort SK3 = 0;

            switch( a_KeyGrp )
            {
                case 1:
                    SK1 = 0x2878;
                    SK2 = 0xD32F;
                    SK3 = 0x84E8;
                    break;

                case 2:
                    SK1 = 0x86C8;
                    SK2 = 0xD67C;
                    SK3 = 0xB972;
                    break;

                default:
                    SK1 = 0x9345;
                    SK2 = 0xC7A0;
                    SK3 = 0xCACA;
                    break;
            }

            uint sk1 = (uint)SK1;
            uint sk2 = (uint)SK2;
            //uint sk3 = (uint)SK3;

            uint seed = (uint)Utils.U16FromByteList( ref a_SeedArr, false );

            uint key = ((seed + sk1) ^ ((seed * sk2) % SK3 ) & 0xffff);

            List<byte> CalcKey = new List<byte>();

            Utils.U16ToByteList( (ushort)key, ref CalcKey );
  
            return CalcKey;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.12.2021
        LAST CHANGE:   15.12.2021
        ***************************************************************************/
        public static List<byte> HondaKey4( List<byte> a_SeedArr )
        {
            const uint SK1 = 0xC93d9173;
            const uint SK2 = 0x3605af46;
            const uint SK3 = 0xc8256528;

            uint seed = (uint)Utils.U32FromByteList( ref a_SeedArr, false );
            uint sdh  = seed >> 16;
            uint sdl  = seed & 0xFFFF;

            uint key   = Utils.ROL32 ( seed + SK1, 2 );
                 key  ^= ( seed + SK2 );
                 key  ^= ( sdh  * sdl );
                 key  += SK3;

            List<byte> CalcKey = new List<byte>();

            Utils.U32ToByteList( key, ref CalcKey );
            CalcKey.AddRange( a_SeedArr );

            return CalcKey;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.12.2021
        LAST CHANGE:   15.12.2021
        ***************************************************************************/
        public static List<byte> HondaKey5( List<byte> a_SeedArr, int a_Var )
        {
            List<byte> sd = new List<byte>( a_SeedArr );

            ushort[,] SKS = new ushort[,]
            {
                { 0x5480, 0x9639, 0x514A },
                { 0xB9C8, 0xE4E9, 0xE938 },
                { 0xCD3C, 0xB4A6, 0xA8D8 },
                { 0x14C5, 0x9EF9, 0x7EE3 },
            };

            int vr = a_Var;
            ushort SK1 = SKS[vr,0];
            ushort SK2 = SKS[vr,1];
            ushort SK3 = SKS[vr,2];

            ushort seed = Utils.U16FromByteList( ref sd, false );

            ushort key  = Utils.ROR16 ( (ushort)(seed + SK1), 14 );
                   key ^= Utils.ROL16 ( (ushort)(seed + SK2),  2 );
                   key += SK3;

            List<byte> CalcKey = new List<byte>();

            Utils.U16ToByteList( key, ref CalcKey );
            CalcKey.AddRange( sd );

            return CalcKey;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.12.2021
        LAST CHANGE:   25.11.2022
        ***************************************************************************/
        public static List<byte> HondaKey6( List<byte> a_SeedArr, string a_AuxStr )
        {
            List<byte> sd = new List<byte>( a_SeedArr );

            if ( a_AuxStr != "none" )
            {
                List<string> segs = Utils.SplitExt( a_AuxStr, "," );
                if ( segs.Count == 3 )
                {
                    for( int i=0; i<3; i++ ) m_HaL6SKs[i] = (ushort)Utils.Hex2Int( segs[i] );
                }
            }

            ushort SK1 = m_HaL6SKs[0]; 
            ushort SK2 = m_HaL6SKs[1]; 
            ushort SK3 = m_HaL6SKs[2]; 

            ushort seed = Utils.U16FromByteList( ref sd, false );

            ushort key  = (ushort)(seed + SK1);
                   key ^= (ushort)(((uint)seed * (uint)SK2) % (uint)SK3);

            List<byte> CalcKey = new List<byte>();

            Utils.U16ToByteList( key, ref CalcKey );

            return CalcKey;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.03.2023
        LAST CHANGE:   01.03.2023
        ***************************************************************************/
        public static List<byte> MirrorSeed( List<byte> a_SeedArr, string a_AuxStr )
        {
            List<byte> sd = new List<byte>( a_SeedArr );

            List<byte> mr = new List<byte>();

            for ( int i=1; i<=sd.Count; i++ )
            {
                mr.Add( sd[sd.Count-i] );
            }

            sd.AddRange( mr );

            return sd;
        }


        /***************************************************************************
        SPECIFICATION: VW Seed n Key Conti
        CREATED:       25.04.2019
        LAST CHANGE:   26.04.2019
        ***************************************************************************/
        public static List<byte> VWKey1 ( List<byte> a_SeedArr )
        {
            uint seed = Utils.U32FromByteList( ref a_SeedArr, false );
            uint rt   = seed + 20103;

            List<byte> ret = new List<byte>();

            Utils.U32ToByteList( rt, ref ret );

            return ret;
        }
    }


} // namespace