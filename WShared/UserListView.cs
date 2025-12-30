using System;
using System.Collections.Generic;
using System.Text;
using NS_AppConfig;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

using NS_WUtilities;

namespace NS_UserList
{
    /***************************************************************************
    SPECIFICATION: UserListView class definition
    CREATED:       05.03.2007
    LAST CHANGE:   05.03.2007
    ***************************************************************************/
    public class UserListView : System.Windows.Forms.ListView
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       05.06.2016
        LAST CHANGE:   03.03.2021
        ***************************************************************************/
        public bool         CtrlPressed { get { return m_CtrlPressed; } }
        public List<ColMem> ColsMem     { get { return m_ColsMem; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       05.03.2007
        LAST CHANGE:   03.03.2021
        ***************************************************************************/
        private const int WM_ERASEBKGND = 0x14;

        protected bool          m_bSortAscending;
        private   List<ColMem>  m_ColsMem;
        private   bool          m_CtrlPressed;
        private   string        m_Name;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       05.03.2007
        LAST CHANGE:   03.03.2021
        ***************************************************************************/
        public UserListView()
          :base()
        {
            m_Name = Name;

            this.SetStyle( ControlStyles.EnableNotifyMessage, true );

            m_bSortAscending = true;
            ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
            MouseUp     += new MouseEventHandler      (listView_MouseUp);
            KeyDown     += new KeyEventHandler        (listView_KeyDown);
            KeyUp       += new KeyEventHandler        (listView_KeyUp);

            m_CtrlPressed = false;

            m_ColsMem = new List<ColMem>();
        }
        
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.06.2016
        LAST CHANGE:   09.06.2016
        ***************************************************************************/
        private void listView_KeyDown( object sender, KeyEventArgs e ) { if (e.Control) m_CtrlPressed = true ;  }
        private void listView_KeyUp  ( object sender, KeyEventArgs e ) { if (e.Control) m_CtrlPressed = false;  }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.06.2016
        LAST CHANGE:   05.06.2016
        ***************************************************************************/
        void listView_MouseUp( object sender, MouseEventArgs e )
        {
            if (! m_CtrlPressed) return;

            m_CtrlPressed = false;

            if( e.Button == MouseButtons.Right )
            {
                ListView lv = sender as ListView;
                if (lv == null) return;

                Point mousePosition = lv.PointToClient(Control.MousePosition);
                ListViewHitTestInfo hit = lv.HitTest(mousePosition);
                int idx = hit.Item.SubItems.IndexOf(hit.SubItem);

                switch( Columns[idx].TextAlign )
                {
                    case HorizontalAlignment.Left:   Columns[idx].TextAlign = HorizontalAlignment.Center; break;
                    case HorizontalAlignment.Center: Columns[idx].TextAlign = HorizontalAlignment.Right;  break;
                    case HorizontalAlignment.Right:  Columns[idx].TextAlign = HorizontalAlignment.Left;   break;
                }
            }
        }

        /***************************************************************************
        SPECIFICATION: Serialization of column header's text and width
        CREATED:       05.03.2007
        LAST CHANGE:   05.03.2021
        ***************************************************************************/
        public void Serialize(ref AppSettings a_Conf)
        {

            if (a_Conf.IsReading)
            {
                if ( a_Conf.DbVersion >= 350 ) return;

                m_ColsMem.Clear();

                int NrCols = a_Conf.Deserialize<int>( );

                for( int i = 0; i < NrCols; i++ )
                {
                    ColMem cm = new ColMem();

                    cm.Text    =      a_Conf.Deserialize<string>();
                    cm.TxtAlgn = (int)a_Conf.Deserialize<HorizontalAlignment>();
                    m_ColsMem.Add(cm);
                }

                foreach ( ColMem cm in m_ColsMem )
                {
                    cm.Width = a_Conf.Deserialize<int>( );
                }
            }
            else
            {
                return;

                a_Conf.Serialize(Columns.Count);

                foreach( ColumnHeader col in Columns )
                {
                    a_Conf.Serialize( col.Text );
                    a_Conf.Serialize( col.TextAlign );
                }

                foreach (ColumnHeader col in Columns)
                {
                    a_Conf.Serialize(col.Width);
                }
            }

            if( a_Conf.IsReading )
            {
                AddColumnAttribs();
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       13.07.2023
        LAST CHANGE:   13.07.2023
        ***************************************************************************/
        public void StoreColumnAttibs()
        {
            m_ColsMem.Clear();

            foreach( ColumnHeader col in Columns )
            {
                ColMem cm = new ColMem( col.Text, col.Width, (int)col.TextAlign );
                m_ColsMem.Add(cm);
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.03.2021
        LAST CHANGE:   01.07.2024
        ***************************************************************************/
        public void AddColumnAttribs()
        {
            return;

            if ( Columns.Count != m_ColsMem.Count ) return;

            for( int i=0; i<Columns.Count; i++ )
            {
                Columns[i].Width     =                       m_ColsMem[i].Width;
                Columns[i].TextAlign = (HorizontalAlignment) m_ColsMem[i].TxtAlgn;
            }
        }

        /***************************************************************************
        SPECIFICATION: Implements the manual sorting of items by columns.
        CREATED:       11.05.2006
        LAST CHANGE:   06.03.2007
        ***************************************************************************/
        public class ListViewItemComparer:IComparer
        {
            private int  m_Col;
            private bool m_bAsc;
            public ListViewItemComparer()
            {
                m_Col  = 0;
                m_bAsc = true;
            }

            public ListViewItemComparer(int a_Column,bool a_bAscending)
            {
                m_Col  = a_Column;
                m_bAsc = a_bAscending;
            }

            public int Compare(object x,object y)
            {
                ListViewItem a,b;

                if ( m_bAsc )
                {
                    a = (ListViewItem)x;
                    b = (ListViewItem)y;
                }
                else
                {
                    b = (ListViewItem)x;
                    a = (ListViewItem)y;
                }

                if ( a.SubItems.Count <= m_Col )
                    return -1;
                if ( b.SubItems.Count <= m_Col )
                    return 1;

                //WUtils.Str2Int()
                //int ai = int.Parse( a.SubItems[m_Col].Text );
                //int bi = int.Parse( b.SubItems[m_Col].Text );

                //return ai - bi;

                return String.Compare(a.SubItems[m_Col].Text,b.SubItems[m_Col].Text);
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.05.2006
        LAST CHANGE:   11.05.2006
        ***************************************************************************/
        protected void listView_ColumnClick(object sender,ColumnClickEventArgs e)
        {
            ListViewItemSorter = new ListViewItemComparer ( e.Column, m_bSortAscending );
            Sort();
            m_bSortAscending = !m_bSortAscending;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.12.2013
        LAST CHANGE:   05.12.2013
        ***************************************************************************/
        public string MergeLine( ListViewItem a_Item )
        {
            string ret = "";

            foreach ( ListViewItem.ListViewSubItem si in a_Item.SubItems )
            {
                ret += si.Text;
            }
            
            return ret;
        }
        
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.11.2006
        PARAMS:        a_NrCols:    all columns if -1
        LAST CHANGE:   20.05.2009
        ***************************************************************************/
        public List<ListViewItem> MergeColumns(bool bOnlySelected, string sSeparator, int a_NrCols)
        {
            List<ListViewItem> al = new List<ListViewItem>();

            foreach ( ListViewItem li in Items )
            {
                if ( li.Selected || !bOnlySelected )
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = "";
                    lvi.Selected = li.Selected;

                    int NrCols = Columns.Count;
                    if (a_NrCols != -1) NrCols = a_NrCols;
 
                    foreach ( ListViewItem.ListViewSubItem col in li.SubItems )
                    {
                        lvi.Text += col.Text; // + "\r\n";
                        if ( --NrCols <= 0 ) break;
                        lvi.Text += sSeparator;
                    }
                    al.Add(lvi);
                }
            }
            return al;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.12.2013
        LAST CHANGE:   09.12.2013
        ***************************************************************************/
        public string[] MergeColumns( )
        {
            //List<ListViewItem> al = new List<ListViewItem>();
            ListViewItem[] al;
            string[]       sa;

            sa = new string      [SelectedItems.Count];
            al = new ListViewItem[SelectedItems.Count];
            SelectedItems.CopyTo(al,0);

            int i = 0;
            
            foreach ( ListViewItem lvi in al )
            {
                //sa[i] = lvi.Text;
                
                foreach ( ListViewItem.ListViewSubItem si in lvi.SubItems )
                {
                    sa[i] += si.Text;
                }
                
                i++;
            }
                
            return sa;
        }

        /***************************************************************************
        SPECIFICATION: Thread save item access
        CREATED:       07.04.2015
        LAST CHANGE:   13.01.2016
        ***************************************************************************/
        private delegate ListViewItem dl_GetItem( int a_ItmIdx );
        public ListViewItem GetItem(int a_ItmIdx)
        {
            if (this.InvokeRequired)
            {
                dl_GetItem d = new dl_GetItem(GetItem);
                return (ListViewItem) this.Invoke(d, new object[] { a_ItmIdx });
            }
            else
            {
                if ( a_ItmIdx >= Items.Count ) return null;
                return Items[a_ItmIdx];
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       07.12.2012
        LAST CHANGE:   30.07.2015
        ***************************************************************************/
        protected override void OnNotifyMessage( Message m )
        {

            switch( m.Msg )
            {
                case WM_ERASEBKGND:  // Filter out the WM_ERASEBKGND message 
                    break;

                default:
                    base.OnNotifyMessage( m );
                    break;
            }
        }

    } // class

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       03.03.2021
    LAST CHANGE:   03.03.2021
    ***************************************************************************/
    public class ColMem
    {
        public string Text    ;
        public int    Width   ; 
        public int    TxtAlgn ;

        public ColMem( )
        {
            Text    = "";
            Width   = 0;
            TxtAlgn = 0;
        }

        public ColMem( string a_Text, int a_Width, int a_TxtAlgn )
        {
            Text    = a_Text   ;
            Width   = a_Width  ;
            TxtAlgn = a_TxtAlgn;
        }
    }

} // namespace
