using System;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization;
using System.Security;
using System.Runtime.Serialization.Formatters.Binary;

//using NS_UserCombo;
using NS_WUtilities;

/***************************************************************************
SPECIFICATION: Contains methods for storing and loading application settings
CREATED:       ??.??.2005
LAST CHANGE:   31.01.2022
***************************************************************************/
namespace NS_AppConfig
{
    #if ! XMLCONFIG
    public class DialogPosSize
    {
        public DialogPosSize()
        {
            Loc      = new Point(-1,-1);
            Siz      = new Size(-1,-1);
            WinStat  = FormWindowState.Normal;
            RestBnds = new Rectangle();
        }

        public DialogPosSize(Form a_rDlg)
        {
            Loc      = a_rDlg.Location;
            
            Bounds   = a_rDlg.RectangleToScreen(new Rectangle(Loc,a_rDlg.ClientSize));
            Siz      = a_rDlg.Bounds.Size;
            WinStat  = a_rDlg.WindowState; 
            RestBnds = a_rDlg.RestoreBounds;
        }

        public Point           Loc;
        public Size            Siz;
        public FormWindowState WinStat;
        //public Point           RestBndsLc;
        //public Size            RestBndsSz;
        public Rectangle       RestBnds;
        public Rectangle       Bounds;

