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
    /// Interaction logic for BoardRules.xaml
    /// </summary>
    public partial class BoardRules : UserControl
    {
        public BoardRules()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty BoardProperty =
            DependencyProperty.Register("Board", typeof(BoardVisual), typeof(BoardRules));
        public BoardVisual Board
        {
            get { return (SettleIn.BoardVisual)GetValue(BoardRules.BoardProperty); }
            set { SetValue(BoardRules.BoardProperty, value); }
        }

    }
}
