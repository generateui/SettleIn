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
    /// Interaction logic for BoardProperties.xaml
    /// </summary>
    public partial class BoardProperties : UserControl
    {
        public BoardProperties()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty BoardProperty =
            DependencyProperty.Register("Board", typeof(BoardVisual), typeof(BoardProperties));
        public BoardVisual Board
        {
            get { return (SettleIn.BoardVisual)GetValue(BoardProperties.BoardProperty); }
            set { SetValue(BoardProperties.BoardProperty, value); }
        }
        private void btnChangeSize_Click(object sender, RoutedEventArgs e)
        {
            int h = 0;
            int w = 0;
            h = (int)slHeight.Value;
            w = (int)slWidth.Value;
            if (h != 0 && w != 0)
            {
                //Board.res.Resize(w, h);
                //((Board)DataContext).Resize(w, h);
            }
        }

    }
}
