using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using NS_AppConfig;
using NS_WordExcel;
using NS_Utilities;
using NS_UserList;
using System.Security.Policy;

namespace NS_UserOut
{
    /***************************************************************************
    SPECIFICATION: Global types
    CREATED:       03.02.2016
    LAST CHANGE:   22.05.2017
    ***************************************************************************/
    public delegate void dl_Close ();
    public delegate void dl_Filter();

    public enum FilterOpt
    {
        FO_HIDELINESWOK,
        FO_HIDELINESWK,
        FO_MARKLINES,
        FO_MARKITEMS,
        FO_NROF
    }

    public partial class UserOutList:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       02.09.2015
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public UserListView    ListCtrl      { get { return userListViewOutp; } } 
        public WordExcelExport WdXlExport    { get { return m_WdXlExport    ; } } 
        public bool            bVisible      { set { m_Visible = value; } }
        public List<string>    ColumnHeaders { get { return m_ColumnHeaders; } }
        public List<XLColumn>  Columns       { get { return m_Columns; } } 
        public bool            AtoHArrange   { get { return autoHArrangeByOpenToolStripMenuItem.Checked; } set { autoHArrangeByOpenToolStripMenuItem.Checked = value; } }
        public string          XlExpFilNm    { set { m_XlExpFilNm = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       31.03.2015
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public event dl_Close  m_eClose;
        public event dl_Filter m_eFilter;

        protected List<ListViewItem>  m_FiltItems;
        protected FilterOpt           m_FilterOpt;
        protected List<string>        m_ColumnHeaders;
        protected List<XLColumn>      m_Columns;
        private   PreferencesUOList   m_Prefs;
        private   WordExcelExport     m_WdXlExport;
        private   bool                m_Visible;
        private   UserTimer           m_FiltInputTmr;
        private   string              m_XlExpFilNm;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       31.03.2015
        LAST CHANGE:   22.05.2025
        ***************************************************************************/
        public UserOutList(  )
        {
            InitializeComponent();
            Utils.InitDialog( this );
            Utils.InitCtrl( menuStripBase );
            Utils.InitDlgSzLoc( this );

            m_FiltItems     = new List<ListViewItem>();
            m_Prefs         = new PreferencesUOList();
            m_WdXlExport    = new WordExcelExport( userListViewOutp );
            m_ColumnHeaders = new List<string>(); 
            m_FilterOpt     = FilterOpt.FO_HIDELINESWOK;
            m_Visible       = true;
            m_FiltInputTmr  = new UserTimer( "FilterInpTmr", 500 );
            m_XlExpFilNm    = "DefaultName";

            userListViewOutp.Font = m_Prefs.TextFont;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.07.2018
        LAST CHANGE:   30.07.2018
        ***************************************************************************/
        private void ShowNrLines()
        {
            textBoxNrLines.Text = string.Format( "{0}", userListViewOutp.Items.Count );
        }

        /***************************************************************************
        SPECIFICATION: Colorize table
        CREATED:       25.06.2015
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        protected void ColorizeTab()
        {
            userListViewOutp.BackColor = m_Prefs.ColBack1.color;

            foreach( ListViewItem it in userListViewOutp.Items )
            {
                int ci = 0;

                it.UseItemStyleForSubItems = false;

                foreach( ListViewItem.ListViewSubItem sit in it.SubItems )
                {
                    if( ci++ % 2 == 0 )
                    {
                        sit.ForeColor   = m_Prefs.ColFore2.color;
                        sit.Font        = m_Prefs.ColFore2.GetFont( m_Prefs.TextFont );    
                        sit.BackColor   = m_Prefs.ColBack2.color;
                    }
                    else
                    {
                        sit.ForeColor = m_Prefs.ColFore1.color;
                        sit.Font      = m_Prefs.ColFore1.GetFont( m_Prefs.TextFont );    
                        sit.BackColor = m_Prefs.ColBack1.color;
                    }
                }
            }

            //userListViewOutp.Update();
            userListViewOutp.Show();
            ShowNrLines();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.03.2023
        LAST CHANGE:   03.06.2024
        ***************************************************************************/
        private void UserOutList_Load( object sender, EventArgs e )
        {
            userListViewOutp.Update();
            //m_FiltInputTmr  = new UserTimer( "FilterInpTmr", 300 );
            m_FiltInputTmr.m_eExpiredHandler += FiltInpTmrHandler;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.04.2015
        LAST CHANGE:   03.06.2024
        ***************************************************************************/
        protected void UserOutList_FormClosing( object sender, FormClosingEventArgs e )
        {
            userComboBoxFilter.AddTextEntry();
            //m_FiltInputTmr.m_eExpiredHandler -= FiltInpTmrHandler;
            e.Cancel = true;
            this.Hide();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.04.2015
        LAST CHANGE:   18.11.2024
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                a_Conf.DeserializeDialog( this );
                autoscrollDownToolStripMenuItem.Checked     = a_Conf.Deserialize<bool>();
                m_FilterOpt                                 = a_Conf.Deserialize<FilterOpt>();
                autoCArrangeByOpenToolStripMenuItem.Checked = a_Conf.Deserialize<bool>();
                autoHArrangeByOpenToolStripMenuItem.Checked = a_Conf.Deserialize<bool>();
                regularExpressionsToolStripMenuItem.Checked = a_Conf.Deserialize<bool>();
            }
            else
            {
                a_Conf.SerializeDialog  ( this );
                a_Conf.Serialize( autoscrollDownToolStripMenuItem.Checked );
                a_Conf.Serialize( m_FilterOpt );
                a_Conf.Serialize( autoCArrangeByOpenToolStripMenuItem.Checked );
                a_Conf.Serialize( autoHArrangeByOpenToolStripMenuItem.Checked );
                a_Conf.Serialize( regularExpressionsToolStripMenuItem.Checked );
            }

            userListViewOutp  .Serialize( ref a_Conf );
            userComboBoxFilter.Serialize( ref a_Conf );
            m_Prefs           .Serialize( ref a_Conf );

            if ( a_Conf.IsReading )
            {
                ColorizeTab();
                CheckFilter( m_FilterOpt );
                userListViewOutp.Font = m_Prefs.TextFont;
                userListViewOutp.AddColumnAttribs();
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.04.2015
        LAST CHANGE:   02.04.2015
        ***************************************************************************/
        protected void clearToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Clear();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.06.2024
        LAST CHANGE:   26.04.2025
        ***************************************************************************/
        private void FiltInpTmrHandler( int Time )
        {
            Filter();
        }
        

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.04.2015
        LAST CHANGE:   22.05.2025
        ***************************************************************************/
        private delegate void dl_Clear();
        public void Clear()
        {
            if (this.InvokeRequired)
            {
                dl_Clear d = new dl_Clear( Clear );
                this.Invoke( d, new object[]{} );
            }
            else
            {
                m_ColumnHeaders.Clear();
                userListViewOutp.Items.Clear();
                m_FiltItems.Clear();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.04.2015
        LAST CHANGE:   17.04.2015
        ***************************************************************************/
        protected void userListViewOutp_SelectedIndexChanged( object sender, EventArgs e )
        {
            userComboBoxFilter.AddTextEntry();
        }

        
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.07.2015
        LAST CHANGE:   13.07.2023
        ***************************************************************************/
        protected void columnsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            userListViewOutp.AutoResizeColumns( ColumnHeaderAutoResizeStyle.ColumnContent );
            userListViewOutp.StoreColumnAttibs();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.07.2015
        LAST CHANGE:   13.07.2023
        ***************************************************************************/
        protected void headersToolStripMenuItem_Click( object sender, EventArgs e )
        {
            userListViewOutp.AutoResizeColumns( ColumnHeaderAutoResizeStyle.HeaderSize );
            userListViewOutp.StoreColumnAttibs();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   11.04.2016
        ***************************************************************************/
        private void RestoreLines()
        {
            userListViewOutp.Items.Clear();

            userListViewOutp.Items.AddRange( m_FiltItems.ToArray() );

            ColorizeTab();        
        }
        
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   03.06.2024
        ***************************************************************************/
        protected delegate List<string> dl_GetFilterWords();
        protected List<string> GetFilterWords()
        {
            if (this.InvokeRequired)
            {
                dl_GetFilterWords d = new dl_GetFilterWords(GetFilterWords);
                return (List<string>)this.Invoke( d, new object[]{} );
            }
            else
            {
                string filter = userComboBoxFilter.Text.ToLower();
                bool   wd     = wordDisjunctionToolStripMenuItem.Checked;

                List<string> ret = null;

                if( wd )
                {
                    ret = Utils.SplitExt( filter, " " );
                }
                else 
                { 
                    ret = new List<string>(); 
                    if (filter != "") ret.Add( filter ); 
                }
                return ret;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.08.2015
        LAST CHANGE:   03.06.2024
        ***************************************************************************/
        protected delegate void dl_FilterLinesWok();
        protected void FilterLinesWok()
        {
            if (this.InvokeRequired)
            {
                dl_FilterLinesWok d = new dl_FilterLinesWok(FilterLinesWok);
                this.Invoke( d, new object[]{} );
            }
            else
            {

                List<string> filts = GetFilterWords();

                userListViewOutp.Items.Clear();

                if (filts.Count == 0) 
                {
                    foreach ( ListViewItem lvi in m_FiltItems ) userListViewOutp.Items.Add( lvi );
                    return;
                }

                List<int> FiltIdxs = new List<int>();
                for (int i=0; i<m_FiltItems.Count; i++) FiltIdxs.Add(i);


                foreach( string filt in filts )
                {
                    if (FiltIdxs.Count == 0) break;

                    int j=-1;
                    foreach( ListViewItem it in m_FiltItems )
                    {
                        j++;
                        int f = FiltIdxs.Find( idx => idx == j );

                        if ( f != j  ) continue;
                        if ( j==0 && FiltIdxs[0] != 0 ) continue;

                        try
                        {
                            if( Utils.RexMatch( it.Text.ToLower(), filt ) )
                            {
                                userListViewOutp.Items.Add(it);
                                FiltIdxs.Remove(j);
                                continue;
                            }

                            foreach( ListViewItem.ListViewSubItem si in it.SubItems )
                            {
                                if( Utils.RexMatch ( si.Text.ToLower(), filt ) )
                                {
                                    userListViewOutp.Items.Add(it);
                                    FiltIdxs.Remove(j);
                                    break;
                                }
                            }
                        }
                        catch( InvalidExpressionException )
                        {
                            return;
                        }
                    }
                }

                ColorizeTab();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2017
        LAST CHANGE:   01.12.2020
        ***************************************************************************/
        protected void FilterLinesWk()
        {
            List<string> filts = GetFilterWords();
            List<string> help  = new List<string>();

            userListViewOutp.Items.Clear();


            if (filts.Count == 0) 
            {
                foreach ( ListViewItem lvi in m_FiltItems ) userListViewOutp.Items.Add( lvi );
                return;
            }

            foreach( string filt in filts )
            {
                foreach( ListViewItem it in m_FiltItems )
                {
                    ListViewItem[] lvi = userListViewOutp.Items.Find( it.Text.ToLower(), false );
                    if ( lvi.Length > 0 ) continue;

                    bool found = false;

                    if( it.Text.ToLower().Contains( filt ) )
                    {
                        continue;
                    }

                    try
                    {
                        foreach( ListViewItem.ListViewSubItem si in it.SubItems )
                        {
                            if( Utils.RexMatch( si.Text.ToLower(), filt ) )
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    catch( InvalidExpressionException )
                    {
                        continue;
                    }

                    if (found) continue;

                    if ( help.Find(i => i == it.Text) != null ) continue;

                    help.Add(it.Text);
                    userListViewOutp.Items.Add(it);
                } 
            }

            ColorizeTab();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   11.01.2022
        ***************************************************************************/
        protected void MarkItems()
        {
            List<string> filts = GetFilterWords();
            m_Prefs.GetMarkerColors();

            ColorizeTab();

            foreach( string filt in filts )
            {
                Color col = m_Prefs.NextMrkCol.color;

                try
                {
                    foreach( ListViewItem it in userListViewOutp.Items )
                    {
                        if ( Utils.RexMatch( it.Text, filt ) )
                        {
                            it.BackColor = col;
                        }

                        foreach( ListViewItem.ListViewSubItem si in it.SubItems )
                        {
                            if( Utils.RexMatch( si.Text.ToLower(), filt ) )
                            {
                                si.BackColor = col;
                            }
                        }
                    }
                }
                catch( InvalidExpressionException )
                {
                    continue;
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   11.01.2022
        ***************************************************************************/
        protected void MarkLines()
        {
            m_Prefs.GetMarkerColors();
            List<string> filts = GetFilterWords(); 

            ColorizeTab();

            foreach( string filt in filts )
            {
                try
                {
                    Color col = m_Prefs.NextMrkCol.color;

                    foreach( ListViewItem it in userListViewOutp.Items )
                    {
                        //if( it.Text.Contains( filt ) )
                        if ( Utils.RexMatch( it.Text, filt ) )
                        {
                            it.BackColor = col;
                            foreach( ListViewItem.ListViewSubItem si in it.SubItems ) si.BackColor = col;
                            continue;
                        }

                        foreach( ListViewItem.ListViewSubItem si in it.SubItems )
                        {
                            //if( si.Text.ToLower().Contains( filt ) )
                            if( Utils.RexMatch( si.Text.ToLower(), filt ) )
                            {
                                it.BackColor = col;
                                foreach( ListViewItem.ListViewSubItem hi in it.SubItems ) hi.BackColor = col;
                                continue;
                            }
                        }
                    }
                }
                catch ( InvalidExpressionException )
                {
                    continue;
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.08.2015
        LAST CHANGE:   03.06.2024
        ***************************************************************************/
        protected void userComboBoxFilter_TextChanged( object sender, EventArgs e )
        {
            m_FiltInputTmr.Restart();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   26.04.2025
        ***************************************************************************/
        protected delegate void dl_Fltr();
        protected void Filter()
        {
            if (this.InvokeRequired)
            {
                dl_Fltr d = new dl_Fltr(Filter);
                this.Invoke( d, new object[]{} );
            }
            else
            {
                switch( m_FilterOpt )
                {
                    case FilterOpt.FO_HIDELINESWOK:  FilterLinesWok();  break;
                    case FilterOpt.FO_HIDELINESWK:   FilterLinesWk ();  break;
                    case FilterOpt.FO_MARKLINES:     MarkLines     ();  break;
                    case FilterOpt.FO_MARKITEMS:     MarkItems     ();  break;
                }

                userListViewOutp.Update();
                ShowNrLines();

                if (m_eFilter != null) m_eFilter();

                userComboBoxFilter.SelectionStart = userComboBoxFilter.Text.Length;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.09.2015
        LAST CHANGE:   06.06.2016
        ***************************************************************************/
        public void ShowColumns() { ShowColumns(null); }

        public void ShowColumns( List<String> a_Columns )
        {
            if ( a_Columns != null ) m_ColumnHeaders = a_Columns;

            bool equal = true;
            int  i     = 0;

            if (m_ColumnHeaders.Count != userListViewOutp.Columns.Count ) equal = false;
            else
            {
                foreach( string col in m_ColumnHeaders )
                {
                    if( col != userListViewOutp.Columns[i++].Text )
                    {
                        equal = false;
                    }
                }
            }

            if( !equal )
            {
                //userListViewOutp.Font = new Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                userListViewOutp.Columns.Clear();

                foreach( string col in m_ColumnHeaders )
                {
                    userListViewOutp.Columns.Add( col.Trim() );
                }
                userListViewOutp.View = View.Details;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.06.2016
        LAST CHANGE:   06.06.2016
        ***************************************************************************/
        public void ShowColumnsExt( List<XLColumn> a_Columns )
        {
            bool equal = true;
            int i = 0;

            m_Columns = a_Columns;

            if( m_Columns.Count != userListViewOutp.Columns.Count ) equal = false;
            else
            {
                foreach( XLColumn col in m_Columns )
                {
                    if( col.Text != userListViewOutp.Columns[i++].Text )
                    {
                        equal = false;
                    }
                }
            }

            if( !equal )
            {
                userListViewOutp.Columns.Clear();

                foreach( XLColumn col in m_Columns )
                {
                    HorizontalAlignment algn = HorizontalAlignment.Center;
                    switch(col.Align)
                    {
                        case 'l': algn = HorizontalAlignment.Left;   break;
                        case 'c': algn = HorizontalAlignment.Center; break;
                        case 'r': algn = HorizontalAlignment.Right;  break;
                    }

                    string cl = col.Text.Trim();
                    userListViewOutp.Columns.Add( col.Text.Trim() );
                    userListViewOutp.Columns[userListViewOutp.Columns.Count-1].TextAlign = algn;
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.09.2015
        LAST CHANGE:   19.11.2015
        ***************************************************************************/
        public void AddLine( List<string> a_Elems ) { AddLine( a_Elems.ToArray() ); }

        private delegate void dl_AddLine( string[] a_Elems );
        public void AddLine( string[] a_Elems )
        {
            if (this.InvokeRequired)
            {
                dl_AddLine d = new dl_AddLine(AddLine);
                this.Invoke( d, new object[]{ a_Elems } );
            }
            else
            {
                ListViewItem it = new ListViewItem();

                int i=0;
                foreach( string cll in a_Elems )
                {
                    if (i++ == 0) it.Text = cll;
                    else
                    {
                        it.SubItems.Add( cll );
                    }
                }

                userListViewOutp.Items.Add( it );
                m_FiltItems           .Add( it );
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.09.2015
        LAST CHANGE:   26.06.2024
        ***************************************************************************/
        private delegate void dl_Refresh();
        public void Refresh()
        {
            if (this.InvokeRequired)
            {
                dl_Refresh d = new dl_Refresh(Refresh);
                this.Invoke( d, new object[]{} );
            }
            else
            {
                userListViewOutp.View = View.Details;

                ColorizeTab();
                Filter();

                if ( autoCArrangeByOpenToolStripMenuItem.Checked || autoHArrangeByOpenToolStripMenuItem.Checked )
                {
                    userListViewOutp.Hide();
                    if( autoCArrangeByOpenToolStripMenuItem.Checked ) userListViewOutp.AutoResizeColumns( ColumnHeaderAutoResizeStyle.ColumnContent );
                    if( autoHArrangeByOpenToolStripMenuItem.Checked ) userListViewOutp.AutoResizeColumns( ColumnHeaderAutoResizeStyle.HeaderSize );
                    //userListViewOutp.Update();
                    userListViewOutp.Show();
                }
                BringToFront();
                Show();
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.04.2016
        LAST CHANGE:   06.04.2016
        ***************************************************************************/
        private delegate void dl_HideExt();
        public void HideExt()
        {
            if (this.InvokeRequired)
            {
                dl_HideExt d = new dl_HideExt(HideExt);
                this.Invoke( d, new object[]{} );
            }
            else
            {
                Hide();
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.11.2015
        LAST CHANGE:   12.01.2016
        ***************************************************************************/
        private delegate void dl_ScrollDown();
        public void ScrollDown()
        {
            if (this.InvokeRequired)
            {
                dl_ScrollDown d = new dl_ScrollDown(ScrollDown);
                this.Invoke( d, new object[]{} );
            }
            else
            {
                if (userListViewOutp.Items.Count < 1) return;
                if (autoscrollDownToolStripMenuItem.Checked)  userListViewOutp.EnsureVisible(userListViewOutp.Items.Count - 1);
            }
        }
        

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.09.2015
        LAST CHANGE:   20.05.2018
        ***************************************************************************/
        private void export2XLToolStripMenuItem_Click(object sender,EventArgs e)
        {
            if (userListViewOutp.Items.Count <= 0) 
            {
                MessageBox.Show("Empty list !\nNothing to export !", "Note");
                return;
            }
            
            ShowInExcel();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.12.2018
        LAST CHANGE:   12.12.2018
        ***************************************************************************/
        private void exportAsCSVToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if (userListViewOutp.Items.Count <= 0) 
            {
                MessageBox.Show("Empty list !\nNothing to export !", "Note");
                return;
            }
            
            Export2CSV();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.05.2018
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public void ShowInExcel()
        {
            m_WdXlExport.XlExpFilNm = m_XlExpFilNm;
            m_WdXlExport.Show();
            m_WdXlExport.Update();
            m_WdXlExport.ShowInExcel();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.11.2016
        LAST CHANGE:   29.03.2017
        ***************************************************************************/
        public bool Save2XL( string a_File )
        {
            if (userListViewOutp.Items.Count <= 0) 
            {
                return false;
            }

            WordExcelExport dlg = new WordExcelExport( userListViewOutp );

            dlg.Show();
            dlg.Update();
            dlg.ShowInExcel( a_File );

            return ! dlg.IsAborted;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.12.2018
        LAST CHANGE:   25.02.2021
        ***************************************************************************/
        public bool Save2CSV( string a_File )
        {
            if (userListViewOutp.Items.Count <= 0) 
            {
                return false;
            }

            try
            {
                StreamWriter wrt = new StreamWriter( a_File );
                string line = "";

                // enter a header line at first 
                foreach ( ColumnHeader ch in userListViewOutp.Columns )
                {
                    line += ch.Text; 
                    line += ";";
                }
                line = line.Remove(line.Length-1,1);
                wrt.WriteLine(line);

                // insert a header line
                foreach( ListViewItem lvi in userListViewOutp.Items )
                {
                    line = "";
                    foreach ( ListViewItem.ListViewSubItem itm in lvi.SubItems )
                    {
                        line += itm.Text + ";";
                    }
                    if (line != "") 
                    {
                        line = line.Remove(line.Length-1);
                        wrt.WriteLine(line);
                    }
                }

                wrt.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Exception in Safe2CSV" );
                return false;
            }

            return true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.12.2018
        LAST CHANGE:   12.12.2018
        ***************************************************************************/
        public void Export2CSV()
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.AddExtension = true;
            dlg.DefaultExt   = "csv";
            dlg.Filter       = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            DialogResult res = dlg.ShowDialog();

            if (res == DialogResult.OK)
            {
                string fname = dlg.FileName;
                Save2CSV(fname);
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       06.09.2015
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        private void prefsToolStripMenuItem_Click(object sender,EventArgs e)
        {
            m_Prefs.ShowDialog();
            userListViewOutp.Font = m_Prefs.TextFont;
            ColorizeTab();
            Filter();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       23.10.2015
        LAST CHANGE:   23.10.2015
        ***************************************************************************/
        private void btnFilterErase_Click( object sender, EventArgs e )
        {
            userComboBoxFilter.Text = "";
            RestoreLines();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2015
        LAST CHANGE:   19.11.2015
        ***************************************************************************/
        private void autoscrollDownToolStripMenuItem_Click( object sender, EventArgs e )
        {
            ScrollDown();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   22.05.2017
        ***************************************************************************/
        private void CheckFilterSub( bool a_HideLinesWok, bool a_HideLinesWk, bool a_MarkLines, bool a_MarkItems )
        {
            hideLinesWithoutKeyToolStripMenuItem.Checked = a_HideLinesWok;
            hideLinesWithKeyToolStripMenuItem   .Checked = a_HideLinesWk;
            markLinesWithKeyToolStripMenuItem   .Checked = a_MarkLines;
            markItemsWithKeyToolStripMenuItem   .Checked = a_MarkItems;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   20.05.2025
        ***************************************************************************/
        private void CheckFilter( FilterOpt a_FiltOpt )
        {
            m_FilterOpt = a_FiltOpt;

            switch( a_FiltOpt )
            {
                case FilterOpt.FO_HIDELINESWOK: CheckFilterSub ( true ,false,false,false ); break;
                case FilterOpt.FO_HIDELINESWK : CheckFilterSub ( false,true ,false,false ); break;
                case FilterOpt.FO_MARKLINES   : CheckFilterSub ( false,false,true ,false ); break;
                case FilterOpt.FO_MARKITEMS   : CheckFilterSub ( false,false,false,true  ); break;
            }

            Filter();
            Cursor = Cursors.Default;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   20.05.2025
        ***************************************************************************/
        private void hideLinesWithoutKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Cursor = Cursors.WaitCursor;
            CheckFilter( FilterOpt.FO_HIDELINESWOK );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2017
        LAST CHANGE:   20.05.2025
        ***************************************************************************/
        private void hideLinesWithKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Cursor = Cursors.WaitCursor;
            CheckFilter( FilterOpt.FO_HIDELINESWK );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   20.05.2025
        ***************************************************************************/
        private void markLinesWithKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Cursor = Cursors.WaitCursor;
            RestoreLines();
            CheckFilter( FilterOpt.FO_MARKLINES );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2016
        LAST CHANGE:   20.05.2025
        ***************************************************************************/
        private void markItemsWithKeyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Cursor = Cursors.WaitCursor;
            RestoreLines();
            CheckFilter( FilterOpt.FO_MARKITEMS );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.02.2020
        LAST CHANGE:   04.02.2020
        ***************************************************************************/
        private void autoCArrangeByOpenToolStripMenuItem_Click(object sender,EventArgs e)
        {
            if (autoCArrangeByOpenToolStripMenuItem.Checked)
            {
                autoHArrangeByOpenToolStripMenuItem.Checked = false;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.02.2020
        LAST CHANGE:   04.02.2020
        ***************************************************************************/
        private void autoHArrangeByOpenToolStripMenuItem_Click(object sender,EventArgs e)
        {
            if (autoHArrangeByOpenToolStripMenuItem.Checked)
            {
                autoCArrangeByOpenToolStripMenuItem.Checked = false;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.01.2024
        LAST CHANGE:   30.01.2024
        ***************************************************************************/
        private void pictBoxFilt_Click( object sender, EventArgs e )
        {
            userComboBoxFilter.DroppedDown = true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.04.2024
        LAST CHANGE:   11.04.2024
        ***************************************************************************/
        private void UserOutList_VisibleChanged( object sender, EventArgs e )
        {
            if ( ! m_Visible ) Visible = false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       26.06.2025
        LAST CHANGE:   26.06.2025
        ***************************************************************************/
        private void setoptWidthToolStripMenuItem_Click( object sender, EventArgs e )
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
            int totalWidth = 0;
            using (Graphics g = userListViewOutp.CreateGraphics())
            {
                for (int col = 0; col < userListViewOutp.Columns.Count; col++)
                {
                    int maxColWidth = TextRenderer.MeasureText(userListViewOutp.Columns[col].Text, userListViewOutp.Font).Width;

                    foreach (ListViewItem item in userListViewOutp.Items)
                    {
                        string text = col == 0 ? item.Text : (col < item.SubItems.Count ? item.SubItems[col].Text : "");
                        int cellWidth = TextRenderer.MeasureText(text, userListViewOutp.Font).Width;
                        if (cellWidth > maxColWidth) maxColWidth = cellWidth;
                    }

                    maxColWidth += 8;
                    totalWidth += maxColWidth;
                }
            }

            Width = totalWidth + SystemInformation.VerticalScrollBarWidth + 20; 
        }
    } // class

    /***************************************************************************
    SPECIFICATION: Global types
    CREATED:       06.06.2016
    LAST CHANGE:   06.06.2016
    ***************************************************************************/
    public class XLColumn
    {
        public string Text;
        public char   Align;

        public XLColumn()
        {
            Text  = "";
            Align = 'l';
        }

        public XLColumn( string a_Text, char a_Align )
        {
            Text  = a_Text;
            Align = a_Align;
        }
    }

} // Namespace
