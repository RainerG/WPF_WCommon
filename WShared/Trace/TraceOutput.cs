using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NS_UserOut;

namespace NS_Trace
{
    /***************************************************************************
    SPECIFICATION: Trace output panel 
    CREATED:       20.03.2016
    LAST CHANGE:   20.03.2016
    ***************************************************************************/
    public class TraceOutput: UserOutText
    {
        /***************************************************************************
        SPECIFICATION: 
        CREATED:       20.03.2016
        LAST CHANGE:   20.03.2016
        ***************************************************************************/
        public TraceOutput()
            : base()
        {
            m_Prefs.OutPath = "d:\\temp\\TraceLogs";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // m_Prefs
            // 
            this.m_Prefs.Location = new System.Drawing.Point(25, 25);
            // 
            // TraceOutput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(776, 509);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "TraceOutput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
