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

using SettleInCommon.Board;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for HexListSelector.xaml
    /// </summary>
    public partial class HexListSelector : UserControl
    {
        public HexListSelector()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty HexListProperty = 
            DependencyProperty.Register("HexList", typeof(XmlHexList), typeof(HexListSelector));

        public XmlHexList HexList
        {
            get { return (XmlHexList)GetValue(HexListSelector.HexListProperty); }
            set { SetValue(HexListSelector.HexListProperty, value); }
        }
    }
}
