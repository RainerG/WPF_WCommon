using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NS_Utilities;
using NS_AppConfig;
using NS_UserTabControl;
using NS_UserColor;

namespace NS_UserOut
{
    public partial class PreferencesUOList:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       06.09.2015
        MODIFIED:      09.06.2022
        ***************************************************************************/
        public ColSelType ColBack1    { get { return userColorSel.GetCol("back 1"); } }
        public ColSelType ColBack2    { get { return userColorSel.GetCol("back 2"); } }
        public ColSelType ColFore1    { get { return userColorSel.GetCol("fore 1"); } }
        public ColSelType ColFore2    { get { return userColorSel.GetCol("fore 2"); } }
        public ColSelType ColMarker   { get { return userColorSel.GetCol("rker 1"); } }
        public ColSelType NextMrkCol  { get { return GetNextMarkerColor(); } }
        public Font  TextFont         { get { return m_Font; } set { m_Font = value; } }


        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       18.04.2013
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        private List<ColSelType> m_MarkerCols;
        private int              m_MColIdx;
        private Font             m_Font;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       18.04.2013
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        public PreferencesUOList ( )
        {
            InitializeComponent();

            List<ColSelType> cols = userColorSel.ColList;

            cols.Add( new ColSelType("Fore 1"   , Color.Blue                 , false, false ) );
            cols.Add( new ColSelType("Back 1"   , Color.FromArgb(222,240,254), false, false ) );
            cols.Add( new ColSelType("Fore 2"   , Color.FromArgb(  0,117,  0), false, false ) );
            cols.Add( new ColSelType("Back 2"   , Color.FromArgb(236,255,236), false, false ) );
            cols.Add( new ColSelType("Marker 1" , Color.FromArgb(255,255,164), false, false ) );
            cols.Add( new ColSelType("Marker 2" , Color.FromArgb(255,187,255), false, false ) );
            cols.Add( new ColSelType("Marker 3" , Color.FromArgb(179,255,179), false, false ) );

            userColorSel.BuildDialog();
            m_MarkerCols = new List<ColSelType>();
            GetMarkerColors();
            SetDefault();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.04.2017
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        public void SetDefault( )
        {
            List<ColSelType> cols = userColorSel.ColList;

            cols[0].color = Color.Blue                 ;
            cols[1].color = Color.FromArgb(222,240,254);
            cols[2].color = Color.FromArgb(  0,117,  0);
            cols[3].color = Color.FromArgb(236,255,236);
            cols[4].color = Color.FromArgb(255,255,164);
            cols[5].color = Color.FromArgb(255,187,255);
            cols[6].color = Color.FromArgb(179,255,179);

            userColorSel.SetColors( cols );

            m_Font = new Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        public void GetMarkerColors()
        {
            m_MarkerCols.Clear();
            m_MColIdx = 0;

            m_MarkerCols.Add( userColorSel.GetCol("ker 1") );
            m_MarkerCols.Add( userColorSel.GetCol("ker 2") );
            m_MarkerCols.Add( userColorSel.GetCol("ker 3") );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        public ColSelType GetNextMarkerColor()
        {
            ColSelType ret = m_MarkerCols[m_MColIdx++];
            if (m_MColIdx >= 3) m_MColIdx = 0;
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                a_Conf.DeserializeDialog( this );
                this.Size = userColorSel.ClntSize;

                if ( a_Conf.DbVersion >= 310 )
                {
                    m_Font = a_Conf.Deserialize<Font>();
                }
            }
            else
            {
                a_Conf.SerializeDialog  ( this );
                a_Conf.Serialize        ( m_Font );
            }

            userColorSel.Serialize( ref a_Conf );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.06.2016
        LAST CHANGE:   04.06.2016
        ***************************************************************************/
        private void PreferencesUOList_FormClosing( object sender, FormClosingEventArgs e )
        {
            userColorSel.OnFormClosing();
            GetMarkerColors();
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
        CREATED:       09.06.2022
        LAST CHANGE:   09.06.2022
        ***************************************************************************/
        private void btnFont_Click( object sender, EventArgs e )
        {
            fontDialog.Font = m_Font;
            fontDialog.ShowDialog();
            m_Font = fontDialog.Font;
        }
    }
}
