using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
using System.Data.Entity;
using System.Linq;

//using SettleInData;
using SettleInCommon.Actions;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.Result;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Actions.Lobby;
using SettleInCommon;
using System.Runtime.Remoting.Messaging;

namespace GameServer
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
        ConcurrencyMode = ConcurrencyMode.Multiple,
        AddressFilterMode = AddressFilterMode.Any)]
    public class GameService : IChat, IDisposable
    {
        #region Static fields

        //thread sync lock object
        private static Object threadSync = new Object();

        //holds a list of chatters, and a delegate to allow the BroadcastEvent to work
        //out which chatter delegate to invoke
        private static Dictionary<XmlUser, GameEventHandler> _Users = new Dictionary<XmlUser, GameEventHandler>();

        //list of games
        private static List<ServerGame> _Games = new List<ServerGame>();

        //Chatlog of the lobby
        private static XmlChatLog _LobbyChatLog = new XmlChatLog();

        //The server as a player
        private static XmlUser _ServerUser = new XmlUser() { ID = 0, Name = "Server" };

        private static int _ConnectedClientsCount = 0;

        public static event GameEventHandler ChatEvent;

        #endregion

        #region Instance fields

        //callback interface for clients
        IChatCallback _Callback = null;

        //delegate used for BroadcastEvent
        public delegate void GameEventHandler(object sender, GameEventArgs e);

        private GameEventHandler myEventHandler = null;

        // Session user 
        private XmlUser _CurrentPerson;

        //private SIEntities _Entities;

        //private UserAdministration _UserAdministration = new UserAdministration();

        #endregion

        #region Constructors

        static GameService()
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
        private bool PlayerExists(int id)
        {
            return (from u in _Users.Keys
                    where u.ID == id
                    select u).Count() > 0;

        }

        /// <summary>
        /// Searches the intenal list of chatters for a particular person, and returns
        /// the individual chatters ChatEventHandler delegate in order that it can be
        /// invoked
        /// </summary>
        /// <param name="name">the name of the <see cref="Common.Person">Person</see> to find</param>
        /// <returns>The True ChatEventHandler delegate for the <see cref="Common.Person">Person</see> who matched
        /// the name input parameter</returns>
        private GameEventHandler GetPersonHandler(int userID)
        {
            XmlUser user = (from u in _Users.Keys
                            where u.ID == userID
                            select u).Single();

            return GetUserHandler(user);
        }

        private GameEventHandler GetUserHandler(XmlUser user)
        {
            return _Users[user];
        }

        private XmlUser GetUser(int userID)
        {
            return (from u in _Users.Keys
                    where u.ID == userID
                    select u).Single();
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
            message.Sender = _ServerUser.ID;

            if (person == null)
            {
                message.Message = "No valid person passed (Usercredentials object == null)";
                return new JoinResult() { FailMessage = message };
            }
            if (String.IsNullOrEmpty(person.Name) ||
                String.IsNullOrEmpty(person.Password))
            {
                message.Message = "either password or username isnt valid: empty password or username";
                return new JoinResult() { FailMessage = message };
            }
            /*
            XmlUser user = _UserAdministration.Authenticate(person);

            // we failed to authenticate, communicate this back to user
            if (user == null)
            {
                message.Message = "either password or username isnt valid, or both";
                return new JoinResult() { FailMessage = message };
            }
             */

            //Yey! we are authenticated. Add user to list of users and 
            // return the lobby state

            bool userAdded = false;
            //create a new ChatEventHandler delegate, pointing to the MyEventHandler() method
            myEventHandler = new GameEventHandler(MyEventHandler);

            //carry out a critical section that checks to see if the new chatter
            //name is already in use, if its not allow the new chatter to be
            //added to the list of chatters, using the person as the key, and the
            //ChatEventHandler delegate as the value, for later invocation
            lock (threadSync)
            {
                /*
                if (!PlayerExists(user.ID))
                {
                    _Users.Add(user, MyEventHandler);
                    userAdded = true;
                    _CurrentPerson = user;
                }
                else
                {
                    message.Message = "It seems that you are already logged in.";
                    return new JoinResult() { FailMessage = message };
                }
                 */
            }

            //if the new chatter could be successfully added, get a callback instance
            //create a new message, and broadcast it to all other chatters, and then 
            //return the list of al chatters such that connected clients may show a
            //list of all the chatters
            if (userAdded)
            {
                _Callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();

                //add this newly joined chatters ChatEventHandler delegate, to the global
                //multicast delegate for invocation
                ChatEvent += myEventHandler;

                XmlLobbyState lobbyState = new XmlLobbyState();

                //carry out a critical section that copy all chatters to a new list
                lock (threadSync)
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
                        foreach (KeyValuePair<XmlUser, GameEventHandler> lobbyUser in _Users)
                            lobbyState.Users.Add(lobbyUser.Key.Copy());
                    }
                    //copy games
                    if (_Games != null)
                    {
                        lobbyState.Games = CreateGameList();
                    }
                }
                /*
                //Say to all other  players we have a new player joined in the lobby
                LobbyJoinedAction lobbyJoined = new LobbyJoinedAction() { NewPlayer = user, Sender = _ServerUser.ID };

                BroadcastMessage(lobbyJoined, user);

                return new JoinResult() { User = user, LobbyState = lobbyState };
                 */
                return null;
            }
            else
            {
                message.Message = "You are already in the list of logged in players!";
                message.Sender = _ServerUser.ID;

                return null;
            }
        }
        public List<XmlGame> CreateGameList()
        {
            List<XmlGame> result = new List<XmlGame>();

            foreach (ServerGame game in _Games)
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
            if (!action.IsValid())
            {
                SayError(action.Sender, "Invalid action! The action object said: _ /r/n/r/n" + action.InvalidMessage);
            }
            else
            {
                ExecuteAction(action);
            }
        }

        private void SayError(int userID, string error)
        {
            MessageFromServerAction message = new MessageFromServerAction();
            message.Message = error;
            Whisper(userID, message);
        }

        private void ExecuteAction(GameAction action)
        {
            // the user should exist in our list of loggedin users
            XmlUser user = (from u in _Users.Keys
                            where u.ID == action.Sender
                            select u).Single();
            if (user == null)
            {
                SayError(_CurrentPerson.ID,
                    "Hey! I thought you are someone else. Tampering with input info aint cool. If you aint, go hunt some developer");
                return;
            }
            if (action is InGameAction)
            {
                ExecuteInGameAction((InGameAction)action);
            }
            if (action is LobbyAction)
            {
                ExecuteLobbyAction((LobbyAction)action);
            }
        }

        /// <summary>
        /// Executes a given InGameAction
        /// </summary>
        /// <param name="inGameAction"></param>
        private void ExecuteInGameAction(InGameAction action)
        {
            ServerGame serverGame = (from g in _Games
                                     where g.Game.ID == action.GameID
                                     select g).Single();

            if (!(action).IsValid(serverGame.Game))
            {
                SayError(_CurrentPerson.ID, "Invalid InGameAction object");
            }
            else
            {
                GameAction inGameAction = serverGame.ExecuteAction((InGameAction)action);
                TurnAction turnAction = inGameAction as TurnAction;

                if (turnAction != null)
                // When we have a turnaction, we must nullify the data sent to
                // spectators and players. No need for them to know what kind of
                // development cards or resources a player has.
                // The data should not even reside on a clients computer. 
                {
                    // before sending, make sure the actual player gets the 
                    // right data
                    Whisper(action.Sender, turnAction);

                    //Now we can nullify it
                    turnAction.NullifyData();

                    //to send it to spectators and players
                    foreach (XmlUser spectator in serverGame.Spectators)
                        Whisper(spectator.ID, turnAction);

                    foreach (XmlUser player in serverGame.Users)
                    {
                        //we already sent it to the actual player, so omit him
                        if (player.ID != action.Sender)
                            Whisper(player.ID, turnAction);
                    }
                }
                else
                {
                    if (inGameAction != null)
                    {
                        // Send message without nullifying data (no need for ingameactions)
                        // to all clients (players & spectators)
                        foreach (XmlUser client in serverGame.Users.Union<XmlUser>(serverGame.Spectators))
                            Whisper(client.ID, inGameAction);
                    }
                    else
                    //Some error occured, only reflect back to the originating client
                    {
                        Whisper(action.Sender, inGameAction);
                    }
                }
            }

        }

        private void ExecuteLobbyAction(LobbyAction lobbyAction)
        {
            if (lobbyAction is TryCreateGameAction)
            {
                TryCreateNewGame(lobbyAction);
            }
            if (lobbyAction is LobbyChatAction)
            {
                SayToLobby(lobbyAction);
            }
            if (lobbyAction is JoinGameAction)
            {
                TryJoinGame((JoinGameAction)lobbyAction);
            }

        }

        private void TryHandleGameAction(InGameAction action)
        {
            if (CheckIfPlayerBelongsToGame(action.GameID, action.Sender))
            {
                //HandleGameAction(action);
            }
            else
            {
                //notify the user some mismatch happened
                MessageFromServerAction response = new MessageFromServerAction();

                response.Message = String.Format(
                    "You, player {0} do not belong to game with ID {1}",
                    action.GameID, _CurrentPerson.Name);

                response.Sender = _ServerUser.ID;
                Whisper(action.Sender, response);
                return;
            }
        }

        private bool CheckIfPlayerBelongsToGame(int gameID, int userID)
        {
            //logic checking if the sender of the action corresponds with a running game ID
            return true;
        }

        private void SayToLobby(GameAction action)
        {
            LobbyChatAction ca = new LobbyChatAction();
            ca.Sender = action.Sender;
            ca.ChatMessage = ((LobbyChatAction)action).ChatMessage;

            //_LobbyChatLog.Say((LobbyChatAction)action);

            BroadcastMessage(ca, null);
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
                    messageBack.Sender = _ServerUser.ID;

                    Whisper(action.Sender, messageBack);
                    return;
                }

                //check if player is already in another game
                if (game.Users != null)
                {
                    foreach (XmlUser player in game.Users)
                    {
                        if (player.Equals(action.Sender))
                        {
                            string message = String.Format("You are already in the game '{0}'", game.Game.Settings.Name);

                            MessageFromServerAction messageBack =
                                new MessageFromServerAction();
                            messageBack.Message = message;
                            messageBack.Sender = _ServerUser.ID;

                            Whisper(action.Sender, messageBack);
                            return;
                        }
                    }
                }

            }

            int gameID = GetNewGameID();
            // create game, user is not in another game hosting or playing
            ServerGame serverGame =
                new ServerGame(((TryCreateGameAction)action).GameSettings);
            serverGame.Host = _CurrentPerson;
            serverGame.ID = gameID;
            serverGame.Users.Add(_CurrentPerson);
            _Games.Add(serverGame);

            //create the message
            GameCreatedAction newGameMessage = new GameCreatedAction();
            newGameMessage.Sender = _ServerUser.ID;
            newGameMessage.ID = gameID;
            newGameMessage.Game = ((TryCreateGameAction)action).GameSettings;

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
        public void Whisper(int userID, GameAction action)
        {
            GameEventArgs e = new GameEventArgs();
            e.Person = _ServerUser.ID;
            e.Action = action;
            try
            {
                GameEventHandler chatterTo;
                //carry out a critical section, that attempts to find the 
                //correct Person in the list of chatters.
                //if a person match is found, the matched chatters 
                //ChatEventHandler delegate is invoked asynchronously
                lock (threadSync)
                {
                    chatterTo = GetPersonHandler(userID);
                    if (chatterTo == null)
                    {
                        throw new KeyNotFoundException("The person \" " + GetUser(userID).Name +
                                                        "\" could not be found");
                    }
                }
                //do a async invoke on the chatter (call the MyEventHandler() method, and the
                //EndAsync() method at the end of the asynch call
                chatterTo.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("whispering failed, could not find the player with ID " + e.Person);
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
            if (_CurrentPerson == null)
                return;

            //get the chatters ChatEventHandler delegate
            GameEventHandler chatterToRemove = GetPersonHandler(_CurrentPerson.ID);

            //carry out a critical section, that removes the chatter from the
            //internal list of chatters
            lock (threadSync)
            {
                _Users.Remove(_CurrentPerson);
            }
            //unwire the chatters delegate from the multicast delegate, so that 
            //it no longer gets invoked by globally broadcasted methods
            ChatEvent -= chatterToRemove;

            GameEventArgs e = new GameEventArgs()
            {
                Person = _CurrentPerson.ID,
                Action = new UserLeftAction()
                {
                    Sender = _CurrentPerson.ID,
                    DateTime = DateTime.Now
                }
            };
            _CurrentPerson = null;

            //broadcast this leave message to all other remaining connected
            //chatters
            BroadcastMessage(e, null);
        }

        #endregion

        #region private methods

        public GameService()
        {
            //_Entities = new SIEntities();
            //_Entities.Connection.Open();
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
                _Callback.Receive(e.Action);
            }
            catch
            {
                Leave();
            }
        }

        /// <summary>
        /// Broadcast the message to all participants of a game
        /// </summary>
        /// <param name="action">Action to broadcast</param>
        /// <param name="game">The game with the participants</param>
        private void BroadcastMessage(InGameAction action, ServerGame game)
        {
            // broadcast message to all users
            foreach (XmlUser user in game.Users)
                BroadcastMessage(action, user);

            // broadcast message to all spectators
            foreach (XmlUser spectator in game.Spectators)
                BroadcastMessage(action, spectator);
        }

        private void BroadcastMessage(GameAction action, XmlUser skipUser)
        {
            GameEventArgs e = new GameEventArgs();
            e.Person = action.Sender;
            e.Action = action;

            BroadcastMessage(e, skipUser);
        }

        /// <summary>
        /// loop through all connected chatters and invoke their 
        /// ChatEventHandler delegate asynchronously, which will firstly call
        /// the MyEventHandler() method and will allow a asynch callback to call
        /// the EndAsync() method on completion of the initial call
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
                GameEventHandler skipHandler = null;
                if (skipUser != null)
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
                AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((GameEventHandler)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                ChatEvent -= d;
            }
        }

        /// <summary>
        /// Removes user from the list of logged in users
        /// </summary>
        private void MarkUserNotConnected()
        {
            if (_CurrentPerson != null)
            {
                _Users.Remove(_CurrentPerson);

                // say to the lobby a user has been disconnected
                UserDisconnectedAction disconnected = new UserDisconnectedAction()
                {
                    UserID = _CurrentPerson.ID,
                    Sender = _ServerUser.ID,
                    DateTime = DateTime.Now
                };
                BroadcastMessage(disconnected, null);
            }
        }

        private int GetNewGameID()
        {
            return 1;
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Object is being destroyed, so a disconnection has been made.
        /// </summary>
        public void Dispose()
        {
            MarkUserNotConnected();
        }

        #endregion

        #region Lobby

        private void TryJoinGame(JoinGameAction joinGame)
        {
            // check if the game ID is a game which is not started yet


            // player canot be already in another game
            IEnumerable<ServerGame> games = from g in _Games
                                            where (from s in g.Spectators
                                                   where s.ID == joinGame.Sender
                                                   select s).Count() > 0
                                            select g;
            if (games.Count() > 0)
            {
                SayError(joinGame.Sender, "You are already in another game");
                return;
            }

            JoinGame(joinGame);
        }

        private void JoinGame(JoinGameAction joinGameAction)
        {
            lock (threadSync)
            {
                ServerGame game = (from g in _Games
                                   where g.Game.ID == joinGameAction.GameID
                                   select g).Single();

                game.Game.Spectators.Add(_CurrentPerson);
            }
            GameJoinedAction gameJoined = new GameJoinedAction()
            {
                DateTime = DateTime.Now,
                GameID = joinGameAction.GameID,
                Sender = joinGameAction.Sender
            };
            BroadcastMessage(gameJoined, null);
        }

        private void LeaveLobby(UserLeftLobbyAction userLeftLobbyAction)
        {
            lock (threadSync)
            {
                _Users.Remove(_CurrentPerson);
            }

        }

        #endregion
    }
    #endregion
}

