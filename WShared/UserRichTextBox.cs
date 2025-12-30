using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using NS_WUtilities;


namespace NS_WUtilities
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       25.08.2018
    LAST CHANGE:   25.08.2018
    ***************************************************************************/
    public class UserRichTextBox : RichTextBox
    {
        /***************************************************************************
        SPECIFICATION: C'tor 
        CREATED:       25.08.2018
        LAST CHANGE:   25.08.2018
        ***************************************************************************/
        public UserRichTextBox()
           :base()
        {
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       25.08.2018
        LAST CHANGE:   25.08.2018
        ***************************************************************************/
        public void Output( string a_Text, Color a_Col, bool a_Bold )
        {
            string txt = a_Text;

            int start = TextLength;
            AppendText( txt );
            int end = TextLength;

            Select( start, end - start );

            SelectionColor = a_Col;
            SelectionBackColor = Color.Transparent;

            Font fnt = SelectionFont;
            if( fnt == null ) fnt = Font;

            if( a_Bold ) fnt = new Font( fnt, FontStyle.Bold );
            else         fnt = new Font( fnt, FontStyle.Regular );

            SelectionFont = fnt;
        }

    } // class
} // namespace
