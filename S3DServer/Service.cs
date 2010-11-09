using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data.SQLite;
using System.Data;
using System.Data.Objects;
using System.Linq;

using SettleInCommon;
using SettleInCommon.Actions;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInData;
using SettleInCommon.Actions;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.Result;
using SettleInCommon.Actions.Turn;
using SettleInCommon.Actions.Lobby;

namespace SettleInServer
{
    #region ChatService
    /// <summary>
    /// This class provides the service that is used by all clients. This class
    /// uses the bindings as specified in the App.Config, to allow a true peer-2-peer
    /// chat to be perfomed.
    /// 
    /// This class also implements the <see cref="IChat">IChat</see> interface in order
    /// to facilitate a common chat interface for all chat clients
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerSession, 
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChat, IDisposable
    {
        #region Static fields

        //thread sync lock object
        private static Object syncObj = new Object();

        //holds a list of chatters, and a delegate to allow the BroadcastEvent to work
        //out which chatter delegate to invoke
        static Dictionary<XmlUser, GameEventHandler> _Users = new Dictionary<XmlUser, GameEventHandler>();

        //list of games
        static List<ServerGame> _Games = new List<ServerGame>();
        
        //Chatlog of the lobby
        static XmlChatLog _LobbyChatLog = new XmlChatLog();

        //The server as a player
        static XmlUser _ServerUser = new XmlUser("Server");

        static int _ConnectedClientsCount = 0;

        public static event GameEventHandler ChatEvent;

        #endregion

        #region Instance fields

        //callback interface for clients
        IChatCallback callback = null;

        //delegate used for BroadcastEvent
        public delegate void GameEventHandler(object sender, GameEventArgs e);

        private GameEventHandler myEventHandler = null;

        // Session user 
        private XmlUser _CurrentPerson;

        private SIEntities _Entities;

        private UserAdministration _UserAdministration = new UserAdministration();

        #endregion

        #region Constructors

        static ChatService()
        {

        }

        #endregion

        #region Helpers

        /// <summary>
        /// Searches the intenal list of chatters for a particular person, and returns
        /// true if the person could be found
        /// </summary>
        /// <param name="name">the name of the <see cref="Common.Person">Person</see> to find</param>
        /// <returns>True if the <see cref="Common.Person">Person</see> was found in the
        /// internal list of chatters</returns>
        private bool checkIfPersonExists(string name)
        {
            foreach (XmlUser p in _Users.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>
        /// Searches the intenal list of chatters for a particular person, and returns
        /// the individual chatters ChatEventHandler delegate in order that it can be
        /// invoked
        /// </summary>
        /// <param name="name">the name of the <see cref="Common.Person">Person</see> to find</param>
        /// <returns>The True ChatEventHandler delegate for the <see cref="Common.Person">Person</see> who matched
        /// the name input parameter</returns>
        private GameEventHandler getPersonHandler(string name)
        {
            foreach (XmlUser p in _Users.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    GameEventHandler chatTo = null;
                    _Users.TryGetValue(p, out chatTo);
                    return chatTo;
                }
            }
            return null;
        }

        /// <summary>
        /// Searches the intenal list of chatters for a particular person, and returns
        /// the actual <see cref="Common.Person">Person</see> whos name matches
        /// the name input parameter
        /// </summary>
        /// <param name="name">the name of the <see cref="Common.Person">Person</see> to find</param>
        /// <returns>The actual <see cref="Common.Person">Person</see> whos name matches
        /// the name input parameter</returns>
        private XmlUser getPerson(string name)
        {
            foreach (XmlUser p in _Users.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return p;
                }
            }
            return null;
        }

        #endregion

        #region Join/Leave

        /// <summary>
        /// Takes a <see cref="Common.Person">Person</see> and allows them
        /// to join the chat room, if there is not already a chatter with
        /// the same name
        /// </summary>
        /// <param name="person"><see cref="Common.Person">Person</see> joining</param>
        /// <returns>An array of <see cref="Common.Person">Person</see> objects</returns>
        public JoinResult Join(XmlUserCredentials person)
        {
            MessageFromServerAction message = new MessageFromServerAction();
            message.Sender = _ServerUser;
             
            if (person == null)
            {
                message.Message = "No valid person passed (Usercredentials object == null)";
                return new JoinResult(false, message);
            }
            if (String.IsNullOrEmpty(person.Name) ||
                String.IsNullOrEmpty(person.Password))
            {
                message.Message = "either password or username isnt valid: empty password or username";
                return new JoinResult(false, message);
            }
            XmlUser user = _UserAdministration.Authenticate(person);

            // we failed to authenticate, communicate this back to user
            if (user == null)
            {
                message.Message = "either password or username isnt valid, or both";
                return new JoinResult(false, message);
            }

            //Yey! we are authenticated. Add user to list of users and 
            // return the lobby state

            bool userAdded = false;
            //create a new ChatEventHandler delegate, pointing to the MyEventHandler() method
            myEventHandler = new GameEventHandler(MyEventHandler);

            //carry out a critical section that checks to see if the new chatter
            //name is already in use, if its not allow the new chatter to be
            //added to the list of chatters, using the person as the key, and the
            //ChatEventHandler delegate as the value, for later invocation
            lock (syncObj)
            {
                if (!checkIfPersonExists(person.Name))
                {
                    _Users.Add(user, MyEventHandler);
                    userAdded = true;
                }
                else
                {
                    message.Message = "either password or username isnt valid, or both";
                    return new JoinResult(false, message);
                }
            }

            //if the new chatter could be successfully added, get a callback instance
            //create a new message, and broadcast it to all other chatters, and then 
            //return the list of al chatters such that connected clients may show a
            //list of all the chatters
            if (userAdded)
            {
                callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
                //GameEventArgs e = new GameEventArgs();
                //e.MessageType = MessageType.UserEnter;
                //e.Person = this._CurrentPerson;
                //BroadcastMessage(e);


                //add this newly joined chatters ChatEventHandler delegate, to the global
                //multicast delegate for invocation
                ChatEvent += myEventHandler;
                XmlLobbyState lobbyState = new XmlLobbyState();

                //carry out a critical section that copy all chatters to a new list
                lock (syncObj)
                {
                    //copy chatlog
                    if (_LobbyChatLog != null)
                    {
                        lobbyState.LobbyChat = _LobbyChatLog.Copy();
                    }

                    //copy users
                    if (_Users != null)
                    {
                        lobbyState.Users = new List<XmlUser>();
                        foreach (KeyValuePair<XmlUser, GameEventHandler> key in _Users)
                            lobbyState.Users.Add(key.Key.Copy());
                    }
                    //copy games
                    if (_Games != null)
                    {
                        lobbyState.Games = CreateGameList();
                    }
                }
                //Say to all other  players we have a new player joined in the lobby
                LobbyJoinedAction lobbyJoined = new LobbyJoinedAction();
                lobbyJoined.NewPlayer = user;
                lobbyJoined.Sender = _ServerUser;
                BroadcastMessage(lobbyJoined, user);

                JoinResult result = new JoinResult(true, null);
                result.LobbyState = lobbyState;

                return result;
            }
            else
            {
                message.Message = "You are already in the list of logged in players!";
                message.Sender = _ServerUser;

                return null;
            }
        }
        public List<XmlGame> CreateGameList()
        {
            List<XmlGame> result = new List<XmlGame>();

            foreach(ServerGame game in _Games)
                result.Add(game.Game.Copy());

            return result;
        }
        /// <summary>
        /// Broadcasts the input msg parameter to all connected chatters
        /// by using the BroadcastMessage() method
        /// </summary>
        /// <param name="msg">The message to broadcast to all chatters</param>
        public void Say(GameAction action)
        {
            ExecuteAction(action);
        }

        private void ExecuteAction(GameAction action)
        {
            if (action is InGameAction)
            {
                IValidAction valid = (IValidAction)((InGameAction)action);
                if (valid != null)
                {

                    ServerGame serverGame = (from g in _Games
                                            where g.Game.ID == ((InGameAction)action).GameID
                                            select g).Single();

                    if (!valid.IsValid(serverGame.Game))
                    {
                        MessageFromServerAction message = new MessageFromServerAction();
                        message.Message = "Invalid action!";
                        Whisper(_CurrentPerson.Name, message);
                    }
                    else
                    {
                        serverGame.ExecuteAction((InGameAction)action);
                    }
                }
            }
            if (action is TryCreateGameAction)
            {
                TryCreateNewGame(action);
            }
            if (action is LobbyChatAction)
            {
                SayToLobby(action);
            }
            if (action is JoinGameAction)
            {
                TryJoinGame((JoinGameAction)action);
            }
            if (action is InGameAction)
            {
                HandleGameAction((InGameAction)action);
            }
        }

        private void TryHandleGameAction(InGameAction action)
        {
            if (CheckIfPlayerBelongsToGame(action.GameID, action.Sender.Name))
            {
                HandleGameAction(action);
            }
            else
            {
                //notify the user some mismatch happened
                MessageFromServerAction response = new MessageFromServerAction();
                
                response.Message = String.Format(
                    "You, player {0} do not belong to game with ID {1}", 
                    action.GameID, action.Sender.Name);
                
                response.Sender = _ServerUser;
                Whisper(action.Sender.Name, response);
                return;
            }
        }

        private void HandleGameAction(InGameAction action)
        {
            // get the corresponding servergame

            //execute the action on the game
        }

        private bool CheckIfPlayerBelongsToGame(int gameID, string user)
        {
            //logic checking if the sender of the action corresponds with a running game ID
            return true;
        }

        private void SayToLobby(GameAction action)
        {
            LobbyChatAction ca = new LobbyChatAction();
            ca.Sender = action.Sender;
            ca.Message = ((LobbyChatAction)action).Message;

            //_LobbyChatLog.Say((LobbyChatAction)action);

            BroadcastMessage(ca, null);
        }

        private void TryJoinGame(JoinGameAction action)
        {
            ServerGame gameToFind = null;
            
            foreach (ServerGame game in _Games)
            {
                if (game.ID == action.GameID)
                {
                    gameToFind = game;
                    break;
                }
            }

            // whoops, user send a wrong ID or game is removed
            // notify the user the game is lost into oblivion
            if (gameToFind == null)
            {
                string message = "The game you where trying to join was not found on the server";

                MessageFromServerAction messageBack = new MessageFromServerAction();
                messageBack.Message = message;
                messageBack.Sender = _ServerUser;

                Whisper(action.Sender.Name, messageBack);
                return;
            }

            //game full, notify user
            if (gameToFind != null)
            {
                if (gameToFind.Users.Count == gameToFind.Settings.MaxPlayers)
                {
                    string message = "The game you where trying to join is already full";

                    MessageFromServerAction messageBack = new MessageFromServerAction();
                    messageBack.Message = message;
                    messageBack.Sender = _ServerUser;

                    Whisper(action.Sender.Name, messageBack);
                    return;
                }

                if (gameToFind.Users.Count < gameToFind.Settings.MaxPlayers)
                {
                    gameToFind.Users.Add(action.Sender);

                    GameJoinedAction gameJoinedAction = new GameJoinedAction();
                    gameJoinedAction.GameID = action.GameID;
                    gameJoinedAction.Player = action.Sender;
                    gameJoinedAction.Sender = _ServerUser;
                    BroadcastMessage(gameJoinedAction, null);
                }
            }
        }

        private void TryCreateNewGame(GameAction action)
        {
            //check if player has already a game
            foreach (ServerGame game in _Games)
            {
                //check if player is already hosting
                if (game.Host.Equals(action.Sender))
                {
                    string message = "You are already hosting a game, game creation failed";
                    
                    MessageFromServerAction messageBack = new MessageFromServerAction();
                    messageBack.Message = message;
                    messageBack.Sender = _ServerUser;
                    
                    Whisper(action.Sender.Name, messageBack);
                    return;
                }

                //check if player is already in another game
                if (game.Users != null)
                {
                    foreach (XmlUser player in game.Users)
                    {
                        if (player.Equals(action.Sender))
                        {
                            string message = String.Format("You are already in the game '{0}'", game.Settings.Name);

                            MessageFromServerAction messageBack =
                                new MessageFromServerAction();
                            messageBack.Message = message;
                            messageBack.Sender = _ServerUser;

                            Whisper(action.Sender.Name, messageBack);
                            return;
                        }
                    }
                }

            }

            int gameID = GetNewGameID();
            // create game, user is not in another game hosting or playing
            ServerGame serverGame = 
                new ServerGame(((TryCreateGameAction)action).GameSettings);
            serverGame.Host = action.Sender;
            serverGame.ID = gameID;
            serverGame.Users.Add(action.Sender);
            _Games.Add(serverGame);

            //create the message
            GameCreatedAction newGameMessage = new GameCreatedAction();
            newGameMessage.Sender = _ServerUser;
            newGameMessage.ID = gameID;
            newGameMessage.Settings = ((TryCreateGameAction)action).GameSettings;
            newGameMessage.Settings.Host = action.Sender;
            
            //broadcast the message
            BroadcastMessage(newGameMessage, null);

        }

        /// <summary>
        /// Broadcasts the input msg parameter to all the <see cref="Common.Person">
        /// Person</see> whos name matches the to input parameter
        /// by looking up the person from the internal list of chatters
        /// and invoking their ChatEventHandler delegate asynchronously.
        /// Where the MyEventHandler() method is called at the start of the
        /// asynch call, and the EndAsync() method at the end of the asynch call
        /// </summary>
        /// <param name="to">The persons name to send the message to</param>
        /// <param name="msg">The message to broadcast to all chatters</param>
        public void Whisper(string to, GameAction action)
        {
            GameEventArgs e = new GameEventArgs();
            e.MessageType = MessageType.ReceiveWhisper;
            e.Person = _ServerUser;
            e.Action = action;
            try
            {
                GameEventHandler chatterTo;
                //carry out a critical section, that attempts to find the 
                //correct Person in the list of chatters.
                //if a person match is found, the matched chatters 
                //ChatEventHandler delegate is invoked asynchronously
                lock (syncObj)
                {
                    chatterTo = getPersonHandler(to);
                    if (chatterTo == null)
                    {
                        throw new KeyNotFoundException("The person whos name is " + to +
                                                        " could not be found");
                    }
                }
                //do a async invoke on the chatter (call the MyEventHandler() method, and the
                //EndAsync() method at the end of the asynch call
                chatterTo.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("whispering failed, could not find the player with name" + e.Person);
            }
        }
        
        /// <summary>
        /// A request has been made by a client to leave the chat room,
        /// so remove the <see cref="Common.Person">Person </see>from
        /// the internal list of chatters, and unwire the chatters
        /// delegate from the multicast delegate, so that it no longer
        /// gets invokes by globally broadcasted methods
        /// </summary>
        public void Leave()
        {
            if (this._CurrentPerson == null)
                return;

            //get the chatters ChatEventHandler delegate
            GameEventHandler chatterToRemove = getPersonHandler(this._CurrentPerson.Name);

            //carry out a critical section, that removes the chatter from the
            //internal list of chatters
            lock (syncObj)
            {
                _Users.Remove(this._CurrentPerson);
            }
            //unwire the chatters delegate from the multicast delegate, so that 
            //it no longer gets invokes by globally broadcasted methods
            ChatEvent -= chatterToRemove;
            GameEventArgs e = new GameEventArgs();
            e.MessageType = MessageType.UserLeave;
            e.Person = this._CurrentPerson;
            this._CurrentPerson = null;
            //broadcast this leave message to all other remaining connected
            //chatters
            BroadcastMessage(e, null);
        }
        #endregion
        
        #region private methods

        public ChatService()
        {
            _Entities = new SIEntities();
            _Entities.Connection.Open();
        }

        /// <summary>
        /// This method is called when ever one of the chatters
        /// ChatEventHandler delegates is invoked. When this method
        /// is called it will examine the events ChatEventArgs to see
        /// what type of message is being broadcast, and will then
        /// call the correspoding method on the clients callback interface
        /// </summary>
        /// <param name="sender">the sender, which is not used</param>
        /// <param name="e">The ChatEventArgs</param>
        private void MyEventHandler(object sender, GameEventArgs e)
        {
            try
            {
                switch (e.MessageType)
                {
                    case MessageType.Receive:
                        callback.Receive(e.Person, e.Action);
                        break;
                    case MessageType.ReceiveWhisper:
                        callback.ReceiveWhisper(e.Person, e.Action);
                        break;
                    case MessageType.UserEnter:
                        //callback.UserEnter(e.User);
                        break;
                    case MessageType.UserLeave:
                        callback.UserLeave(e.Person);
                        break;
                }
            }
            catch
            {
                Leave();
            }
        }
        private void BroadcastMessage(GameAction action, XmlUser skipUser)
        {
            GameEventArgs e = new GameEventArgs();
            e.MessageType = MessageType.Receive;
            e.Person = action.Sender;
            e.Action = action;

            BroadcastMessage(e, skipUser);
        }

        /// <summary>
        ///loop through all connected chatters and invoke their 
        ///ChatEventHandler delegate asynchronously, which will firstly call
        ///the MyEventHandler() method and will allow a asynch callback to call
        ///the EndAsync() method on completion of the initial call
        /// </summary>
        /// <param name="e">The ChatEventArgs to use to send to all connected chatters</param>
        private void BroadcastMessage(GameEventArgs e, XmlUser skipUser)
        {
            GameEventHandler temp = ChatEvent;

            //loop through all connected chatters and invoke their 
            //ChatEventHandler delegate asynchronously, which will firstly call
            //the MyEventHandler() method and will allow a asynch callback to call
            //the EndAsync() method on completion of the initial call
            if (temp != null)
            {
                GameEventHandler skipHandler=null;
                if (skipUser !=null) 
                {
                    skipHandler = (from KeyValuePair<XmlUser, GameEventHandler> u in _Users 
                              where u.Key.Name.Equals(
                              skipUser.Name, StringComparison.OrdinalIgnoreCase)
                              select u.Value).Single();
                }
                int num = 0;
                foreach (GameEventHandler handler in temp.GetInvocationList())
                {
                    if (!(skipHandler == handler))
                    {
                        handler.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
                        num++;
                    }
                }
                Console.WriteLine("broadcasting msg to {0} players of type {1}", num, e.Action.GetType().ToString());
            }
        }


        /// <summary>
        /// Is called as a callback from the asynchronous call, so simply get the
        /// delegate and do an EndInvoke on it, to signal the asynchronous call is
        /// now completed
        /// </summary>
        /// <param name="ar">The asnch result</param>
        private void EndAsync(IAsyncResult ar)
        {
            GameEventHandler d = null;

            try
            {
                //get the standard System.Runtime.Remoting.Messaging.AsyncResult,and then
                //cast it to the correct delegate type, and do an end invoke
                System.Runtime.Remoting.Messaging.AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((GameEventHandler)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                ChatEvent -= d;
            }
        }

        private int GetNewGameID()
        {
            return 1;
        }
        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            Console.WriteLine(_CurrentPerson.Name + " disconnected");
        }

        #endregion
    }
    #endregion
}

