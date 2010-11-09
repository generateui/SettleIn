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
    /// Interaction logic for AssignablePortsSelector.xaml
    /// </summary>
    public partial class AssignablePortsSelector : UserControl
    {
        public AssignablePortsSelector()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty BoardProperty =
            DependencyProperty.Register("Board", typeof(BoardVisual), typeof(AssignablePortsSelector));
        public BoardVisual Board
        {
            get { return (SettleIn.BoardVisual)GetValue(AssignablePortsSelector.BoardProperty); }
            set { SetValue(AssignablePortsSelector.BoardProperty, value); }
        }

    }
}
