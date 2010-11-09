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
using SettleIn.UI.Editor;

namespace SettleIn
{
    /// <summary>
    /// Interaction logic for ucNewBoard.xaml
    /// </summary>
    public partial class ucNewBoard : UserControl
    {
        public delegate void BoardPickedEvent(XmlBoard board);
        public event BoardPickedEvent BoardPicked;
        public event MapCreatorMain.CancelledHandler Cancelled; 
        public ucNewBoard()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ucNewBoard_Loaded);
        }

        void ucNewBoard_Loaded(object sender, RoutedEventArgs e)
        {
            listBox1.ItemsSource = Core.Instance.Boards;
        }

        private void btnUseBoard_Click(object sender, RoutedEventArgs e)
        {
            if (BoardPicked != null)
            {
                mainViewport.Board = null;
                BoardPicked(((XmlBoard)listBox1.SelectedItem).Copy());
            }            
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainViewport != null)
            {
                XmlBoard board = listBox1.SelectedItem as XmlBoard;
                //lblName.Content = board.Name;
                mainViewport.Board = new BoardVisual(board.Copy());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainViewport.Board = new BoardVisual(new XmlBoard(8, 8));
        }

        private void btnBlankBoard_Click(object sender, RoutedEventArgs e)
        {
            BoardPicked(new XmlBoard((int)slWidth.Value, (int)slHeight.Value));
        }
    }
}
