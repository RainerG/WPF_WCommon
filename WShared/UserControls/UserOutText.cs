using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

using NS_AppConfig;
using NS_WordExcel;
using NS_UserList;
using NS_Utilities;
using NS_UserColor;
using NS_UserCombo;

namespace NS_UserOut
{
    public partial class UserOutText:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       11.08.2015
        LAST CHANGE:   30.10.2020
        ***************************************************************************/
        public RichTextBox  TextBox   { get { return richTextBox; }  }
        public bool         FormAlive { get { return m_FormAlive; }  }
        public string       FiltKey   { get { return userComboBoxFilter.Text; } }
        public List<string> Markers   { get { return m_Markers;   }  }
        public string       MarkKey   { set { SetMarkKey( value );  } }
        public ColSelType   ColHdr    { get { return m_Prefs.ColHead; } }
        public ColSelType   Col1      { get { return m_Prefs.Col1; } }
        public ColSelType   Col2      { get { return m_Prefs.Col2; } }
        public ColSelType   Col3      { get { return m_Prefs.Col3; } }
        public ColSelType   Col4      { get { return m_Prefs.Col4; } }
        public ColSelType   Col5      { get { return m_Prefs.Col5; } }
        public ColSelType   ColMark   { get { return m_Prefs.ColMark; } }
        public ColSelType   ColErr    { get { return m_Prefs.ColErr;  } }
        public ColSelType   ColSepar  { get { return m_Prefs.ColSepar;  } }
        public RecOut       Record    { get { return m_RecOut; } }
        public UserOutList  OutList   { get { return m_OutList; } } 
        public string       Selection { get { return GetSelection(); } }
        public bool         bVisible  { set { m_Visible = value; } }

