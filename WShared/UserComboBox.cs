using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

using NS_UserTextEditor;
using NS_AppConfig;
using NS_Utilities;
using System.Runtime.InteropServices;

namespace NS_UserCombo 
{
    /// <summary>
    /// Summary description for FileComboBox.
    /// </summary>
    /***************************************************************************
    SPECIFICATION: More comfortable ComboBox
    CREATED:       2004
    LAST CHANGE:   28.08.2015
    ***************************************************************************/
    public class UserComboBox: System.Windows.Forms.ComboBox
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       16.09.2015
        LAST CHANGE:   10.12.2024
        ***************************************************************************/
        public bool    ReadOnly   { get { return m_bReadOnly; } set { m_bReadOnly = value; if ( m_bReadOnly ) ToolTip = TOOLTP_EDITFILT; } }
        public bool    HasFilt    { get { return m_CmbFilter.HasFilt; } }
        public bool    NoDelAll   { set { m_NoDelAll = value; } }
        public string  ToolTip    { set { m_Tooltip.SetToolTip(this,value); } }
        public string  Txt        { get { return Text; } set { SetText( value ); } }
        public int     MaxNrItems { set {  m_iMaxNrItems = value; } }



        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       2004
        LAST CHANGE:   18.11.2024
        ***************************************************************************/
        protected const string TOOLTP_DELHIST  = "Ctrl-Alt-D-left click: Delete history";
        protected const string TOOLTP_EDITHIST = "Ctrl-Alt-left click: Edit history";
        protected const string TOOLTP_EDITFILT = "Ctrl-Alt-F-left click: Edit filter";

        private   int           m_iMaxNrItems   = -1;  // unlimited;
        protected bool          m_bCtrlPressed  = false;
        protected bool          m_bAltPressed   = false;
        protected bool          m_bShiftPressed = false;
        protected bool          m_bDPressed     = false;
        protected bool          m_bFPressed     = false;
        protected List<string>  m_HistMem;
        protected ToolTip       m_Tooltip;

        private   string  m_sOldText;
        private   string  m_HlpText;
        private   bool    m_bReadOnly;
        private   bool    m_NoDelAll;

        public static List <UserComboBox> m_Inst = new List<UserComboBox>();
        
