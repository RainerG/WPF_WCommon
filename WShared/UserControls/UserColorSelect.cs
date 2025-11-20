using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using NS_UserColor;
using NS_AppConfig;

namespace NS_UserColor
{
    public partial class UserColorSelect:UserControl
    {
        /***************************************************************************
        SPECIFICATION: Accessors 
        CREATED:       22.05.2016
        LAST CHANGE:   06.06.2020
        ***************************************************************************/
        public List<ColSelType>      ColList   { get { return m_ColSelects; } }
        public List<UserColorPicker> ColPicks  { get { return m_ColPicks;   } }
        public Size                  ClntSize  { get { return m_ClntSize;   } }

        /***************************************************************************
        SPECIFICATION: Members 
        CREATED:       21.05.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private   List<UserColorPicker> m_ColPicks;
        private   List<ColSelType>      m_ColSelects;
        private   Label                 m_LblColors;
        private   Label                 m_LblBold;
        private   Label                 m_LblItal;
        private   Size                  m_ClntSize;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       22.05.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public UserColorSelect()
        {
            InitializeComponent();

            m_ColPicks   = new List<UserColorPicker>();
            m_ColSelects = new List<ColSelType>();
            m_LblBold    = new Label();
            m_LblItal    = new Label();
            m_LblColors  = new Label();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.05.2016
        LAST CHANGE:   07.05.2021
        ***************************************************************************/
        public Size BuildDialog()
        {
            int  ht = 25;
            int  wt = 250; 
            Size sz = new Size(wt,ht);
            int  x  = 15;
            int  y  = 20;
            int  dy = ht;

            m_LblBold  .Text = "Bold";   m_LblBold  .Location = new Point( x+wt-58, y );
            m_LblItal  .Text = "Italic"; m_LblItal  .Location = new Point( x+wt-29, y );
            m_LblColors.Text = "Color";  m_LblColors.Location = new Point( x+82, y );

            m_LblBold  .Width = 28; // m_LblBold  .BackColor = Color.Beige;
            m_LblItal  .Width = 35; // m_LblItal  .BackColor = Color.Beige;
            m_LblColors.Width = 50; // m_LblColors.BackColor = Color.Beige;


            y+=dy;

            m_ColPicks.Clear();
            foreach( ColSelType cs in m_ColSelects )
            {
                UserColorPicker cp = new UserColorPicker( cs.text, cs.color, cs.bold, cs.ital, cs.hasbold, sz ,x, y );
                m_ColPicks.Add(cp);
                y+=dy;
            }

            this.Controls.Add( m_LblColors );
            this.Controls.Add( m_LblBold   );
            this.Controls.Add( m_LblItal   );

            foreach( UserColorPicker cp in m_ColPicks )
            {
                this.Controls.Add( cp );
                cp.Refresh();
            }

            this.Size = m_ClntSize = new Size( sz.Width + 25, (dy * m_ColPicks.Count) + dy + 40 );
            return this.Size;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.05.2016
        LAST CHANGE:   18.11.2024
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                int nrcols = a_Conf.Deserialize<int>();
                for ( int i=0; i<nrcols; i++ )  m_ColPicks[i].Serialize( ref a_Conf );
                CopyProps();
            }
            else
            {
                a_Conf.Serialize( m_ColPicks.Count );
                foreach( UserColorPicker cp in m_ColPicks ) cp.Serialize( ref a_Conf );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.05.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public ColSelType GetCol( string a_Text )
        {
            ColSelType ret = m_ColSelects.Find( c => c.text.ToLower().Contains( a_Text.ToLower() ) );
            if( ret == null )
            {
                ret = new ColSelType( "not found", Color.Black, false, false );
                //Debug.Assert(false,"GetCol argument: " + a_Text);
            }
            return ret;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.04.2017
        LAST CHANGE:   05.04.2017
        ***************************************************************************/
        public void SetColors( List<ColSelType> a_Cols )
        {
            if (a_Cols.Count != m_ColPicks.Count) return;

            m_ColSelects = a_Cols;
            int i=0; 

            foreach( ColSelType cst in m_ColSelects )
            {
                m_ColPicks[i].color = cst.color;
                m_ColPicks[i].bold  = cst.bold;
                m_ColPicks[i].ital  = cst.ital;
                i++;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        private void CopyProps()
        {
            for( int i=0; i < m_ColPicks.Count; i++ )
            {
                UserColorPicker cp = m_ColPicks[i];
                ColSelType cs = m_ColSelects[i];
                cs.color = cp.color; 
                cs.bold  = cp.bold;
                cs.ital  = cp.ital;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       19.11.2020
        LAST CHANGE:   19.11.2020
        ***************************************************************************/
        public void ShowMarkerBoxes( bool a_Show )
        {
            List<UserColorPicker> ucps = m_ColPicks.FindAll( cp => cp.text.Contains("Marker") );

            foreach( UserColorPicker ucp in ucps ) ucp.ShowBoxes( a_Show );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   22.05.2016
        ***************************************************************************/
        public void OnFormClosing()
        {
            CopyProps();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.05.2016
        LAST CHANGE:   21.05.2016
        ***************************************************************************/
        private void ColorSelector_FormClosing( object sender, FormClosingEventArgs e )
        {
            CopyProps();
        }
    } // class

    /***************************************************************************
    SPECIFICATION: Globals 
    CREATED:       21.05.2016
    LAST CHANGE:   14.07.2022
    ***************************************************************************/
    public class ColSelType
    {
        public string text;
        public Color  color;
        public bool   bold;
        public bool   ital;
        public bool   hasbold;
        public uint   argb;

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       21.05.2016
        LAST CHANGE:   14.07.2022
        ***************************************************************************/
        public ColSelType( Color a_Color, bool a_Bold, bool a_Ital )
        {
            text    = "";
            argb    = 0;
            color   = a_Color;
            bold    = a_Bold;
            ital    = a_Ital;
            hasbold = true;
        }

        public ColSelType( string a_Text, Color a_Color, bool a_Bold )
        {
            text    = a_Text;
            color   = a_Color;
            bold    = a_Bold;
            ital    = false;
            hasbold = true;
        }

        public ColSelType( string a_Text, Color a_Color, bool a_Bold, bool a_Ital )
        {
            text    = a_Text;
            color   = a_Color;
            bold    = a_Bold;
            ital    = a_Ital;
            hasbold = true;
        }

        public ColSelType( string a_Text, Color a_Color, bool a_Bold, bool a_Ital, bool a_HasBold )
            :this( a_Text, a_Color, a_Bold, a_Ital )
        {
            hasbold = a_HasBold;
        }

        public ColSelType( ColSelType a_Src )
        {
            Copy( a_Src );
        }

        public void Copy( ColSelType a_Src )
        {
            text    = a_Src.text    ; 
            color   = a_Src.color   ;
            bold    = a_Src.bold    ;
            ital    = a_Src.ital    ;
            hasbold = a_Src.hasbold ;
        }

        public Font GetFont( Font a_Fnt )
        {
            FontStyle fs = bold ? FontStyle.Bold : FontStyle.Regular;
            if (ital) fs |= FontStyle.Italic;
            return new Font( a_Fnt, fs );
        }

        public uint GetARGB()
        {
            uint ret = color.A; 
            ret <<= 8;
            ret  |= color.R;
            ret <<= 8;
            ret  |= color.G;
            ret <<= 8;
            ret  |= color.B;
            return ret;
        }

        public string GetARGBStr()
        {
            return string.Format( "@C{0:X8}", GetARGB() );
        }

    }


}