        public ColSelType GetCol( string a_ColSel ) { return m_Prefs.GetCol( a_ColSel ); }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       31.03.2015
        LAST CHANGE:   20.11.2020
        ***************************************************************************/
        private     FindString          m_FindString;
        private     bool                m_SortDirection;
        protected   PreferencesUOText   m_Prefs;
        private     UserOutList         m_OutList;
        private     bool                m_FormAlive;
        private     List<string>        m_UndoMem;
        private     List<string>        m_TempMem;
        private     List<string>        m_Markers;
        private     Color               m_ColMem;
        private     RecOut              m_RecOut;
        private     List<MarkIndex>     m_MarkIdices;
        private     bool                m_Visible;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       27.02.2015
        LAST CHANGE:   11.04.2024
        ***************************************************************************/
        public UserOutText()
        {
            InitializeComponent();
            Utils.InitDialog(this);
            Utils.InitCtrl(menuStrip);
            Utils.InitDlgSzLoc( this );

            //Font sfnt = richTextBox.SelectionFont;
            //Font nfnt = new Font( "Courier New",sfnt.Size, sfnt.Style | FontStyle.Bold );
            //richTextBox.SelectionFont = nfnt;

            richTextBox.WordWrap    = false;
            richTextBox.RightMargin = 1000000;

            m_SortDirection = true;
            m_FindString    = new FindString(ref richTextBox);
            m_Prefs         = new PreferencesUOText();
            m_OutList       = new UserOutList();
            m_UndoMem       = new List<string>();
            m_TempMem       = new List<string>();
            m_Markers       = new List<string>();
            m_MarkIdices    = new List<MarkIndex>();
            m_RecOut        = new RecOut();
            m_FormAlive     = false;
            m_Visible       = true ;
            BuildContextMenu();

            m_Prefs.TextFont = richTextBox.Font;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.12.2016
        LAST CHANGE:   16.12.2016
        ***************************************************************************/
        private void AddTextEntry()
        {
            userComboBoxMark  .AddTextEntry();
            userComboBoxFilter.AddTextEntry();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.02.2015
        LAST CHANGE:   11.05.2021
        ***************************************************************************/
        private void OutText_FormClosing( object sender, FormClosingEventArgs e )
        {
            AddTextEntry();
            Clear();
            m_FormAlive = false;
            e.Cancel    = true;
            this.Hide();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.02.2015
        LAST CHANGE:   05.04.2024
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                m_FormAlive = true;
                a_Conf.DeserializeDialog( this );
                wordDisjunctionToolStripMenuItem.Checked = a_Conf.Deserialize<bool>();
                autoScrollToolStripMenuItem.Checked      = a_Conf.Deserialize<bool>();
                if ( a_Conf.DbVersion >= 330 ) caseSensitiveToolStripMenuItem.Checked = a_Conf.Deserialize<bool>();
            }
            else
            {
                a_Conf.SerializeDialog( this );
                a_Conf.Serialize( wordDisjunctionToolStripMenuItem.Checked );
                a_Conf.Serialize( autoScrollToolStripMenuItem     .Checked );
                a_Conf.Serialize( caseSensitiveToolStripMenuItem  .Checked );
            }

            m_FindString      .Serialize( ref a_Conf );
            m_Prefs           .Serialize( ref a_Conf );
            m_OutList         .Serialize( ref a_Conf );
            userComboBoxMark  .Serialize( ref a_Conf );
            userComboBoxFilter.Serialize( ref a_Conf );

            if ( a_Conf.IsReading )
            {
                EnabWDMenuItems();
                richTextBox.Font = m_Prefs.TextFont;
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.04.2015
        LAST CHANGE:   10.06.2024
        ***************************************************************************/
        protected void Scroll()
        {
            //this.ToFront();
            richTextBox.ScrollToCaret();
            BringToFront();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.04.2015
        LAST CHANGE:   26.06.2025
        ***************************************************************************/
        private delegate void dl_Clear();
        public void Clear( )
        {
            if( this.InvokeRequired )
            {
                dl_Clear d = new dl_Clear( Clear );
                this.Invoke( d, new object[]{} );
            }
            else
            {
                m_RecOut.Clear();
                m_OutList.Clear();
                m_TempMem.Clear();
                m_UndoMem.Clear();
                richTextBox.Clear();
                richTextBox.Text = "";
                richTextBox.Update();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.06.2025
        LAST CHANGE:   16.06.2025
        ***************************************************************************/
        public delegate void dl_SetMarkKey( string a_Key );

        public void SetMarkKey( string a_Key )
        {
            if( this.InvokeRequired )
            {
                dl_SetMarkKey d = new dl_SetMarkKey( SetMarkKey );
                this.Invoke( d, new object[] { a_Key } );
            }
            else
            {
                userComboBoxMark.Text = a_Key;
                userComboBoxMark.AddTextEntry();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.04.2015
        LAST CHANGE:   10.02.2021
        ***************************************************************************/
        public delegate void dl_ShowOutput( string a_Text, ColSelType a_Col, bool a_LF );

        public void ShowOutput( string a_Text, string a_ColSel, bool a_LF )
        {
            ColSelType cs = m_Prefs.GetCol( a_ColSel );
            ShowOutput( a_Text, cs, a_LF );
        }

        public void ShowOutput( string a_Text, Color a_Col, bool a_LF = false )     { ShowOutput( a_Text, new ColSelType( "", a_Col, false, false ), a_LF ); }
        public void ShowOutput( string a_Text, Color a_Col, bool a_Bld, bool a_LF ) { ShowOutput( a_Text, new ColSelType( "", a_Col, a_Bld, false ), a_LF ); }
        public void ShowOutput( string a_Text, string a_ColSel )             { ShowOutput( a_Text, a_ColSel, false ); }             
        public void ShowOutput( string a_Text  )                             { ShowOutput( a_Text, "head" ); }
        public void ShowOutput( string a_Text, ColSelType a_Col )            { ShowOutput( a_Text, a_Col, false ); }
        public void ShowOutput( string a_Text, ColSelType a_Col, bool a_Bold, bool a_LF ) 
        {  // for backward compatibility
            ColSelType col = new ColSelType( a_Col );
            col.bold = a_Bold;
            ShowOutput( a_Text, a_Col, a_LF ); 
        }

        public void ShowOutput( string a_Text, ColSelType a_Col, bool a_LF )
        {
            if( this.InvokeRequired )
            {
                dl_ShowOutput d = new dl_ShowOutput( ShowOutput );
                this.Invoke( d, new object[] { a_Text, a_Col, a_LF } );
            }
            else
            {
                //Thread.Sleep(1);
                string  txt = a_Text;
                Color   col = a_Col.color;

                List<string> mrks = GetFilterWords( ref userComboBoxMark );

                if( a_LF ) txt += "\n";

                m_RecOut.Record(txt);

                int start = richTextBox.TextLength;
                richTextBox.AppendText(txt);
                int end   = richTextBox.TextLength;

                richTextBox.Select(start, end - start);
                richTextBox.SelectionColor = a_Col.color;
                SetFont( a_Col.bold, a_Col.ital );

                m_Prefs.GetMarkerColors();

                // dye temporary markers
                foreach( string mrk in m_Markers )
                {
                    DyeSubstring( mrk, start, m_Prefs.ColMark, false );
                }
    
                // dye the filter keywords
                int ix = 0;
                foreach( string mrk in mrks )
                {
                    DyeSubstring( mrk, start, m_Prefs.NextMrkCol, false );
                }

                // dye the separators
                do
                {
                    ix = txt.IndexOf("@C");
                    if ( ix != -1 )
                    {
                        if ( ix + 10 > txt.Length ) break;
                        string cl  = txt.Substring( ix+2, 8 );
                        int cli = (int)Utils.Hex2UInt( cl );
                        col = Color.FromArgb( cli );

                        txt = txt.Remove(ix,10);
                        richTextBox.Select(start + ix,10);
                        richTextBox.SelectedText = "";

                        int stt = ix;
                        ix = txt.IndexOf("\n",stt);

                        if (ix > stt)
                        {
                            richTextBox.Select( start + stt, ix-stt );
                            richTextBox.SelectionColor = col;
                        }
                    }
                } while( ix > 0 ); 

            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void SetFont( bool a_Bold, bool a_Ital )
        {
            Font fnt = richTextBox.SelectionFont;
            if (fnt == null) fnt = richTextBox.Font;

            if      (   a_Bold &&   a_Ital ) fnt = new Font( fnt, FontStyle.Bold | FontStyle.Italic );
            else if (   a_Bold && ! a_Ital ) fnt = new Font( fnt, FontStyle.Bold );
            else if ( ! a_Bold &&   a_Ital ) fnt = new Font( fnt, FontStyle.Italic );
            else                             fnt = new Font( fnt, FontStyle.Regular );

            richTextBox.SelectionFont = fnt;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void SetSelCol( ColSelType a_Col )
        {
            bool fore  = m_Prefs.MarkLtrs;

            if (fore)
            {
                richTextBox.SelectionColor     = a_Col.color;
                SetFont( a_Col.bold, a_Col.ital );
            }
            else
            {
                richTextBox.SelectionBackColor = a_Col.color;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.04.2019
        LAST CHANGE:   05.06.2025
        ***************************************************************************/
        private void DyeSubstring( string a_Mrk, int a_Start, ColSelType a_Col, bool a_Invers )
        {
            int idx = -1;
            int ix  =  0;

            string txt = richTextBox.Text; 
            bool   rex = m_Prefs.RegExpr;
            bool   cs  = caseSensitiveToolStripMenuItem.Checked;

            txt = txt.Substring( a_Start );
            m_MarkIdices.Clear();

            string mrk = a_Mrk;

            do 
            {
                if ( rex ) idx = Utils.RexFindIndex( txt, ref mrk, ix, cs );
                else       idx = ( cs ? txt : txt.ToLower() ).IndexOf ( cs ? mrk : mrk.ToLower(), ix );

                if( idx != -1 )
                {
                    ix = idx + mrk.Length - 1;
                    m_MarkIdices.Add( new MarkIndex( a_Start + idx, mrk.Length ) );
                }
            } while (idx != -1);

            MarkText( a_Col, a_Invers );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   05.06.2025
        ***************************************************************************/
        private void DyeLine( string a_Mrk, int a_Start, ColSelType a_Col, bool a_Invers )
        {
            int idx = -1;
            int ix  =  0;
            int stt = a_Start;

            string txt     = richTextBox.Text; 
            int    lix     = txt.Length - 1;
            bool   a_Fore  = m_Prefs.MarkLtrs;
            bool   a_Regex = m_Prefs.RegExpr;
            bool   cs      = caseSensitiveToolStripMenuItem.Checked;

            m_MarkIdices.Clear();

            do 
            {
                string mrk = a_Mrk;
                if ( a_Regex ) idx = Utils.RexFindIndex( txt, ref mrk, ix, cs );
                else           idx = ( cs ? txt : txt.ToLower() ).IndexOf       ( cs ? mrk : mrk.ToLower(), ix );

                if( idx != -1 )
                {
                    int sidx = txt.Substring( stt, idx ).LastIndexOf("\n") + 1;
                    int ridx = txt.Substring( idx + mrk.Length -1, lix - idx - mrk.Length ).IndexOf("\n");
                    int lidx = ridx + idx + mrk.Length;

                    m_MarkIdices.Add( new MarkIndex( sidx, lidx - sidx ) );

                    ix = lidx;
                }
            } while (idx != -1);

            MarkText( a_Col, a_Invers );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.11.2020
        LAST CHANGE:   20.11.2020
        ***************************************************************************/
        private void MarkText( ColSelType a_Col, bool a_Invers )
        {
            if ( a_Invers )
            {
                int sidx = 0;
                int len  = 0;
                int off  = 0;

                foreach( MarkIndex mi in m_MarkIdices )
                {
                    len = mi.idx - off;
                    richTextBox.Select( sidx, len );
                    SetSelCol( a_Col );
                    off = mi.idx + mi.len;
                    sidx = off;
                }

                richTextBox.Select( off, richTextBox.Text.Length - off );
                SetSelCol( a_Col );
            }
            else
            {
                foreach( MarkIndex mi in m_MarkIdices )
                {
                    richTextBox.Select( mi.idx, mi.len );
                    SetSelCol( a_Col );
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.03.2016
        LAST CHANGE:   25.03.2016
        ***************************************************************************/
        private delegate void dl_Deselect();
        public void Deselect()
        {
            if ( this.InvokeRequired )
            {
                dl_Deselect d = new dl_Deselect( Deselect );
                this.Invoke( d, new object[]{} );
            }
            else
            {
                richTextBox.DeselectAll();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.10.2020
        LAST CHANGE:   23.10.2020
        ***************************************************************************/
        private delegate string dl_GetSelection();
        public string GetSelection()
        {
            if ( this.InvokeRequired )
            {
                dl_GetSelection d = new dl_GetSelection( GetSelection );
                return (string) this.Invoke( d, new object[]{} );
            }
            else
            {
                return richTextBox.SelectedText;
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.09.2015
        LAST CHANGE:   26.06.2025
        ***************************************************************************/
        private void clearToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Clear();
            //richTextBox.Clear();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.09.2015
        LAST CHANGE:   23.09.2015
        ***************************************************************************/
        private void findStringToolStripMenuItem_Click( object sender, EventArgs e )
        {
            m_FindString.Show();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2015
        LAST CHANGE:   19.11.2015
        ***************************************************************************/
        private string GetSelected()
        {
            richTextBox.HideSelection = false; //for showing selection  
            string selectedText = richTextBox.SelectedText;

            if ( selectedText.IndexOf("\n") == -1)
            {
                MessageBox.Show( "Select more than 1 line before !","Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return "";
            }

            return selectedText;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.09.2015
        LAST CHANGE:   19.11.2015
        ***************************************************************************/
        private void SortSelection( bool a_Dir )
        {
            string selectedText = GetSelected();

            if (selectedText == "") return;

            int    index        = richTextBox.Text.IndexOf(selectedText);
            int    len          = selectedText.Length;

            // discard last linefeed 
            int lastidx = selectedText.Length-1;
            if (selectedText[lastidx] == '\n') selectedText = selectedText.Remove(lastidx,1);

            /*Sorting*/
            string[] lines = selectedText.Split("\n".ToArray());

            if (a_Dir)     Array.Sort(lines, delegate(string str1, string str2) { return str1.CompareTo(str2); });
            else           Array.Sort(lines, delegate(string str1, string str2) { return str2.CompareTo(str1); });

            string sortedText = "";
            foreach( string line in lines ) sortedText += line + "\n";
            // discard last LF
            lastidx     = sortedText.Length-1;
            sortedText  = sortedText.Remove(lastidx);

            // replace recent selection by sorted selection
            richTextBox.Text = richTextBox.Text.Replace(selectedText,sortedText);
            richTextBox.Select(index, selectedText.Length);
            //richTextBox.SelectionColor     = fc;
            //richTextBox.SelectionBackColor = bc;
            //richTextBox.SelectionFont      = fnt;
            //richTextBox.Update();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.09.2015
        LAST CHANGE:   23.09.2015
        ***************************************************************************/
        private void sortSelectionToolStripMenuItem_Click( object sender, EventArgs e )
        {
            m_SortDirection = ! m_SortDirection;
            SortSelection( m_SortDirection );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.09.2015
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        public string SaveOutput( string a_Fname, bool a_AsTxt, bool a_Sel )
        {
            try
            {
                string fname = Utils.GetFilenameBody( a_Fname );
                string dir   = Utils.GetPath( a_Fname );
                fname = Utils.ConcatPaths( dir, fname );

                if( a_AsTxt )
                {
                    fname += ".txt";
                    int fidx = 1;
                    while( File.Exists( fname ) )
                    {
                        fname = Utils.ConcatPaths( dir, a_Fname );
                        fname += "_" + fidx++.ToString();
                        fname += ".txt";
                    }
                    if ( a_Sel )
                    {
                        string txt = richTextBox.SelectedText;
                        File.WriteAllText( fname, txt );
                    }
                    else richTextBox.SaveFile( fname, RichTextBoxStreamType.PlainText );
                }
                else
                {
                    fname += ".rtf";
                    int fidx = 1;
                    while ( File.Exists( fname ) )
                    {
                        fname  = Utils.ConcatPaths( dir, a_Fname );
                        fname += "_" + fidx++.ToString();
                        fname += ".rtf";
                    }
                    richTextBox.SaveFile( fname );
                }

                return fname;
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Exception in SaveOutput" );
                return "";
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.04.2024
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        public string SaveOutput( bool a_AsTxt, bool a_Sel, string a_Dir = "" )
        {
            string dir   = m_Prefs.OutPath;
            string fname = "";

            if ( a_Dir != "" ) dir = a_Dir;

            try
            {
                if (! Directory.Exists(dir) ) Directory.CreateDirectory(dir);

                if ( a_AsTxt )
                {
                    fname = Utils.ConcatPaths(dir, "Out_" + Utils.GetTimeStamp() + ".txt");
                    SaveOutput( fname, true, a_Sel );
                    ShowOutput( "Saved selection to => "   + fname + "\n" );
                }
                else
                {
                    fname = Utils.ConcatPaths(dir, "Out_" + Utils.GetTimeStamp() + ".rtf");
                    SaveOutput( fname, false, a_Sel );
                    ShowOutput( "Saved text output to => " + fname + "\n" );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving " + fname );
            }

            return fname;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.05.2019
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        private void saveAndHandOverToWordpadToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string fname = SaveOutput( false, false );

            Process.Start( "wordpad.exe", fname );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.05.2019
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        private void openInEditorToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string fname = SaveOutput( true, false );

            Utils.Edit( fname );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.08.2021
        LAST CHANGE:   24.04.2024
        ***************************************************************************/
        private void openSelInEditorToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string fname = SaveOutput( true, true );

            Utils.Edit( fname );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.05.2022
        LAST CHANGE:   12.05.2022
        ***************************************************************************/
        private void openSelPathInEditorToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string path = richTextBox.SelectedText;
            path = path.Replace("\n","");
            Utils.Edit( path ); 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.05.2025
        LAST CHANGE:   13.05.2025
        ***************************************************************************/
        private void openSelpathInExplorerToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string path = richTextBox.SelectedText;
            path = path.Replace("\n","");
            Utils.Explorer( path );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.05.2025
        LAST CHANGE:   13.05.2025
        ***************************************************************************/
        private void openselPathInExplorerToolStripMenuItem1_Click( object sender, EventArgs e )
        {
            string path = richTextBox.SelectedText;
            path = path.Replace("\n","");
            Utils.Explorer( path, true );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.09.2015
        LAST CHANGE:   09.08.2021
        ***************************************************************************/
        private void saveOutputToolStripMenuItem_Click(object sender,EventArgs e)
        {
            SaveOutput( false, false );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.09.2015
        LAST CHANGE:   25.03.2021
        ***************************************************************************/
        private void preferencesToolStripMenuItem_Click(object sender,EventArgs e)
        {
            m_Prefs.ShowDialog();
            richTextBox.Font = m_Prefs.TextFont;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2015
        LAST CHANGE:   19.11.2015
        ***************************************************************************/
        private void exportSelToXLToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string dir   = m_Prefs.OutPath;
            string fname = "";

            try
            {
                if (! Directory.Exists(dir) ) Directory.CreateDirectory(dir);

                fname = Utils.ConcatPaths(dir, "Out_" + Utils.GetTimeStamp() + ".csv");
                StreamWriter file = new StreamWriter(fname);
 
                string selectedText = GetSelected();
                string separs       = m_Prefs.Separs;

                if (selectedText == "") return;

                string[] lines = selectedText.Split("\n".ToArray());
            

                foreach( string ln in lines )
                {
                    string line = "";
                    string[] segs = ln.Split(separs.ToArray());
                    foreach( string seg in segs) line += seg + ";";
                    file.WriteLine(line);
                }

                ShowOutput( "Saving selection to => " + fname + "\n" );
                file.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving " + fname );
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2015
        LAST CHANGE:   25.09.2023
        ***************************************************************************/
        private void exportSelTolistViewToolStripMenuItem_Click( object sender, EventArgs e )
        {
            try
            {
                int maxcols = 0; 

                string selectedText = GetSelected();
                string separs       = m_Prefs.Separs;

                if (selectedText == "") return;

                string[] lines = selectedText.Split("\n".ToArray());

                m_OutList.Clear();

                foreach( string ln in lines )
                {
                    if (ln == "") continue;
                    List<string> segs = Utils.SplitExt( ln, separs );

                    m_OutList.AddLine(segs);
                    if (maxcols < segs.Count ) maxcols = segs.Count;
                }

                List<string> cols = new List<string>();
                for (int i=0; i<maxcols; i++)
                {
                    string col = "Col" + i.ToString();
                    cols.Add(col);
                }
                m_OutList.ShowColumns(cols);
                //m_OutList.AutoColmnAdapt();
                m_OutList.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error exporting to list view" );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.01.2016
        LAST CHANGE:   17.01.2016
        ***************************************************************************/
        private delegate int dl_GetNrLines();
        public int GetNrLines()
        {
            if( this.InvokeRequired )
            {
                dl_GetNrLines d = new dl_GetNrLines(GetNrLines);
                return (int) this.Invoke(d, new object[]{});
            }
            else return richTextBox.Lines.Length;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.01.2016
        LAST CHANGE:   16.06.2025
        ***************************************************************************/
        private delegate void dl_Refresh();
        public void Refresh()
        {
            if( this.InvokeRequired )
            {
                dl_Refresh d = new dl_Refresh(Refresh);
                this.Invoke( d, new object[]{});
            }
            else
            {
                Show();
                if (autoScrollToolStripMenuItem.Checked)
                {
                    Scroll();
                    BringToFront();
                }
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.05.2021
        LAST CHANGE:   17.05.2021
        ***************************************************************************/
        private delegate void dl_ToFront();
        public void ToFront()
        {
            if( this.InvokeRequired )
            {
                dl_ToFront d = new dl_ToFront(ToFront);
                this.Invoke( d, new object[]{});
            }
            else
            {
                BringToFront();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.02.2016
        LAST CHANGE:   16.02.2016
        ***************************************************************************/
        private void buttonDelMark_Click( object sender, EventArgs e )
        {
            userComboBoxMark.Text = "";
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   08.11.2024
        ***************************************************************************/
        private void SaveUndo()
        {
            userComboBoxMark  .AddTextEntry();
            userComboBoxFilter.AddTextEntry();
            m_UndoMem.Clear();
            m_UndoMem.AddRange( richTextBox.Lines );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.04.2016
        LAST CHANGE:   09.04.2016
        ***************************************************************************/
        private void LoadUndo()
        {
            richTextBox.Clear();
            richTextBox.Lines = (string[])m_UndoMem.ToArray();
            LoadCol();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.04.2016
        LAST CHANGE:   08.11.2017
        ***************************************************************************/
        private void SaveCol() { SaveCol(0); }
        private void SaveCol( int a_LineNr )
        {
            int idx = richTextBox.GetFirstCharIndexFromLine(a_LineNr);
            if (idx < 0) return;
            richTextBox.Select( idx,1 );
            m_ColMem = richTextBox.SelectionColor;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.04.2016
        LAST CHANGE:   09.04.2016
        ***************************************************************************/
        private void LoadCol()
        {
            richTextBox.SelectAll();
            richTextBox.SelectionColor = m_ColMem;
            richTextBox.DeselectAll();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.04.2016
        LAST CHANGE:   09.04.2016
        ***************************************************************************/
        private void SaveTemp() { SaveTemp(0); }
        private void SaveTemp( int a_LineNr )
        {
            m_TempMem.Clear();
            m_TempMem.AddRange( richTextBox.Lines );
            SaveCol( a_LineNr );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.04.2016
        LAST CHANGE:   09.04.2016
        ***************************************************************************/
        private void LoadTemp()
        {
            richTextBox.Clear();
            richTextBox.Lines = (string[]) m_TempMem.ToArray();
            LoadCol();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.06.2016
        LAST CHANGE:   05.04.2024
        ***************************************************************************/
        protected List<string> GetFilterWords( ref UserComboBox a_Combo )
        {
            string filter = a_Combo.Text;
            bool   wd     = wordDisjunctionToolStripMenuItem.Checked;
            bool   cs     = caseSensitiveToolStripMenuItem  .Checked;

            List<string> ret = null;

            if( wd )
            {
                ret = Utils.SplitExt( filter, " " );
            }
            else 
            { 
                ret = new List<string>(); 
                if (filter != "") ret.Add( cs ? filter : filter.ToLower() ); 
            }
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        void markAllLinesContainingKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveCol();

            List<string> filts = GetFilterWords( ref userComboBoxFilter );
            m_Prefs.GetMarkerColors();
            
            foreach( string filt in filts )
            {   
                ColSelType col = m_Prefs.NextMrkCol;
                DyeLine( filt, 0, col, false );
            }
            richTextBox.Update();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void markAllFoundKeysToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveCol();

            List<string> filts = GetFilterWords( ref userComboBoxFilter );
            m_Prefs.GetMarkerColors();
            bool fore = m_Prefs.MarkLtrs;
            bool reg  = m_Prefs.RegExpr;
            
            foreach( string filt in filts )
            {   
                ColSelType col = m_Prefs.NextMrkCol;
                DyeSubstring( filt, 0, col, false );
            }
            richTextBox.Update();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private bool ContainsKey( string a_Text, string a_Key )
        {
            if ( m_Prefs.RegExpr )
            {
                return Utils.RexMatch( a_Text, a_Key );
            }
            return a_Text.Contains( a_Key );
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.06.2016
        LAST CHANGE:   18.12.2016
        ***************************************************************************/
        private bool ContainsKey( string a_Text )
        {
            List<string> filts = GetFilterWords( ref userComboBoxFilter );

            foreach( string filt in filts ) 
            {
                if (a_Text.Contains( filt ) ) return true;
            }
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void markAllLinesWithoutKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveCol();

            List<string> filts = GetFilterWords( ref userComboBoxFilter );
            m_Prefs.GetMarkerColors();
            
            foreach( string filt in filts )
            {   
                ColSelType col = m_Prefs.NextMrkCol;
                DyeLine( filt, 0, col, true );
            }
            richTextBox.Update();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.11.2020
        LAST CHANGE:   20.11.2020
        ***************************************************************************/
        private void wordDisjunctionToolStripMenuItem_Click( object sender, EventArgs e )
        {
            EnabWDMenuItems();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.11.2020
        LAST CHANGE:   20.11.2020
        ***************************************************************************/
        private void EnabWDMenuItems( )
        {
            markAllLinesWithoutKeyToolStripMenuItem.Enabled     = ! wordDisjunctionToolStripMenuItem.Checked;
            deleteAllLinesContainitKeyToolStripMenuItem.Enabled = ! wordDisjunctionToolStripMenuItem.Checked;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void invertMarkToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveCol();

            for( int i = 0; i < richTextBox.Lines.Length; i++ )
            {
                int start = richTextBox.GetFirstCharIndexFromLine(i);
                int len   = richTextBox.Lines[i].Length;
                richTextBox.Select(start,len);
                if (  richTextBox.SelectionBackColor == m_Prefs.GetCol("ker1").color ||
                      richTextBox.SelectionBackColor == m_Prefs.GetCol("ker2").color ||
                      richTextBox.SelectionBackColor == m_Prefs.GetCol("ker3").color )
                      richTextBox.SelectionBackColor = Color.Transparent;
                else  richTextBox.SelectionBackColor = m_Prefs.ColMark.color;
            }
            richTextBox.Update();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void deleteMarkedLinesToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveCol();

            List<RTLineType> mem = new List<RTLineType>();

            for( int i = 0; i < richTextBox.Lines.Length; i++ )
            {
                int start = richTextBox.GetFirstCharIndexFromLine(i);
                //int len   = richTextBox.Lines[i].Length;
                richTextBox.Select( start, 1 );
                if( richTextBox.SelectionBackColor != m_Prefs.ColMark.color )
                {
                    mem.Add( new RTLineType(richTextBox.Lines[i] + "\n", richTextBox.SelectionColor) );
                }
            }

            richTextBox.Clear();

            int st = 0;
            int ln = 0;

            foreach( RTLineType rtl in mem )
            {
                ln = rtl.line.Length;
                richTextBox.Text += rtl.line;
                richTextBox.Select(st,ln);
                richTextBox.SelectionColor = rtl.color;
                st += ln;
            }
            
            LoadCol();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   20.11.2020
        ***************************************************************************/
        private void leaveAllLinesContainigKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveTemp(1);

            List<string> filt = new List<string>();

            if ( wordDisjunctionToolStripMenuItem.Checked )
            {
                List<string> filts = Utils.SplitExt( FiltKey, ",; " ); 
                foreach( string flt in filts )
                {
                    List<string> fl = m_TempMem.FindAll( f => ContainsKey( f, flt ) );
                    filt.AddRange( fl );
                }
            }
            else
            {
                filt = m_TempMem.FindAll( f => ContainsKey( f, FiltKey ) );
            }

            richTextBox.Clear();
            richTextBox.Lines = (string[])filt.ToArray();

            LoadCol();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   20.11.2020
        ***************************************************************************/
        private void deleteAllLinesContainitKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveTemp(1);

            List<string> filt = m_TempMem.FindAll( ln => ! ContainsKey( ln, FiltKey ) );

            richTextBox.Clear();
            richTextBox.Lines = (string[])filt.ToArray();

            LoadCol();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   08.04.2016
        ***************************************************************************/
        private void deleteAllRightFromKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveTemp();

            int filtlen = FiltKey.Length;

            for( int i=0; i<m_TempMem.Count; i ++ )
            {
                int idx = m_TempMem[i].IndexOf( FiltKey );
                if (idx != -1) m_TempMem[i] = m_TempMem[i].Remove( idx + filtlen );
            }

            LoadTemp();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   08.04.2016
        ***************************************************************************/
        private void deleteAllLeftFromKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveUndo();
            SaveTemp();

            for( int i=0; i<m_TempMem.Count; i ++ )
            {
                int idx = m_TempMem[i].IndexOf( FiltKey );
                if (idx != -1) m_TempMem[i] = m_TempMem[i].Remove( 0,idx );
            }

            LoadTemp();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.04.2016
        LAST CHANGE:   08.04.2016
        ***************************************************************************/
        private void undoToolStripMenuItem_Click( object sender, EventArgs e )
        {
            LoadUndo();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   04.06.2016
        ***************************************************************************/
        private void richTextBox_MouseUp( object sender, MouseEventArgs e )
        {
            return;
            if (e.Button == MouseButtons.Right) BuildContextMenu();
        }

        /***************************************************************************
        SPECIFICATION: Context menu 
        CREATED:       04.06.2016
        LAST CHANGE:   26.09.2022
        ***************************************************************************/
        private void BuildContextMenu()
        {
            ContextMenu cm = new ContextMenu();

            MenuItem it_hex2dec = new MenuItem("Paste selection as dec.");
            MenuItem it_dec2hex = new MenuItem("Paste selection as hex.");

            it_hex2dec.Click += new EventHandler( CM_Hex2Dec );
            it_dec2hex.Click += new EventHandler( CM_Dec2Hex );

            MenuItem it_cpy = new MenuItem("Copy");
            MenuItem it_cut = new MenuItem("Cut");
            MenuItem it_pst = new MenuItem("Paste");

            it_cpy.Click += new EventHandler( CM_Copy );
            it_cut.Click += new EventHandler( CM_Cut  );
            it_pst.Click += new EventHandler( CM_Paste );

            cm.MenuItems.Add( it_hex2dec );
            cm.MenuItems.Add( it_dec2hex );
            cm.MenuItems.Add( "-" );
            cm.MenuItems.Add( it_cpy );
            cm.MenuItems.Add( it_cut );
            cm.MenuItems.Add( it_pst );

            richTextBox.ContextMenu = cm;
        }

        private void CM_Copy ( object sender, EventArgs e ) { richTextBox.Copy(); }
        private void CM_Cut  ( object sender, EventArgs e ) { richTextBox.Cut(); }
        private void CM_Paste( object sender, EventArgs e ) 
        { 
            if (! Clipboard.ContainsText() ) return;
            richTextBox.Paste();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.09.2022
        LAST CHANGE:   26.09.2022
        ***************************************************************************/
        private void CM_Hex2Dec( object sender, EventArgs e ) { PasteHexDec( false ); }
        private void CM_Dec2Hex( object sender, EventArgs e ) { PasteHexDec( true  ); }

        private void PasteHexDec( bool a_Hex )
        {
            string sval = richTextBox.SelectedText;

            string nval = "";
            if ( ! sval.EndsWith(" ") ) nval += " "; 
            if ( a_Hex ) nval += string.Format("{0:X8} ", Utils.Str2UInt( sval ) );
            else         nval += string.Format("{0} "   , Utils.Hex2UInt( sval ) );

            richTextBox.SelectionStart += richTextBox.SelectionLength;

            richTextBox.SelectionLength = 0;
            richTextBox.SelectedText = nval;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.12.2016
        LAST CHANGE:   18.12.2016
        ***************************************************************************/
        private void richTextBox_KeyPress( object sender, KeyPressEventArgs e )
        {
            if (e.KeyChar == 6) // ctrl F
            {
                m_FindString.Show();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.12.2016
        LAST CHANGE:   18.12.2016
        ***************************************************************************/
        private void richTextBox_KeyDown( object sender, KeyEventArgs e )
        {
            if (e.KeyCode == Keys.F3 && m_FindString.Visible) 
            {
                m_FindString.FindStr();
            }
        }


        /***************************************************************************
        SPECIFICATION: Functions from web in order to add BMPs to the rich text box
        CREATED:       12.05.2021
        LAST CHANGE:   12.05.2021
        ***************************************************************************/
        public void AppendImage( Image img )
        {
            var rtf = new StringBuilder();

            // Append the RTF header
            rtf.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033");
            // Create the font table using the RichTextBox's current font and append
            // it to the RTF string
            rtf.Append(GetFontTable(this.Font));
            // Create the image control string and append it to the RTF string
            rtf.Append(GetImagePrefix(img));
            // Create the Windows Metafile and append its bytes in HEX format
            rtf.Append(getRtfImage(img));
            // Close the RTF image control string
            rtf.Append(@"}");
            richTextBox.SelectedRtf = rtf.ToString();
        }

        private enum EmfToWmfBitsFlags
        {
            EmfToWmfBitsFlagsDefault = 0x00000000,
            EmfToWmfBitsFlagsEmbedEmf = 0x00000001,
            EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,
            EmfToWmfBitsFlagsNoXORClip = 0x00000004
        };

        private struct RtfFontFamilyDef
        {
            public const string Unknown = @"\fnil";
            public const string Roman = @"\froman";
            public const string Swiss = @"\fswiss";
            public const string Modern = @"\fmodern";
            public const string Script = @"\fscript";
            public const string Decor = @"\fdecor";
            public const string Technical = @"\ftech";
            public const string BiDirect = @"\fbidi";
        }

        [DllImport("gdiplus.dll")]
        private static extern uint GdipEmfToWmfBits(IntPtr _hEmf,
          uint _bufferSize, byte[] _buffer,
          int _mappingMode, EmfToWmfBitsFlags _flags);


        private string GetFontTable(Font font)
        {
            var fontTable = new StringBuilder();
            // Append table control string
            fontTable.Append(@"{\fonttbl{\f0");
            fontTable.Append(@"\");
            var rtfFontFamily = new HybridDictionary();
            rtfFontFamily.Add(FontFamily.GenericMonospace.Name, RtfFontFamilyDef.Modern);
            rtfFontFamily.Add(FontFamily.GenericSansSerif, RtfFontFamilyDef.Swiss);
            rtfFontFamily.Add(FontFamily.GenericSerif, RtfFontFamilyDef.Roman);
            rtfFontFamily.Add("UNKNOWN", RtfFontFamilyDef.Unknown);

            // If the font's family corresponds to an RTF family, append the
            // RTF family name, else, append the RTF for unknown font family.
            fontTable.Append(rtfFontFamily.Contains(font.FontFamily.Name) ? rtfFontFamily[font.FontFamily.Name] : rtfFontFamily["UNKNOWN"]);
            // \fcharset specifies the character set of a font in the font table.
            // 0 is for ANSI.
            fontTable.Append(@"\fcharset0 ");
            // Append the name of the font
            fontTable.Append(font.Name);
            // Close control string
            fontTable.Append(@";}}");
            return fontTable.ToString();
        }

        private string GetImagePrefix(Image _image)
        {
            float xDpi, yDpi;
            var rtf = new StringBuilder();
            using (Graphics graphics = CreateGraphics())
            {
                xDpi = graphics.DpiX;
                yDpi = graphics.DpiY;
            }
            // Calculate the current width of the image in (0.01)mm
            var picw = (int)Math.Round((_image.Width / xDpi) * 2540);
            // Calculate the current height of the image in (0.01)mm
            var pich = (int)Math.Round((_image.Height / yDpi) * 2540);
            // Calculate the target width of the image in twips
            var picwgoal = (int)Math.Round((_image.Width / xDpi) * 1440);
            // Calculate the target height of the image in twips
            var pichgoal = (int)Math.Round((_image.Height / yDpi) * 1440);
            // Append values to RTF string
            rtf.Append(@"{\pict\wmetafile8");
            rtf.Append(@"\picw");
            rtf.Append(picw);
            rtf.Append(@"\pich");
            rtf.Append(pich);
            rtf.Append(@"\picwgoal");
            rtf.Append(picwgoal);
            rtf.Append(@"\pichgoal");
            rtf.Append(pichgoal);
            rtf.Append(" ");

            return rtf.ToString();
        }

        private string getRtfImage(Image image)
        {
            // Used to store the enhanced metafile
            MemoryStream stream = null;
            // Used to create the metafile and draw the image
            Graphics graphics = null;
            // The enhanced metafile
            Metafile metaFile = null;
            try
            {
                var rtf = new StringBuilder();
                stream = new MemoryStream();
                // Get a graphics context from the RichTextBox
                using (graphics = CreateGraphics())
                {
                    // Get the device context from the graphics context
                    IntPtr hdc = graphics.GetHdc();
                    // Create a new Enhanced Metafile from the device context
                    metaFile = new Metafile(stream, hdc);
                    // Release the device context
                    graphics.ReleaseHdc(hdc);
                }

                // Get a graphics context from the Enhanced Metafile
                using (graphics = Graphics.FromImage(metaFile))
                {
                    // Draw the image on the Enhanced Metafile
                    graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
                }

                // Get the handle of the Enhanced Metafile
                IntPtr hEmf = metaFile.GetHenhmetafile();
                // A call to EmfToWmfBits with a null buffer return the size of the
                // buffer need to store the WMF bits.  Use this to get the buffer
                // size.
                uint bufferSize = GdipEmfToWmfBits(hEmf, 0, null, 8, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
                // Create an array to hold the bits
                var buffer = new byte[bufferSize];
                // A call to EmfToWmfBits with a valid buffer copies the bits into the
                // buffer an returns the number of bits in the WMF.  
                uint _convertedSize = GdipEmfToWmfBits(hEmf, bufferSize, buffer, 8, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
                // Append the bits to the RTF string
                foreach (byte t in buffer)
                {
                    rtf.Append(String.Format("{0:X2}", t));
                }
                return rtf.ToString();
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
                if (metaFile != null)
                    metaFile.Dispose();
                if (stream != null)
                    stream.Close();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ?
        LAST CHANGE:   14.06.2024
        ***************************************************************************/
        private void UserOutText_VisibleChanged( object sender, EventArgs e )
        {
            if ( ! m_Visible)
            {
                Visible = false;
                return;
            }

            if ( ! Visible  ) BringToFront();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.01.2024
        LAST CHANGE:   30.01.2024
        ***************************************************************************/
        private void picBoxMark_Click( object sender, EventArgs e )
        {
            userComboBoxMark.DroppedDown = true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.06.2025
        LAST CHANGE:   26.06.2025
        ***************************************************************************/
        private void setOptWidthToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SetOptimalWidth();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.06.2025
        LAST CHANGE:   26.06.2025
        ***************************************************************************/
        public void SetOptimalWidth()
        {
            int maxWidth = 0;
            using (Graphics g = richTextBox.CreateGraphics())
            {
                foreach (string line in richTextBox.Lines)
                {
                    int width = (int)TextRenderer.MeasureText(g, line, richTextBox.Font).Width;
                    if (width > maxWidth) maxWidth = width;
                }
            }
            
            Width = maxWidth + 70;
        }

    } // class

    /***************************************************************************
    SPECIFICATION: Global types
    CREATED:       08.04.2016
    LAST CHANGE:   08.04.2016
    ***************************************************************************/
    public class RTLineType
    {
        public Color  color;
        public string line;

        public RTLineType( string a_Line, Color a_Color )
        {
            color = a_Color;
            line  = a_Line;
        }
    }

    /***************************************************************************
    SPECIFICATION: Records the printed text as long as recording is true
    CREATED:       18.05.2016
    LAST CHANGE:   18.05.2016
    ***************************************************************************/
    public class RecOut
    {
        private string text;
        private bool   recording;

        public RecOut()
        {
            text      = "";
            recording = false;
        }

        public void Record( string a_Text )
        {
            if (recording) 
            {
                lock(text)
                {
                    text += a_Text;
                    Debug.WriteLine( "Rec: " + text + " - RecEnd" );
                }
            }
        }

        public void Start() { recording = true; }
        public void Stop()  { recording = false; }
        public void Clear() { lock(text) { Debug.WriteLine("Rec.Clear"); text = ""; } }
        public void Reset() { Clear(); Start(); }

        public string GetText()
        {
            string ret;
            lock(text)
            {
                ret = text;
                Debug.WriteLine( "GetTxt: " + text + " - GetTxtEnd");
            }
            return ret;
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       20.11.2020
    LAST CHANGE:   20.11.2020
    ***************************************************************************/
    public class MarkIndex
    {
        public int idx;
        public int len;

        public MarkIndex( int a_Idx, int a_Len )
        {
            idx = a_Idx;
            len = a_Len;
        }
    }

} // namespace
