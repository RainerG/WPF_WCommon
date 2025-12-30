using System; 
using System.IO;
//using System.IO.Compression;
using System.Windows.Forms;
using System.Globalization;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
//using System.ComponentModel;
//using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Win32;

namespace NS_WUtilities
{
	/// <summary>
	/// Summary description for  
	/// </summary>

	public class WUtils
	{
        [DllImport("kernel32.dll")]
        public static extern int GetVolumeInformation(string strPathName,
                                                       StringBuilder strVolumeNameBuffer,
                                                       int lngVolumeNameSize,
                                                       int lngVolumeSerialNumber,
                                                       int lngMaximumComponentLength,
                                                       int lngFileSystemFlags,
                                                       string strFileSystemNameBuffer,
                                                       int lngFileSystemNameSize);

        [DllImport("kernel32.dll")]
        public static extern int GetDriveType(string driveLetter);


        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        /***************************************************************************
        SPECIFICATION: constants
        CREATED:       17.04.2015
        LAST CHANGE:   09.01.2024
        ***************************************************************************/
        public const string EDITOR_UE   = "C:\\Program Files\\IDM Computer Solutions\\UltraEdit\\uedit64.exe";
        public const string EDITOR_NPP  = "C:\\Program Files\\Notepad++\\notepad++.exe";
        public const string EDITOR_NPP2 = "C:\\Program Files (x86)\\Notepad++\\notepad++.exe";
        public const string EDITOR_NPP3 = "C:\\LegacyApp\\Notepad++\\Notepad++.exe";
        public const string EDITOR_NPP4 = "C:\\LegacyApp\\Notepad++32\\Notepad++.exe";
 

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       17.09.2015
        LAST CHANGE:   7/15/2024
        ***************************************************************************/
        public  const  uint       INVALIDNR = 0xdeadbeef;

        public  static bool        LsbByteOrder;  // only for transport between classes
        public  static uint        SocVariant;    //       "
        private static DateTime    m_TimeMem;
        private static CultureInfo m_CI = CultureInfo.CurrentCulture;

        public  static readonly string[] CULTCODES = {"de-DE", "en-DE", "en-US", "en-GB", "en-IN", "ja" };

        /***************************************************************************
        SPECIFICATION: Ctor
        CREATED:       26.04.2006
        LAST CHANGE:   26.04.2006
        ***************************************************************************/
        public WUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.04.2006
        LAST CHANGE:   26.04.2006
        ***************************************************************************/
        static public string LimitPath(string a_sPath, int a_iMaxLen)
        {
            const string DOTS   = "...";

            string s;
            int iSubLen = (a_iMaxLen - DOTS.Length) / 2;

            if (a_sPath.Length > a_iMaxLen)
            {
                s =  a_sPath.Substring(0,iSubLen);
                s += DOTS;
                s += a_sPath.Substring(a_sPath.Length-iSubLen,iSubLen);
            }
            else   s = a_sPath;

            return s;
        }

        static public bool NoFile(string sFilePath)
        {
            if (File.Exists(sFilePath)) return false;

            MessageBox.Show("'" + sFilePath + "' not found","Error");

            return true;
        }

