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
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SettleIn.UI;
using SettleInCommon;
using SettleIn.GameServer;

namespace SettleIn
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        WelcomeWindow _WelcomeWindow = new WelcomeWindow();

        public Main()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Main_Loaded);
            _WelcomeWindow.LobbyJoined += new WelcomeWindow.LobbyJoinedHandler(_WelcomeWindow_NewGame);
            _WelcomeWindow.CreateMap += new WelcomeWindow.ActionHandler(_WelcomeWindow_CreateMap);
            _WelcomeWindow.NewTestGame+=new WelcomeWindow.ActionHandler(_WelcomeWindow_NewTestGame);
            _WelcomeWindow.NewScene += new WelcomeWindow.ActionHandler(_WelcomeWindow_NewScene);
        }

        void _WelcomeWindow_NewScene()
        {
            _SceneMain.Visibility = Visibility.Visible;
            _MapCreatorMain.Visibility = Visibility.Hidden;
            _TestGameMain.Visibility = Visibility.Hidden;
            ucLobby.Visibility = Visibility.Hidden;
            _WelcomeWindow.WindowState = WindowState.Minimized;
        }

        void  _WelcomeWindow_NewTestGame()
        {
            _MapCreatorMain.Visibility = Visibility.Hidden;
            _TestGameMain.Visibility = Visibility.Visible;
            ucLobby.Visibility = Visibility.Hidden;
            this.BringIntoView();
            _WelcomeWindow.WindowState = WindowState.Minimized;
        }

        void _WelcomeWindow_CreateMap()
        {
            _MapCreatorMain.Visibility = Visibility.Visible;
            _TestGameMain.Visibility = Visibility.Hidden;
            ucLobby.Visibility = Visibility.Hidden;
            this.BringIntoView();
            _WelcomeWindow.WindowState = WindowState.Minimized;
        }

        void _WelcomeWindow_NewGame(XmlLobbyState lobby) 
        {
            _MapCreatorMain.Visibility = Visibility.Hidden;
            ucLobby.Visibility = Visibility.Visible;
            ucLobby.LobbyState = lobby;
            _TestGameMain.Visibility = Visibility.Hidden;
            //_GameMain.Login(user, pass);
            this.BringIntoView();
            _WelcomeWindow.WindowState = WindowState.Minimized;
        }

        void Main_Loaded(object sender, RoutedEventArgs e)
        {
            _WelcomeWindow.Show();
        }

    }
}