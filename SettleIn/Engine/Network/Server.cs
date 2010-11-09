using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows;  // For MessageBox

using SettleInCommon.Gaming;
using SettleInCommon.Actions;
using SettleInCommon.User;
using SettleInCommon;
using SettleInCommon.Actions.Result;
using SettleIn.GameServer;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Actions.Lobby;
using SettleInCommon.Actions.InGame;

namespace SettleIn
{
    #region Public enums/event args

    /// <summary>
    /// Proxy event args
    /// </summary>
    public class ProxyEventArgs : EventArgs
    {
        /// <summary>
        /// Current list of <see cref="Commmon.Person">Chatters</see>
        /// </summary>
        public XmlLobbyState lobby;
    }

    /// <summary>
    /// Proxy callback event args
    /// </summary>
    public class ProxyCallBackEventArgs : EventArgs
    {
        /// <summary>
        /// The incoming message
        /// </summary>
        public GameAction Action;
        /// <summary>
        /// The <see cref="Commmon.Person">Chatters</see> who created the
        /// message
        /// </summary>

        public XmlUser Person = null;
    }

    #endregion

    #region Proxy_Singleton class
    /// <summary>
    /// Provides a thread safe singleton which deals with
    /// interacting with the <see cref="Chatters.ChatService">ChatService</see>
    /// This class implements <see cref="IChatCallback">IChatCallback</see> as such
    /// implementation details are found here for the expected <see cref="IChatCallback">IChatCallback</see>
    /// interface methods
    /// This class also provides 2 events for subscribers to hook to, namely
    /// OnProxyCallBackEvent / OnProxyEvent.
    /// </summary>
    public class Server : IChatCallback, IChat
    {
        #region Instance Fields

        // singleton variables
        private static Server singleton = null;
        private static readonly object singletonLock = new object();

        private ChatClient _NetworkProxy;
        private HotseatServer _LocalProxy;

        private delegate void HandleDelegate(XmlLobbyState lobby);
        private delegate void HandleErrorDelegate();

        public delegate void GameActionReceivedHandler(GameAction action);
        public delegate void TurnActionReceivedHandler(TurnAction turnAction);
        public delegate void LobbyActionReceivedHandler(LobbyAction lobbyAction);
        public delegate void InGameActionReceivedHandler(InGameAction lobbyAction);

        public event GameActionReceivedHandler GameActionReceived;
        public event TurnActionReceivedHandler TurnActionReceived;
        public event LobbyActionReceivedHandler LobbyActionReceived;
        public event InGameActionReceivedHandler InGameActionReceived;

        //main proxy event
        public delegate void ProxyEventHandler(object sender, ProxyEventArgs e);
        public event ProxyEventHandler ProxyEvent;

        //callback proxy event
        public delegate void ProxyCallBackEventHandler(object sender, ProxyCallBackEventArgs e);
        public event ProxyCallBackEventHandler ProxyCallBackEvent;

        public event EventHandler<JoinCompletedEventArgs> JoinCompleted;
        public event EventHandler<AsyncCompletedEventArgs> LeaveCompleted;
        public event EventHandler<AsyncCompletedEventArgs> SayCompleted;

