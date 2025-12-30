using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace NS_WUtilities
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       04.11.2021
    LAST CHANGE:   04.11.2021
    ***************************************************************************/
    public class _7zipArchive
    {
        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       04.11.2021
        LAST CHANGE:   04.11.2021
        ***************************************************************************/
        public _7zipArchive()
        {

        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       04.11.2021
        LAST CHANGE:   04.11.2021
        ***************************************************************************/
        public void ExtractFile( string a_Src, string a_Dst )
        {
            // If the directory doesn't exist, create it.
            if ( ! Directory.Exists(a_Dst) )  Directory.CreateDirectory( a_Dst );

            string zPath = @"C:\Program Files\7-Zip\7zG.exe";
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = "x \"" + a_Src + "\" -o" + a_Dst;
                Process x = Process.Start( pro );
                x.WaitForExit();
            }
            catch ( System.Exception ex ) 
            {
                MessageBox.Show( ex.Message, "Error extracting 7Z archive" );
            }
        }

        public void CreateZip()
        {
            string sourceName = @"d:\a\example.txt";
            string targetName = @"d:\a\123.zip";
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = @"C:\Program Files\7-Zip\7zG.exe";
            p.Arguments = "a -tgzip \"" + targetName + "\" \"" + sourceName + "\" -mx=9";
            p.WindowStyle = ProcessWindowStyle.Hidden;
            Process x = Process.Start(p);
            x.WaitForExit();
        }

    }
}
