using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl
    {
        public Spinner()
        {
            InitializeComponent();
        }

        public void Stop()
        {
            sb.Stop(ellipse2);
            sb.Stop(ellipse3);
            sb.Stop(ellipse4);
            sb.Stop(ellipse5);
            sb.Stop(ellipse6);
            sb.Stop(ellipse7);
        }
    }
}