        private UserTextEditor m_TextEditor;
        private ComboFilter    m_CmbFilter;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       ??.??.2004
        LAST CHANGE:   21.11.2024
        ***************************************************************************/
        public UserComboBox()
            : base()
        {
            m_Inst.Add( this );

            KeyUp                += new KeyEventHandler(Combo_KeyUp);
            KeyDown              += new KeyEventHandler(Combo_KeyDown);
            KeyPress             += new KeyPressEventHandler(Combo_KeyPress);
            //SelectedIndexChanged += new EventHandler(Combo_SelectedIndexChanged);
            Click                += new EventHandler(Combo_Click);

            this.AutoCompleteMode   = AutoCompleteMode  .SuggestAppend;
            this.AutoCompleteSource = AutoCompleteSource.CustomSource;
            
            m_sOldText   = "";
            m_HlpText    = "";
            m_TextEditor = new UserTextEditor(this);
            m_CmbFilter  = new ComboFilter();
            m_HistMem    = new List<string>();
            m_Tooltip    = new ToolTip(); 

            m_bReadOnly  = false;
            m_NoDelAll   = false;
            SetToolTip("");

            MaxDropDownItems = 53;  // for low resolution desktops
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.12.2007
        LAST CHANGE:   05.06.2025
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf, bool a_WithHist = true)
        {
            if (a_Conf.IsReading)
            {
                a_Conf.DeserializeComboBox( this, a_WithHist );
                if (a_WithHist)
                {
                    int cnt = a_Conf.Deserialize<int>();
                    m_HistMem.Clear();
                    for( int i=0; i<cnt; i++ )
                    {
                        string s = a_Conf.Deserialize<string>();
                        m_HistMem.Add(s);
                    }
                }
            }
            else
            {
                a_Conf.SerializeComboBox( this, a_WithHist );
                if (a_WithHist)
                {
                    a_Conf.Serialize(m_HistMem.Count);
                    foreach ( string hm in m_HistMem ) a_Conf.Serialize( hm );
                }
            }

            m_TextEditor.Serialize(ref a_Conf);
            m_CmbFilter .Serialize(ref a_Conf);

            if (a_Conf.IsReading)
            {
                DiscardDoubles();
                FilterHist();
                AddTTFilter("");
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.11.2024
        LAST CHANGE:   10.12.2024
        ***************************************************************************/

        public static void DelAllItems()
        {
            foreach( UserComboBox cmb in m_Inst )
            {
                if (cmb.m_bReadOnly) continue;
                if (cmb.m_NoDelAll)  continue;

                cmb.Items    .Clear();
                cmb.m_HistMem.Clear();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.04.2019
        LAST CHANGE:   31.07.2023
        ***************************************************************************/
        public void Copy( UserComboBox a_Src )
        {
            Txt      = a_Src.Txt;
            ReadOnly = a_Src.ReadOnly;

            m_HistMem.Clear();
            m_HistMem.AddRange( a_Src.m_HistMem );

            Items.Clear();
            foreach ( string itm in a_Src.Items ) Items.Add(itm);
        }

        /***************************************************************************
        SPECIFICATION: Fixes an error in C#'s combo box:
                       Text assignment fails if operand is already in Items but 
                       with different (upper/lower)case.
        CREATED:       17.05.2023
        LAST CHANGE:   17.05.2023
        ***************************************************************************/
        protected void SetText( string a_Txt )
        {
            Text = a_Txt;

            if ( Text != a_Txt && Text.ToLower() == a_Txt.ToLower() )
            {
                for ( int i=0; i<Items.Count; i++ )
                {
                    string it = (string)Items[i];

                    if ( it.ToLower() == a_Txt.ToLower() ) 
                    {
                        Items.RemoveAt( i );
                        Items.Add( a_Txt );
                        break;
                    }
                }
                
                Text = a_Txt;
            }
        }

        //public override string Text
        //{
        //    get { return base.Text; }
        //    set
        //    {
        //        string txt = Text;
        //        Debug.WriteLine( "Text = " + Text );
        //        base.Text = value;
        //    }
        //}

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.07.2020
        LAST CHANGE:   06.02.2023
        ***************************************************************************/
        protected void SetToolTip( string a_Text )
        {
            if (ReadOnly)  m_Tooltip.SetToolTip( this, TOOLTP_EDITFILT + a_Text );
            else           m_Tooltip.SetToolTip( this, TOOLTP_EDITHIST + "\n" + TOOLTP_DELHIST + "\n" + TOOLTP_EDITFILT + a_Text );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.07.2020
        LAST CHANGE:   20.01.2022
        ***************************************************************************/
        protected void AddTTFilter( string a_ToolTp )
        {
            if (m_CmbFilter.Filters != "") SetToolTip( " (" + m_CmbFilter.Filters + ")" + a_ToolTp );
            else                           SetToolTip( a_ToolTp );  
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   30.07.2025
        ***************************************************************************/
        public bool AlreadyIn( string a_Str )
        {
            if (a_Str.Trim() == "") return true;   // white spaces are rejected

            if (Items.Count > m_HistMem.Count) SyncHist();

            if ( m_HistMem.Find( hm => hm == a_Str) != null ) return true; 

            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   28.01.2025
        ***************************************************************************/
        public bool AddTextEntry()
        {
            m_HlpText = Text;
            if (AlreadyIn(Text)) return false; 

            Items.Add(Text);
            m_HistMem.Add(Text);
            m_sOldText = Text;

            SelectedIndex = Items.IndexOf(Text);                

            if (m_iMaxNrItems != -1)
            {
                // all entries over m_iMaxNrItems are discarded
                while(Items.Count > m_iMaxNrItems) 
                {
                    Items.RemoveAt(0);
                    m_HistMem.RemoveAt(0);
                }
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.11.2019
        LAST CHANGE:   04.05.2020
        ***************************************************************************/
        public void DiscardDoubles()
        {
            if (Items.Count > m_HistMem.Count) SyncHist();

            List<string> hlp = new List<string>();
            Items.Clear();

            foreach ( string h in m_HistMem )
            {
                if ( hlp.Find( t => t == h ) != null ) continue;
                hlp.Add(h);
                Items.Add(h);
            }

            m_HistMem.Clear();
            m_HistMem.AddRange(hlp);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.05.2020
        LAST CHANGE:   04.05.2020
        ***************************************************************************/
        public void SyncHist()
        {
            m_HistMem.Clear();
            foreach ( string itm in Items ) m_HistMem.Add(itm);
        }

        /***************************************************************************
        SPECIFICATION: Workaround for a combo update issue
        CREATED:       07.04.2018
        LAST CHANGE:   07.04.2018
        ***************************************************************************/
        public void Restore()
        {
            if (m_HlpText == "") return;
            if (Text != m_HlpText)
            {
                Text = m_HlpText;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   07.11.2019
        ***************************************************************************/
        private void Combo_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            InitKeys();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   06.02.2023
        ***************************************************************************/
        private void Combo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt)              m_bAltPressed   = true;
            if (e.Control)          m_bCtrlPressed  = true;
            if (e.KeyValue == 0x46) m_bFPressed     = true;
            
            if (m_bReadOnly) return;

            if (e.KeyValue == 0x44) m_bDPressed     = true;
            if (e.Shift)            m_bShiftPressed = true;
            
            m_sOldText = this.Text;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.09.2015
        LAST CHANGE:   01.04.2022
        ***************************************************************************/
        private void Combo_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ( m_bReadOnly )
            {
                if ( m_bCtrlPressed && m_bAltPressed && m_bDPressed ) return;
                if ( m_bCtrlPressed && m_bAltPressed )
                {
                    if ( ! m_bFPressed ) return;
                }
                e.Handled = true;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.10.2006
        LAST CHANGE:   20.01.2022
        ***************************************************************************/
        protected void Combo_Click(object sender, System.EventArgs e)
        {
            ExecTips("");
            InitKeys();
        }
        
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.01.2022
        LAST CHANGE:   20.01.2022
        ***************************************************************************/
        protected void ExecTips( string a_ToolTp )
        {
            if      (m_bCtrlPressed && m_bAltPressed  && m_bDPressed ) DelItems();
            else if (m_bCtrlPressed && m_bAltPressed  && m_bFPressed ) EditFilter( a_ToolTp );
            else if (m_bCtrlPressed && m_bAltPressed                 ) EditItems();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.01.2022
        LAST CHANGE:   20.01.2022
        ***************************************************************************/
        protected void InitKeys()
        {
            m_bAltPressed   = false;
            m_bCtrlPressed  = false;
            m_bShiftPressed = false;
            m_bDPressed     = false;
            m_bFPressed     = false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       ??.??.2004
        LAST CHANGE:   28.11.2024
        ***************************************************************************/
        protected override void OnSelectedIndexChanged( System.EventArgs e )
        {
            if (m_bCtrlPressed)
            {
                m_bCtrlPressed = false;
                if (m_bAltPressed)
                {
                    DialogResult r = MessgeBox.Show("Delete All ? (Yes)\nEdit list ? (No)\nAbort ? (Cancel)","Warning",MessageBoxButtons.YesNoCancel);
                    switch(r)
                    {
                        case DialogResult.OK:
                            this.Items.Clear();
                            break;

                        case DialogResult.Cancel:
                            EditItems();
                            break;
                    }
                }
                else
                {
                    int idx  = SelectedIndex;
                    string txt = (string)Items[idx];

                    DialogResult r = MessgeBox.Show("Delete \"" + txt + "\" ?","Warning",MessageBoxButtons.YesNo);
                    if (r == DialogResult.OK)
                    {
                        Items.RemoveAt(idx);
                    }
                }

                this.Text = m_sOldText;
                //Debug.Print( "Text = " + m_sOldText );
            }
            else base.OnSelectedIndexChanged(e);
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.11.2022
        LAST CHANGE:   18.11.2022
        ***************************************************************************/
        private void Combo_ClientSizeChanged(object sender, System.EventArgs e)
        {
            int sc = Utils.GetWindowsScaling();

            switch( sc )
            {
                case 100: MaxDropDownItems = 70; break;  
                case 125: MaxDropDownItems = 60; break;  
                case 150: MaxDropDownItems = 50; break;  
                case 175: MaxDropDownItems = 40; break;  
            }
        }

        

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.12.2007
        LAST CHANGE:   21.11.2024
        ***************************************************************************/
        private void EditItems()
        {
            if ( m_bReadOnly ) return;

            m_TextEditor.GetComboItems();
            if ( DialogResult.OK == m_TextEditor.ShowDialog() )
            {
                this.Clear();
                m_HistMem.Clear();
                foreach ( string line in m_TextEditor.m_aOutput )
                {
                    if ( "" != line )
                    {
                        Items.Add(line);
                        m_HistMem.Add(line);
                    }
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.01.2022
        LAST CHANGE:   21.11.2024
        ***************************************************************************/
        private void DelItems()
        {
            if (m_bReadOnly) return;

            if ( MessageBox.Show("Discard entry history ?","Warning",MessageBoxButtons.YesNo) == DialogResult.No ) return;

            Items.Clear();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.06.2023
        LAST CHANGE:   05.06.2023
        ***************************************************************************/
        private void ColorizeFilt()
        {
            if ( m_CmbFilter.HasFilt ) BackColor = Color.LightBlue;
            else                       BackColor = Color.White;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.11.2019
        LAST CHANGE:   06.02.2023
        ***************************************************************************/
        protected void EditFilter( string s_ToolTp )
        {
            m_CmbFilter.ShowDialog();      
            FilterHist();
            AddTTFilter( s_ToolTp );
            ColorizeFilt();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.11.2019
        LAST CHANGE:   29.11.2024
        ***************************************************************************/
        public void FilterHist()
        {
            SyncHist();

            string filt = m_CmbFilter.Filters;
            bool   inv  = m_CmbFilter.Inverse;
            bool   rex  = m_CmbFilter.RegEx;
            List<string> segs = Utils.SplitExt( filt, " ,;" );
            if ( rex )
            {
                segs.Clear();
                segs.Add( filt );
            }

            if (segs.Count == 0)
            {
                Items.Clear();
                foreach( string s in m_HistMem ) Items.Add(s);
                return;
            }

            Items.Clear();

            if (inv)
            {
                foreach ( string s in segs )
                {
                    List<string> fnd = m_HistMem.FindAll( hm => rex ? ! Utils.RexMatch( hm, filt ) : ! hm.Contains( s ) );
                    foreach ( string f in fnd ) Items.Add(f); 
                }
            }
            else
            {
                foreach ( string s in segs )
                {
                    List<string> fnd = m_HistMem.FindAll( hm => rex ? Utils.RexMatch( hm, filt ) : hm.Contains( s ) );
                    foreach ( string f in fnd ) Items.Add(f); 
                }
            }

            ColorizeFilt();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       2004
        LAST CHANGE:   2004
        ***************************************************************************/
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.11.2019
        LAST CHANGE:   15.11.2019
        ***************************************************************************/
        public void Clear()
        {
            Items.Clear();
            m_HistMem.Clear();
        }
    } // class


    /***************************************************************************
    SPECIFICATION: Combo box with centered Text and Items
    CREATED:       19.11.2024
    LAST CHANGE:   19.11.2024
    ***************************************************************************/
    public class UserComboCenter:UserComboBox
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       19.11.2024
        LAST CHANGE:   19.11.2024
        ***************************************************************************/
        [DllImport( "user32.dll" )]
        static extern int GetWindowLong( IntPtr hWnd, int nIndex );
        [DllImport( "user32.dll" )]
        static extern int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong );
        const int GWL_STYLE = -16;
        const int ES_LEFT   = 0x0000;
        const int ES_CENTER = 0x0001;
        const int ES_RIGHT  = 0x0002;
        [StructLayout( LayoutKind.Sequential )]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public int Width { get { return Right - Left; } }
            public int Height { get { return Bottom - Top; } }
        }
        [DllImport( "user32.dll" )]
        public static extern bool GetComboBoxInfo( IntPtr hWnd, ref COMBOBOXINFO pcbi );

        [StructLayout( LayoutKind.Sequential )]
        public struct COMBOBOXINFO
        {
            public int    cbSize;
            public RECT   rcItem;
            public RECT   rcButton;
            public int    stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       19.11.2024
        LAST CHANGE:   19.11.2024
        ***************************************************************************/
        public UserComboCenter()
            :base()
        {
            DropDown       += new EventHandler( Combo_DropDown );
            DropDownClosed += new EventHandler( Combo_DropDownClosed );

            DrawMode = DrawMode.OwnerDrawFixed;
        }


        /***************************************************************************
        SPECIFICATION: This fixes following ComboBox error: Text is assigned a wrong 
                       value, if the previous Text value is a substring of an Item.
        CREATED:       19.11.2024
        LAST CHANGE:   19.11.2024
        ***************************************************************************/
        private string m_Txt;
        protected void Combo_DropDown(object sender, System.EventArgs e)
        {
            m_Txt = Text;
        }

        protected void Combo_DropDownClosed(object sender, System.EventArgs e)
        {
            Text = m_Txt;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2024
        LAST CHANGE:   19.11.2024
        ***************************************************************************/
        protected override void OnHandleCreated( EventArgs e )
        {
            base.OnHandleCreated( e );
            SetupEdit();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2024
        LAST CHANGE:   19.11.2024
        ***************************************************************************/
        private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
        private void SetupEdit()
        {
            var info = new COMBOBOXINFO();
            info.cbSize = Marshal.SizeOf( info );
            GetComboBoxInfo( this.Handle, ref info );
            var style = GetWindowLong( info.hwndEdit, GWL_STYLE );
            style |= 1;
            SetWindowLong( info.hwndEdit, GWL_STYLE, style );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2024
        LAST CHANGE:   19.11.2024
        ***************************************************************************/
        protected override void OnDrawItem( DrawItemEventArgs e )
        {
            base.OnDrawItem( e );
            e.DrawBackground();
            var txt = "";

            if( e.Index >= 0 )  txt = GetItemText( Items[e.Index] );

            TextRenderer.DrawText( e.Graphics, txt, Font, e.Bounds, ForeColor, TextFormatFlags.Left | TextFormatFlags.HorizontalCenter );
        }
    } // class

} // namespace