        public bool IsInit()
        {
            if (-1 != Loc.X)            return false;
            if (-1 != Loc.Y)            return false;
            if (-1 != Siz.Width)        return false;
            if (-1 != Siz.Height)       return false;
            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.04.2006
        LAST CHANGE:   20.02.2011
        ***************************************************************************/
        public void Read(Form a_Dlg)
        {
            Loc        = a_Dlg.Location;
            Bounds     = a_Dlg.RectangleToScreen(new Rectangle(Loc,a_Dlg.ClientSize));       
            Siz        = Bounds.Size;
            WinStat    = a_Dlg.WindowState; 
            RestBnds   = a_Dlg.RestoreBounds;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.04.2006
        LAST CHANGE:   20.02.2011
        ***************************************************************************/
        public void Write(Form a_Dlg)
        {
            a_Dlg.Location    = Loc;
            a_Dlg.Size        = Siz;       // 20.02.2011
            a_Dlg.WindowState = WinStat;
            //a_Dlg.RestoreBounds = rb;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.01.2009
        LAST CHANGE:   18.01.2009
        ***************************************************************************/
        public void WriteLoc( Form a_Dlg )
        {
            a_Dlg.Location = Loc;
        }
    }

    /***************************************************************************
    SPECIFICATION: AppSettings class definition
    CREATED:       2005
    LAST CHANGE:   15.10.2025
    ***************************************************************************/
    public class AppSettings
    {
        const int MAX_COMBO_ENTRIES = 50;

        private int  m_iReadDbVersion;
        private int  m_iDbVersion;
        private bool m_Init;

        public int  DbVersion 
        {
            get { return m_iReadDbVersion; } 
        }

        public int CurrDbVersion
        {
            set { m_iDbVersion = value; }
            get { return m_iDbVersion;  } 
        }

        private bool m_bReading;
        public  bool IsReading
        {
            get { return m_bReading;  }
            set { m_bReading = value; }
        }
        public bool IsWriting
        {
            get { return ! m_bReading;  }
            set { m_bReading = ! value; }
        }

        public string FileName
        {
            get { return m_FileName;  }
            set { m_FileName = value; }
        }

        public bool Initializing { get { return m_Init; } }

        public int m_OldVersion1;   // needed for ReadDlbLocation
        public int m_OldVersion2;   // needed for FileComboBox in Backup

        protected string   m_FileName;
        private FileStream m_Stream;

        private class ScreensXComparer: IComparer<Screen>
        {
            public int Compare( Screen x, Screen y )
            {
                if (x.Bounds.X < y.Bounds.X) return -1;
                if (x.Bounds.X > y.Bounds.X) return 1;
                return 0; // X == y
            }
        }

        private class ScreensYComparer: IComparer<Screen>
        {
            public int Compare( Screen x, Screen y )
            {
                if (x.Bounds.Y < y.Bounds.Y) return -1;
                if (x.Bounds.Y > y.Bounds.Y) return 1;
                return 0; // X == y
            }
        }

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       2005
        LAST CHANGE:   31.01.2022
        ***************************************************************************/
        public AppSettings(string a_sFileName,int a_iDbVersion)
           : this( a_sFileName )
        {
            m_iDbVersion  = a_iDbVersion;
            m_OldVersion1 = a_iDbVersion;
            m_OldVersion2 = 0; 
        }


        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       29.04.2006
        LAST CHANGE:   31.01.2022
        ***************************************************************************/
        public AppSettings( string a_sFileName )
        {
            m_iDbVersion = 0;
            m_FileName   = a_sFileName;
            m_Stream     = null;

            m_OldVersion1 = 0;
            m_OldVersion2 = 0;

            m_Init = true;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   24.03.2020
        ***************************************************************************/
        public bool OpenRead( ) { return OpenRead( m_FileName ); }
        public bool OpenRead( string a_Fname )
        {
            if (! File.Exists( a_Fname )) return false;
            m_Stream   = File.OpenRead( a_Fname );
            m_bReading = true;
            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.04.2013
        LAST CHANGE:   26.04.2013
        ***************************************************************************/
        public bool DeleteFile()
        {
            if ( File.Exists( m_FileName ) )
            {
                Close();
                File.Delete( m_FileName );
                return true;
            }
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   23.03.2020
        ***************************************************************************/
        public bool OpenWrite() { return OpenWrite( m_FileName ); }
        public bool OpenWrite( string a_FName )
        {
            bool ret   = false;
            m_bReading = false;

            try
            {
                m_Stream = File.Create( a_FName );
                ret = true;
            }
            catch(Exception e)
            {
                MessageBox.Show( e.Message, "Error in writing " + a_FName );
            }
            return ret;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   31.01.2022
        ***************************************************************************/
        public void Close()
        {
            m_Init = false;
            if (m_Stream == null) return;
            m_Stream.Close();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   13.02.2007
        ***************************************************************************/
        public void Serialize(object iObj)
        {
            try
            {
                BinaryFormatter bf  = new BinaryFormatter();
                bf.Serialize(m_Stream,iObj);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,"Error in serialization of " + iObj.ToString());
                throw e;
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   30.01.2014
        ***************************************************************************/
        public object Deserialize()
        {
            return Deserialize<object>();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.01.2014
        LAST CHANGE:   30.09.2025
        ***************************************************************************/
        public T Deserialize<T>()
        {
            T ret = default(T);

            try
            {
                BinaryFormatter bf  = new BinaryFormatter();
                ret = (T)bf.Deserialize(m_Stream);
            }
            catch(ArgumentNullException e )
            {
                MessageBox.Show(e.Message,"Argument null exception in deserialization");
                throw e;
            }
            catch( SerializationException e )
            {
                MessageBox.Show(e.Message,"Exception in deserialization");
                throw e;
            }
            catch (SecurityException e )
            {
                MessageBox.Show(e.Message,"Security exception in deserialization");
                throw e;
            }
            catch (InvalidCastException e )
            {
                MessageBox.Show( e.Message,"Invalid cast exception in deserialization" );
                throw e;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,"Error in deserialization");
                throw e;
            }

            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.03.2020
        LAST CHANGE:   01.03.2021
        Application template:
                                if ( m_Config.Export( ref m_ExportFname ) )
                                {
                                        Serialize( ref m_Config );
                                        m_Config.Close();
                                }
        ***************************************************************************/
        //public bool Export( ref string a_Fname )
        //{
        //    if ( WUtils.FileOpenDlg( ref a_Fname, true ) )
        //    {
        //        OpenWrite( a_Fname );

        //        if ( a_Fname.EndsWith( "Stat.ini" ) )
        //        {
        //            MessageBox.Show( "The file ending ...Stat.ini is reserved !\nselect another ending !", "Invalid Selection" );
        //            return false;
        //        }

        //        return true;
        //    }

        //    return false;
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.03.2020
        LAST CHANGE:   01.03.2021
        Application template:
                                if ( m_Config.Import( ref m_ExportFname ) )
                                {
                                    Serialize( ref m_Config );
                                    m_Config.Close();
                                }
        ***************************************************************************/
        //public bool Import( ref string a_Fname )
        //{
        //    if ( WUtils.FileOpenDlg( ref a_Fname, false ) )
        //    {
        //        bool ok = OpenRead( a_Fname );

        //        if ( a_Fname.EndsWith( "Stat.ini" ) )
        //        {
        //            MessageBox.Show( "The file name ...Stat.ini is not a valid INI file\nit is reserved !", "Invalid Selection" );
        //            return false;
        //        }

        //        return ok;
        //    }

        //    return false;
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.05.2020
        LAST CHANGE:   15.06.2023
        ***************************************************************************/
        public void MakeExportFname( ref string a_Fname, string a_PostFx )
        {
            string fnbody = WUtils.GetFilenameBody( a_Fname );
            string path   = WUtils.GetPath        ( a_Fname );
            string ext    = WUtils.GetExtension   ( a_Fname );
            string comp   = SystemInformation.ComputerName;

            bool fexists  = File.Exists( a_Fname );

            List<string> segs = WUtils.SplitExt( fnbody, "_-" );
            int nrsgs = segs.Count;

            string nr = segs[nrsgs-1];
            int inr = -1;
            if ( WUtils.IsNrStr(nr) ) inr = WUtils.Str2Int(nr);

            if (nrsgs == 4)
            {
                string fn = "";
                for (int i=0; i<nrsgs-1; i++) fn += segs[i] + "_";
                fn += nr;
                fn += "." + ext;
                fexists = File.Exists( WUtils.ConcatPaths( path, fn ) );
            }

            string newbdy = "";
    
            if (nrsgs < 2) newbdy = fnbody  + "_" + a_PostFx;
            else
            {
                if (inr == -1 || !fexists) { newbdy = segs[0] + "_" + a_PostFx + "_" + comp + "_1"; inr = 1; }
                else                         newbdy = segs[0] + "_" + a_PostFx + "_" + comp + "_" + nr;
            }

            string newfile = WUtils.ConcatPaths( path, newbdy ) + "." + ext;
            
            int brkidx = 0;

            while ( File.Exists( newfile ) )
            {
                newbdy = WUtils.GetFilenameBody( newfile );
                if (inr == -1) newbdy += "_1";
                else
                {
                    int ix  = newbdy.LastIndexOf("_" + inr.ToString() );
                    newbdy  = newbdy.Remove(ix);
                    newbdy += "_" + (++inr).ToString();
                }

                newfile = WUtils.ConcatPaths( path, newbdy ) + "." + ext;

                if (--brkidx > 100) break;
            }

            a_Fname = newfile;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   05.06.2025
        ***************************************************************************/
        public void SerializeComboBox( ComboBox iComboBox, bool a_WithHist = true )
        {
            try
            {
                BinaryFormatter bf  = new BinaryFormatter();

                bf.Serialize(m_Stream,(string)iComboBox.Text);

                if ( ! a_WithHist ) return;

                int cnt = iComboBox.Items.Count;

                if (cnt > MAX_COMBO_ENTRIES) cnt = MAX_COMBO_ENTRIES;

                bf.Serialize(m_Stream,cnt);

                IEnumerator en = iComboBox.Items.GetEnumerator();
                en.Reset();
                while(en.MoveNext())
                {
                    string itm = (string)en.Current;
                    bf.Serialize(m_Stream,itm);
                    if (--cnt == 0) break;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show( iComboBox.ToString() + "\n" + e.Message,"Error in serialization of ...");
                throw e;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   05.06.2025
        ***************************************************************************/
        public bool DeserializeComboBox( ComboBox a_ComboBox, bool a_WithHist = true )
        {
            string DefText = a_ComboBox.Text;
            try
            {
                a_ComboBox.Items.Clear();  // erase it before

                BinaryFormatter bf   = new BinaryFormatter();
                a_ComboBox.Text      = (string)bf.Deserialize(m_Stream);

                if ( ! a_WithHist ) return true;

                int cnt              = (int)   bf.Deserialize(m_Stream);

                for (int i=0; i<cnt; i++)
                {
                    string itm = (string)bf.Deserialize(m_Stream);
                    a_ComboBox.Items.Add( (string) itm );
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\nInitializing default !","Error in deserialization of combo box");
                a_ComboBox.Items.Clear();
                a_ComboBox.Text = DefText;
                m_Stream.Close();
                throw e;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.06.2007
        LAST CHANGE:   23.06.2007
        ***************************************************************************/
        public void SerializeCheckedListBox(CheckedListBox iChListBox)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize( m_Stream, iChListBox.Items.Count );

                //for ( int i=0; i<cnt; i++ )
                //{
                //    bf.Serialize(m_Stream, iChListBox.Items[i].Text);
                //    bf.Serialize(m_Stream, iChListBox.Items[i].checked);
                //}

                foreach ( Object itm in iChListBox.Items )
                {
                    bf.Serialize(m_Stream, itm);
                }

                bf.Serialize( m_Stream, iChListBox.CheckedIndices.Count );

                foreach ( int chked in iChListBox.CheckedIndices )
                {
                    bf.Serialize( m_Stream, chked );
                }
            }
            catch ( Exception e )
            {
                MessageBox.Show(e.Message,"Error in serialization of " + iChListBox.ToString());
                throw e;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.06.2007
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public bool DeserializeCheckedListBox(CheckedListBox a_ChListBox)
        {
            try
            {
                a_ChListBox.Items.Clear();  // erase it before

                BinaryFormatter bf = new BinaryFormatter();

                int cnt = (int)bf.Deserialize(m_Stream);  // number of entries

                for (int i=0; i<cnt; i++)
                {
                    a_ChListBox.Items.Add(bf.Deserialize(m_Stream));
                }

                cnt = (int) bf.Deserialize(m_Stream);  // number of checked indices

                for (int i=0; i<cnt; i++)
                {
                    int idx = (int)bf.Deserialize(m_Stream);
                    a_ChListBox.SetItemChecked(idx,true);
                }
            }
            catch ( Exception e )
            {
                MessageBox.Show(e.Message,"Error in deserialization of" + a_ChListBox.ToString());
                a_ChListBox.Items.Clear();
                throw e;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   13.02.2007
        ***************************************************************************/
        public bool WriteDlgLocation(DialogPosSize iDLoc)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(m_Stream,iDLoc.Loc);
                bf.Serialize(m_Stream,iDLoc.Siz);
                bf.Serialize(m_Stream,iDLoc.WinStat);
                bf.Serialize(m_Stream,iDLoc.RestBnds);
            }
            catch ( Exception e )
            {
                MessageBox.Show(e.Message,"Error of serialization of " + iDLoc.ToString());
                throw e;
            }

            return true;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public bool ReadDlgLocation(DialogPosSize oDLoc)
        {
            try
            {
                BinaryFormatter bf  = new BinaryFormatter();
                oDLoc.Loc           = (Point)          bf.Deserialize(m_Stream);
                oDLoc.Siz           = (Size)           bf.Deserialize(m_Stream);
                oDLoc.WinStat       = (FormWindowState)bf.Deserialize(m_Stream);
                oDLoc.RestBnds      = (Rectangle)      bf.Deserialize(m_Stream);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,"Error in deserialization of dialog parameters");
                throw e;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.04.2006
        LAST CHANGE:   29.04.2006
        ***************************************************************************/
        public bool SerializeDialog(Form a_rDlg)
        {
           DialogPosSize dps = new DialogPosSize(a_rDlg); 

           return WriteDlgLocation(dps);
        }
        

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.04.2006
        LAST CHANGE:   11.04.2024
        ***************************************************************************/
        public bool DeserializeDialog( Form a_rDlg )
        {
            DialogPosSize dps = new DialogPosSize( a_rDlg ); 

            List<Screen> scrns = new List<Screen>( Screen.AllScreens );

            int ScrnWdth = 0;
            int ScrnHght = 0;
            int ScrnX    = 0;
            int ScrnY    = 0;

            scrns.Sort( new ScreensXComparer() );
            //scrns.Sort( new ScreensYComparer() );

            foreach( Screen scrn in scrns )
            {
                if (scrn.Bounds.X == 0)  ScrnWdth  = scrn.Bounds.Width;
                else                     ScrnWdth += scrn.Bounds.Width;

                if (scrn.Bounds.Y == 0)  ScrnHght  = scrn.Bounds.Height;
                else                     ScrnHght += scrn.Bounds.Height;
            }

            bool ret = ReadDlgLocation(dps);

            // Correcting the bounds in case of changed screen resolution
            if (dps.Loc.X + dps.Siz.Width  > ScrnWdth ) dps.Loc.X = ScrnWdth - dps.Siz.Width;
            if (dps.Loc.Y + dps.Siz.Height > ScrnHght ) dps.Loc.Y = ScrnHght - dps.Siz.Height;

            if (dps.Loc.X < ScrnX) dps.Loc.X = ScrnX;
            if (dps.Loc.Y < ScrnY) dps.Loc.Y = ScrnY;

            a_rDlg.Location      = dps.Loc;
            a_rDlg.Size          = dps.Siz;
            a_rDlg.WindowState   = dps.WinStat;
            a_rDlg.StartPosition = FormStartPosition.Manual;

            if ( dps.WinStat == FormWindowState.Minimized )
            {
                a_rDlg.Location = dps.RestBnds.Location;
                a_rDlg.Size     = dps.RestBnds.Size;
            }
            
            return ret;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.04.2006
        LAST CHANGE:   18.03.2013
        ***************************************************************************/
        public void DeserializeDbVersion()
        {
            try
            {
                m_iDbVersion = m_iReadDbVersion = (int)Deserialize();
                     
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Error in deserialization of data base version");
                throw e;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.07.2015
        LAST CHANGE:   31.07.2015
        ***************************************************************************/
        //public bool Allowed4DBGT( int a_DbVersion )
        //{
        //    if( IsReading )
        //    {
        //        if ( DbVersion > a_DbVersion ) return true;
        //        return false;
        //    }
        //    else return true;
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.07.2015
        LAST CHANGE:   31.07.2015
        ***************************************************************************/
        public bool IsReadingAndDBST( int a_DbVersion )
        {
            if ( IsReading && DbVersion < a_DbVersion ) return true;
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.05.2018
        LAST CHANGE:   04.05.2018
        ***************************************************************************/
        //public bool InsertIfDBGT ( int a_DbVersion )
        //{
        //    if ( IsReading && DbVersion > a_DbVersion ) return true;
        //    if ( IsWriting )                            return true;
        //    return false;
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       8/31/2016
        LAST CHANGE:   8/31/2016
        ***************************************************************************/
        public bool WarnIfDBST( int a_DbVersion )
        {
            if ( IsReading && DbVersion < a_DbVersion )
            {
                MessageBox.Show("The data base modifications are too extensive for preserving your settings.\nA new data base will be created instead !","Note");
                return true;
            }
            return false;
        }
    } // class
    #endif // ! XMLCONFIG
} // namespace


/* Template
public void Serialize( ref AppSettings a_Conf )
{
    if( a_Conf.IsReading )
    {
        a_Conf.DeserializeDialog( this );
    }
    else
    {
        a_Conf.SerializeDialog  ( this );
    }
}
*/
