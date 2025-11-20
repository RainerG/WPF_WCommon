using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using NS_AppConfig;
using NS_Utilities;
using NS_UserColor;

namespace NS_UserOut
{
    /***************************************************************************
    SPECIFICATION: Preferences dialog for UserOutText
    CREATED:       27.09.2015
    LAST CHANGE:   27.09.2015
    ***************************************************************************/
    public partial class PreferencesUOText:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       27.09.2015
        LAST CHANGE:   25.03.2021
        ***************************************************************************/
        public string OutPath { get {return fileComboOutpath.Text; } set { fileComboOutpath.Text = value; } }
        public string Separs  { get {return GetSepars(); } }  

        public ColSelType ColHead    { get { return  userColSel.GetCol("head")   ; } }
        public ColSelType ColErr     { get { return  userColSel.GetCol("err")    ; } }
        public ColSelType Col1       { get { return  userColSel.GetCol("print1") ; } }
        public ColSelType Col2       { get { return  userColSel.GetCol("print2") ; } }
        public ColSelType Col3       { get { return  userColSel.GetCol("print3") ; } }
        public ColSelType Col4       { get { return  userColSel.GetCol("print4") ; } }
        public ColSelType Col5       { get { return  userColSel.GetCol("print5") ; } }
        public ColSelType ColMark    { get { return  userColSel.GetCol("marker1"); } }
        public ColSelType ColSepar   { get { return  userColSel.GetCol("separ")  ; } }
        public ColSelType NextMrkCol { get { return GetNextMarkerColor(); } }
        public bool  MarkLtrs   { get { return cbMarkLtrs.Checked; } }
        public bool  RegExpr    { get { return cbRegEx.Checked; } }
        public Font  TextFont   { get { return m_Font; } set { m_Font = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       28.04.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public Color MARKER_COL = Color.FromArgb( 255,255,102 );
        private List<ColSelType> m_MarkerCols;
        private List<ColSelType> m_DefaultCols;
        private int              m_MColIdx;
        private Font             m_Font;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       27.09.2015
        LAST CHANGE:   26.05.2021
        ***************************************************************************/
        public PreferencesUOText()
        {
            InitializeComponent();
            Utils.InitCtrl(userTabControl);

            userComboBoxPrefSepar.Text  = "\" ,;\"";
            OutPath                     = "d:\\temp";

            List<ColSelType> csl = userColSel.ColList;

            csl.Add( new ColSelType( "Headlines", Color.Black                    , false, false ) );
            csl.Add( new ColSelType( "Errors",    Color.Red                      , false, false ) );
            csl.Add( new ColSelType( "Print1",    Color.Blue                     , false, false ) );
            csl.Add( new ColSelType( "Print2",    Color.Green                    , false, false ) );
            csl.Add( new ColSelType( "Print3",    Color.FromArgb (0  ,  64, 128 ), false, false ) );
            csl.Add( new ColSelType( "Print4",    Color.FromArgb (0  , 128, 128 ), false, false ) );
            csl.Add( new ColSelType( "Print5",    Color.FromArgb (255, 128,  64 ), false, false ) );
            csl.Add( new ColSelType( "Separator", Color.DarkGray                 , false, false ) );
            csl.Add( new ColSelType( "Marker1",   Color.FromArgb (255, 255, 102 ), false, false, false ) );
            csl.Add( new ColSelType( "Marker2",   Color.FromArgb (255, 147, 255 ), false, false, false ) );
            csl.Add( new ColSelType( "Marker3",   Color.FromArgb (138, 255, 138 ), false, false, false ) );

            m_DefaultCols = new List<ColSelType>();
            foreach( ColSelType cs in csl )
            {
                m_DefaultCols.Add( new ColSelType( cs ) );
            }

            Size sz = userColSel.BuildDialog();

            int ht = sz.Height + 100;
            this.MaximumSize = new Size ( MaximumSize.Width, ht );
            this.MinimumSize = new Size ( MinimumSize.Width, ht );

            m_MarkerCols = new List<ColSelType>();
            GetMarkerColors();

            m_Font = new Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.04.2017
        LAST CHANGE:   02.11.2020
        ***************************************************************************/
        public void SetDefault( )
        {
            List<ColSelType> cols = userColSel.ColList;

            int ix = 0;
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );
            cols[ix].Copy ( m_DefaultCols[ix++] );

            userColSel.SetColors( cols );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public void GetMarkerColors()
        {
            m_MarkerCols.Clear();
            m_MColIdx = 0;

            m_MarkerCols.Add( userColSel.GetCol("ker1") );
            m_MarkerCols.Add( userColSel.GetCol("ker2") );
            m_MarkerCols.Add( userColSel.GetCol("ker3") );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public ColSelType GetNextMarkerColor()
        {
            ColSelType ret = m_MarkerCols[m_MColIdx++];
            if (m_MColIdx >= 3) m_MColIdx = 0;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   22.05.2016
        ***************************************************************************/
        public ColSelType GetCol( string a_ColSel )
        {
            return userColSel.GetCol( a_ColSel );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2015
        LAST CHANGE:   19.11.2015
        ***************************************************************************/
        private string GetSepars()
        {
            string ret = userComboBoxPrefSepar.Text;
            ret = ret.Replace("\"","");
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.04.2016
        LAST CHANGE:   18.11.2024
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                Size sz = this.Size;
                a_Conf.DeserializeDialog( this );
                Size szn = new Size( this.Width, sz.Height );
                this.Size = szn;

                cbMarkLtrs.Checked = a_Conf.Deserialize<bool>();
                cbRegEx   .Checked = a_Conf.Deserialize<bool>();
                m_Font             = a_Conf.Deserialize<Font>();
            }
            else
            {
                a_Conf.SerializeDialog  ( this );
                a_Conf.Serialize( cbMarkLtrs.Checked );
                a_Conf.Serialize( cbRegEx   .Checked );
                a_Conf.Serialize( m_Font );
            }

            fileComboOutpath     .Serialize( ref a_Conf );
            userComboBoxPrefSepar.Serialize( ref a_Conf );
            userColSel           .Serialize( ref a_Conf );

            if ( a_Conf.IsReading )
            {
                ShowMrkBoxes();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.04.2016
        LAST CHANGE:   05.07.2019
        ***************************************************************************/
        private void btnOutPthBrowse_Click( object sender, EventArgs e )
        {
            fileComboOutpath.BrowseFolder();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   22.05.2016
        ***************************************************************************/
        private void PreferencesUOText_FormClosing( object sender, FormClosingEventArgs e )
        {
            userColSel.OnFormClosing();
            GetNextMarkerColor();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.04.2017
        LAST CHANGE:   05.04.2017
        ***************************************************************************/
        private void btnFactDflt_Click( object sender, EventArgs e )
        {
            SetDefault();        
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.11.2020
        LAST CHANGE:   18.11.2020
        ***************************************************************************/
        private void linkLabelRexHlp_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            Process.Start("https://regex101.com");
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void cbMarkLtrs_CheckedChanged( object sender, EventArgs e )
        {
            ShowMrkBoxes();
        }

        private void ShowMrkBoxes()
        {
            bool chk = cbMarkLtrs.Checked;
            userColSel.ShowMarkerBoxes( chk );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.03.2021
        LAST CHANGE:   25.03.2021
        ***************************************************************************/
        private void btnFont_Click( object sender, EventArgs e )
        {
            fontDialog.Font = m_Font;
            fontDialog.ShowDialog();
            m_Font = fontDialog.Font;
        }
    } // class

} // namespace
 