        public void JoinAsync(SettleInCommon.User.XmlUserCredentials credentials)
        {
            if (_NetworkProxy != null)
            {
                _NetworkProxy.JoinAsync(credentials);
            }
        }
        public IChat Proxy { get { return _NetworkProxy; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Blank constructor
        /// </summary>
        private Server()
        {
            bool IsHotseat = true;
            if (IsHotseat)
            {
                _LocalProxy = new HotseatServer(this);
                _LocalProxy.JoinCompleted += new EventHandler<JoinCompletedEventArgs>(Proxy_JoinCompleted);
                _LocalProxy.LeaveCompleted += new EventHandler<AsyncCompletedEventArgs>(Proxy_LeaveCompleted);
                _LocalProxy.SayCompleted += new EventHandler<AsyncCompletedEventArgs>(Proxy_SayCompleted);
            }
            else
            {
                _NetworkProxy = new ChatClient(new InstanceContext(this));
                _NetworkProxy.JoinCompleted += new EventHandler<JoinCompletedEventArgs>(Proxy_JoinCompleted);
                _NetworkProxy.LeaveCompleted += new EventHandler<AsyncCompletedEventArgs>(Proxy_LeaveCompleted);
                _NetworkProxy.SayCompleted += new EventHandler<AsyncCompletedEventArgs>(Proxy_SayCompleted);
            }
        }

        void Proxy_SayCompleted(object sender, AsyncCompletedEventArgs e)
        {
            SayCompleted(sender, e);
        }

        void Proxy_LeaveCompleted(object sender, AsyncCompletedEventArgs e)
        {
            OnLeaveCompleted(sender, e);
        }

        void Proxy_JoinCompleted(object sender, JoinCompletedEventArgs e)
        {
            OnJoinCompleted(sender, e);
        }

        private IChat _Proxy
        {
            get
            {
                if (_NetworkProxy != null)
                    return _NetworkProxy;
                else
                    return _LocalProxy;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calls by either the Receive() or ReceiveWhisper() <see cref="IChatCallback">IChatCallback</see>
        /// method implementations, and simply raises the OnProxyCallBackEvent() event
        /// to any subscribers
        /// </summary>
        /// <param name="sender">The <see cref="Common.Person">current chatter</see></param>
        /// <param name="message">The message</param>
        /// <param name="callbackType">Could be <see cref="CallBackType">CallBackType.Receive</see> or
        /// <see cref="CallBackType">CallBackType.ReceiveWhisper</see></param>
        public void Receive(GameAction action)
        {
            TurnAction turnAction = action as TurnAction;
            if (turnAction != null)
            {
                OnTurnActionReceived(turnAction);
                return;
            }

            InGameAction inGameAction = action as InGameAction;
            if (inGameAction != null)
            {
                OnInGameActionReceived(inGameAction);
                return;
            }
            LobbyAction lobbyAction = action as LobbyAction;
            if (lobbyAction != null)
            {
                OnLobbyActionReceived(lobbyAction);
                return;
            }
            
            //Fall through, raise default event
            OnGameActionReceived(action);
        }

        #endregion

        void OnJoinCompleted(object sender, JoinCompletedEventArgs e)
        {
            if (JoinCompleted != null)
                JoinCompleted(sender, e);
        }

        void OnLeaveCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (LeaveCompleted != null)
                LeaveCompleted(sender, e);
        }

        void OnSayCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (SayCompleted != null)
                SayCompleted(sender, e);
        }

        void OnGameActionReceived(GameAction action)
        {
            if (GameActionReceived != null)
                GameActionReceived(action);
        }

        void OnLobbyActionReceived(LobbyAction action)
        {
            if (LobbyActionReceived != null)
                LobbyActionReceived(action);
        }

        void OnTurnActionReceived(TurnAction action)
        {
            if (TurnActionReceived != null)
                TurnActionReceived(action);
        }
        void OnInGameActionReceived(InGameAction action)
        {
            if (InGameActionReceived != null)
                InGameActionReceived(action);
        }

        /// <summary>
        /// Raises the event for connected subscribers
        /// </summary>
        /// <param name="e"><see cref="ProxyCallBackEventArgs">ProxyCallBackEventArgs</see> event args</param>
        protected void OnProxyCallBackEvent(ProxyCallBackEventArgs e)
        {
            if (ProxyCallBackEvent != null)
            {
                // Invokes the delegates. 
                ProxyCallBackEvent(this, e);
            }
        }

        /// <summary>
        /// Raises the event for connected subscribers
        /// </summary>
        /// <param name="e"><see cref="ProxyEventArgs">ProxyEventArgs</see> event args</param>
        protected void OnProxyEvent(ProxyEventArgs e)
        {
            if (ProxyEvent != null)
            {
                // Invokes the delegates. 
                ProxyEvent(this, e);
            }
        }

        /// <summary>
        /// Returns a singleton <see cref="Proxy_Singleton">Proxy_Singleton</see>
        /// </summary>
        /// <returns>a singleton <see cref="Proxy_Singleton">Proxy_Singleton</see></returns>
        public static Server Instance
        {
            get
            {
                //critical section, which ensures the singleton
                //is thread safe
                lock (singletonLock)
                {
                    if (singleton == null)
                    {
                        singleton = new Server();
                    }
                    return singleton;
                }
            }
        }

        /// <summary>
        /// First calls the ChatProxy.Leave (ClientBase&ltIChat&gt.Leave()) 
        /// method, and finally calls the AbortProxy() method
        /// </summary>
        public void ExitChatSession()
        {
            try
            {
                _NetworkProxy.Leave();
            }
            catch { }
            finally
            {
                AbortProxy();
            }
        }

        /// <summary>
        /// Calls the ChatProxy.Abort (ClientBase&ltIChat&gt.Abort()) and also
        /// the ChatProxy.Close (ClientBase&ltIChat&gt.Close()) methods
        /// </summary>
        public void AbortProxy()
        {
            if (_NetworkProxy != null)
            {
                _NetworkProxy.Abort();
                _NetworkProxy.Close();
                _NetworkProxy = null;
            }
        }

        public IAsyncResult BeginReceive(GameAction action, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceive(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #region IChat Members
        // Since we act as a proxy between the client and the network or hotseat
        // server, we bubble through all calls to the current proxy

        public JoinResult Join(XmlUserCredentials credentials)
        {
            return _Proxy.Join(credentials);
        }

        public IAsyncResult BeginJoin(XmlUserCredentials credentials, AsyncCallback callback, object asyncState)
        {
            return _Proxy.BeginJoin(credentials, callback, asyncState);
        }

        public JoinResult EndJoin(IAsyncResult result)
        {
            return _Proxy.EndJoin(result);
        }

        public void Leave()
        {
            _Proxy.Leave();
        }

        public IAsyncResult BeginLeave(AsyncCallback callback, object asyncState)
        {
            return _Proxy.BeginLeave(callback, asyncState);
        }

        public void EndLeave(IAsyncResult result)
        {
            _Proxy.EndLeave(result);
        }

        public void Say(GameAction action)
        {
            _Proxy.Say(action);
        }

        public IAsyncResult BeginSay(GameAction action, AsyncCallback callback, object asyncState)
        {
            return _Proxy.BeginSay(action, callback, asyncState);
        }

        public void EndSay(IAsyncResult result)
        {
            _Proxy.EndSay(result);
        }

        #endregion
    }
    #endregion
}
