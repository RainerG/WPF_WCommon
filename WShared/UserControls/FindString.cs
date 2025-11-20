using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using NS_AppConfig;
using NS_Utilities;


namespace NS_UserOut
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       03.07.2013
    LAST CHANGE:   18.03.2022
    ***************************************************************************/
    public partial class FindString:Form
    {
        private RichTextBox   m_TextBox;
        private DialogPosSize m_PosSize;
        private int           m_StartIdx;
        private int           m_EndIdx;
        private int           m_CurrIdx;
        private Font          m_Font;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       03.07.2013
        LAST CHANGE:   18.03.2022
        ***************************************************************************/
        public FindString ( ref RichTextBox a_TextBox )
        {
            InitializeComponent();

            m_TextBox  = a_TextBox;
            m_StartIdx = 0;
            m_EndIdx   = 0;
            m_CurrIdx  = 0;
            tbResult.BackColor = tbResult.BackColor;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.07.2013
        LAST CHANGE:   18.03.2022
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if ( a_Conf.IsReading )
            {
                a_Conf.DeserializeDialog   ( this );
                m_PosSize = new DialogPosSize( this );
                checkBoxReverse  .Checked = (bool)a_Conf.Deserialize<bool>();
                checkBoxWholeWord.Checked = (bool)a_Conf.Deserialize<bool>();
                checkBoxMatchCase.Checked = (bool)a_Conf.Deserialize<bool>();
                if ( a_Conf.DbVersion >= 300 )
                {
                    checkBoxRegEx.Checked = (bool)a_Conf.Deserialize<bool>();
                    checkBoxWrap .Checked = (bool)a_Conf.Deserialize<bool>();
                }
            }
            else
            {
                a_Conf.SerializeDialog   ( this );
                a_Conf.Serialize( checkBoxReverse  .Checked );
                a_Conf.Serialize( checkBoxWholeWord.Checked );
                a_Conf.Serialize( checkBoxMatchCase.Checked );
                a_Conf.Serialize( checkBoxRegEx    .Checked );
                a_Conf.Serialize( checkBoxWrap     .Checked );
            }

            comboBoxFind.Serialize( ref a_Conf );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.07.2013
        LAST CHANGE:   03.07.2013
        ***************************************************************************/
        private void FindString_Load ( object sender, EventArgs e )
        {
            if( null != m_PosSize ) m_PosSize.Write( this );
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.07.2013
        LAST CHANGE:   18.03.2022
        ***************************************************************************/
        private void FindString_FormClosing ( object sender, FormClosingEventArgs e )
        {
            e.Cancel = true;
            if (m_PosSize != null) m_PosSize.Read( this );
            this.Hide();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.12.2016
        LAST CHANGE:   18.03.2022
        ***************************************************************************/
        public void FindStr()
        {
            comboBoxFind.AddTextEntry();

            bool found = false;

            RichTextBoxFinds finds = RichTextBoxFinds.None;
            RegexOptions     ropts = RegexOptions    .Multiline;

            if ( checkBoxMatchCase.Checked ) finds  = RichTextBoxFinds.MatchCase;
            if ( checkBoxReverse  .Checked ) finds |= RichTextBoxFinds.Reverse;
            if ( checkBoxWholeWord.Checked ) finds |= RichTextBoxFinds.WholeWord;

            if ( ! checkBoxMatchCase.Checked ) ropts |= RegexOptions.IgnoreCase;
            if (   checkBoxReverse  .Checked ) ropts |= RegexOptions.RightToLeft;

            if ( checkBoxReverse.Checked )
            {
                m_CurrIdx  = m_TextBox.SelectionStart;
                m_StartIdx = 0;
                m_EndIdx   = m_CurrIdx;
            }
            else
            {
                m_CurrIdx  = m_TextBox.SelectionStart + m_TextBox.SelectionLength;
                m_StartIdx = m_CurrIdx;
                m_EndIdx   = m_TextBox.Text.Length;
            }

            m_TextBox.Update();

            int idx = 0;

            if ( checkBoxRegEx.Checked )
            {
                string txt = m_TextBox.Text.Substring( m_StartIdx, m_EndIdx - m_StartIdx );
                Match m = Regex.Match( txt, comboBoxFind.Text, ropts );
                if (checkBoxReverse.Checked) idx = m.Index;
                else                         idx = m.Index + m_StartIdx;

                if ( m.Success )
                {
                    m_TextBox.Select( idx, m.Value.Length );
                    m_TextBox.Focus();
                    found = true;
                }
            }
            else
            {
                idx = m_TextBox.Find( comboBoxFind.Text, m_StartIdx, m_EndIdx, finds );

                if ( idx != -1 )
                {
                    m_TextBox.Select( idx, comboBoxFind.Text.Length );
                    m_TextBox.Focus();
                    found = true;
                }
            }

            if ( ! found && checkBoxWrap.Checked )
            {
                if ( checkBoxReverse.Checked ) m_TextBox.Select ( m_TextBox.TextLength, 0 );
                else                           m_TextBox.Select ( 0,0 );
            }

            if (found)
            {
                tbResult.ForeColor = Color.Blue; 
                tbResult.Text = "match";
            }
            else
            {
                tbResult.ForeColor = Color.Orange; 
                tbResult.Text = "not found";             
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.07.2013
        LAST CHANGE:   03.07.2013
        ***************************************************************************/
        private void buttonFind_Click ( object sender, EventArgs e )
        {
            FindStr();
        }

        private void buttonFind_KeyPress( object sender, KeyPressEventArgs e )
        {
            //if (e.KeyChar == 18)
            //{
            //    int i=0;
            //}
        }

        private void FindString_KeyPress( object sender, KeyPressEventArgs e )
        {

        }

        private void comboBoxFind_KeyPress( object sender, KeyPressEventArgs e )
        {
            //if (e.KeyChar == 18)
            //{
            //    int i=0;
            //}
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.12.2016
        LAST CHANGE:   18.12.2016
        ***************************************************************************/
        private void comboBoxFind_KeyDown( object sender, KeyEventArgs e )
        {
            if (e.KeyValue == 114) // F3
            {
                FindStr();
            }
        }
    }
}
