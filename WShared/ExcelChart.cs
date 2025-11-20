using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

using NS_UserList;

namespace NS_WordExcel
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       12.05.2018
    LAST CHANGE:   12.05.2018
    ***************************************************************************/
    public class ExcelChart
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       12.05.2018
        LAST CHANGE:   20.05.2018
        ***************************************************************************/
        private Excel.Application   m_XLApp;
        private Excel.Workbook      m_XLWrkBk;
        private Excel.Worksheet     m_XLWrkSht;

        private int m_Dist;
        private int m_Top;
        private int m_Left;
        private int m_Page;
        private int m_Height;
        private int m_Width;

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       12.05.2018
        LAST CHANGE:   20.05.2018
        ***************************************************************************/
        public ExcelChart( Excel.Application a_App, int a_Page )
        {
            m_XLApp    = a_App;
            m_XLWrkBk  = m_XLApp.Workbooks[1];
            m_Dist     = 10;
            m_Top      = m_Dist;
            m_Left     = m_Dist;
            m_Height   = 200;
            m_Width    = 1200;
            m_Page     = a_Page;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       12.05.2018
        LAST CHANGE:   08.09.2025
        ***************************************************************************/
        public void CreateChart( string a_HdrX, string a_HdrY, int a_LastRw, Excel.XlRgbColor a_Color )
        {
            m_XLWrkSht = m_XLWrkBk.Worksheets[1];
            Excel.Range rng = m_XLWrkSht.Columns;
        }

        public void CreateChart( int a_ColX, int a_ColY, int a_LastRw, Excel.XlRgbColor a_Color )
        {
            object misValue = System.Reflection.Missing.Value;

            m_XLWrkSht = m_XLWrkBk.Worksheets[1];

            Excel.Worksheet wsht;
            if ( m_Page > m_XLWrkBk.Worksheets.Count ) 
            {
                wsht = m_XLWrkBk.Worksheets.Add(After: m_XLWrkSht);
                m_XLApp.ActiveWindow.DisplayGridlines = false;
            }
            else wsht = m_XLWrkBk.Worksheets[m_Page];

            wsht.Name = "Charts";

            Excel.Range chartRangeX = m_XLWrkSht.Range[m_XLWrkSht.Cells[2,a_ColX], m_XLWrkSht.Cells[a_LastRw, a_ColX]];
            Excel.Range chartRangeY = m_XLWrkSht.Range[m_XLWrkSht.Cells[1,a_ColY], m_XLWrkSht.Cells[a_LastRw, a_ColY]];

            Excel.ChartObjects xlCharts = (Excel.ChartObjects)wsht.ChartObjects(Type.Missing);
            Excel.ChartObject  myChart  = (Excel.ChartObject)xlCharts.Add( m_Left, m_Top, m_Width, m_Height );
            Excel.Chart chartPage = myChart.Chart;

            chartPage.SetSourceData( chartRangeY, Excel.XlRowCol.xlColumns );
            chartPage.ChartType = Excel.XlChartType.xlLine; 
            
            Excel.Series ser = chartPage.SeriesCollection(1);
            ser.XValues            = chartRangeX;
            ser.Border.Color       = (int)a_Color;
            ser.Format.Line.Weight = 2;

            m_Top += m_Height + m_Dist;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.05.2018
        LAST CHANGE:   20.05.2018
        ***************************************************************************/
        public void CreateCharts( List<ChartFormat> m_ChrtFrmts, int a_LstRw )
        {
            foreach ( ChartFormat cf in m_ChrtFrmts )
            {
                CreateChart( cf.XCol, cf.YCol, a_LstRw, cf.Color );
            }
        }

    } // class 
} // namespace
