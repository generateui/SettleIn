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
using System.Windows.Media.Animation;
using System.ComponentModel;

using SettleInCommon;
using SettleInCommon.User;
using SettleInCommon.Actions;
using SettleIn.GameServer;

namespace SettleIn.UI
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public delegate void ActionHandler();
        public delegate void LobbyJoinedHandler(XmlLobbyState lobby);
        public event LobbyJoinedHandler LobbyJoined;
        public event ActionHandler ContinueGame;
        public event ActionHandler CreateMap;
        public event ActionHandler NewTestGame;
        public event ActionHandler NewScene;

        private void OnLobbyJoined(XmlLobbyState lobby)
        {
            if (LobbyJoined != null)
            {
                spCredentials.Visibility = Visibility.Collapsed;
                LobbyJoined(lobby);
            }
        }

        private void OnContinueGame()
        { if (ContinueGame != null) ContinueGame(); }
        
        private void OnCreateMap()
        { if (CreateMap != null) CreateMap(); }

        private void OnNewTestGame()
        { if (NewTestGame != null) NewTestGame(); }

        private void OnNewScene()
        { if (NewScene != null) NewScene(); }

        public WelcomeWindow()
        {
            InitializeComponent();

            UpdateSettingsInUI();

            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
            Server.Instance.ProxyEvent += new Server.ProxyEventHandler(Instance_ProxyEvent);
            Server.Instance.JoinCompleted+=new EventHandler<JoinCompletedEventArgs>(Proxy_JoinCompleted);
        }

        void Instance_ProxyEvent(object sender, ProxyEventArgs e)
        {
            if (e.lobby != null)
            {
                OnLobbyJoined(e.lobby);
            }
            else
            {
                lblCreateAccount.Content = "Login failed";
            }
        }

        void UpdateSettingsInUI()
        {
            if (Properties.Settings.Default.Credentials != null)
            {
                cbxLoginName.Text = Properties.Settings.Default.Credentials.Name;
                txtPassword.Text = Properties.Settings.Default.Credentials.Password;
            }
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Credentials")
            {
                UpdateSettingsInUI();
            }
        }

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            XmlUserCredentials xu = new XmlUserCredentials()
            { Name = cbxLoginName.Text, Password = txtPassword.Text};

            Server.Instance.JoinCompleted += new EventHandler<JoinCompletedEventArgs>(Proxy_JoinCompleted);
            
            // set ui status on logging in
            spLoginStatus.Visibility = Visibility.Visible;
            btnPlay.IsEnabled = false;
            spinLoginStatus.Visibility = Visibility.Visible;
           
            Server.Instance.JoinAsync(xu);
        }

        void Proxy_JoinCompleted(object sender, JoinCompletedEventArgs e)
        {
            if (e.Result.User != null)
            {
                lblLoginStatis.Content = "Login successful!";
                this.WindowState = WindowState.Minimized;
                spinLoginStatus.Visibility = Visibility.Hidden;
                Core.Instance.CurrentPlayer = e.Result.User;
                OnLobbyJoined(e.Result.LobbyState);
            }
            else
            {
                lblLoginStatis.Content = String.Format(
                    "Login failed :( Message from server: \r\n {0}",
                    e.Result.FailMessage.Message);
                btnPlay.IsEnabled = true;
                spinLoginStatus.Visibility = Visibility.Hidden;
            }
        }

        private void bCreate_Click(object sender, RoutedEventArgs e)
        {
            OnCreateMap();
        }

        private void lblCreateAccount_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*
            DoubleAnimation da = new DoubleAnimation();
            if (spCreateAccount.Height == 0)
            {
                da.From = 0;
                da.To = 350;
                da.Duration = new TimeSpan(0, 0, 0, 0, 500);
                da.AutoReverse = false;
                spCreateAccount.BeginAnimation(FrameworkElement.HeightProperty, da);
            }
            else
            {
                da.From = 350;
                da.To = 0;
                da.Duration = new TimeSpan(0, 0, 0, 0, 500);
                da.AutoReverse = false;
                spCreateAccount.BeginAnimation(FrameworkElement.HeightProperty, da);
            }
             */
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnNewTestGame();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OnNewScene();
        }
    }
}
