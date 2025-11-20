 using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

using NS_Utilities;

namespace NS_Utilities
{
    /***************************************************************************
    SPECIFICATION: Class
    CREATED:       16.12.2015
    LAST CHANGE:   16.12.2015
    ***************************************************************************/
    public class SelfUpdate
    {
        /***************************************************************************
        SPECIFICATION: Types
        CREATED:       16.12.2015
        LAST CHANGE:   07.06.2017
        ***************************************************************************/
        private const string SOURCE = "\\\\cw01.contiwan.com\\root\\loc\\lndp\\DIDK7869\\AnalysisTools";
        private const string BATCH  = "SelfUpdate.bat";

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       16.12.2015
        LAST CHANGE:   20.10.2017
        ***************************************************************************/
        private string        m_AppName;
        private string        m_EcuCfg;
        private string        m_AuxName;
        private List<string>  m_AuxNames;
        private Form          m_Parent;
        private bool          m_Lock;

        /***************************************************************************
        SPECIFICATION: C'tors
        CREATED:       16.12.2015
        LAST CHANGE:   20.10.2017
        ***************************************************************************/
        public SelfUpdate( Form a_Parent, string a_Appname, string a_EcuCfg )
        {
            m_AppName   = a_Appname;
            m_Parent    = a_Parent;
            m_EcuCfg    = a_EcuCfg;
            m_AuxName   = "";
            m_AuxNames  = null;
        }

        public SelfUpdate( Form a_Parent, string a_Appname, string a_EcuCfg, string a_Auxname )
            :this( a_Parent, a_Appname, a_EcuCfg )
        {
            m_AuxName = a_Auxname;
        }