        static public bool NoFolder(string sDirectory)
        {
            if (Directory.Exists(sDirectory)) return false;

            MessageBox.Show("'" + sDirectory + "' not found","Error");

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.03.2006
        LAST CHANGE:   06.05.2009
        ***************************************************************************/
        static public string GetDriveName(string drive)
        {
            //receives volume name of drive
            StringBuilder volname = new StringBuilder(256);
            //receives serial number of drive,not in case of network drive(win95/98)
            int sn          = new int();
            int maxcomplen  = new int();//receives maximum component length
            int sysflags    = new int();//receives file system flags
            string sysname  = new string(new char[1]); //receives the file system name
            int retval      = new int();//return value

            if (drive == null) return "";

            string[] fracs = drive.Split(':');  // only the drive letter 

            if (fracs.Length < 1) return "";

            retval = GetVolumeInformation( fracs[0]+":", volname, 256, sn, maxcomplen, sysflags, sysname, 256 );

            if(0 == volname.Length)
            {
                switch( GetDriveType(drive) )
                {
                    case 5:     volname.Insert(0,"CD-ROM");         break;
                    case 3:     volname.Insert(0,"Fixed");          break;
                    case 2:     volname.Insert(0,"Removable disc"); break;
                    case 4:     volname.Insert(0,"Remote disc");    break;
                    case 6:     volname.Insert(0,"RAM disc");       break;
                    default:    volname.Insert(0,"");               break;
                }
            }

            return volname.ToString();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.09.2015
        LAST CHANGE:   25.09.2015
        ***************************************************************************/
        static public string AddExtension( string a_Fname, string a_Ext )
        {
            string ret = a_Fname;
            if ( ! ret.Contains(a_Ext) ) ret += a_Ext;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.04.2006
        LAST CHANGE:   03.02.2023
        ***************************************************************************/
        static public string ConcatPaths(string a_A,string a_B)
        {
            if (a_B   =="" ) return a_A;
            if (a_B.Length > 1 && a_B[1]==':') return a_B;
            if (a_A   =="" ) return a_B;

            string a = a_A.Replace("/","\\");
            string b = a_B.Replace("/","\\");

            if(b.StartsWith("\\"))
            {
                if(a.EndsWith("\\"))  return a + b.Remove(0,1);
                else                  return a + b;
            }
            else
            {
                if(a.EndsWith("\\"))  return a + b;
                else                  return a + "\\" + b;
            }
        }

        static public string ConcatPaths(string a,string b,string c)
        {
            string r = ConcatPaths( a, b );
                   r = ConcatPaths( r, c );
            return r;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.11.2013
        LAST CHANGE:   26.04.2021
        ***************************************************************************/
        static public string GoOneUp( string sPath ) { return GoOneUp( sPath, false ); }

        static public string GoOneUp( string a_Path, bool a_PathOnly )
        {
            if( a_Path.Length < 3 ) return a_Path + "\\";
            if( a_Path.Length < 5 ) return a_Path;

            string[] pa = a_Path.Split( '\\' );

            if (pa.Length == 1) return a_Path;

            List<string> pth = new List<string>( pa );

            if (pth.Count < 2) return a_Path + "\\";

            if (a_PathOnly) pth.RemoveAt( pth.Count - 1 );
            else            pth.RemoveAt( pth.Count - 2 );
                
            string ret = "";

            foreach( string p in pth )
            {
                ret += p + "\\";
            }

            if (ret.Length > 4) ret = ret.Remove( ret.Length-1, 1 );

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:      05.09.2016
        LAST CHANGE:  01.10.2021
        ***************************************************************************/
        static public string RelPath2Abs( string a_RelPath, bool a_IsFile )
        {
            if ( ! a_RelPath.StartsWith("..\\") ) return a_RelPath;

            string sect = a_RelPath;
            do
            {
                sect = sect.Remove( 0,3 );
            } while (sect.StartsWith("..\\"));

            string path = GetCurrDir();

            for (int i=0; i<20; i++) 
            {
                string pth = ConcatPaths( path, sect );
                if (a_IsFile) { if (File     .Exists(pth)) return pth; } 
                else          { if (Directory.Exists(pth)) return pth; }
                path = GoOneUp( path, ! a_IsFile );
            }

            return a_RelPath;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.01.2013
        LAST CHANGE:   18.12.2019
        ***************************************************************************/
        static public string CancelGoUps( string sPath )
        {
            string[] pa = sPath.Split('\\');

            List<string> pth = new List<string>(pa);

            pth.RemoveAll( p => p == ".."  );

            string ret = "";

            foreach ( string p in pth )
            {
                ret += p + "\\";
            }

            ret = ret.Remove(ret.Length-1,1);

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10/10/2016
        LAST CHANGE:   10/10/2016
        ***************************************************************************/
        static public bool FilesREqual( string a_FileA, string a_FileB )
        {
            if (! File.Exists(a_FileA) ) return false;
            if (! File.Exists(a_FileB) ) return false;

            FileInfo fia = new FileInfo(a_FileA);
            FileInfo fib = new FileInfo(a_FileB);

            if ( fia.Length         != fib.Length )         return false;
            if ( fia.LastWriteTime  != fib.LastWriteTime  ) return false;

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.09.2015
        LAST CHANGE:   08.09.2020
        ***************************************************************************/
        static public string SearchFileRecursively( string a_Fname, string a_StartPath )
        {
            string path = a_StartPath;

            try
            {
                List<string> files = new List<string>( Directory.GetFiles( path ) );

                string file = files.Find( f => f.ToLower().Contains( a_Fname.ToLower() ) );

                if (file!=null) return file;

                string[] dirs = Directory.GetDirectories( path );

                foreach( string dir in dirs )
                {
                    string ret = SearchFileRecursively( a_Fname, dir );

                    if (ret != "") return ret;
                }

                return "";
            }
            catch( Exception )
            {
                return "";
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:      26.09.2016
        LAST CHANGE:  26.09.2016
        ***************************************************************************/
        static private int          m_NestLevel   = 0;
        static private List<string> m_FailedPaths = new List<string>();

        static public string SearchDirRecursively( string a_StartPath, string a_Dir )
        {
            string   path = a_StartPath;
            string   dir  = a_Dir.ToLower();
 
            if (m_NestLevel++ > 3)      return "";
            if (path.Contains(".git") ) return "";
            string fp = m_FailedPaths.Find( p => path == p );
            if (fp != null)             return "";
            try
            {
                List<string> dirs = new List<string>( Directory.GetDirectories( path ) );

                foreach( string dr in dirs )
                {
                    if( dr.ToLower().Contains( dir ) ) return dr;

                    string ret = SearchDirRecursively( dr, a_Dir );
                    m_NestLevel--;

                    if( ret != "" ) return ret;
                    m_FailedPaths.Add( dr );
                }
            }
            catch( Exception )
            {
            }

            return "";
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.09.2016
        LAST CHANGE:   02.06.2020
        ***************************************************************************/
        static public string SearchDir( string a_Dir )
        {
            List<string> segs = SplitExt( a_Dir, "\\" );
            if (segs.Count == 0) return "";

            string dir        = segs[segs.Count-1].ToLower(); 
            string path       = GetCurrDir();

            m_FailedPaths.Clear();

            string pth = ConcatPaths(path,dir);

            if (Directory.Exists(pth)) return pth;

            for (int i=0; i<4; i++) path = GoOneUp(path,true);

            for (int i=0; i<10; i++ )
            {
                path = GoOneUp(path,true);

                if (!Directory.Exists(path)) return "";

                m_NestLevel = 0;

                string dr = SearchDirRecursively( path, dir );

                if (dr != "") return dr;
            }

            return "";
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.07.2023
        LAST CHANGE:   13.10.2023
        ***************************************************************************/
        static public bool DeleteDir( string a_Dir, bool a_FilesOnly = false )
        {
            if ( ! Directory.Exists( a_Dir ) ) return false;

            try
            {
                string[] fls = Directory.GetFiles( a_Dir );

                foreach( string f in fls ) File.Delete( f );

                string[] drs = Directory.GetDirectories( a_Dir );

                foreach( string d in drs ) DeleteDir( d, a_FilesOnly ); // recursive call

                if ( ! a_FilesOnly ) Directory.Delete( a_Dir );
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error deleting " + a_Dir );
                return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.11.2023
        LAST CHANGE:   21.01.2025
        ***************************************************************************/
        static public List<string> FilesInDir( string a_Dir )
        {
            List<string> ret = new List<string>();

            if ( ! Directory.Exists( a_Dir ) ) return ret;

            try
            {
                string[] fls = Directory.GetFiles( a_Dir );

                ret.AddRange(fls);

                string[] drs = Directory.GetDirectories( a_Dir );

                foreach( string d in drs ) ret.AddRange( FilesInDir( d ) ); // recursive call
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error browsing " + a_Dir );
                return ret;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.09.2020
        LAST CHANGE:   08.09.2020
        ***************************************************************************/
        static public string SearchFile( string a_File )
        {
            string ret = a_File; 

            if( ! File.Exists( a_File ) )
            {
                string fn = GetFilename( a_File );
                string po = GetPath( a_File );
                string ps = CancelGoUps( po );
                string pn = SearchDir( ps );
                if (pn != "")
                {
                    ret = ConcatPaths( pn, fn );
                }
                else 
                {
                    po = Directory.GetCurrentDirectory();

                    for (int j=0; j<10; j++ )
                    {
                        for ( int i=0; i<3; i++ ) po = GoOneUp( po, true );
                        pn = SearchFileRecursively( fn, po );

                        if (File.Exists(pn) )
                        {
                            ret = pn;
                            break;
                        }
                    }
                }
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: Retrieves a filename from a fully qualified path
        CREATED:       15.03.2007
        LAST CHANGE:   22.02.2017
        ***************************************************************************/
        static public string GetFilename(string sPath)
        {
            string path = sPath.Replace("/","\\");

            int idx = path.LastIndexOf("\\");

            if (-1 == idx)              return path;
            if (idx >= path.Length - 1) return "";

            return path.Substring(idx + 1);
        }


        /***************************************************************************
        SPECIFICATION: Retrieves a filename body from a fully qualified path
        CREATED:       24.05.2009
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        static public string GetFilenameBody( string sPath, bool a_WithPath = false )
        {
            string ret= "";

            string fn = GetFilename(sPath);

            string[] hlp = fn.Split('.');

            if (hlp.Length < 2) return hlp[0];

            for( int i = 0; i < hlp.Length - 1; i++ )
            {
                ret += hlp[i];  
                ret += ".";
            }

            if (ret.EndsWith(".")) ret = ret.Remove(ret.Length-1);

            if ( a_WithPath )
            {
                ret = ConcatPaths( GetPath(sPath), ret );
            }
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.05.2009
        LAST CHANGE:   15.04.2019
        ***************************************************************************/
        static public string GetExtension(string sPath)
        {
            string[] hlp = sPath.Replace("\"","").Split('.');

            int len = hlp.Length;

            if (len < 2) return "";

            return hlp[len - 1];
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.11.2020
        LAST CHANGE:   23.11.2020
        ***************************************************************************/
        static public string GetFileFilterStr( string a_Fname )
        {
            string ext = GetExtension( a_Fname );

            string ret = ext.ToUpper() + " files (*." + ext.ToLower() + ")|*." + ext.ToLower() + "|All files (*.*)|*.*";

            return ret; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.03.2007
        LAST CHANGE:   06.06.2025
        ***************************************************************************/
        static public string GetPath(string sPathFile)
        {
            if ( DirExists( sPathFile) ) return sPathFile;

            string path = sPathFile.Replace("/","\\");
            int idx = path.LastIndexOf('\\');

            if (-1 == idx) return sPathFile;

            return path.Substring(0,idx);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.11.2023
        LAST CHANGE:   08.11.2023
        ***************************************************************************/
        static public string AlignPath( string sPathFile, bool a_Tolower = false )
        {
            string path = sPathFile.Replace("\\","/");
            if ( a_Tolower ) path = path.ToLower();
            return path;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.10.2021
        LAST CHANGE:   13.10.2021
        ***************************************************************************/
        static public long GetFileLen( string a_Path )
        {
            if ( ! File.Exists(a_Path) ) return -1;

            FileInfo fi = new FileInfo( a_Path );
            return fi.Length;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.04.2009
        LAST CHANGE:   16.04.2009
        ***************************************************************************/
        static public string ReplaceDriveLetter(string sPath, string sDrive)
        {
            string ret = null;

            string[] hlp = sDrive.Split(':');
            string   ltr = hlp[0];

            string[] fracs = sPath.Split(':');

            if (fracs.Length < 1) return sPath;

            ret  = ltr;
            ret += ":";
 
            for ( int i=1; i<fracs.Length; i++ )
            {
                ret += fracs[i];    
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: returns the drive letter inclusive ':'.
        CREATED:       16.04.2009
        LAST CHANGE:   27.04.2011
        ***************************************************************************/
        static public string GetDriveLetter(string sPath)
        {
            string[] hlp = sPath.Split(':');

            if ( hlp.Length < 2 ) return "";

            return hlp[0] + ":";
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.05.2009
        LAST CHANGE:   08.05.2009
        ***************************************************************************/
        static public int RemoveNumber(ref string a_Name)
        {
            int idx = a_Name.IndexOfAny("0123456789".ToCharArray());

            if ( idx == -1 ) return idx;

            int count = 0;
            for ( int i = idx;i < a_Name.Length;i++ )
            {
                char c = a_Name[i];

                if ( c >= '0' && c <= '9' )
                {
                    count++;
                }
                else break;
            }

            a_Name = a_Name.Remove(idx,count);

            return idx;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.05.2009
        LAST CHANGE:   19.05.2009
        ***************************************************************************/
        static public int ExtractNumber(string a_Str)
        {
            int idx = a_Str.IndexOfAny("0123456789".ToCharArray());

            if ( idx == -1 ) return 0;

            int count = 0;
            for ( int i = idx; i < a_Str.Length; i++ )
            {
                char c = a_Str[i];

                if ( c >= '0' && c <= '9' )
                {
                    count++;
                }
                else break;
            }

            return int.Parse(a_Str.Substring(idx,count));
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.05.2009
        LAST CHANGE:   08.05.2009
        ***************************************************************************/
        static public string DeleteNumber(string a_Name)
        {
            string ret = a_Name;

            RemoveNumber(ref ret);

            return ret;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.05.2009
        LAST CHANGE:   08.05.2009
        ***************************************************************************/
        static public string ReplaceNumber(string a_Name,string a_Nr)
        {
            string ret = a_Name;

            int idx = RemoveNumber(ref ret);

            if ( idx == -1 ) return a_Name;

            ret = ret.Insert(idx,a_Nr);

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.02.2025
        LAST CHANGE:   01.07.2025
        ***************************************************************************/
        static public bool Align( int a_AlnMnt, ref int a_Val )
        {
            int ret = a_Val % a_AlnMnt;

            if (ret == 0) return false; 

            ret = a_AlnMnt - ret;

            a_Val += ret;

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.07.2025
        LAST CHANGE:   01.07.2025
        ***************************************************************************/
        static public bool AlignUp( int a_AlnMnt, ref int a_Val )
        {
            int ret = a_Val % a_AlnMnt;

            a_Val += a_AlnMnt - ret;

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.04.2013
        LAST CHANGE:   18.04.2013
        ***************************************************************************/
        static public Color PickColor ( Color inCol )
        {
            Color rCol = inCol;

            ColorDialog dlg = new ColorDialog();
            dlg.Color = inCol;
            DialogResult dres = dlg.ShowDialog();

            if( dres == DialogResult.OK )
            {
                rCol = dlg.Color;
            }

            return rCol;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.03.2013
        LAST CHANGE:   11.12.2013
        ***************************************************************************/
        static public Double Str2Double( string a_sArg )
        {
            try
            {
                bool bSign = false;

                string resp = a_sArg;

                if ( resp.StartsWith("0x") )
                {
                    int v = Hex2Int( resp );
                    resp  = v.ToString();
                }

                int idx = resp.IndexOfAny( "0123456789".ToCharArray() );

                if ( idx == -1 ) return 0.0;

                if ( idx > 0 && resp[idx-1] == '-' ) bSign = true;

                resp = resp.Remove( 0, idx );

                idx = resp.LastIndexOfAny( "0123456789".ToCharArray() ) + 1;

                if( idx < resp.Length ) resp = resp.Remove( idx );

                if ( resp.IndexOf(".") != -1 && resp.IndexOf(",") != -1 )
                { // , is supposed to be a separator of more numbers
                    string[] nrs = resp.Split(',');
                    resp = nrs[0];
                }

                string ds = m_CI.NumberFormat.CurrencyDecimalSeparator;

                resp = resp.Replace( '.', ds[0] );
                

                Double ret = Double.Parse( resp );

                if ( bSign ) ret *= -1.0;

                return ret;
            }
            catch (System.FormatException)
            {
                return(0.0);                                            	
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Str2Double exception");
                
                return 0.0;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.05.2016
        LAST CHANGE:   19.05.2016
        ***************************************************************************/
        static public List<double> Str2Doubles( string a_Arg )
        {
            string str = a_Arg;

            List<double> ret = new List<double>();

            try
            {
                do
                {
                    int idxs = str.IndexOfAny( "0123456789".ToCharArray() );

                    if ( idxs == -1 ) return ret;

                    int i = 0;
                    for( i = idxs; i < str.Length; i++ )
                    {
                        char ch = str[i];
                        if ( char.IsDigit( ch ) ) continue;
                        if ( ch == '.')           continue;
                        if ( ch == ',')           continue;
                        break;
                    }

                    string ds = str.Substring(idxs,i-idxs);
                    ret.Add( double.Parse(ds) );

                    str = str.Remove(0,i);

                } while ( str.Length > 0 );
            }
            catch (System.FormatException)
            {
                // do nothing
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Str2Double exception");
            }

            return ret;                                            	
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.01.2014
        LAST CHANGE:   30.01.2014
        ***************************************************************************/
        static public string GetAppDir() { return GetAppDir( true ); } 
        static public string GetAppDir( bool a_BckSlsh )
        {
            string ret = Application.StartupPath;
            if (a_BckSlsh) ret += "\\";
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.01.2014
        LAST CHANGE:   30.01.2014
        ***************************************************************************/
        static public string GetCurrDir()
        {
            return Directory.GetCurrentDirectory() + "\\";
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.06.2020
        LAST CHANGE:   16.06.2020
        ***************************************************************************/
        static public string ReplacePath( ref string a_File, string a_Path )
        {
            string fname = GetFilename( a_File );
            a_File = ConcatPaths( a_Path, fname );
            return a_File;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.02.2017
        LAST CHANGE:   15.02.2017
        ***************************************************************************/
        static private void TraverseTree( string a_Dir, string a_FilePat, ref List<string> a_FileList )
        {
            string[] dirs = Directory.GetDirectories( a_Dir );
            string[] fils = Directory.GetFiles      ( a_Dir, a_FilePat ); 

            a_FileList.AddRange(fils);

            foreach( string dir in dirs )
            {
                TraverseTree( dir, a_FilePat, ref a_FileList );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.02.2017
        LAST CHANGE:   10.11.2020
        ***************************************************************************/
        static public List<string> GetFilesRecursive( string a_Dir, string a_FilePat )
        {
            List<string> files = new List<string>();

            if ( Directory.Exists(a_Dir) ) TraverseTree( a_Dir, a_FilePat, ref files );

            return files;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.07.2018
        LAST CHANGE:   19.07.2018
        ***************************************************************************/
        static public bool CopyFolder(string SourcePath, string DestinationPath)
        {
           SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
           DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";
 
           try
           {
              if (Directory.Exists(SourcePath))
              {
                 if (Directory.Exists(DestinationPath) == false)
                 {
                    Directory.CreateDirectory(DestinationPath);
                 }

                 foreach (string files in Directory.GetFiles(SourcePath))
                 {
                    FileInfo fileInfo = new FileInfo(files);
                    fileInfo.CopyTo(string.Format(@"{0}\{1}", DestinationPath, fileInfo.Name), true);
                 }

                 foreach (string drs in Directory.GetDirectories(SourcePath))
                 {
                    DirectoryInfo directoryInfo = new DirectoryInfo(drs);
                    if (CopyFolder(drs, DestinationPath + directoryInfo.Name) == false)
                    {
                       return false;
                    }
                 }
              }
              return true;
           }
           catch (Exception ex)
           {
              return false;
           }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.01.2024
        LAST CHANGE:   23.01.2024
        ***************************************************************************/
        static public bool FileCopyExt( string a_Src, string a_Dst, bool a_OvrWrt = true )
        {
            string dir = GetPath( a_Src );
            string fil = GetFilename( a_Src );
            string dfl = GetFilename( a_Dst );

            if ( ! Directory.Exists( dir ) ) return false;

            string[] fils = Directory.GetFiles( dir, fil );

            foreach( string f in fils )
            {
                string fn  = GetFilename(f);
                string dst = a_Dst.Replace( dfl, fn );

                File.Copy( f, dst, a_OvrWrt );
                File.SetLastWriteTimeUtc( dst, DateTime.UtcNow );  
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.01.2014
        LAST CHANGE:   30.01.2014
        ***************************************************************************/
        static public string GetSlnDir()
        {
            string slndir = GetCurrDir();
            slndir = GoOneUp(slndir);
            slndir = GoOneUp(slndir);
            slndir = GoOneUp(slndir);
            return slndir;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.04.2009
        LAST CHANGE:   27.02.2015
        ***************************************************************************/
        static public string GetDriveByVolName( string sVolName )
        {
            if ( sVolName == "" )               return null;
            if ( sVolName.Contains("Fixed") )   return null;
            if ( sVolName.Contains("Volume"))   return null;

            string[] drv = Directory.GetLogicalDrives(); 

            foreach(string d in drv)
            {
                if (d[0]=='A' || d[0]=='B') continue;

                string vn = GetDriveName(d);

                if (sVolName.Contains(vn))
                {
                    return d.Substring(0,2);
                }
            }

            return null;
        }

        
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.03.2015
        LAST CHANGE:   30.06.2022
        ***************************************************************************/
        static public UInt32 Hex2UInt( string hexval )
        {
            try
            {
                string hex = hexval.Replace("0x","").Trim();
                       hex = hex.Replace("h","");
                       hex = hex.Replace(" ","");
                       hex = hex.Replace("u","");
                       hex = hex.Replace("U","");
                return Convert.ToUInt32 ( hex, 16 );
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.03.2022
        LAST CHANGE:   28.07.2023
        ***************************************************************************/
        static public int Hex2Int( string hexval )
        {
            try
            {
                bool min = false;
                string hex = hexval.Replace("0x","").Trim();
                hex = hex.Replace("h","");
                if (hex.StartsWith("-"))
                {
                    min = true;
                    hex = hex.Replace("-","");
                }
                hex = hex.Replace(" ","");
                hex = hex.Replace("u","");
                hex = hex.Replace("U","");
                int ret = Convert.ToInt32 ( hex, 16 );
                if (min) ret = -ret;
                return ret;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.08.2015
        LAST CHANGE:   07.12.2022
        ***************************************************************************/
        static public byte Hex2Byte( string hexval )
        {
            try
            {
                string hv = hexval.Replace("u","");
                hv = hv.Replace("0x","");
                hv = hv.Replace("h","");
                return Convert.ToByte ( hv, 16 );
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.06.2015
        LAST CHANGE:   08.06.2022
        ***************************************************************************/
        static public UInt32 Str2UInt( string strval )
        {
            try
            {
                switch(strval)
                {
                    case "none":
                    case "---":
                    case "-":
                    case "":
                        return 0;
                }

                if( strval.Contains( "." ) ) return (UInt32)Str2Dbl(strval);

                string s = strval.ToLower().Replace("u","");
                       s = s     .ToLower().Replace("l","");
                       s = s     .ToLower().Replace("(","");
                       s = s     .ToLower().Replace(")","");
                if (s.IndexOfAny("abcdef".ToCharArray()) != -1) return Hex2UInt(s);
                if (s.IndexOf("0x") != -1)                      return Hex2UInt(s);
                if (s.IndexOf("-")  != -1)                      return (UInt32)Convert.ToUInt32( s );
                return Convert.ToUInt32 ( s );
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n=> " + strval, "Exception in Str2UInt" );
                throw new ArgumentException( "string to uint conversion error: " + strval );
                return INVALIDNR;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.12.2018
        LAST CHANGE:   10.12.2018
        ***************************************************************************/
        static public UInt32 Str2CanID( string a_Str )
        {
            string str = a_Str;
            bool hex = str.Contains("0x");
            str = str.Replace("0x","");
            bool ext = str.Contains("x");
            str = str.Replace("x","");
            if (hex) str = "0x" + str;
            uint ret = Str2UInt( str );
            if (ext) ret |= 0x80000000;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.06.2015
        LAST CHANGE:   09.04.2021
        ***************************************************************************/
        static public Int32 Str2Int ( string strval ) { return (Int32)Str2Long( strval ); } 
        static public long  Str2Long( string strval )
        {
            try
            {
                switch(strval)
                {
                    case "none":
                    case "---":
                    case "-":
                    case "":
                        return 0;
                }

                if( strval.Contains( "." ) ) return (long)Str2Dbl(strval);

                string s = strval.ToLower().Replace("u","");
                if (s.IndexOf("0x") != -1) return (long)Hex2Int(s);
                if (s.IndexOf("-")  != -1) return (long)Convert.ToInt64( s );
                return Convert.ToInt64 ( s );
            }
            catch( ThreadAbortException )
            {
                return INVALIDNR;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n=> " + strval, "Exception in Str2Long");
                return INVALIDNR;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.10.2020
        LAST CHANGE:   05.05.2023
        ***************************************************************************/
        static public byte Str2Byte( string a_Str ) { return Str2Byte( a_Str, 10 ); }
        static public byte Str2Byte( string a_Str, int a_Base )
        {
            int    bse = a_Base;
            string str = a_Str;

            try
            {
                switch( str )
                {
                    case "none": return 0xff;
                }

                if ( str.Contains("0x") || str.ToLower().Contains("h") ) return Hex2Byte( str );
                return Convert.ToByte( str, bse );
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message + "\n=> " + str, "Exception in Str2Byte" );
                return 0xFF;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.06.2015
        LAST CHANGE:   22.06.2015
        ***************************************************************************/
        static public Double Str2Dbl( string strval )
        {
            if (strval == "---") return 0.0;

            string s = strval.Replace(".",",");
            return Convert.ToDouble ( s );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.01.2016
        LAST CHANGE:   17.10.2016
        ***************************************************************************/
        static public bool Str2Bool( string strval )
        {
            string s = strval.ToLower();
            if ( s.Contains("false") ) return false;
            if ( s.Contains("true" ) ) return true;
            if ( s.Contains("0") )     return false;
            if ( s.Contains("1") )     return true;
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       9/22/2016
        LAST CHANGE:   9/22/2016
        ***************************************************************************/
        static public byte Byte2BCD( int  a_Byte ) { return Byte2BCD( (byte)a_Byte ); }
        static public byte Byte2BCD( byte a_Byte )
        {
            int low  = a_Byte % 10;
            int hig  = a_Byte - low;
                hig /= 10;

            int ret  = hig * 0x10;
                ret += low;

            return (byte)ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.03.2015
        LAST CHANGE:   22.05.2023
        ***************************************************************************/
        static public List<string> SplitExt( string a_Line, string a_Sep, bool a_Tolower = false )
        {
            string[] hlp;

            List<string> ret = new List<string>();
            
            if (a_Line == null) return ret;

            string ln = a_Line.Trim();

            hlp = ln.Split( a_Sep.ToCharArray() );

            foreach( string s in hlp )
            {
                if( s != "" ) 
                {
                    if (a_Tolower) ret.Add( s.Trim().ToLower() );
                    else           ret.Add( s.Trim() ); 
                }
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: It does not ignore empty segments
        CREATED:       22.05.2023
        LAST CHANGE:   27.03.2024
        ***************************************************************************/
        static public List<string> SplitExt2( string a_Line, string a_Sep, bool a_Trim = true )
        {
            string[] hlp;

            List<string> ret = new List<string>();
            
            if (a_Line == null) return ret;

            string ln = a_Trim ? a_Line.Trim() : a_Line;

            hlp = ln.Split( a_Sep.ToCharArray() );

            foreach( string s in hlp ) ret.Add( a_Trim ? s.Trim() : s ); 

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.05.2016
        LAST CHANGE:   30.01.2023
        ***************************************************************************/
        static public List<string> Split2Lst( string a_Line, string a_Sep )
        {
            string[] hlp;

            List<string> ret = new List<string>();

            string ln = a_Line.Trim();

            hlp = ln.Split( a_Sep.ToCharArray() );

            foreach( string s in hlp ) ret.Add( s.Trim().Replace("'","") ); 

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.11.2016
        LAST CHANGE:   26.11.2016
        ***************************************************************************/
        static public List<string> SplitOnly( string a_Line, string a_Sep )
        {
            string[] hlp;

            hlp = a_Line.Split( a_Sep.ToCharArray() );

            List<string> ret = new List<string>(hlp);
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.03.2015
        LAST CHANGE:   14.05.2025
        ***************************************************************************/
        static public void Edit( string fpath, int linenr = -1 )
        {
            string args = fpath;

            try
            {
                args = RelPath2Abs( args, true );

                string dir  = GetPath(args);
                if (dir == "" ) dir = Directory.GetCurrentDirectory();
                
                if ( ! Directory.Exists( dir ) )
                {
                    DialogResult dres =  MessageBox.Show("Directory " + dir + " does not exist !\nCreate ?","Note",MessageBoxButtons.YesNo);
                    if ( dres == DialogResult.Yes )
                    {
                        CreatePath(dir);
                    }
                    else return;
                }

                switch ( GetExtension(args).ToLower() )
                {
                    case "xls" :
                    case "xlsx":
                    case "xlsm":
                        Process.Start( "Excel.exe",args );
                        return;

                    case "bin":
                        string pth = IsProgramInstalled( "HexView");
                        if (pth == "") pth = "c:\\Tools\\HexView\\hexview.exe";
                        else           pth = ConcatPaths( pth, "hexview.exe" );

                        if ( File.Exists( pth ) )
                        {
                            Process.Start( pth, args );
                           return;
                        }
                        break;
                }

                args = "\"" + args + "\"";

                if( linenr > -1 )  args += " -n" + linenr.ToString();

                if ( File.Exists( EDITOR_NPP  ) ) { Process.Start( EDITOR_NPP,  args ); return; }
                if ( File.Exists( EDITOR_NPP2 ) ) { Process.Start( EDITOR_NPP2, args ); return; }
                if ( File.Exists( EDITOR_NPP3 ) ) { Process.Start( EDITOR_NPP3, args ); return; }
                if ( File.Exists( EDITOR_NPP4 ) ) { Process.Start( EDITOR_NPP4, args ); return; }

                if ( File.Exists( EDITOR_UE ) )
                {
                    if ( linenr > -1 ) args += " -l" + linenr.ToString();
                    Process.Start( EDITOR_UE, args );
                    return;
                }

                Process.Start( "notepad.exe", args );
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error opening " + args );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.05.2025
        LAST CHANGE:   13.05.2025
        ***************************************************************************/
        static public void Explorer( string fpath, bool a_Path = false )
        {
            string args = fpath;

            try
            {
                args = RelPath2Abs( args, true );

                string dir  = GetPath(args);
                if (dir == "" ) dir = Directory.GetCurrentDirectory();
                
                if ( ! Directory.Exists( dir ) )
                {
                    DialogResult dres =  MessageBox.Show("Directory " + dir + " does not exist !\nCreate ?","Note",MessageBoxButtons.YesNo);
                    if ( dres == DialogResult.Yes )
                    {
                        CreatePath(dir);
                    }
                    else return;
                }

                string pth = args;

                if (a_Path)
                {
                    pth = GetPath( args );

                    string tcp = IsProgramInstalled( "Total Commander" );
                    tcp = ConcatPaths( tcp,  "TOTALCMD64.EXE" );

                    if ( File.Exists( tcp ) )
                    {
                        Process.Start( tcp, pth );
                        return;
                    }
                }

                Process.Start( "explorer.exe", pth );
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error opening " + args );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.05.2025
        LAST CHANGE:   13.05.2025
        ***************************************************************************/
        static public string IsProgramInstalled( string programDisplayName )
        {
            string ret = "";
            string prgdspnm = programDisplayName.ToLower();

            foreach( var item in Registry.LocalMachine.OpenSubKey( "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall" ).GetSubKeyNames() )
            {
                object programName = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + item ).GetValue( "DisplayName" );
                string prognm = programName as string;

                if (programName == null ) continue;

                if( prognm.ToLower().Contains( prgdspnm.ToLower() ) )
                {
                    ret = (string)Registry.LocalMachine.OpenSubKey( "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + item ).GetValue( "InstallLocation" );
                    break;
                }
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.04.2016
        LAST CHANGE:   28.04.2016
        ***************************************************************************/
        static public bool CreatePath( string a_Path )
        {
            try
            {
                string[] segs = a_Path.Split("\\".ToCharArray());
                string path = "";

                foreach( string seg in segs )
                {
                    if (seg.Contains(".")) return true;
                    if (seg.Contains(":")) path = seg;
                    else
                    {
                        path += "\\" + seg;
                        if( ! Directory.Exists( path ) ) Directory.CreateDirectory(path);
                    }
                }

                return true;
            }
            catch( Exception )
            {
                return false;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.09.2015
        LAST CHANGE:   03.09.2015
        ***************************************************************************/
        static public UInt32 Bytes2UInt( byte b1, byte b2, byte b3, byte b4 )
        {
            UInt32 
            ret =  b1;
            ret <<= 8;
            ret += b2;
            ret <<= 8; 
            ret += b3;
            ret <<= 8; 
            ret += b4;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.04.2024
        LAST CHANGE:   12.04.2024
        ***************************************************************************/
        static public bool ShlByteArr( ref List<byte> a_Arr, bool a_NewBit = false )
        {
            bool ret = (int)( a_Arr[0] & 0x80 ) != 0;
            bool a = false;
            bool b = a_NewBit;

            for( int i = a_Arr.Count - 1; i >= 0; i-- )
            {
                a = (int)( a_Arr[i] & 0x80 ) != 0;

                a_Arr[i] <<= 1;

                a_Arr[i] |= (byte)(b ? 1 : 0);

                b = a;
            }
            
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.04.2017
        LAST CHANGE:   17.08.2017
        ***************************************************************************/
        //static public List<string> File2List( string a_DfltDir )
        //{
        //    try
        //    {
        //        OpenFileDialog dlg = new OpenFileDialog();

        //        dlg.InitialDirectory = a_DfltDir;

        //        DialogResult res = dlg.ShowDialog();

        //        if (res == DialogResult.Abort) return null;

        //        string file = dlg.FileName;

        //        List<string> ret = new List<string>( File.ReadAllLines(file) );

        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show( ex.Message, "Error reading file" );
        //        return null;            	
        //    }
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.03.2022
        LAST CHANGE:   21.03.2022
        ***************************************************************************/
#if false // not yet needed
        static public string File2Text( string a_DfltDir )
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();

                dlg.InitialDirectory = a_DfltDir;

                DialogResult res = dlg.ShowDialog();

                if (res == DialogResult.Abort) return null;

                string file = dlg.FileName;

                string ret = File.ReadAllText(file);

                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Error reading file" );
                return null;            	
            }
        }
#endif
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   01.09.2015
        ***************************************************************************/
        static private byte ToByteListSub( UInt32 a_Wrd )   
        { 
            return (byte)( a_Wrd & 0x000000ff ); 
        }

        static private byte ToByteListSub( UInt16 a_Wrd )   
        { 
            return (byte)( a_Wrd & 0x00ff ); 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       31.08.2015
        LAST CHANGE:   31.08.2015
        ***************************************************************************/
        static public void U8ToByteList( byte a_Byte, ref List<byte> a_List )
        {
            a_List.Add( a_Byte );
        }

        static public void U8ToByteList( byte a_Byte, byte[] a_List, ref int a_Idx )
        {
            a_List[ a_Idx++ ] = a_Byte;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   12.08.2015
        ***************************************************************************/
        static public void U16ToByteList( UInt16 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub(  (UInt16)(a_Wrd >> 8)  ) );
            a_List.Add( ToByteListSub(           a_Wrd        ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.07.2020
        LAST CHANGE:   21.07.2020
        ***************************************************************************/
        static public void U16ToByteList( UInt16 a_Wrd, byte[] a_List, ref int a_Idx )
        {
            a_List[ a_Idx++ ] = ToByteListSub(  (UInt16)(a_Wrd >> 8)  );
            a_List[ a_Idx++ ] = ToByteListSub(           a_Wrd        );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.09.2015
        LAST CHANGE:   21.09.2015
        ***************************************************************************/
        static public void U16ToByteListSwp( UInt16 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub(           a_Wrd        ) );
            a_List.Add( ToByteListSub(  (UInt16)(a_Wrd >> 8)  ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.09.2015
        LAST CHANGE:   03.09.2015
        ***************************************************************************/
        static public void U24ToByteList( UInt32 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub( a_Wrd >> 16 ) );
            a_List.Add( ToByteListSub( a_Wrd >>  8 ) );
            a_List.Add( ToByteListSub( a_Wrd       ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2015
        LAST CHANGE:   20.10.2020
        ***************************************************************************/
        static public void I32ToByteList( int  a_Wrd, ref List<byte> a_List ) { U32ToByteList( (uint) a_Wrd, ref a_List ); }
        static public void U32ToByteList( uint a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub( a_Wrd >> 24 ) );
            a_List.Add( ToByteListSub( a_Wrd >> 16 ) );
            a_List.Add( ToByteListSub( a_Wrd >>  8 ) );
            a_List.Add( ToByteListSub( a_Wrd       ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.03.2017
        LAST CHANGE:   05.03.2017
        ***************************************************************************/
        static public void U64ToByteList( UInt64 a_DWrd, ref List<byte> a_List )
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
        static public void U64ToByteListSwp( UInt64 a_DWrd, ref List<byte> a_List )
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
        CREATED:       29.06.2016
        LAST CHANGE:   29.06.2016
        ***************************************************************************/
        static public void U32ToByteList( uint a_Wrd, ref List<byte> a_List, ref int a_Idx )
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
        static public void U32ToByteList( uint a_Wrd, byte[] a_List, ref int a_Idx )
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
        static public void U32ToByteListSwp( UInt32 a_Wrd, ref List<byte> a_List )
        {
            a_List.Add( ToByteListSub( a_Wrd       ) );
            a_List.Add( ToByteListSub( a_Wrd >>  8 ) );
            a_List.Add( ToByteListSub( a_Wrd >> 16 ) );
            a_List.Add( ToByteListSub( a_Wrd >> 24 ) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.01.2017
        LAST CHANGE:   18.01.2023
        ***************************************************************************/
        static public void U16ToByteList( ushort a_Wrd, ref List<byte> a_List, ref int a_Idx )
        {
            if ( a_Idx >= a_List.Count-1 ) return;
            a_List[a_Idx++] = (byte)( a_Wrd >>  8 );
            a_List[a_Idx++] = (byte)( a_Wrd       );
        }

        /***************************************************************************
        SPECIFICATION: a_BLst is copied into a_List at index a_Idx
        CREATED:       14.11.2019
        LAST CHANGE:   14.11.2019
        ***************************************************************************/
        static public bool ByteListToByteList( List<byte> a_BLst, ref List<byte> a_List, ref int a_Idx )
        {
            if ( a_List.Count < a_Idx + a_BLst.Count ) return false;
            
            a_List.RemoveRange( a_Idx, a_BLst.Count );
            a_List.InsertRange( a_Idx, a_BLst );

            a_Idx += a_BLst.Count;

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.01.2021
        LAST CHANGE:   09.08.2022
        ***************************************************************************/
        public static List<byte> CreateByteList( byte a_Val, int a_Length ) { return CreateByteList( a_Val, (uint)a_Length ); }
        public static List<byte> CreateByteList( byte a_Val, uint a_Length )
        {
            List<byte> ret = new List<byte>();
            for (int i=0; i<a_Length; i ++) ret.Add(a_Val);
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.04.2021
        LAST CHANGE:   15.04.2021
        ***************************************************************************/
        public static List<byte> ByteListSub( List<byte> a_Src, int a_Start, int a_Length )
        {
            byte[] ret = new byte[a_Length];

            a_Src.CopyTo( a_Start, ret, 0, a_Length );

            return new List<byte>(ret);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.01.2017
        LAST CHANGE:   10.01.2017
        ***************************************************************************/
        static public int UintLen( UInt32 a_Wrd )
        {
            if (a_Wrd > 0xffffff) return 4;
            if (a_Wrd > 0xffff)   return 3;
            if (a_Wrd > 0xff)     return 2;
            if (a_Wrd > 0)        return 1;
            return 0;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.09.2015
        LAST CHANGE:   16.09.2015
        ***************************************************************************/
        static public void StrToByteList( string a_Str, ref List<byte> a_List )
        {
            foreach( char c in a_Str ) a_List.Add((byte)c);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.07.2019
        LAST CHANGE:   10.07.2019
        ***************************************************************************/
        static public List<byte> Str2ByteList( string a_Str )
        {
            List<byte> ret = new List<byte>();
            StrToByteList( a_Str, ref ret );
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.04.2020
        LAST CHANGE:   09.09.2024
        ***************************************************************************/
        static public bool ByteListCmp( List<byte> a_Arr, int a_Start, string a_BtArr )
        {
            List<byte> btarr = new List<byte>();
            
            List<string> segs = ChopByteStr( a_BtArr );
            if (segs == null) return false;

            foreach( string bt in segs )
            {
                Byte b = Hex2Byte(bt); 
                btarr.Add( b );
            }

            int cnt = btarr.Count;

            if ( cnt + a_Start > a_Arr.Count ) return false;

            for ( int i = 0; i < cnt; i++ )
            {
                if ( a_Arr[ i + a_Start ] != btarr[ i ] ) return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.04.2020
        LAST CHANGE:   17.04.2020
        ***************************************************************************/
        static public bool ByteListCmp( List<string> a_Arr, int a_Start, string a_BtArr )
        {
            List<string> segs = ChopByteStr( a_BtArr );
            if (segs == null) return false;

            int cnt = segs.Count;
            
            if ( cnt + a_Start > a_Arr.Count ) return false;

            for ( int i = 0; i < cnt; i++ )
            {
                if ( a_Arr[ i + a_Start ] != segs[ i ] ) return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: Byte list compare
        CREATED:       16.12.2021
        LAST CHANGE:   16.12.2021
        ***************************************************************************/
        static public bool ByteListCmp( List<byte> a_LstA, List<byte> a_LstB )
        {
            if ( a_LstA.Count != a_LstB.Count ) return false;

            for ( int i = 0; i < a_LstA.Count; i++ )
            {
                if ( a_LstA[i] != a_LstB[i] ) return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: Chops a byte worm (11 22 33 44 ...) into a list of byte strings
                       returns null if a_ByteStr does not have the proper format
        CREATED:       15.04.2020
        LAST CHANGE:   17.04.2020
        ***************************************************************************/
        static public List<string> ChopByteStr( string a_ByteStr )
        {
            string line = a_ByteStr;

            List<string> segs = SplitExt( line, " ,\t" );

            if ( segs.Count == 0 )   return null;
            if (! IsHex(segs[0]) )   return null;
            if (segs[0].Length < 2)  return null;

            if (segs.Count < 3)
            {
                segs = new List<string>();
                while( line.Length > 1 ) 
                {
                    segs.Add(line.Substring(0,2));
                    line = line.Remove(0,2);
                }
            }
            else
            {
                List<string> lst = segs.FindAll( s => s.Length > 2 || ! IsHex(s) );
                foreach ( string l in lst )
                {
                    if (Str2UInt(l) > 0xff) segs.Remove( l );
                }
            }

            return segs;
        }

        /***************************************************************************
        SPECIFICATION: Chops up the byte list in chunks of length a_Len.
        CREATED:       01.06.2023
        LAST CHANGE:   01.06.2023
        ***************************************************************************/
        static public List<List<byte>> ChopByteList( List<byte> a_Data, int a_Len )
        {
            List<List<byte>> ret = new List<List<byte>>();

            List<byte> hlp = new List<byte>( a_Data );

            do
            {
                int ln = hlp.Count >= a_Len ? a_Len : hlp.Count;
                byte[] arr = new byte[ln];

                hlp.CopyTo( 0, arr, 0, ln );
                hlp.RemoveRange( 0, ln );

                ret.Add( new List<byte>(arr) );
            } while( hlp.Count > 0 );

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: Polimorphic calls
        CREATED:       21.09.2015
        LAST CHANGE:   14.10.2022
        ***************************************************************************/
        static public void VarToByteList( string       a_Str, ref List<byte> a_List ) { StrToByteList ( a_Str, ref a_List); }
        static public void VarToByteList( List<byte>   a_Str, ref List<byte> a_List ) { a_List.AddRange( a_Str ); }
        //static public void VarToByteList( List<ushort> a_Str, ref List<byte> a_List ) { U16ListToByteList ( a_Str, ref a_List); }
        static public void VarToByteList( byte         a_Var, ref List<byte> a_List ) { U8ToByteList  ( a_Var, ref a_List); }
        static public void VarToByteList( UInt16       a_Var, ref List<byte> a_List ) { U16ToByteList ( a_Var, ref a_List); }
        static public void VarToByteList( UInt32       a_Var, ref List<byte> a_List ) { U32ToByteList ( a_Var, ref a_List); }
        static public void VarToByteList( int          a_Var, ref List<byte> a_List ) { I32ToByteList ( a_Var, ref a_List); }
        static public void SwpToByteList( UInt16       a_Var, ref List<byte> a_List ) { U16ToByteListSwp ( a_Var, ref a_List); }
        static public void SwpToByteList( UInt32       a_Var, ref List<byte> a_List ) { U32ToByteListSwp ( a_Var, ref a_List); }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.02.2017
        LAST CHANGE:   14.04.2025
        ***************************************************************************/
        static public List<byte> HexStr2ByteList( string a_HexStr )
        {
            List<byte> ret = new List<byte>();

            string hexstr = a_HexStr.Replace("0x","");
                   while ( hexstr.IndexOf(' ') != -1 ) { hexstr = hexstr  .Replace(" " ,""); }
                   hexstr = hexstr  .Replace("," ,"");
                   hexstr = hexstr  .Replace("\r","");
                   hexstr = hexstr  .Replace("\n","");
                   hexstr = hexstr  .Replace("\t","");
            if (hexstr.Length == 1) hexstr = "0" + hexstr;

            while(hexstr.Length > 1)
            {
                string hex = hexstr.Substring(0,2);
                ret.Add( Hex2Byte(hex) );
                hexstr = hexstr.Remove(0,2);
            }

            return ret;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.07.2020
        LAST CHANGE:   24.02.2021
        ***************************************************************************/
        static public string ByteList2HexStr( List<byte> a_Data ) { return ByteList2HexStr( a_Data.ToArray(), false ); }
        static public string ByteList2HexStr( byte[] a_Data )     { return ByteList2HexStr( a_Data, false ); }
        static public string ByteList2HexStr( List<byte> a_Data, bool a_Spce ) { return ByteList2HexStr( a_Data.ToArray(), a_Spce ); }
        static public string ByteList2HexStr( byte[] a_Data, bool a_Spce )
        {
            string ret = "";

            foreach( byte b in a_Data )
            {
                ret += string.Format( "{0:X2}", b );
                if (a_Spce) ret += " ";
            }
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.05.2021
        LAST CHANGE:   07.05.2021
        ***************************************************************************/
        static public List<byte> Base64toByteList( string a_BaseStr )
        {
            List<byte> ret = new List<byte>();
            foreach( char c in a_BaseStr ) ret.Add( (byte)c );
            return ret;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.08.2015
        LAST CHANGE:   07.07.2025
        ***************************************************************************/
        static public byte U8FromByteList( ref List<byte> a_List, ref int a_Idx ) { return a_List[a_Idx++]; }
        static public byte U8FromByteList( ref List<byte> a_List ) 
        {
            if ( a_List.Count == 0 ) return 0;
            byte ret = a_List[0];
            a_List.RemoveAt(0);
            return ret;
        }
        static public byte U8FromByteList( byte[] a_List, ref int a_Idx )
        {
            if ( a_List.Length <= a_Idx ) return 0;
            return a_List[a_Idx++]; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.09.2018
        LAST CHANGE:   18.09.2018
        ***************************************************************************/
        static public UInt16 U16FromByteList( ref List<byte> a_List, bool a_Swp )
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
        static public UInt16 U16FromByteList( ref List<byte> a_List )
        {
            UInt16 ret = 0;

            if (a_List.Count < 2) return 0;

            ret += a_List[0]; a_List.RemoveAt(0); ret <<= 8;
            ret += a_List[0]; a_List.RemoveAt(0);

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.02.2023
        LAST CHANGE:   17.02.2023
        ***************************************************************************/
        static public Int16 I16FromByteList( ref List<byte> a_List )
        {
            Int16 ret = 0;

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
        static public UInt32 U24FromByteList( ref List<byte> a_List )
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
        static public UInt32 U32FromByteList( ref List<byte> a_List, bool a_Swp )
        {
            uint ret = U32FromByteList( ref a_List );   
            if ( a_Swp ) ret = SwapBytes32(ret);
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: returns 4 bytes of top of the stack and reduces the stack
        CREATED:       12.08.2015
        LAST CHANGE:   16.11.2022
        ***************************************************************************/
        static public UInt32 U32FromByteList( ref List<byte> a_List )
        {
            while ( a_List.Count < 4 ) a_List.Insert(0,0);

            UInt32 ret = 0;

            for (int i=0; i<4; i++)
            {
                ret += a_List[0];
                a_List.RemoveAt(0);
                if ( i < 3 ) ret <<= 8;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.11.2022
        LAST CHANGE:   07.11.2022
        ***************************************************************************/
        static public int I32FromByteList( ref List<byte> a_List )
        {
            return (int)U32FromByteList( ref a_List );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.01.2016
        LAST CHANGE:   02.07.2020
        ***************************************************************************/
        static public UInt32 U32FromByteList( ref List<byte> a_List, ref uint a_Idx ) 
        {
            int idx = (int) a_Idx;
            return U32FromByteList( ref a_List, ref idx );
        }

        static public UInt32 U32FromByteList( ref List<byte> a_List, ref int a_Idx ) 
        { 
            return U32FromByteList( a_List.ToArray(), ref a_Idx ); 
        }
        static public UInt32 U32FromByteList( byte[] a_List, ref uint a_Idx )
        { 
            int idx = (int)a_Idx; 
            return U32FromByteList(a_List, ref idx); 
        }

        static public UInt32 U32FromByteList( byte[] a_List, ref int a_Idx )
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

        static public UInt32 U32FromByteList( List<byte> a_List ) { return U32FromByteList( a_List, 0 ); }
        static public UInt32 U32FromByteList( List<byte> a_List, int a_Idx )
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

        /***************************************************************************
        SPECIFICATION:  
        CREATED:       24.05.2016
        LAST CHANGE:   24.05.2016
        ***************************************************************************/
        static public double DoubleFromByteList( ref List<byte> a_List, ref int a_Idx )
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
        static public float FloatFromByteList( ref List<byte> a_List, ref int a_Idx )
        {
            float ret = BitConverter.ToSingle( a_List.ToArray(), a_Idx );
            a_Idx += 4;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.06.2016
        LAST CHANGE:   22.11.2018
        ***************************************************************************/
        static public List<byte> ListFromByteList( ref List<byte> a_List, int a_NrBytes )
        {
            List<byte> ret = new List<byte>();

            int len = a_NrBytes;

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
        static public int FindStrInByteList( string a_Key, ref List<byte> a_List, int a_Idx )
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
        LAST CHANGE:   08.06.2022
        ***************************************************************************/
        static public int FindBytListInBytLst( List<byte> a_MainLst, List<byte> a_SubLst ) { return FindBytListInBytLst( a_MainLst, 0, a_SubLst ); }
        static public int FindBytListInBytLst( List<byte> a_MainLst, int a_StartIdx, List<byte> a_SubLst )
        {
            int bidx = 0;

            for ( int i = a_StartIdx; i < a_MainLst.Count; i++ )
            {
                if ( a_MainLst[i] != a_SubLst[bidx] ) 
                {
                    if (bidx > 0) i -= bidx;
                    bidx=0;
                    continue;
                }

                bidx++;

                if (bidx >= a_SubLst.Count ) return i - a_SubLst.Count + 1;
            }

            return -1;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.09.2015
        LAST CHANGE:   16.09.2015
        ***************************************************************************/
        static public string GetTimeStamp()
        {
            DateTime dt = DateTime.Now.ToLocalTime();
            return string.Format("{0:yyMMdd_HHmmss}", dt);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.10.2020
        LAST CHANGE:   26.10.2020
        ***************************************************************************/
        static public string GetTimeStamp( bool a_Reset )
        {
            if (a_Reset) m_TimeMem = DateTime.Now;

            DateTime dt = DateTime.Now.ToLocalTime();
            string ret = string.Format("{0:yy.MM.dd - HH:mm:ss}", dt);
            if (a_Reset) return ret;
            TimeSpan dtime = DateTime.Now - m_TimeMem;

            ret += " - " + dtime;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.09.2015
        LAST CHANGE:   16.09.2015
        ***************************************************************************/
        static public string StrRangeCheck( string a_Str, int a_Min, int a_Max )
        {
            string ret = a_Str;

            if ( Str2Int(ret) > a_Max ) ret = a_Max.ToString();
            if ( Str2Int(ret) < a_Min ) ret = a_Min.ToString();

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.11.2015
        LAST CHANGE:   17.08.2022
        ***************************************************************************/
        static public bool IsNr ( char a_Nr )
        {
            if (a_Nr >= '0' && a_Nr <= '9' ) return true;
            return false;
        }

        static public bool IsHex( char a_Nr )
        {
            if ( IsNr( a_Nr ) )               return true;
            if ( a_Nr >= 'a' && a_Nr <= 'f' ) return true;
            if ( a_Nr >= 'A' && a_Nr <= 'F' ) return true;
            return false;
        }

        static public bool IsHex( string a_Nr )
        {
            string inp = a_Nr;

            if ( inp.Trim().ToLower().StartsWith("0x") )
            {
                inp = inp.Remove(0,2);
            }

            for( int i=0; i<inp.Length; i++ )
            {
                if ( ! IsHex( inp[i] ) ) return false;
            }

            return true;
        }

        static public bool IsNrStr( string a_Nr )
        {
            for( int i=0; i<a_Nr.Length; i++ )
            {
                if ( ! IsNr( a_Nr[i] ) ) return false;
            }
            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.11.2016
        LAST CHANGE:   25.11.2016
        ***************************************************************************/
        static public char GetASCII( byte a_Byte )
        {
            if ( a_Byte < 0x20 ) return '.';
            if ( a_Byte > 0x7e ) return '.';
            return (char)a_Byte;
        }

        /***************************************************************************
        SPECIFICATION: CRC32 
        CREATED:       16.09.2015
        LAST CHANGE:   24.10.2023
        ***************************************************************************/
        static uint[] crcTable;

        public static uint Crc32( byte[] stream, int offset, uint length, uint crc, bool xoronret = true )
        {
            uint c;
            if( crcTable == null )
            {   // populating CRC table
                crcTable = new uint[256];
                for( uint n = 0; n <= 255; n++ )
                {
                    c = n;
                    for( var k = 0; k <= 7; k++ )
                    {
                        if( ( c & 1 ) == 1 )
                            c = 0xEDB88320 ^ ( ( c >> 1 ) & 0x7FFFFFFF );
                        else
                            c = ( ( c >> 1 ) & 0x7FFFFFFF );
                    }
                    crcTable[n] = c;
                }
            }

            c = crc ^ 0xffffffff;
            var endOffset = offset + length;
            for( var i = offset; i < endOffset; i++ )
            {
                uint idx = ( c ^ stream[i] ) & 255;
                uint op  = ( c >> 8 ) & 0xFFFFFF;
                c = crcTable[idx] ^ op;
            }
            return xoronret ? c ^ 0xffffffff : c;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.09.2015
        LAST CHANGE:   18.09.2018
        ***************************************************************************/
        public static UInt16 SwapBytes16(UInt16 value)
        {
          return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.09.2015
        LAST CHANGE:   18.09.2015
        ***************************************************************************/
        public static UInt32 SwapBytes32(UInt32 value)
        {
          return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                 (value & 0x00FF0000U) >> 8  | (value & 0xFF000000U) >> 24;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.03.2021
        LAST CHANGE:   04.03.2021
        ***************************************************************************/
        public static Int64  SwapBytes64(Int64 value) { return (Int64)SwapBytes64( (UInt64) value); }
        public static UInt64 SwapBytes64(UInt64 value)
        {
            UInt64 l = value & 0xffffffff;
            UInt64 h = value >> 32;

            l = SwapBytes32( (uint) l );
            h = SwapBytes32( (uint) h );

            UInt64 ret  = l >> 32;
                   ret |= h;

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.04.2024
        LAST CHANGE:   16.04.2024
        ***************************************************************************/
        public static sbyte Compl2( byte a_Arg )
        {
            sbyte ret = 0;

            if ( ( a_Arg & 0x80 ) != 0 )
            {
                ret = (sbyte)~a_Arg;
                ret++;
                ret *= -1;
            }
            else ret = (sbyte)a_Arg;
            
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2023
        LAST CHANGE:   22.05.2023
        ***************************************************************************/
        public static string IPv6Adr2Str( string a_Adr )
        {
            string       ret  = "";
            List<string> segs = SplitExt2( a_Adr, ":" );

            int cnt   = segs.Count;
            int fidx  = segs.FindIndex    ( s => s=="" );
            //int lidx  = segs.FindLastIndex( s => s=="" );

            if ( fidx != -1 )
            {
                string ins = "";
                int    nns = 8-cnt+1;
                for ( int i=0; i < nns; i++ )
                {
                    ins += "0";
                    if (i<nns-1) ins+=":";
                }

                segs[fidx] = ins;
            }

            foreach( string s in segs ) ret += s+":";

            ret = ret.Remove(ret.Length-1);

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.08.2023
        LAST CHANGE:   11.08.2023
        ***************************************************************************/
        public static bool IpAdrCheck( string a_IpAdr )
        {
            List<string> segs = null;
            if ( a_IpAdr.Contains(".") )
            { // IP4
                segs = SplitExt2( a_IpAdr, "." );
                if ( segs.Count == 4 ) return true;
            }
            else 
            { // IP6
                segs = SplitExt2( a_IpAdr, ":" );
                int fi = segs.FindIndex( s => s == "" );
                if ( fi > 0 && fi < segs.Count - 1 ) return true;
                if ( fi == -1 )
                { // no double colon
                    if ( segs.Count == 6 ) return true;
                }
            }

            return false;

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2023
        LAST CHANGE:   22.05.2023
        ***************************************************************************/
        public static List<ushort> IPv6Adr2Lst( string a_Adr )
        {
            List<ushort> ret = new List<ushort>();

            string adr = IPv6Adr2Str( a_Adr );

            List<string> ips = SplitExt( adr, ":" );

            foreach( string ip in ips )
            {
                ret.Add( (ushort)Hex2UInt( ip ) );
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2023
        LAST CHANGE:   22.05.2023
        ***************************************************************************/
        public static void Mac2Lst( string a_MAC, ref List<byte> r_Mac )
        {
            r_Mac.Clear();
            List<string> segs = SplitExt( a_MAC, ":" );
            foreach( string s in segs ) r_Mac.Add( (byte)Hex2Byte(s) );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.09.2015
        LAST CHANGE:   18.09.2015
        ***************************************************************************/
        public static string CorrectIPAddr( string a_Addr )
        {
            string txt = a_Addr;
            txt = txt.Replace(",",".");
            txt = txt.Replace("-",".");
            txt = txt.Replace(";",".");
            return txt;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.12.2015
        LAST CHANGE:   11.12.2015
        ***************************************************************************/
        public static bool ShowDocument( string a_FileName )
        {
            string helpfile = "";

            try
            {
                helpfile = ConcatPaths( Application.StartupPath, a_FileName );

                int idx = 0;
                while ( ! File.Exists( helpfile ) )
                {
                    helpfile = GoOneUp( helpfile );
                    if ( ++idx > 3 )  break;
                }

                System.Diagnostics.Process.Start( helpfile );
                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, helpfile + " not found");          
  	            return false;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.03.2016
        LAST CHANGE:   09.01.2025
        ***************************************************************************/
        public static string Array2Str( List<byte> a          )                 { return Array2Str( a.ToArray(), a.Count ); }
        public static string Array2Str( List<byte> a, int len )                 { return Array2Str( a.ToArray(), len );     }
        public static string Array2Str( List<byte> a, int len, bool a_Ascii )   { return Array2Str( a.ToArray(), len, a_Ascii ); }
        public static string Array2Str( byte[] a )                              { return Array2Str( a, a.Length );  } 
        public static string Array2Str( byte[] a, int len )                     { return Array2Str( a, len, true ); }
        public static string Array2Str( byte[] a, int len, bool a_Ascii )
        {
#if false
            string ret="";

            for ( int i = 0; i < len; i++ ) ret += a[i].ToString("X2") + " ";

            if (! a_Ascii) return ret; 

            if (len > 0) ret += "- ";

            for ( int i = 0; i < len; i++ ) ret += GetASCII(a[i]);

            return ret;
#else
            StringBuilder ret = new StringBuilder();

            for ( int i = 0; i < len; i++ ) ret.AppendFormat( "{0:X2} ", a[i] );

            if (! a_Ascii) return ret.ToString(); 

            if (len > 0) ret.Append("- ");

            for ( int i = 0; i < len; i++ ) ret.Append(GetASCII(a[i]));

            return ret.ToString();
#endif 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.03.2021
        LAST CHANGE:   09.01.2025
        ***************************************************************************/
        public static string Array2Asc( byte[] a )   { return Array2Asc( a, a.Length ); } 
        public static string Array2Asc( byte[] a, int len )
        {
            StringBuilder ret = new StringBuilder();

            foreach( byte b in a ) ret.Append( GetASCII(b) );

            return ret.ToString();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.03.2019
        LAST CHANGE:   15.03.2021
        ***************************************************************************/
        public static string Array2AscSwp( byte[] a )   { return Array2AscSwp( a, a.Length ); } 
        public static string Array2AscSwp( byte[] a, int len )
        {
            string ret="";

            for ( int i = len - 1; i >= 0; i-- ) ret += GetASCII(a[i]);

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.10.2021
        LAST CHANGE:   22.10.2021
        ***************************************************************************/
        public static string Str2Ascii( string a_Str )
        {
            string ret = "";
            foreach( char c in a_Str )
            {
                ret += string.Format( "{0:X2}", (byte)c );
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.03.2016
        LAST CHANGE:   23.03.2016
        ***************************************************************************/
        public static uint Ld( uint a_Arg )
        {
            uint ret = 0;
            uint arg = a_Arg;

            while( arg > 1 )
            {
                arg >>= 1;
                ret ++;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: Splits up a string to strings with length of 2
        CREATED:       07.04.2016
        LAST CHANGE:   07.04.2016
        ***************************************************************************/
        public static List<string> Nrline2Bytes(string a_Line)
        {
            List<string> ret = new List<string>();
            string       s   = a_Line;

            while( s.Length > 2 )
            {
                ret.Add(s.Substring(0,2));
                s = s.Remove(0,2);
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.12.2016
        LAST CHANGE:   12.01.2017
        ***************************************************************************/
        public static bool ParseTime( out DateTime o_DateTime, string a_TimeDate )
        {
            Object ob = null;

            if (a_TimeDate.Contains("/"))
            {
                if ( a_TimeDate.Contains("AM") || a_TimeDate.Contains("PM") )   
                {
                    ob = ParseTimeSub( a_TimeDate,"en-US" ); 
                    if (ob != null) 
                    { 
                        o_DateTime = (DateTime)ob; 
                        if ( o_DateTime > DateTime.Now )
                        {
                            ob = ParseTimeSub( a_TimeDate,"en-DE" ); 
                            if (ob == null)
                            {
                                ob = ParseTimeSub( a_TimeDate,"en-US" ); 
                                return true;
                            }
                            o_DateTime = (DateTime)ob; 
                        }
                        return true; 
                    }
                }
                else
                {
                    ob = ParseTimeSub( a_TimeDate,"de-DE" ); 
                    if (ob != null) 
                    { 
                        o_DateTime = (DateTime)ob; 

                        if ( o_DateTime > DateTime.Now )
                        {
                            ob = ParseTimeSub( a_TimeDate,"en-US" ); 
                            if (ob == null)
                            {
                                ob = ParseTimeSub( a_TimeDate,"en-DE" ); 
                                return true;
                            }
                            o_DateTime = (DateTime)ob; 
                        }
                        return true; 
                    }
                }
            }
            else 
            {
                ob = ParseTimeCult( a_TimeDate ); if (ob != null) { o_DateTime = (DateTime)ob; return true; }
            }

            o_DateTime = DateTime.Now;
            return false;
        }

        private static Object ParseTimeSub( string a_TimeDate, string a_Culture )
        {
            try
            {
                return DateTime.Parse( a_TimeDate, new CultureInfo( a_Culture, false ) );
            }
            catch( Exception )
            {
                return null;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.12.2024
        LAST CHANGE:   21.05.2025
        ***************************************************************************/
        private static Object ParseTimeCult( string a_TimeDate )
        {
            Object      ob      = null;
            CultureInfo culture = CultureInfo.InvariantCulture; 

            try
            {
                foreach ( string clt in CULTCODES )
                {
                    ob = ParseTimeSub( a_TimeDate, clt );
                    if ( ob != null ) break;
                }

                if (ob == null)
                {
                    ob = DateTime.ParseExact( a_TimeDate, "ddd MMM dd HH:mm:ss yyyy zzz", culture );
                }
            }
            catch( Exception )
            {
               ob = DateTime.ParseExact( a_TimeDate, "ddd MMM d HH:mm:ss yyyy zzz", culture );
            }

            return ob;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.12.2016
        LAST CHANGE:   16.12.2022
        ***************************************************************************/
        public static void FillList<T>( ref List<T> a_Lst, T a_Val, int a_Len ) { FillList<T>( ref a_Lst, a_Val, (uint) a_Len); }
        public static void FillList<T>( ref List<T> a_Lst, T a_Val, uint a_Len )
        {
            for (int i=0; i<a_Len; i++) a_Lst.Add( a_Val );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.12.2022
        LAST CHANGE:   19.01.2023
        ***************************************************************************/
        public static void FillByteList( ref List<byte> a_Lst, byte a_Val, int a_Len )
        {
            if ( a_Val == 0 ) a_Lst = new List<byte>( new byte[a_Len] );
            else              FillList<byte>( ref a_Lst, a_Val, a_Len );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.04.2021
        LAST CHANGE:   16.04.2021
        ***************************************************************************/
        public static string FillString( string a_Pat, int a_Nr )
        {
            string ret = "";
            for( int i = 0; i < a_Nr; i++ ) ret += a_Pat;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.01.2017
        LAST CHANGE:   11.01.2017
        ***************************************************************************/
        public static Font GetDlgFont( float a_Size ) { return GetDlgFont(a_Size, false); } 
        public static Font GetDlgFont( float a_Size, bool a_Bold ) 
        { 
            FontStyle fs = a_Bold ? FontStyle.Bold : FontStyle.Regular;
            return new Font("Tahoma", a_Size, fs, GraphicsUnit.Point, ((byte)(0))); 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.01.2017
        LAST CHANGE:   11.01.2017
        ***************************************************************************/
        public static float GetFontSz( Control a_Ctrl )
        {
            Graphics grph = a_Ctrl.CreateGraphics();
            int dpi = (int)grph.DpiX;
            float fsz = 0;

            switch (dpi)
            {
                case  96: fsz = 8.25F; break;
                case 120: fsz = 6.5F ; break;
                default : fsz = 7.0F ; break;
            }
            return fsz;
        }

        /***************************************************************************
        SPECIFICATION: In order to compensate issues with 125% screen scaling
        CREATED:       11.01.2017
        LAST CHANGE:   11.01.2017
        ***************************************************************************/
        public static float InitDialog( Form a_Dlg )
        {
            float fsz  = GetFontSz( a_Dlg );
            a_Dlg.Font = GetDlgFont( fsz );
            a_Dlg.AutoScaleMode = AutoScaleMode.Dpi;
            return fsz;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.01.2017
        LAST CHANGE:   11.01.2017
        ***************************************************************************/
        public static float InitUsrCtrl( UserControl a_Ctrl )
        {
            float fsz   = GetFontSz(a_Ctrl);
            a_Ctrl.Font = GetDlgFont(fsz);
            a_Ctrl.AutoScaleMode = AutoScaleMode.Dpi;
            return fsz;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.01.2017
        LAST CHANGE:   11.01.2017
        ***************************************************************************/
        public static float InitCtrl( Control a_Ctrl )
        {
            float fsz   = GetFontSz(a_Ctrl);
            a_Ctrl.Font = GetDlgFont(fsz);
            return fsz;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.10.2019
        LAST CHANGE:   05.11.2019
        ***************************************************************************/
        public static void InitDlgSzLoc( Control a_Dlg )
        {
            Rectangle rct = Screen.PrimaryScreen.Bounds;

            int wdth = rct.Width * 2 / 3;
            int hght = rct.Height * 2 / 3;
            int x = rct.Width / 6;
            int y = rct.Height / 6;

            a_Dlg.Size     = new Size( wdth, hght );
            a_Dlg.Location = new Point( x,y );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       31.10.2019
        LAST CHANGE:   05.11.2019
        ***************************************************************************/
        public static void InitDlgWdth( Control a_Dlg )
        {
            Rectangle rct = Screen.PrimaryScreen.Bounds;

            a_Dlg.Width = rct.Width * 2 / 3;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.02.2020
        LAST CHANGE:   05.02.2020
        ***************************************************************************/
        public static void MoveLocation( Control a_Dlg, Point a_Dif )
        {
            Point loc = a_Dlg.Location;
            Size  sz  = a_Dlg.Size;

            //if ( (loc.X < sz.Width * -1 ) || (loc.Y < sz.Height * -1) ) loc = new Point(0,0);

            Point dst = new Point ( loc.X + a_Dif.X, loc.Y + a_Dif.Y );
            a_Dlg.Location = dst;
        }

        /***************************************************************************
        SPECIFICATION: Same as Directory.Exists, but faster
        CREATED:       31.07.2018
        LAST CHANGE:   16.06.2025
        ***************************************************************************/
        public static bool DirExists( string a_Path )
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo( a_Path );
                return di.Exists;
            }
            catch( Exception )
            {
                return false;
            }
        }

        /***************************************************************************
        SPECIFICATION: Same as File.Exists, but faster
        CREATED:       31.07.2018
        LAST CHANGE:   31.07.2018
        ***************************************************************************/
        public static bool FileExists( string a_Path )
        {
            FileInfo fi = new FileInfo( a_Path );
            return fi.Exists;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.11.2020
        LAST CHANGE:   25.01.2024
        ***************************************************************************/
        public static string[] FileExistsExt( string a_Path )
        {
            string dir = GetPath    ( a_Path );
            string fil = GetFilename( a_Path );

            if ( ! Directory.Exists( dir ) ) return new string[0];

            string[] fils = Directory.GetFiles( dir, fil );

            return fils;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.08.2020
        LAST CHANGE:   06.08.2020
        ***************************************************************************/
        private static String WildCardToRegular( String value ) 
        {
            return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$"; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.08.2020
        LAST CHANGE:   11.01.2022
        ***************************************************************************/
        public static bool RexMatch( string a_Text, string a_Filt )
        {
            try
            {
                Match mch = Regex.Match( a_Text.ToLower(), a_Filt.ToLower() );
                return mch.Success;
            }
            catch( Exception ex )
            {
                throw new InvalidExpressionException( "Regex: invalid expression" );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.10.2021
        LAST CHANGE:   11.07.2022
        ***************************************************************************/
        public static string RexSubstr( string a_Text, string a_Filt ) { return RexSubstr( a_Text, a_Filt, false ); }
        public static string RexSubstr( string a_Text, string a_Filt, bool a_CaseSens )
        {
            try
            {
                Match mch;
                if ( a_CaseSens) mch = Regex.Match( a_Text.ToLower(), a_Filt.ToLower() );
                else             mch = Regex.Match( a_Text, a_Filt );
                return mch.Value;
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "RexSubstr exception" );
                return "";
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.11.2020
        LAST CHANGE:   05.06.2025
        ***************************************************************************/
        public static int RexFindIndex( string a_Text, ref string a_Patt, int a_Idx, bool a_CaseSens = false )
        {
            try
            {
                string txt = a_Text.Substring(a_Idx);

                Match mch = null;
                if ( a_CaseSens ) mch = Regex.Match( txt, a_Patt );
                else              mch = Regex.Match( txt.ToLower(), a_Patt.ToLower() );

                if ( ! mch.Success ) return -1;

                int ix = 0;
                if ( a_CaseSens ) ix = a_Text.IndexOf( mch.Value, a_Idx );
                else              ix = a_Text.ToLower().IndexOf( mch.Value, a_Idx );

                a_Patt = mch.Value;
            
                return ix;
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "RexFindIndex exception" );
                return -1;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.07.2020
        LAST CHANGE:   02.07.2020
        ***************************************************************************/
        public T BitFld<T>( T a_Var, int a_BitPos, int a_NrBts )
        {
            T ret = a_Var;
            a_BitPos += a_NrBts;
            //ret = (ret >> (int)( a_BitPos - a_NrBts ));
            //T mask = (T)( Math.Pow(2,a_NrBts) - 1 );
            //ret &= mask;
            return ret;
        }

        public static UInt64 BitFld( UInt64 a_Var, int a_NrBts , ref int a_BitPos )
        {
            UInt64 ret = a_Var;
            a_BitPos += a_NrBts;
            ret = ret >> ( a_BitPos - a_NrBts );
            UInt64 mask = (UInt64)( Math.Pow(2,a_NrBts) - 1 );
            ret &= mask;
            return ret;
        }

        public static UInt32 BitFld( UInt32 a_Var, int a_NrBts, ref int a_BitPos )
        {
            UInt32 ret = a_Var;
            a_BitPos += a_NrBts;
            ret = (UInt32)(ret >> ( a_BitPos - a_NrBts ));
            UInt32 mask = (UInt32)( Math.Pow(2,a_NrBts) - 1 );
            ret &= mask;
            return ret;
        }

        public static UInt16 BitFld( UInt16 a_Var, int a_NrBts, ref int a_BitPos )
        {
            UInt16 ret = a_Var;
            a_BitPos += a_NrBts;
            ret = (UInt16)(ret >> ( a_BitPos - a_NrBts ));
            UInt16 mask = (UInt16)( Math.Pow(2,a_NrBts) - 1 );
            ret &= mask;
            return ret;
        }
     
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.12.2020
        LAST CHANGE:   19.07.2024
        ***************************************************************************/
        public static void EditIncludes( string a_Filename, List<string> a_Keys )
        {
            string file = a_Filename;
            string path = GetPath( file );

            List<string> incs = new List<string>();

            if (File.Exists( file ) )
            {
                string pth = GetPath( file );
                string[] lines = File.ReadAllLines( file );
                foreach( string ln in lines )
                {
                    string l = ln.ToLower();
                    if ( l.Trim().StartsWith(";") ) continue;  // skip comments
                    if ( ! HasKey( l, a_Keys ) )    continue;
                    List<string> segs = SplitExt( l, ";" );

                    if (segs.Count >= 2)
                    {
                        if ( HasKey( segs[0], a_Keys) )
                        {
                            incs.Add(segs[1]);
                            string f = ConcatPaths( path, segs[1] );
                            EditIncludes( f, a_Keys );  // recursive call
                        }
                    }
                }

                foreach( string inc in incs )
                {
                    string f = ConcatPaths(pth,inc);
                    if ( File.Exists( f ) ) Edit( f );
                }
            }
        }

        private static bool HasKey( string a_Line, List<string> a_Keys )
        {
            if ( a_Keys == null ) return false;

            string ln = a_Line.ToLower();

            string f = a_Keys.Find( k => ln.Contains( k ) );

            return ( f != null );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.02.2021
        LAST CHANGE:   14.02.2021
        ***************************************************************************/
        public static string ShowMac( byte[] a_Mac )
        {
            string ret = "";

            if ( a_Mac.Length != 6 )
            {
                ret = "ee:ee:ee:ee:ee:ee";
                return ret;
            }

            for ( int i=0; i<a_Mac.Length; i++ )
            {
                ret += string.Format( "{0:X2}", a_Mac[i] );
                if (i<a_Mac.Length-1) ret += ":";
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.07.2019
        LAST CHANGE:   24.02.2021
        ***************************************************************************/
        public static string ByteArr2Ip( byte[] a_Ip )
        {
            string ret = "";

            for ( int i=0; i<a_Ip.Length; i++ )
            {
                ret += string.Format( "{0:000}", a_Ip[i] );
                if (i<a_Ip.Length-1) ret += ".";
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       14.10.2022
        LAST CHANGE:   14.10.2022
        ***************************************************************************/
        public static string UshortArr2Ip6( ushort[] a_Ip )
        {
            string ret = "";

            for ( int i=0; i<a_Ip.Length; i++ )
            {
                ret += string.Format( "{0:0000}", a_Ip[i] );
                if (i<a_Ip.Length-1) ret += ":";
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.02.2021
        LAST CHANGE:   25.02.2021
        ***************************************************************************/
        public static string Ms2TimeStr( uint a_Ms )
        {
            uint hr  = a_Ms / 1000 / 3600;
            uint min = a_Ms / 1000 / 60 - hr * 60;
            uint sec = a_Ms / 1000      - min * 60;
            uint ms  = a_Ms % 1000;

            return string.Format( "{0:00}:{1:00}:{2:00},{3:000}", hr,min,sec,ms );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.03.2021
        LAST CHANGE:   20.01.2023
        ***************************************************************************/
        //public static bool FileOpenDlg( ref string a_Fname, bool a_Write )
        //{
        //    string path = GetPath        ( a_Fname );
        //    string name = GetFilenameBody( a_Fname );
        //    string ext  = GetExtension   ( a_Fname );

        //    FileDialog dlg = null;

        //    if ( a_Write ) dlg = new SaveFileDialog();
        //    else           dlg = new OpenFileDialog();

        //    dlg.Filter           = "Configuration files (*." + ext + ")|*." + ext + "|All files (*.*)|*.*";
        //    dlg.InitialDirectory = path;
        //    dlg.FileName         = name + "." + ext;
        //    dlg.CheckFileExists  = ! a_Write;
        //    dlg.ValidateNames    = true;
            
        //    SendKeys.Send( "{HOME}" ); // workaround for partly hidden filename

        //    DialogResult res = dlg.ShowDialog();

        //    a_Fname = dlg.FileName;

        //    return res == DialogResult.OK;
        //}


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.07.2018
        LAST CHANGE:   07.05.2021
        ***************************************************************************/
        public static List<string> ExtractArgs( ref string a_Csv )
        {
            List<string> ret = new List<string>();

            int sttix = a_Csv.LastIndexOf("(");
            if (sttix == -1)    return ret;
            int endix = a_Csv.LastIndexOf(")");
            if (endix == -1)    return ret;
            if (sttix >= endix) return ret;
            string args = a_Csv.Substring(sttix+1,endix-sttix-1);
            a_Csv = a_Csv.Remove(sttix,endix-sttix+1);
            ret = SplitExt(args,",");
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: U32 Rotate left
        CREATED:       15.12.2021
        LAST CHANGE:   15.12.2021
        ***************************************************************************/
        public static uint ROL32 ( uint a_Val, int a_NrBits )
        {
            uint ret   = a_Val;
            uint rst   = a_Val >> (32 - a_NrBits);
                 ret <<= a_NrBits;
                 ret  |= rst;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: U16 Rotate left
        CREATED:       15.12.2021
        LAST CHANGE:   15.12.2021
        ***************************************************************************/
        public static ushort ROL16 ( ushort a_Val, int a_NrBits )
        {
            ushort ret   = a_Val;
            ushort rst   = (ushort)(a_Val >> (16 - a_NrBits));
                   ret <<= a_NrBits;
                   ret  |= rst;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.12.2021
        LAST CHANGE:   15.12.2021
        ***************************************************************************/
        public static ushort ROR16 ( ushort a_Val, int a_NrBits )
        {
            ushort ret   = a_Val;
            ushort rst   = (ushort)(a_Val << (16 - a_NrBits));
                   ret >>= a_NrBits;
                   ret  |= rst;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.02.2022
        LAST CHANGE:   23.02.2022
        ***************************************************************************/
        public static bool ContainsOneOf( string a_Inp, string a_CSKeys )
        {
            List<string> keys = SplitExt( a_CSKeys, ",; " );
            if (keys.Count == 0) return false;

            foreach( string key in keys )
            {
                if ( a_Inp.Contains(key) ) return true;
            }

            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.06.2022
        LAST CHANGE:   21.06.2022
        ***************************************************************************/
        public static void ExtractTar( String a_Fname, String a_DstDir )
        {
            Stream inStream = File.OpenRead(a_Fname);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(inStream);
            tarArchive.ExtractContents(a_DstDir);
            tarArchive.Close();

            inStream.Close();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.06.2022
        LAST CHANGE:   22.10.2023
        ***************************************************************************/
        public static void ExtractTgz( String a_Fname, String a_DstDir )
        {
#if false
            string err, std;
            return Exec7Z( "x ", a_Fname, a_DstDir, out std, out err );
#else
            Stream inStream   = File.OpenRead(a_Fname);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(a_DstDir);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
#endif
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.11.2022
        LAST CHANGE:   18.11.2022
        ***************************************************************************/
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
        
            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }  

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.11.2022
        LAST CHANGE:   18.11.2022
        ***************************************************************************/
        public static float GetDispScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES); 
        
            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
        
            return ScreenScalingFactor; // 1.25 = 125%
        }

        public static int GetWindowsScaling()
        {
            return (int)( 100 * GetDispScalingFactor() );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.02.2023
        LAST CHANGE:   17.02.2023
        ***************************************************************************/
        public static bool ExtractFile2Dir( string a_ContnerPth, string a_ExtrPth )
        {
            try
            {
                switch( GetExtension(a_ContnerPth).ToLower() )
                {
                    case "7z":
                        _7zipArchive z7a = new _7zipArchive();
                        z7a.ExtractFile( a_ContnerPth, a_ExtrPth );
                        break;

                    case "zip":
                        //FileStream zipToOpen = new FileStream( a_ContnerPth, FileMode.Open );
                        //ZipArchive za        = new ZipArchive( zipToOpen, ZipArchiveMode.Read );

                        string cont = a_ContnerPth.Replace( "\\", "/" );
                        string extr = a_ExtrPth   .Replace( "\\", "/" );
                        //string cont = a_ContnerPth;
                        //string extr = a_ExtrPth;

                        //_7zipArchive z7a2 = new _7zipArchive();
                        //z7a2.ExtractFile( a_ContnerPth, a_ExtrPth );


                        //ZipFile.ExtractToDirectory( cont, extr, Encoding.Default );
                        //ZipArchive arc = ZipFile.OpenRead( cont );
                        break;
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Exception unpacking " + a_ContnerPth.ToString() );
            }

            return true;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.10.2023
        LAST CHANGE:   26.10.2023
        ***************************************************************************/
        public static bool Exec7Z( string a_Args, string a_ContFile, string a_Args2, out string o_Std, out string o_Err )
        {
            o_Std = "";
            o_Err = "";
            if ( ! File.Exists(a_ContFile) ) return false;
            string curdir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory( GetPath(a_ContFile) );
            ProcessStartInfo pro = new ProcessStartInfo();
            pro.WindowStyle = ProcessWindowStyle.Hidden;
            pro.FileName    = ConcatPaths( Application.StartupPath, "7z.exe" );
            pro.Arguments   = a_Args + a_ContFile + " " + a_Args2 + " -aoa";
            pro.UseShellExecute        = false;
            pro.CreateNoWindow         = true;
            pro.RedirectStandardError  = true;
            pro.RedirectStandardOutput = true;
            Process x = Process.Start(pro);
            while( ! x.HasExited && x.Responding ) Thread.Sleep(1);
            o_Err = x.StandardError .ReadToEnd();
            o_Std = x.StandardOutput.ReadToEnd();
            if (x != null) x.Close();

            Directory.SetCurrentDirectory( curdir ); // restore current directory
            return true;
        }

        /***************************************************************************
        SPECIFICATION: Renames a container in a container with the same name
        CREATED:       22.10.2023
        LAST CHANGE:   11.11.2025
        ***************************************************************************/
        public static string RenCtrInCtr( string a_CtrFile, bool a_WithPath = true )
        {
            string std,err;

            string pth = GetPath        ( a_CtrFile );
            string ext = GetExtension   ( a_CtrFile );
            string fn  = GetFilename    ( a_CtrFile );
            string fnb = GetFilenameBody( a_CtrFile );

            string nf  = fnb + "_1." + ext;

            Exec7Z( "rn ", a_CtrFile, fn + " " + nf, out std, out err ); // rename fn to nf inside the container

            Exec7Z( "l ", a_CtrFile, nf, out std, out err );

            if ( ! std.Contains( nf ) ) return a_CtrFile;  // if new file name exist in the container

            fn = a_WithPath ? ConcatPaths( pth, nf ) : nf;

            return fn;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.02.2023
        LAST CHANGE:   21.01.2025
        ***************************************************************************/
        public static bool ExtractFile( string a_ContnerPth, string a_FileInCtr, string a_DstFile, out string o_Exception )
        {
            bool   ret;
            string err;
            string std;
            string ctrpth = "";

            try
            {
                o_Exception = "";

                switch( GetExtension(a_ContnerPth).ToLower() )
                {
                    case "7z" :
                        ctrpth = RenCtrInCtr( a_ContnerPth );
                        ret = Exec7Z( "x ", ctrpth, a_FileInCtr, out std, out err );
                        break;

                    case "tgz":
                        ctrpth = RenCtrInCtr( a_ContnerPth );
                        if ( ctrpth != a_ContnerPth ) ret = Exec7Z( "x ", a_ContnerPth, GetFilename( ctrpth ), out std, out err );  // open the container in the container
                        break;

                    case "zip":
                    case "tar":
                        ctrpth = a_ContnerPth;
                        break;

                    default: return false;
                }

                ret = Exec7Z( "x ", ctrpth, a_FileInCtr, out std, out err );
                if ( std.Contains("No files to process") ) o_Exception = a_FileInCtr + " not found in " + a_ContnerPth;
                return ret;
            }
            catch( Exception ex )
            {
                o_Exception = ex.Message;
                //MessageBox.Show( ex.Message, "Exception unpacking " + a_ContnerPth.ToString() );
            }

            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2025
        LAST CHANGE:   12.08.2025
        ***************************************************************************/
        public static string ExpandPthWldcrd( string a_Path )
        {
            List<string> segs = SplitExt( a_Path, "\\/" );

            int ix = segs.FindIndex( p => p.Contains("*") || p.Contains("?") );

            if ( ix == -1 ) return a_Path;

            string pth = "";
            for( int i = 0; i< ix; i++ )
            {
                pth = ConcatPaths( pth, segs[i] );
            }

            string[] dirs = Directory.GetDirectories( pth, segs[ix] );
            string[] fils = null;

            if ( dirs.Length == 0 )
            {
                fils = Directory.GetFiles( pth, segs[ix] );
                if (fils.Length == 0 ) return a_Path;
            }

            pth = ConcatPaths( pth, dirs.Length > 0 ? dirs[0] : fils[0] );

            for ( int i=ix+1; i< segs.Count; i++ )
            {
                pth = ConcatPaths( pth, segs[i] );
            }

            return pth;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       31.07.2024
        LAST CHANGE:   31.07.2024
        ***************************************************************************/
        public static void ShiftLoc( Form a_Dlg )
        {
            const int offs = 30;

            string    own = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            Process[] sips = Process.GetProcessesByName( own );

            int tms = sips.Length - 1;
            if (tms == 0) return;

            Point loc = a_Dlg.Location;
            loc.X += offs * tms;
            loc.Y += offs * tms;
            a_Dlg.Location = loc;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.02.2025
        LAST CHANGE:   12.02.2025
        ***************************************************************************/
        public static List<string> ParseIpPort( string a_IpPrt )
        {
            List<string> segs = SplitExt( a_IpPrt, ":" );
            if (segs.Count > 2) // ip6
            {
                segs = SplitExt( a_IpPrt, "-" );
            }
            return segs;
        }

        /***************************************************************************
        SPECIFICATION: Supplements the config name with the number of running 
                       instances (+1).
        CREATED:       15.10.2025
        LAST CHANGE:   15.10.2025
        ***************************************************************************/
        public static  string GetConfigName( string a_CurrFN )
        {
            string own   = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            string fname = a_CurrFN;

            Process op = Process.GetCurrentProcess();

            Process[] procs = Process.GetProcessesByName( own );

            if ( procs.Length <= 1 ) return fname;

            int nrinst = procs.Length;

            string fn = GetFilenameBody( fname, true );

            char last = fn[fn.Length-1];

            if ( IsNr(last) ) fn = fn.Remove( fn.Length-1, 1 );

            fn += nrinst.ToString();
            fname = fn += ".ini";

            return fname;
        }

    } // Utils class

    public class CriticalSectionLock : IDisposable 
    {
	    private CriticalSection mCriticalSection;

	    // Acquires the lock:

	    public CriticalSectionLock( CriticalSection criticalSection ) 
	    {
		    mCriticalSection = criticalSection;
		    mCriticalSection.Enter();
	    }

	    // Releases the lock:

	    public void Dispose() 
	    {
		    mCriticalSection.Leave();
	    }
    }
 
    /// The C/C++ CRITIAL_SECTION store debug information like which thread holds the lock,
    /// so this class is also named CriticalSection to emphasize the similarity.
    /// See: http://msdn.microsoft.com/en-us/magazine/cc164040.aspx
    /// 
    /// Using the Monitor.Enter/Exit is the same as using the lock statement. See:
    /// http://msdn.microsoft.com/en-us/library/ms173179.aspx
    /// </summary>
    /// <seealso cref="CriticalSectionLock"/>
    public class CriticalSection 
    {
	    private const int UNOWNED_THREAD_ID = -1;

	    // This is not a property just in case the debugger has problems evaluating it
	    // and it is public so that the compiler doesn't try to optimize it away.

	    public int OwnerThreadId = UNOWNED_THREAD_ID;

	    internal void Enter()
	    {
		    // When we were able to lock 'this' then it is safe to store the owning thread id:

		    Monitor.Enter( this );
		    OwnerThreadId = Thread.CurrentThread.ManagedThreadId;
	    }

	    internal void Leave()
	    {
		    // We are still locked so we can safely erase the owner thread id:

		    OwnerThreadId = UNOWNED_THREAD_ID;
		    Monitor.Exit( this );
	    }
    }
} // namespace
