using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;

using Word  = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel; 

using NS_UserList;
using NS_Utilities;
//using Microsoft.Office.Interop.Word;

namespace NS_WordExcel
{
    public partial class WordExcelExport:Form
    {
        /***************************************************************************
        SPECIFICATION: Types
        CREATED:       14.01.2016
        LAST CHANGE:   06.06.2016
        ***************************************************************************/
        private class CllFormat
        {
            public int OleForeCol { get { return ColorTranslator.ToOle(ForeColor); } }
            public int OleBackCol { get { return ColorTranslator.ToOle(BackColor); } }

            public Color ForeColor;
            public Color BackColor;

            public Excel.XlHAlign HAlign;
        }

        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       21.01.2014
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public bool IsAborted               { get { return m_bAborted;  } }
        public List<NumFormat>   NumFrmts   { get { return m_NumFrmts;  } }
        public List<ChartFormat> ChrtFrmts  { get { return m_ChrtFrmts; } }
        public string            XlExpFilNm { set { m_XlExpFilNm = value; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       21.01.2014
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        private delegate bool dl_IsAborted   ();
        private delegate void dl_CloseDlg    ();
        private delegate void dl_HideDlg     ();
        private delegate void dl_showText    ( string a_sText );

        private bool                m_bAborted;
        private string              m_File;
        private string              m_XlExpFilNm;
        private UserListView        m_Log;
        private Excel.Application   m_XLApp;
        private Excel.Workbook      m_XLWrkBk;
        private Excel.Worksheet     m_XLWrkSht;
        private List<NumFormat>     m_NumFrmts;
        private List<ChartFormat>   m_ChrtFrmts;


        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       04.12.2013
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public WordExcelExport( )
        {
            InitializeComponent();
            m_bAborted   = false;
            m_Log        = null;
            textBox.Text = "Writing log to buffer";
            m_NumFrmts   = new List<NumFormat>  ();
            m_ChrtFrmts  = new List<ChartFormat>();
            m_XlExpFilNm = "Default";
        }

        public WordExcelExport( UserListView a_tLog )
          :this()
        {
            m_Log = a_tLog;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   21.01.2014
        ***************************************************************************/
        private delegate void dl_ShowProgress( int a_iLine, int a_iLines );
        void ShowProgress( int a_iLine, int a_iLines ) 
        {
            if ( this.InvokeRequired )
            {
                dl_ShowProgress d = new dl_ShowProgress(ShowProgress);
                this.Invoke(d, new object[] { a_iLine, a_iLines });
            }
            else
            {
                SetProgress( a_iLine, a_iLines );                
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   21.01.2014
        ***************************************************************************/
        void ShowText( string a_sText ) 
        {
            if ( this.InvokeRequired )
            {
                dl_showText d = new dl_showText(ShowText);
                this.Invoke(d, new object[] { a_sText });
            }
            else
            {
                textBox.Text = a_sText;
                Update();
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   21.01.2014
        ***************************************************************************/
        void CloseDlg( ) 
        {
            if ( this.InvokeRequired )
            {
                dl_CloseDlg d = new dl_CloseDlg(CloseDlg);
                this.Invoke(d, new object[] { });
            }
            else
            {
                Close( );                
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   21.01.2014
        ***************************************************************************/
        void HideDlg( ) 
        {
            if ( this.InvokeRequired )
            {
                dl_HideDlg d = new dl_HideDlg(HideDlg);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.Hide ();                
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.11.2013
        LAST CHANGE:   09.02.2023
        ***************************************************************************/
        private void WordExcelExport_Load( object sender, EventArgs e )
        {
            usrPrgrssBar.ShowVal(0);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.12.2013
        LAST CHANGE:   09.02.2023
        ***************************************************************************/
        public void SetProgress( int iCurrent, int iMaximum )
        {
            UInt64 iPercent = 100 * (UInt64)iCurrent / (UInt64)iMaximum;

            usrPrgrssBar.ShowVal((uint)iPercent);
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   21.01.2014
        ***************************************************************************/
        private void buttonAbort_Click( object sender, EventArgs e )
        {
            m_bAborted = true;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   29.03.2017
        ***************************************************************************/
        public bool ShowInExcel( string a_File )
        {
            OpenInExcel( a_File );
            return ! m_bAborted;
        }

        public void ShowInExcel( )
        {
            ThreadStart thrStart = new ThreadStart( OpenInExcel );
            Thread showThrd = new Thread( thrStart );
            showThrd.Name   = "Show in XL thread";
            showThrd.Start();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       01.06.2016
        LAST CHANGE:   01.06.2016
        ***************************************************************************/
        public object[,] ReadXLFile( string a_Fname )
        {
            try
            {
                Excel.Application oExcel = new Excel.Application();
                m_XLWrkBk                = oExcel.Workbooks.Open( a_Fname );
                Excel.Worksheet  ws      = m_XLWrkBk.Worksheets[1];

                Excel.Range ur = ws.UsedRange;

                object[,] arr = (object[,])ur.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                m_XLWrkBk.Close();
                oExcel.Quit();

                return arr;
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error in reading " + a_Fname );
                return null;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       29.08.2018
        LAST CHANGE:   29.05.2025
        ***************************************************************************/
        public void OpenExcel( string a_Fname ) { OpenExcel( a_Fname, null ); }
        public void OpenExcel( string a_Fname, string a_Sheet )
        {
            try
            {
                Excel.Application oExcel = IsXlOpened( a_Fname );
                if ( oExcel == null ) oExcel = new Excel.Application();

                m_XLWrkBk = oExcel.Workbooks.Open( a_Fname, true, false );
                if (m_XLWrkBk != null)
                {
                    if (a_Sheet==null)  m_XLWrkSht = m_XLWrkBk.Worksheets[1];
                    else                m_XLWrkSht = m_XLWrkBk.Worksheets[a_Sheet];
                    //Excel.Range xlRange = ws.UsedRange;

                    oExcel.Visible = true;
                    m_XLWrkSht.Activate();
                }

            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error in opening " + a_Fname );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.07.2019
        LAST CHANGE:   04.07.2019
        ***************************************************************************/
        private Excel.Application IsXlOpened( string wbook )
        {
            Excel.Application exApp = null;
            try
            {
                exApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject( "Excel.Application" );

                exApp.Workbooks.Cast<Microsoft.Office.Interop.Excel.Workbook>().FirstOrDefault(x => x.Name == wbook);
                return exApp;
            }
            catch( Exception )
            {
                return null;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       30.08.2018
        LAST CHANGE:   31.05.2025
        ***************************************************************************/
        public void AppendLine( List<XlCell> a_Line, int a_Offs, bool a_CpyFrmt )
        {
            try
            {
                if ( m_XLWrkSht == null ) return;

                var _ = m_XLWrkSht.UsedRange;
                Excel.Range last = m_XLWrkSht.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell );
                last.Select();

                
                int rw = last.Row + 1;
                Excel.Range frst = m_XLWrkSht.Cells [ rw, a_Offs+1 ] ;
                frst.Select();

                foreach ( XlCell cll in a_Line )
                {
                    frst.Value        = cll.Value;
                    frst.NumberFormat = cll.Format;
                    SetCellFormat( cll.Format, frst );
                    frst = frst.Offset[0,1];
                }

                if (! a_CpyFrmt) return;

                // copy format from previous line
                rw--;
                Excel.Range line = m_XLWrkSht.Rows[rw];
                line.Copy(Type.Missing);
                line = m_XLWrkSht.Rows[rw+1];
                line.PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteFormats);
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error appending XL line" );
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public void OpenInExcel( ) { OpenInExcel(""); }
        public void OpenInExcel( object a_File )
        {
            try
            {
                m_File = (string)a_File;

                if( m_File == "" )
                {
                    m_File = m_XlExpFilNm;
                    if( m_File != "" && m_File.ToLower().IndexOf( ".xl" ) == -1 ) m_File += ".xlsx";
                }

                int hdrw = 1;
                int strw = hdrw + 1;
                int stcl = 1;
                int rows = m_Log.Items  .Count;
                int cols = m_Log.Columns.Count;
                int lstcl = stcl + cols - 1;
                int lstrw = hdrw + rows;
                int c=1;
                int r=1;
                Excel.Range rg;

	            object missing   = System.Reflection.Missing.Value;
                object readOnly  = false;
                object isVisible = true;

	            //Start Excel and create a new document.
	            m_XLApp     =  new Excel.Application();
                m_XLWrkBk   =  m_XLApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                m_XLWrkSht  =  (Excel.Worksheet)m_XLWrkBk.Worksheets[1];
                m_XLWrkSht.Name = "Data";

                m_XLApp.ActiveWorkbook.Windows.get_Item(1).DisplayGridlines = false;
                
                // read column headers
                foreach ( ColumnHeader col in m_Log.Columns )
                {
                    rg                = (Excel.Range)m_XLWrkSht.Cells[ hdrw, c++ ];
                    rg.Font.Bold      = true;
                    rg.Font.Color     = ColorTranslator.ToOle(Color.Blue);
                    rg.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                    rg.Value2         = col.Text;

                    switch( col.TextAlign )
                    {
                        case HorizontalAlignment.Center: rg.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; break;
                        case HorizontalAlignment.Left  : rg.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;   break;
                        case HorizontalAlignment.Right : rg.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;  break;
                    }
                }

                // read column colors
                List<CllFormat> frmts = new List<CllFormat>();
                c = 1;

                foreach( ListViewItem.ListViewSubItem it in m_Log.GetItem(0).SubItems )
                {
                    rg = (Excel.Range)m_XLWrkSht.Cells[hdrw, c++];
                    CllFormat frmt = new CllFormat();
                    frmt.ForeColor = it.ForeColor;
                    frmt.BackColor = it.BackColor;
                    frmt.HAlign    = (Excel.XlHAlign)rg.HorizontalAlignment;
					
                    frmts.Add(frmt);                    
                }

                // copy list into array
                string[,] arr = new string[rows,cols];

                for( r = 0; r < rows; r++ )
                {
                    ListViewItem li = m_Log.GetItem(r);
                    string txt = "";

                    for( c = 0; c < cols; c++ )
                    {
                        try
                        {
                            txt = li.SubItems[c].Text;
                        }
                        catch (System.Exception ex)
                        {
                        	txt = "";
                        }
                        arr[r,c] = txt;

                    }

                    if (IsAborted) break;

                    ShowProgress( r, rows );
                }

                // copy array to excel chart
                Excel.Range c1 = (Excel.Range)m_XLWrkSht.Cells[strw , stcl];
                Excel.Range c2 = (Excel.Range)m_XLWrkSht.Cells[lstrw, lstcl];
                            rg = (Excel.Range)m_XLWrkSht.get_Range(c1,c2);

                rg.Value = arr;

                SetCellFormats( strw, lstrw );

                m_XLApp.Visible = true;

                m_XLWrkSht.Activate();

                // set filter
                //rg.Columns.Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                rg.Borders[Excel.XlBordersIndex.xlEdgeBottom]      .LineStyle   = Excel.XlLineStyle.xlContinuous;
                rg.Borders[Excel.XlBordersIndex.xlEdgeRight]       .LineStyle   = Excel.XlLineStyle.xlContinuous;
                rg.Borders[Excel.XlBordersIndex.xlEdgeLeft]        .LineStyle   = Excel.XlLineStyle.xlContinuous;
                rg.Borders[Excel.XlBordersIndex.xlEdgeTop]         .LineStyle   = Excel.XlLineStyle.xlContinuous;
                rg.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle   = Excel.XlLineStyle.xlContinuous;
                rg.Borders[Excel.XlBordersIndex.xlInsideVertical]  .LineStyle   = Excel.XlLineStyle.xlContinuous;

                var hc1 = m_XLWrkSht.Cells[hdrw,stcl];
                var hc2 = m_XLWrkSht.Cells[hdrw,lstcl];

                Excel.Range headln = m_XLWrkSht.Range[hc1,hc2];
                headln.AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                headln.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                m_XLWrkSht.Rows[strw].Select();
                m_XLApp.ActiveWindow.FreezePanes = true;                

                // Set column colors
                c = stcl;
                foreach( CllFormat fmt in frmts )
                {
                    c1                     = m_XLWrkSht.Cells[strw,c];
                    c2                     = m_XLWrkSht.Cells[lstrw,c];
                    rg                     = m_XLWrkSht.Range[c1,c2];
                    rg.Font.Color          = fmt.OleForeCol;
                    rg.Interior.Color      = fmt.OleBackCol;
                    rg.HorizontalAlignment = fmt.HAlign;
                    c++;
                }

                // Make the column widths fit automatically
                c2 = m_XLWrkSht.Cells[lstrw, lstcl];
                rg = m_XLWrkSht.Range[hc1,c2];
                rg.Columns.AutoFit();

                ExcelChart XlChrt = new ExcelChart( m_XLApp, 2 );
                XlChrt.CreateCharts( m_ChrtFrmts, lstrw );

                CloseExcel();
            }
            catch (System.Exception ex)
            {
            	MessageBox.Show(ex.Message, "Failed to open and control Excel remotely");
                m_bAborted = true;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       03.09.2018
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        private void SetCellFormat( string a_Format, Excel.Range a_Rng )
        {
            switch( a_Format )
            {
                //case "hh:mm:ss":
                //    foreach ( Excel.Range cell in a_Rng )  cell.Value = Convert.ToDateTime(cell.Value);
                //    break;

                case "@":
                    RngToDecimal( a_Rng );
                    a_Rng.NumberFormat = "0.0";
                    break;

                case "0.00":
                    RngToDecimal( a_Rng );
                    a_Rng.NumberFormat = "0.00";
                    //foreach ( Excel.Range cell in a_Rng )  cell.Value = Convert.ToDouble(cell.Value);
                    break;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.09.2025
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        private void RngToDecimal( Excel.Range a_Rng )
        {
            object[,] values = (object[,])a_Rng.Value2; 
            for (int i = 1; i <= values.GetLength(0); i++)
            {
                for (int j = 1; j <= values.GetLength(1); j++)
                {
                    if ( values[i, j] != null ) values[i, j] = Convert.ToDecimal( values[i, j] ); 
                }
            }
            a_Rng.Value2 = values; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.05.2018
        LAST CHANGE:   09.05.2018
        ***************************************************************************/
        void SetCellFormats (int a_FrstRw, int a_LastRw )
        {
            foreach ( NumFormat nf in m_NumFrmts )
            {
                int col = nf.Col;

                Excel.Range c1 = (Excel.Range)m_XLWrkSht.Cells[a_FrstRw, col];
                Excel.Range c2 = (Excel.Range)m_XLWrkSht.Cells[a_LastRw, col];
                Excel.Range rg = (Excel.Range)m_XLWrkSht.get_Range(c1,c2);

                rg.NumberFormat = nf.Format;

                SetCellFormat( nf.Format, rg );
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       05.05.2018
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public void CloseExcel() 
        {
            usrPrgrssBar.ShowVal(0);

            if ( m_File != "" )
            {
                if (File.Exists(m_File)) File.Delete(m_File);
                m_XLWrkBk.SaveAs( m_File );
                //m_XLWrkBk.Close();
                //m_XLApp.Quit();
            }

            CloseDlg();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.09.2019
        LAST CHANGE:   29.05.2025
        ***************************************************************************/
        public void SaveExcel()
        {
            if (m_XLWrkBk != null) m_XLWrkBk.Save();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       02.05.2018
        LAST CHANGE:   02.05.2018
        ***************************************************************************/
        public void CreateXLDiagram( int a_ColX, int a_ColY )
        {
            //Excel.Chart chrt = m_XLApp.Charts.Add( Excel.XlChartType.xlLine );
            //chrt.SetSourceData( m_XLApp.Range["B2,C25"],Excel.XlRowCol.xlColumns );

            Excel.Range chartRange ; 

            object misValue = System.Reflection.Missing.Value;
            //xlWorkBook = xlApp.Workbooks.Add(misValue);


            Excel.ChartObjects xlCharts = (Excel.ChartObjects)m_XLWrkSht.ChartObjects(Type.Missing);
            Excel.ChartObject  myChart  = (Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
            Excel.Chart chartPage = myChart.Chart;

            chartRange = m_XLWrkSht.get_Range("B2", "C25");
            chartPage.SetSourceData(chartRange, Excel.XlRowCol.xlColumns);
            chartPage.ChartType = Excel.XlChartType.xlLine; 
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       21.01.2014
        LAST CHANGE:   21.01.2014
        ***************************************************************************/
        public void ShowInWord( string sFname, bool bOpenWord )
        {
            WordArgs tArgs = new WordArgs( sFname, bOpenWord );

            ParameterizedThreadStart thrStart = new ParameterizedThreadStart( OpenInWord );
            Thread showThrd = new Thread( thrStart );
            showThrd.Start( tArgs );
        }
        
        /***************************************************************************
        SPECIFICATION: Writes the log to an RTF file and opens word if 
				               bOpenWord == true
        CREATED:       22.11.2013
        LAST CHANGE:   21.01.2014
        ***************************************************************************/
        public void OpenInWord( object a_tArgs )
        {
            WordArgs tArgs = (WordArgs) a_tArgs;

            try
            {
                System.Drawing.Font hfnt = new System.Drawing.Font( "Arial", 8.0f, FontStyle.Bold );
                System.Drawing.Font nfnt = new System.Drawing.Font( "Courier New", 6.5f );

                RichTextBox rtb = new RichTextBox();

                rtb.SelectionFont = hfnt;
                rtb.SelectionBackColor = Color.LightGray;

                // read column headers
                foreach ( ColumnHeader col in m_Log.Columns )
                {
                    rtb.SelectedText += col.Text + "\t";

                    if (IsAborted) break;
                }
                rtb.SelectedText += "\n";

                int NrLines = m_Log.Items.Count;

                // read the listview itself
                for ( int i=0; i<NrLines; i++ )
                {
                    ListViewItem li = m_Log.Items[i];

                    if ( IsAborted ) break;

                    foreach( System.Windows.Forms.ListViewItem.ListViewSubItem si in li.SubItems )
                    {
                        rtb.SelectionFont      = nfnt;
                        rtb.SelectionBackColor = si.BackColor;
                        rtb.SelectionColor     = si.ForeColor;

                        rtb.SelectedText += si.Text + "\t";

                        if ( IsAborted ) break;
                    }

                    rtb.SelectedText += "\n";

                    ShowProgress( i, NrLines );
                }

                if( !IsAborted )
                {
                    rtb.SaveFile( tArgs.Fname );

                    if( tArgs.OpenWord ) OpenWord( tArgs.Fname );
                }
            }
            catch( System.FieldAccessException ex )
            {
                MessageBox.Show( ex.Message, "Failed to write " + tArgs.Fname );
            }
            catch (System.Exception ex)
            {
                MessageBox.Show( ex.Message, "Failed to export to " + tArgs.Fname );
                m_bAborted = true;
            }
            finally
            {
                CloseDlg();
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       28.11.2013
        LAST CHANGE:   28.11.2013
        ***************************************************************************/
        private bool OpenWord( string sFname )
        {
            try
            {
	            object missing   = System.Reflection.Missing.Value;
                object fileName  = sFname;
                object readOnly  = false;
                object isVisible = true;

                ShowProgress( 25, 100 );
                ShowText( "Opening in Word" );

	            //Start Word and create a new document.
	            Word.Application oWord = new Word.Application();
	            Word.Document    oDoc  = new Word.Document();
                
                oWord.Visible = true;
                
                oDoc = oWord.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref  missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible,ref missing, ref missing, ref missing, ref missing);
                ShowProgress( 75, 100 );
                oDoc.PageSetup.PaperSize   = Word.WdPaperSize  .wdPaperA3;
                oDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                oDoc.Select();
                oDoc.DefaultTabStop = 100;
                oDoc.Activate();

                return true;
            }
            catch (System.Exception ex)
            {
            	MessageBox.Show(ex.Message, "Failed to open and control WORD remotely");
                return false;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       27.01.2014
        LAST CHANGE:   03.04.2017
        ***************************************************************************/
        private void WordExcelExport_FormClosing( object sender, FormClosingEventArgs e )
        {
            e.Cancel = true;

            this.Hide();
        }
    }


    public class WordArgs
    {
        public string Fname    { get { return sFname;    } set { sFname = value; } }
        public bool   OpenWord { get { return bOpenWord; } set { bOpenWord = value; } }

        private string sFname;
        private bool   bOpenWord;

        public WordArgs( string a_sFname, bool a_bOpenWord )
        {
            sFname    = a_sFname;
            bOpenWord = a_bOpenWord;
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       09.05.2018
    LAST CHANGE:   09.05.2018
    ***************************************************************************/
    public class NumFormat
    {
        public string Format;
        public int    Col;

        public NumFormat( int a_Col, string a_Format )
        {
            Col    = a_Col;
            Format = a_Format;
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       20.05.2018
    LAST CHANGE:   06.09.2025
    ***************************************************************************/
    public class ChartFormat
    {
        public Excel.XlRgbColor Color;
        public int              XCol ;
        public int              YCol ;
        public string           XHdr ;
        public string           YHdr ;

        public ChartFormat( int a_XCol, int a_YCol, Excel.XlRgbColor a_Color )
        {
            Color = a_Color;
            XCol  = a_XCol ;
            YCol  = a_YCol ;
            XHdr  = "";
            YHdr  = "";
        }

        public ChartFormat( string a_XHdr, string a_YHdr, Excel.XlRgbColor a_Color )
        {
            Color = a_Color;
            XCol  = -1;
            YCol  = -1;
            XHdr  = a_XHdr;
            YHdr  = a_YHdr;
        }
    }

    /***************************************************************************
    SPECIFICATION: 
    CREATED:       03.09.2018
    LAST CHANGE:   03.09.2018
    ***************************************************************************/
    public class XlCell
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       03.09.2018
        LAST CHANGE:   03.09.2018
        ***************************************************************************/
        public string Value;
        public string Format;

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       03.09.2018
        LAST CHANGE:   03.09.2018
        ***************************************************************************/
        public XlCell()
        {
            Value  = "";
            Format = "";
        }

        public XlCell( string a_Value )
        {
            Value  = a_Value;
            Format = "";
        }

        public XlCell( string a_Value, string a_Format )
        {
            Value  = a_Value;
            Format = a_Format;
        }
    }
} // Namespace