        public SelfUpdate( Form a_Parent, string a_Appname, string a_EcuCfg, List<string> a_Auxnames )
            :this( a_Parent, a_Appname, a_EcuCfg )
        {
            m_AuxNames = a_Auxnames;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.12.2015
        LAST CHANGE:   21.12.2017
        ***************************************************************************/
        private bool WriteBatch( bool a_App )
        {
            StreamWriter wrt = new StreamWriter( BATCH );

            try
            {
                string src = Utils.ConcatPaths( SOURCE, m_AppName );
                string dir = m_AppName;

                string asp = Application.StartupPath;
                if (asp.Contains("Peak"))
                {
                    if (a_App) src += "Peak";
                    dir += "Peak";
                }

                if (asp.Contains("Ebers"))
                {
                    if (a_App) src += "Ebers";
                    dir += "Ebers";
                }

                if( ! Directory.Exists( src ) )
                {
                    MessageBox.Show("Source not accessible: " + src, "Folder not found !");
                    wrt.Close();
                    return false;
                }

                wrt.WriteLine( "@echo OFF" );
                wrt.WriteLine( "echo *************************************************************************************************************" );
                wrt.WriteLine( "echo *********************** Please be patient and wait until this window closes by itself ***********************" );
                wrt.WriteLine( "echo *************************************************************************************************************" );
                wrt.WriteLine( "" );
                wrt.WriteLine( "set SERV=" + SOURCE );
                wrt.WriteLine( "echo Server: %SERV%" );
                wrt.WriteLine( "echo Local : " + asp );
                wrt.WriteLine( "set CPY=xcopy /D /Y /R /S" );

                wrt.WriteLine( "set SRC=%SERV%\\" + dir );
                wrt.WriteLine( "set CFG=" + m_EcuCfg );

                wrt.WriteLine( "" );
                wrt.WriteLine( "if not exist %SRC% goto NOSERVER" );
                wrt.WriteLine( "" );

                if (a_App)
                {
                    wrt.WriteLine( "if not exist ..\\" + dir + " md ..\\" + dir );
                    wrt.WriteLine( "cd ..\\" + dir );

                    //if (m_EcuCfg != "")
                    //{
                    //    wrt.WriteLine( "if exist %CFG% rd /Q /S %CFG%" );
                    //    if (m_EcuCfg.StartsWith("Sc")) wrt.WriteLine( "if exist NaEcuConfigs rd /Q /S NaEcuConfigs" );
                    //}

                    wrt.WriteLine( "ren *.ini *.2ini" );
                    wrt.WriteLine( "%CPY% %SRC%\\*.* *.*" );
                    wrt.WriteLine( "del *.ini" );
                    wrt.WriteLine( "ren *.2ini *.ini" );
                    wrt.WriteLine( "start " + m_AppName + ".exe" );
                }

                if ( m_AuxName != "" )
                {
                    string dst = Utils.ConcatPaths( "..\\", m_AuxName );
                           src = Utils.ConcatPaths( SOURCE, m_AuxName );
                    wrt.WriteLine( "set SRC=" + src );
                    wrt.WriteLine( "set DST=" + dst );
                    wrt.WriteLine( "echo Searching for new ISP versions ..." );
                    wrt.WriteLine( "%CPY% %SRC%\\*.* %DST%\\*.*" );
                }

                // delete deprecated files
                wrt.WriteLine( "" );
                wrt.WriteLine( "echo deleting deprecated files ...." );
                wrt.WriteLine("del %CFG%\\EC_ARS410NN.cfg");
                wrt.WriteLine("del %CFG%\\EC_ARS410RT.cfg");
                wrt.WriteLine("del %CFG%\\EC_MFC431.cfg");
                wrt.WriteLine("del %CFG%\\EC_MFC510DPU.cfg");
                wrt.WriteLine("del %CFG%\\EC_MFC510IUC.cfg");
                wrt.WriteLine("del %CFG%\\EC_SRR320TA.cfg");
                wrt.WriteLine("del %CFG%\\EC_SRR320TA.cfg");
                wrt.WriteLine("del %CFG%\\EC_MFL420MI27.cfg");
                wrt.WriteLine("del %CFG%\\EC_ARS4L1.cfg");
                wrt.WriteLine("del QuickStartGuide.pdf");
                wrt.WriteLine("rm -r %SERV%\\ISP_Files\\MFC431TA19_Files\\ARS*");

                if ( m_AuxNames != null )
                {
                    foreach( string anm in m_AuxNames )
                    {
                        string dst = Utils.ConcatPaths( "..\\ISP_Files", anm );
                               src = Utils.ConcatPaths( "%SERV%\\ISP_Files", anm );
                        wrt.WriteLine( "set SRC=" + src );
                        wrt.WriteLine( "set DST=" + dst );
                        wrt.WriteLine( "echo Searching in %SRC% for new ISP versions ..." );
                        wrt.WriteLine( "%CPY% %SRC%\\*.* %DST%\\*.*" );
                    }
                }

                // footer
                wrt.WriteLine("");
                wrt.WriteLine("goto END");
                wrt.WriteLine("");
                wrt.WriteLine(":NOSERVER");
                wrt.WriteLine("echo Folder %SRC% not yet available");
                wrt.WriteLine("pause");
                wrt.WriteLine("");
                wrt.WriteLine(":END");

                wrt.Close();
                return true;
            }
            catch
            {
                MessageBox.Show( "Error creating copy script " + BATCH, "Error" );
                wrt.Close();
                return false;
            }
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.12.2015
        LAST CHANGE:   20.02.2017
        ***************************************************************************/
        public void Execute()
        {
            bool app = false;
            if ( m_AppName != "") app = true;
            if ( ! WriteBatch(app) ) return;

            m_Lock = true;

            Thread thrd = new Thread( new ThreadStart( CopyThread ) );
            thrd.Start();

            if (m_Parent != null) m_Parent.Close();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       16.12.2015
        LAST CHANGE:   20.02.2017
        ***************************************************************************/
        private void CopyThread()
        {
            Process prc = System.Diagnostics.Process.Start( BATCH );
            if (m_AppName == "") prc.WaitForExit();
        }
    }
}
