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
using SettleInCommon.Gaming;

using SettleInCommon.Board;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGame : UserControl
    {
        public delegate void NewGameClickedHandler();
        public event NewGameClickedHandler NewGameClicked;

        private void OnNewGameClicked()
        {
            if (NewGameClicked != null)
                NewGameClicked();
        }
        private XmlGameSettings _Settings = new XmlGameSettings();

        public XmlGameSettings Settings
        {
            get { return _Settings; }
            set { _Settings = value; }
        }

        public NewGame()
        {
            InitializeComponent();

            //ObjectDataProvider prov = (ObjectDataProvider)FindResource("employeeData");
            //prov.ObjectInstance = Settings;

            cbxMaps.DataContext = Core.Instance.Boards;

            mainViewport.Board = new BoardVisual();
            //_Settings.Name = String.Format("{0}'s fun game",
                //Core.Instance.CurrentPlayer.Name);
        }
        private void cbxMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XmlBoard selectedBoard = cbxMaps.SelectedItem as XmlBoard;
            if (selectedBoard != null)
            {
                mainViewport.Board.Board = selectedBoard.Copy();
                _Settings.Map = selectedBoard.ID;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            OnNewGameClicked();
        }
    }
}
