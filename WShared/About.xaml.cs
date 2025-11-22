using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shared
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About:Window
    {
        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       22.11.2025
        LAST CHANGE:   22.11.2025
        ***************************************************************************/
        private string m_Vers;

        public About( string a_Vers )
        {
            InitializeComponent();

            m_Vers = a_Vers;
        }
    }
}
