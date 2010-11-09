using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleIn.GameServer;
using SettleInCommon.Gaming;
using SettleInCommon.Board;
using SettleInCommon.User;
using SettleInCommon.Gaming.DevCards;
using System.Collections.ObjectModel;
using SettleInCommon.Actions;
using SettleInCommon.Actions.Result;
using System.ComponentModel;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.Lobby;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Gaming.GamePhases;

namespace SettleIn
{
    public class HotseatServer : IChat, IChatCallback
    {
        XmlGame _Game;
        IChatCallback _CallBack;
        Random _Random = new Random();

        public HotseatServer(IChatCallback callback)
        {
            _CallBack = callback;
        }

        public event System.EventHandler<JoinCompletedEventArgs> JoinCompleted;
        public event System.EventHandler<AsyncCompletedEventArgs> LeaveCompleted;
        public event System.EventHandler<AsyncCompletedEventArgs> SayCompleted;

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

        #region IChat Members

        public JoinResult Join(XmlUserCredentials credentials)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginJoin(XmlUserCredentials credentials, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public JoinResult EndJoin(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void Leave()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginLeave(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndLeave(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void Say(GameAction action)
        {
            InGameAction inGameAction = action as InGameAction;
            if (inGameAction != null && !(action is HostStartsGameAction))
            {
                if (!inGameAction.IsValid(_Game))
                {
                    _CallBack.Receive(new MessageFromServerAction()
                    {
                        Message = String.Format("Invalid action! \r\n Reason: {0}",
                            inGameAction.InvalidMessage),
                        Sender = Core.Instance.ServerUser.ID,
                        DateTime = DateTime.Now
                    });
                    return;
                }
                if (_Game.ActionsQueue.Count > 0)
                // we have some expected actions in the queue, process them first
                {
                    if (_Game.ActionsQueue.IsExpected(inGameAction, _Game))
                    {
                        // we encountered an expected action on the queue
                        _Game.ActionsQueue.Dequeue();
                    }
                    else
                    {
                        // Grab the expected action
                        InGameAction expected = _Game.ActionsQueue.Peek();

                        // Notify we did not xpect that action
                        _CallBack.Receive(new MessageFromServerAction()
                        {
                            Message = String.Format("Invalid action! \r\n Reason: Expected {0} from player {1}, but instead got {2} from player {3}",
                                expected.GetType().ToString(), _Game.GetPlayer(expected.Sender),
                                inGameAction.GetType().ToString(), _Game.GetPlayer(inGameAction.Sender)),
                            Sender = Core.Instance.ServerUser.ID,
                            DateTime = DateTime.Now
                        });
                        return;
                    }
                }
            }
            HostStartsGameAction startGame = action as HostStartsGameAction;
            if (startGame != null)
            {
                CreateNewGame(startGame);
                StartGameAction startIt = new StartGameAction()
                {
                    Game = _Game.Copy(),
                    Sender = Core.Instance.ServerUser.ID,
                };
                inGameAction = startIt;
                _Game.Phase = _Game.Phase.Next(_Game);
                _Game.Phase.Start(_Game);
            }

            // Make sure to check if we need to switch a phase

            ProcessAction(inGameAction);

            // A phase switches when the first action on the queue equals the action type specified
            // by the current phase
            if (_Game.ActionsQueue.Count() > 0 &&
                _Game.ActionsQueue.Peek().GetType() == _Game.Phase.EndAction())
            {
                // First process the action switching phases
                InGameAction actionToProcess = _Game.ActionsQueue.Dequeue();
                ProcessAction(actionToProcess);

                // Swith to next phase and then start the phase
                _Game.Phase = _Game.Phase.Next(_Game);
                _Game.Phase.Start(_Game);
            }

            // If we have a notification, such as longest road, largest army or traderoutes changed,
            // server should initiate sending message
            if (_Game.ActionsQueue.Count > 0)
            {
                // Notify players of the next phase if present
                if (_Game.ActionsQueue.Peek().Sender == 0)
                {
                    InGameAction serverNotification = _Game.ActionsQueue.Dequeue();
                    _CallBack.Receive(serverNotification);
                    _Game.Phase.ProcessAction(serverNotification, _Game);
                }
            }
        }

        private void ProcessAction(InGameAction action)
        {
            BuildRoadAction buildRoad = action as BuildRoadAction;
            if (buildRoad != null)
            {
                /*
                buildRoad.Intersection.HexPoint1.
                IEnumerable<Dis = _Game.Board.Hexes.OfType<DiscoveryHex>
                foreach (HexLocation hex in _Game.Board.Hexes.Where(h=>h.Location.Equals(
                 */
            }
            RollVolcanoDiceAction rollVolcanoDice = action as RollVolcanoDiceAction;
            if (rollVolcanoDice != null)
            {
                // Roll the volcano dice
                rollVolcanoDice.Dice = _Random.Next(1, 6);
            }
            RollDiceAction rollDice = action as RollDiceAction;
            if (rollDice != null)
            {
                RollDice(rollDice);
                return;
            }
            GameChatAction gamechat = action as GameChatAction;
            if (gamechat != null)
            {
                gamechat.PerformTurnAction(_Game);
                _CallBack.Receive(gamechat);
                return;
            }

            RobPlayerAction robPlayer = action as RobPlayerAction;
            if (robPlayer != null)
            {
                if (robPlayer.PlayerID != null)
                {
                    // Get the victim
                    GamePlayer victim = _Game.GetPlayer((int)robPlayer.PlayerID);
                    
                    // Set the stolen resource
                    // Here we fool the player into thinking he has already picked a resource.
                    // Actually, the UI for the player to steal from another player is just a fake ui.
                    // The purpose of the UI is solely choosing a player to rob from, not the specific 
                    // resource being robbed. The actual stolen resource is determined below.
                    // This is done on purpose, to let the player think he has control, so players won't
                    // get agitated over that. 
                    // Randomization call should remain on server, to avoid tampering
                    robPlayer.StolenResource = victim.Resources[_Random.Next(0, victim.Resources.Count - 1)];
                    action = robPlayer;
                }
            }

            BuyDevcardAction buyDevcard = action as BuyDevcardAction;
            if (buyDevcard != null)
            {
                // Do devcards administration
                buyDevcard.DevCard = _Game.DevCards.Last();
                _Game.DevCards.Remove(_Game.DevCards.Last());
            }

            // The rest of the actions simply can be performed without any additional server logic
            _CallBack.Receive(action);
            _Game.Phase.ProcessAction(action, _Game);
        }

        void CreateNewGame(HostStartsGameAction startGame)
        {
            _Game = new XmlGame()
            {
                Board = Core.Instance.Boards.ToList().Find
                           (b => b.ID == startGame.Settings.Map),
                Settings = startGame.Settings,
                Bank = new ResourceList(19, 19, 19, 19, 19, 100),
                Robber = new HexLocation(0, 0),
                Pirate = new HexLocation(0, 1),
                Players = new List<GamePlayer>()
                   {
                       new GamePlayer()
                       {
                           XmlPlayer = new XmlUser()
                           {
                               Name="Piet",
                               ID=1
                           },
                           Color = "#FF0000",
                           IsOnTurn=true
                       },
                       new GamePlayer()
                       {
                           XmlPlayer = new XmlUser()
                           {
                               Name="Henk",
                               ID=2
                           },
                           Color = "#FFFF00"
                       },
                       new GamePlayer()
                       {
                           XmlPlayer = new XmlUser()
                           {
                               Name="Truus",
                               ID=3
                           },
                           Color = "#0000FF"
                       },
                       new GamePlayer()
                       {
                           XmlPlayer = new XmlUser()
                           {
                               Name="Klaas",
                               ID=4
                           },
                           Color = "#00FF00"
                       }
                   }
            };
            _Game.DevCards = ShuffleDevcardsDeck(StandardDevCardStack.NormalStack);
            _Game.Board.PrepareForPlay(startGame.Settings);
            _Game.LongestRouteChanged += new XmlGame.LongestRouteChangedHandler(_Game_LongestRouteChanged);
            _Game.LargestArmyChanged += new XmlGame.LargestArmyChangedHandler(_Game_LargestArmyChanged);
        }


        void _Game_LargestArmyChanged(GamePlayer player)
        {
            LargestArmyAchievedAction newLargestArmy = new LargestArmyAchievedAction()
            {
                GamePlayer = player
            };
            newLargestArmy.PerformTurnAction(_Game);
            _CallBack.Receive(newLargestArmy);
        }

        void _Game_LongestRouteChanged(GamePlayer player, Route newRoute)
        {
            LongestRouteAchievedAction newLongestRoute = new LongestRouteAchievedAction()
            {
                GamePlayer = player,
                Route = newRoute
            };
            newLongestRoute.PerformTurnAction(_Game);
            _CallBack.Receive(newLongestRoute);
        }

        private ObservableCollection<DevelopmentCard> ShuffleDevcardsDeck(StandardDevCardStack standardDevCardStack)
        {
            List<DevelopmentCard> result = new List<DevelopmentCard>();

            // Create a list of all development cards

            // initialize list
            List<DevelopmentCard> allDevs = new List<DevelopmentCard>();

            // Add development cards of each type to the list
            for (int i = 0; i < standardDevCardStack.YopCount; i++)
                allDevs.Add(new YearOfPlenty());
            for (int i = 0; i < standardDevCardStack.RobberCount; i++)
                allDevs.Add(new Soldier());
            for (int i = 0; i < standardDevCardStack.MonoCount; i++)
                allDevs.Add(new Monopoly());
            for (int i = 0; i < standardDevCardStack.VpCount; i++)
                allDevs.Add(new VictoryPoint());
            for (int i = 0; i < standardDevCardStack.RbCount; i++)
                allDevs.Add(new RoadBuilding());

            // Create a list to associate random value to each development card
            List<KeyValuePair<int, DevelopmentCard>> list =
                new List<KeyValuePair<int, DevelopmentCard>>();

            // Associate the random int value to each development card
            foreach (DevelopmentCard dev in allDevs)
                list.Add(new KeyValuePair<int, DevelopmentCard>(_Random.Next(), dev));

            // Sort the list by the random number
            var sorted = from item in list
                         orderby item.Key
                         select item;

            // Copy values to resultlist and add proper ID
            int id = 0;
            foreach (KeyValuePair<int, DevelopmentCard> pair in sorted)
            {
                pair.Value.ID = id;
                result.Add(pair.Value);
                id++;
            }

            return new ObservableCollection<DevelopmentCard>(result);
        }

        private void RollDice(RollDiceAction rollDice)
        {
            // Do actual dicerolling
            // TODO: add call to dicerolling webservice voor 100% randomness
            rollDice.Dice1 = _Random.Next(1, 6);
            rollDice.Dice2 = _Random.Next(1, 6);

            // Notify players a player rolled dice
            _CallBack.Receive(rollDice);

            // Perform the action
            _Game.Phase.ProcessAction(rollDice, _Game);
        }

        public IAsyncResult BeginSay(GameAction action, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndSay(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IChatCallback Members

        public void Receive(GameAction action)
        {

        }

        public IAsyncResult BeginReceive(GameAction action, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceive(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
