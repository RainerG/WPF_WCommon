using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NS_AppConfig;
using NS_UserColor;

namespace NS_UserColor
{
    public partial class ColorSelector:Form //ColorSelector
    {
        /***************************************************************************
        SPECIFICATION: Accessors 
        CREATED:       04.06.2016
        LAST CHANGE:   06.06.2020
        ***************************************************************************/
        public Size             ClntSize  { get { return m_Size; } }
        public UserColorSelect  UsrColSel { get { return userColSel; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       22.05.2016
        LAST CHANGE:   22.05.2016
        ***************************************************************************/
        protected       List<ColSelType> m_ColSels;
        private         Size m_Size;

        /***************************************************************************
        SPECIFICATION: C'tor 
        CREATED:       21.05.2016
        LAST CHANGE:   11.01.2017
        ***************************************************************************/
        public ColorSelector( )
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            m_ColSels = userColSel.ColList;

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   02.04.2019
        ***************************************************************************/
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

            userColSel.Serialize( ref a_Conf );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   22.05.2016
        ***************************************************************************/
        public ColSelType GetCol( string a_Col ) { return userColSel.GetCol( a_Col ); }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   06.06.2020
        ***************************************************************************/
        protected void BuildDialog( Form a_Dlg )
        {
            Size sz = userColSel.BuildDialog();
            this.ClientSize = new Size( sz.Width, sz.Height );
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            int ht = sz.Height + 40;
            m_Size = a_Dlg.Size = new Size( sz.Width + 20, ht );

            a_Dlg.MaximumSize = new Size( a_Dlg.MaximumSize.Width, ht );
            a_Dlg.MinimumSize = new Size( a_Dlg.MinimumSize.Width, ht );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       22.05.2016
        LAST CHANGE:   22.05.2016
        ***************************************************************************/
        private void ColorSelector_FormClosing( object sender, FormClosingEventArgs e )
        {
            userColSel.OnFormClosing();
        }
    } // class
} // namespace
