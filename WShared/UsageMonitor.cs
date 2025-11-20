using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Drawing;

using NS_Utilities;

namespace NS_Utilities
{
    /***************************************************************************
    SPECIFICATION: class
    CREATED:       15.12.2015
    LAST CHANGE:   30.03.2016
    ***************************************************************************/ 
    public class UsageMonitor 
    {
        /***************************************************************************
        SPECIFICATION: Types 
        CREATED:       15.12.2015
        LAST CHANGE:   21.08.2025
        ***************************************************************************/
        const string    PATH1 = "t:\\x-gerlachr_uidg9686";
        const string    PATH2 = "\\\\automotive-wan.com\\Root\\SMT\\didk7869\\Temp\\Logs";
        //const string    PATH2 = "\\\\cw01.contiwan.com\\root\\loc\\lndp\\didk7869\\Temp\\Logs";
        const string    SERV1 = "lud9v9zw";
        const string    SERV2 = "didk7869";
        const int       DIST  = 20;

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       15.12.2015
        LAST CHANGE:   01.03.2022
        ***************************************************************************/
        public bool Enable { set { m_Enable = value; } }

        private string m_Path1;
        private string m_Path2;
        private string m_Path;
        private string m_Name;
        private string m_Release;
        private string m_Auxiliary;
        private bool   m_Enable;

        private FileAttributes m_Attribs;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       15.12.2015
        LAST CHANGE:   01.03.2022
        ***************************************************************************/
        public UsageMonitor( string a_Name, string a_Release )
        {
            m_Path1   = "";
            m_Path2   = "";
            m_Path    = "";
            m_Name    = a_Name;
            m_Release = a_Release;
            m_Enable  = true;
            m_Attribs = FileAttributes.Hidden;
            try
            {
                m_Path1 = Utils.ConcatPaths( Directory.GetCurrentDirectory(), m_Name);
                m_Path2 = Utils.ConcatPaths(PATH2, m_Name);
            }
            catch( UnauthorizedAccessException )
            {
                // do nothing
            }
            catch( ArgumentException )
            {
                // do nothing
            }
            catch( Exception )
            {
                // do nothing
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.12.2015
        LAST CHANGE:   01.03.2022
        ***************************************************************************/
        public void Write( string a_Aux )
        {
            //#if DEBUG
            //return;
            //#endif
            if ( ! m_Enable ) return;

            m_Auxiliary = "Aux: ";
            m_Path      = m_Path2;

            if (a_Aux == null) m_Auxiliary += "none";
            else               m_Auxiliary += a_Aux.Replace( " ", "_" );
            Thread wrthr = new Thread( new ThreadStart( WriteThread ) );
            wrthr.Start();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.05.2019
        LAST CHANGE:   21.08.2025
        ***************************************************************************/
        public bool OnLoad( )
        {
            if (! IsOnServer() )
            {
                Thread wrthr = new Thread( new ThreadStart( WriteThread ) );
                wrthr.Start();
                return true;
            }

            string name = m_Name.Replace( ".log", "" );

            BigNote dlg = new BigNote();

            dlg.Size = new Size( 900, 400 );

            dlg.RchTxtBox.Output( "\nWarning !!!\n\n", Color.Red, true );

            dlg.RchTxtBox.Output( "Please never execute ", Color.Black, false );
            dlg.RchTxtBox.Output( name, Color.Black, true );
            dlg.RchTxtBox.Output( " from the network drive directly !\n", Color.Black, false );

            dlg.RchTxtBox.Output( "Instead, copy the ", Color.Black, false );
            dlg.RchTxtBox.Output( name, Color.Black, true );
            dlg.RchTxtBox.Output( " folder to your local hard drive.\n\n", Color.Black, false );

            dlg.RchTxtBox.Output( "Otherwise you are blocking the update process !!!\n\n", Color.Orange, true );
            dlg.RchTxtBox.Output( "Please close this window in order to unblock the NW again !", Color.Black, false );

            dlg.RchTxtBox.SelectAll();
            dlg.RchTxtBox.SelectionAlignment = HorizontalAlignment.Center;
            dlg.RchTxtBox.DeselectAll();

            dlg.ExitTimer.Start( 20000 );

            dlg.TopMost = true;
            dlg.ShowDialog();
            
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.05.2019
        LAST CHANGE:   17.05.2019
        ***************************************************************************/
        public bool OnClosing()
        {
            if (! IsOnServer() ) return true;
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.05.2019
        LAST CHANGE:   17.05.2019
        ***************************************************************************/
        public bool IsOnServer()
        {
            string pth = m_Path1.ToLower();
            if ( pth.Contains(SERV1) ) return true;
            if ( pth.Contains(SERV2) ) return true;
            return false;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.12.2015
        LAST CHANGE:   07.11.2022
        ***************************************************************************/
        private void WriteThread()
        {
            try
            {
                if ( ! m_Enable ) return;

                string path = Utils.GetPath( m_Path );
                if ( ! Utils.DirExists( path ) ) return;

                string user = SystemInformation.UserName;
                string comp = SystemInformation.ComputerName;

                DateTime now   = DateTime.Now;

                StreamWriter wrt2 = null;

                if (user.Contains("uidg9686")) return;  // omit myself

                if ( m_Path != "" ) wrt2 = new StreamWriter(m_Path,true);

                string line  = "";
                       line += string.Format( "User: {0,-15} "    , user );
                       line += string.Format( "Computer: {0,-15} ", comp );
                       line += string.Format( "Time: {0,-26} "    , now  );

                if ( m_Auxiliary != null && m_Auxiliary != "" ) 
                {
                       line += string.Format( "{0,-22} ", m_Auxiliary );
                }

                       line += string.Format( "{0,-22} ", m_Release );

                if (wrt2 != null) 
                { 
                    wrt2.WriteLine(line);   
                    wrt2.Close(); 
                    File.SetAttributes( m_Path, m_Attribs );
                }
            }
            catch( UnauthorizedAccessException )
            {
                // do nothing
            }
            catch( ArgumentException )
            {
                // do nothing
            }
            catch( IOException )
            {
                // do nothing
            }
        }
    }
}
