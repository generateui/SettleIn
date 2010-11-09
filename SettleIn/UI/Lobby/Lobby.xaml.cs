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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Timers;

using SettleInCommon;
using SettleInCommon.Actions;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Actions.Lobby;
using SettleInCommon.Actions.Result;
using SettleIn.GameServer;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : UserControl
    {
        // List of games waiting for players to join
        private ObservableCollection<XmlGame> _GamesPending =
            new ObservableCollection<XmlGame>();

        //List of connected players
        private ObservableCollection<XmlUser> _ConnectedPlayers =
            new ObservableCollection<XmlUser>();

        //Chatlog of the lobby
        private ObservableCollection<XmlChatItem> _LobbyChatLog =
            new ObservableCollection<XmlChatItem>();

        private delegate void ProxySingletonProxyEventDelegate(object sender, ProxyEventArgs e);
        private delegate void TimerElapsedDelegate(object sender, ElapsedEventArgs e);

        //Credentials of the current player
        private XmlUserCredentials _CurrentUserCredentials;

        // Game settings of the selected game
        private XmlGame _SelectedGame;

        private ChatClient _ServerClient;

        private System.Timers.Timer _Timer;

        public ObservableCollection<XmlGame> GamesPending
        {
            get { return _GamesPending; }
        }
        public XmlLobbyState LobbyState
        {
            set
            {
                if (value != null)
                {
                    if (value.Games != null)
                    {
                        _GamesPending.Clear();
                        foreach (XmlGame game in value.Games)
                            _GamesPending.Add(game);
                    }
                    if (value.Users != null)
                    {
                        _ConnectedPlayers.Clear();
                        foreach (XmlUser user in value.Users)
                            _ConnectedPlayers.Add(user);
                    }
                    if (value.LobbyChat != null)
                    {
                        _LobbyChatLog.Clear();
                        foreach (XmlChatItem item in value.LobbyChat.Items)
                            _LobbyChatLog.Add(item);
                    }
                }
            }
        }

        public ChatClient ServerClient
        {
            get { return _ServerClient; }
            set
            {
                _ServerClient = value;
                //GameServer.Receive r = new Receive(
                _ServerClient.SayCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(_ServerClient_SayCompleted);
            }
        }

        void _ServerClient_SayCompleted(object sender, AsyncCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Lobby()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Lobby_Loaded);

            Server.Instance.LobbyActionReceived += new Server.LobbyActionReceivedHandler(Instance_LobbyActionReceived);

            Server.Instance.ProxyCallBackEvent += 
                new Server.ProxyCallBackEventHandler(Lobby_ProxyCallBackEvent);

            uiNewGame.NewGameClicked += 
                new NewGame.NewGameClickedHandler(uiNewGame_NewGameClicked);

            _Timer = new System.Timers.Timer(1000);
            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(_Timer_Elapsed);
            _Timer.Enabled = true;
        }

        void Instance_LobbyActionReceived(LobbyAction action)
        {
             /*
                XmlChatItem item = new XmlChatItem();
                item.User = GetUser(action.Sender);
                item.Message = ((LobbyChatAction)action).ChatMessage;
                _LobbyChatLog.Add(item);


            //Console.WriteLine("received action: {0}", action.Message);
            if (action is TryCreateGameAction)
            {
                //_GamesPending.Add(((Chat2Action)action).GameSettings);
            }
            if (action is GameCreatedAction)
            {
                AddGame((GameCreatedAction)action);
            }
            /*
            if (action is MessageFromServerAction)
            {
                XmlChatItem chatItem = new XmlChatItem();
                chatItem.DateTime = DateTime.Now;
                chatItem.Message = ((MessageFromServerAction)action).Message;
                chatItem.User = Core.Instance.ServerUser;
                chatItem.Type = EChatItemType.System;
                _LobbyChatLog.Add(chatItem);
            }
             */
            if (action is GameJoinedAction)
            {
                GameJoinedAction gameJoinedAction = action as GameJoinedAction;
                XmlGame gameToFind = null;

                foreach (XmlGame game in _GamesPending)
                {
                    if (game.ID == gameJoinedAction.GameID)
                    {
                        gameToFind = game;
                        break;
                    }
                }

                if (gameToFind == null)
                {
                    throw new Exception("Hmm, game not found... I must have missed a gameaddition update");
                }
                else
                {
                    gameToFind.AddPlayer(GetUser(action.Sender));
                }
            }
        }

        void uiNewGame_NewGameClicked()
        {
            
            TryCreateGameAction newGame = new TryCreateGameAction()
            {
                Sender = Core.Instance.CurrentPlayer.ID,
                GameSettings = uiNewGame.Settings
            };

            Server.Instance.Proxy.Say(newGame);
             
            cvNewGame.Visibility = Visibility.Collapsed;
            _GamesPending.Add(new XmlGame() { Settings = uiNewGame.Settings });
        }

        void _Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new TimerElapsedDelegate(_Timer_Elapsed),
                    sender,
                    new object[] { e });
                return;
            }
            //lblStatus.Content = "State of connection:" +Proxy_Singleton.GetInstance().State.ToString();
        }

        public void Login(XmlLobbyState lobby)
        {

        }

        // called when the user logs in
        void Lobby_ProxyEvent(object sender, ProxyEventArgs e)
        {
            // when this method gets called, it's usually not by the UI thread.
            // this codeblock ensures the correct thread is used
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new ProxySingletonProxyEventDelegate(Lobby_ProxyEvent),
                    sender,
                    new object[] { e });
                return;
            }

        }

        /// <summary>
        /// We received a message from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Lobby_ProxyCallBackEvent(object sender, ProxyCallBackEventArgs e)
        {
            Receive(e.Person.Name, e.Action);
        }

        void Lobby_Loaded(object sender, RoutedEventArgs e)
        {
            lbxGames.DataContext = _GamesPending;
            lbxUsers.DataContext = _ConnectedPlayers;
            lbxChats.DataContext = _LobbyChatLog;
        }

        private void txtGeneralChatMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Do();
            }
        }

        private void Do()
        {
            LobbyChatAction chat2 = new LobbyChatAction() { ChatMessage = txtGeneralChatMessage.Text, Sender = Core.Instance.CurrentPlayer.ID };
            SettleIn.Server.Instance
                .Proxy.Say(chat2);
        }

        private XmlUser GetUser(int userID)
        {
            return (from u in _ConnectedPlayers
                    where u.ID == userID
                    select u).Single();
        }

        private void Receive(string name, GameAction action)
        {

        }
        private void AddGame(GameCreatedAction action)
        {
            _GamesPending.Add(new XmlGame() { Settings = action.Game, ID = action.ID });
        }


        private void lbxGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _SelectedGame = (XmlGame)lbxGames.SelectedItem;
            //spGameDetails.DataContext = _SelectedGame;
        }

        private void btnStartNewGame_Click(object sender, RoutedEventArgs e)
        {
            OpenNewGamePane();
        }

        private void OpenNewGamePane()
        {
            if (cvNewGame.Visibility == Visibility.Hidden)
                cvNewGame.Visibility = Visibility.Visible;
            else
                cvNewGame.Visibility = Visibility.Hidden;
        }

        private void btnJoinGame_Click(object sender, RoutedEventArgs e)
        {
            JoinGameAction action = new JoinGameAction();
            action.Sender = Core.Instance.CurrentPlayer.ID;
            action.GameID = _SelectedGame.ID;
            Server.Instance.Proxy.Say(action);
        }
    }
}
