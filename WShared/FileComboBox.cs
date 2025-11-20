using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using NS_AppConfig;
using NS_Utilities;

namespace NS_UserCombo
{
	/// <summary>
	/// Summary description for FileComboBox.
	/// </summary>
	public class FileComboBox: UserComboBox
	{
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       14.04.2015
        LAST CHANGE:   24.09.2021
        ***************************************************************************/
        public bool   DragDir    { set { m_bDragDir = value; } }
        public string VolumeName { get { return m_VolumeName; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       ??.??.2004
        LAST CHANGE:   20.01.2022
        ***************************************************************************/
        protected const string TOOLTP_DISCRDPTHS = "\nShift-Alt-left click: Discard dead paths / files";

        private bool     m_bDragDir;   // if true only directories are dropped, otherwise files
        private string   m_VolumeName;


        /***************************************************************************
        SPECIFICATION: C'tors 
        CREATED:       ??.??.2004
        LAST CHANGE:   07.11.2019
        ***************************************************************************/
        public FileComboBox()
            : base()
        {
            m_bDragDir   = true;
            SubCtor();
        }

        public FileComboBox(bool bDragDir)
            : base()
		{
            m_bDragDir  = bDragDir;
            SubCtor();
		}

        private void SubCtor()
        {
            this.m_VolumeName = "";
            this.AllowDrop = true;
            this.DragDrop  += new System.Windows.Forms.DragEventHandler(this.FileCombo_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileCombo_DragEnter);

            Click          -= new EventHandler(Combo_Click);
            Click          += new EventHandler(FileCombo_Click);

            this.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       2004
        MODIFIED:      04.11.2021
        ***************************************************************************/
        public DialogResult BrowseFileRead ()                   { return BrowseFileRead( "Text files (*.txt)|*.txt|All files (*.*)|*.*" ); }
        public DialogResult BrowseFileRead ( string a_sFilter ) { return BrowseFileRead( a_sFilter, Utils.GetPath(Text) ); }
        public DialogResult BrowseFileRead ( string a_sFilter, string a_sPath )
        {
            string defpth = a_sPath;

            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = a_sFilter;

            defpth = Utils.RelPath2Abs(defpth,false);

            fd.InitialDirectory = defpth;
            DialogResult res = fd.ShowDialog();

            if( res == DialogResult.OK )
            {
                if( ! AlreadyIn( Text ) ) Items.Add( Text );
                Text = fd.FileName;

                switch(Utils.GetExtension(Text).ToLower())
                {
                    case "7z":
                        _7zipArchive z7a = new _7zipArchive();
                        z7a.ExtractFile( Text, "d:\\temp\\test");
                        break;

                    case "zip":
                        FileStream zipToOpen = new FileStream( Text, FileMode.OpenOrCreate );
                        ZipArchive za        = new ZipArchive( zipToOpen, ZipArchiveMode.Read );
                        break;
                }

                if( !AlreadyIn( Text ) ) Items.Add( Text );
            }
            return res;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       2004
        LAST CHANGE:   02.08.2024
        ***************************************************************************/
        public DialogResult BrowseFolder( Control a_Owner = null ) { return BrowseFolder( Text, a_Owner ); }

        public DialogResult BrowseFolder( string a_DefPath, Control a_Owner = null )
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.ShowNewFolderButton = true;

            string defpth = a_DefPath;
            string dir    = "";

            if ( defpth.StartsWith("..") )
            {
                dir = Directory.GetCurrentDirectory();
                dir = Utils    .GoOneUp(dir,true);
                if (! Directory.Exists(dir)) dir    = "";
                else                         defpth = defpth.Replace( "..", dir );
            }

            for ( int i=0; i<10; i++ ) 
            {
                if ( Directory.Exists ( defpth ) ) break;
                defpth = Utils.GoOneUp( defpth, true );
            }

            fd.SelectedPath = defpth;
            DialogResult res = fd.ShowDialog( a_Owner != null ? a_Owner : this.Parent );

            if (res == DialogResult.OK) 
            {
                AddTextEntry();
                Text = fd.SelectedPath;

                if (dir != "") 
                {
                    // cutoff drive letters 
                    Text = Text.ToUpper().Replace(dir.ToUpper(),"..");
                    if (! Directory.Exists(Text) ) Text = fd.SelectedPath;
                }

                AddTextEntry();
            }

            return res;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.03.2006
        LAST CHANGE:   11.03.2006
        ***************************************************************************/
        private void FileCombo_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop,false))
            {
                string[] sa = (string[])e.Data.GetData(DataFormats.FileDrop);
                string   fn = sa[0];
                string   dir = fn;
                if (! Directory.Exists(fn) && m_bDragDir)
                {
                    dir = Directory.GetParent(fn).FullName;
                }
                ((FileComboBox)sender).Text = dir;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.08.2015
        LAST CHANGE:   07.11.2019
        ***************************************************************************/
        private void FileCombo_Click(object sender, System.EventArgs e)
        {
            ExecTips( TOOLTP_DISCRDPTHS );

            if (m_bShiftPressed && m_bAltPressed) DiscardDeadPaths();

            InitKeys();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.09.2015
        LAST CHANGE:   19.08.2021
        ***************************************************************************/
        private bool FileDirExists( string a_Path )
        {
            if  ( Directory.Exists (a_Path) ) return true;
            if  ( File     .Exists (a_Path) ) return true;

            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.08.2015
        LAST CHANGE:   19.08.2021
        ***************************************************************************/
        private void DiscardDeadPaths()
        {
            if ( MessageBox.Show("Discard dead paths / files ?","Warning",MessageBoxButtons.YesNo) == DialogResult.No ) return;

            for( int i = Items.Count-1; i >= 0; i-- )
            {
                string pth = (string)Items[i];
                if (! FileDirExists( pth ) ) Items.RemoveAt(i);
            }

            if ( ! FileDirExists(Text) )
            {
                if (Items.Count > 0) Text = (string)Items[0];
                else                 Text = "";
            }

            SyncHist();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.03.2006
        LAST CHANGE:   11.03.2006
        ***************************************************************************/
        private void FileCombo_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            e.Effect    = DragDropEffects.Copy;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.04.2009
        LAST CHANGE:   20.01.2022
        ***************************************************************************/
        public new void Serialize(ref AppSettings a_Conf)
        {
            base.Serialize(ref a_Conf);

            if ( a_Conf.IsReading )
            {
                m_VolumeName = a_Conf.Deserialize<string>( );
            }
            else
            {
                m_VolumeName = Utils.GetDriveName(this.Text);   // get volume name of current path
                a_Conf.Serialize( m_VolumeName );
            }

            if ( a_Conf.IsReading )
            {
                string tt = m_Tooltip.GetToolTip( this );
                tt += TOOLTP_DISCRDPTHS;
                m_Tooltip.SetToolTip( this, tt );
            }

        }
        

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.12.2014
        LAST CHANGE:   21.12.2014
        ***************************************************************************/
        public string[] CorrectPathByVolName(string sVolName)
        {
            m_VolumeName = sVolName;
            return CorrectPathByVolName();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.04.2009
        LAST CHANGE:   27.02.2015
        ***************************************************************************/
        public string[] CorrectPathByVolName()
        {
            if ( m_VolumeName == "" )                return null;
            if ( m_VolumeName.Contains("Fixed")  )   return null;
            if ( m_VolumeName.Contains("Volume") )   return null;

            string[] drv = Directory.GetLogicalDrives(); 
            
            foreach(string d in drv)
            {
                if (d[0]=='A' || d[0]=='B') continue;

                string vn = Utils.GetDriveName(d);

                if (m_VolumeName.Contains(vn))
                {
                    string[] ret = new string[2];

                    ret[0]    = Utils.GetDriveLetter(this.Text);
                    this.Text = Utils.ReplaceDriveLetter(this.Text,d);
                    ret[1]    = Utils.GetDriveLetter(this.Text);
                    return ret;
                }
            }

            return null;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.03.2015
        LAST CHANGE:   24.03.2015
        ***************************************************************************/
        public void Edit( )
        {
            Utils.Edit( Text );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.04.2015
        LAST CHANGE:   17.04.2015
        ***************************************************************************/
        public void Edit( string fpath )
        {
            Utils.Edit( fpath );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.03.2015
        LAST CHANGE:   26.03.2015
        ***************************************************************************/
        public void Edit( string fpath, int linenr )
        {
            Utils.Edit( fpath, linenr );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.08.2025
        LAST CHANGE:   12.08.2025
        ***************************************************************************/
        //public void SetAutoComplete()
        //{
        //    string dir = Text;
        //    if ( dir == null ) return;
        //    if ( dir == ""   ) return; 

        //    if ( ! Directory.Exists(dir) ) return;

        //    string[] dirs = Directory.GetDirectories(dir); 
        //    if ( dirs.Length == 0 )
        //    {
        //        string[] fils = Directory.GetFiles(dir);
        //        List<string> fls = new List<string>();
        //        foreach ( string f in fils ) fls.Add( Utils.GetFilename(f) );
        //        AutoCompleteCustomSource.AddRange( fls.ToArray() );
        //        return;
        //    }
        //    List<string> drs = new List<string>();
        //    foreach ( string d in dirs ) drs.Add( Utils.GetFilename(d) );
        //    AutoCompleteCustomSource.AddRange( drs.ToArray() );
        //}


	} // class
} // namespace
