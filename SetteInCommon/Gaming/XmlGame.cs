using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using SettleInCommon.Actions;
using SettleInCommon.Actions.InGame;
using SettleInCommon.User;
using SettleInCommon.Board;
using SettleInCommon.Gaming.DevCards;
using System.Xml;
using System.IO;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Gaming.GamePhases;

namespace SettleInCommon.Gaming
{
    [DataContract]
    [KnownType(typeof(XmlUser))]
    [KnownType(typeof(XmlGameSettings))]
    [KnownType(typeof(XmlChatLog))]
    [KnownType(typeof(GamePhase))]
    [KnownType(typeof(DetermineFirstPlayerGamePhase))]
    [KnownType(typeof(EndedGamePhase))]
    [KnownType(typeof(LobbyGamePhase))]
    [KnownType(typeof(PlacementGamePhase))]
    [KnownType(typeof(PlacePortGamePhase))]
    public class XmlGame : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Private members

        // Players playing in the game
        private List<GamePlayer> _Players = new List<GamePlayer>();

        // Spectators (mods and interested players) in the game
        private List<XmlUser> _Spectators = new List<XmlUser>();

        // Log of actions of the game
        private GameLog _GameLog = new GameLog();

        // Queue of game actions to expect
        private ActionQueue _ActionsQueue = new ActionQueue();

        // Game phase (State pattern)
        GamePhase _Phase = new LobbyGamePhase();

        [DataMember]
        public ActionQueue ActionsQueue
        {
            get { return _ActionsQueue; }
            set { _ActionsQueue = value; }
        }

        private int _CurrentTurn = 0;
        private GamePlayer _PlayerOnTurn;

        public int CurrentTurn
        {
            get { return _CurrentTurn; }
        }

        private GamePlayer _PlayingPlayer;
        private HexLocation _Pirate = new HexLocation(1, 0);

        [DataMember]
        public HexLocation Pirate
        {
            get { return _Pirate; }
            set
            {
                if (value != _Pirate)
                {
                    OnPropertyChanging("Pirate");
                    _Pirate = value;
                    OnPropertyChanged("Pirate");
                }
            }
        }
        public GamePlayer NextPlayer
        {
            get
            {
                int index = _Players.IndexOf(PlayerOnTurn) + 1;
                if (index == _Players.Count)
                {
                    index = 0;
                }
                return _Players[index];
            }
        }

        public GamePlayer PlayingPlayer
        {
            get
            {
                if (_PlayingPlayer == null)
                    if (_Players != null)
                        if (Players.Count > 0)
                            _PlayingPlayer = _Players[0];
                return _PlayingPlayer;
            }
            set
            {
                if (value != _PlayingPlayer)
                {
                    _PlayingPlayer = value;
                    OnPropertyChanged("PlayingPlayer");
                }
            }
        }

        public List<GamePlayer> GetOpponents(int playerID)
        {
            List<GamePlayer> result = new List<GamePlayer>();

            foreach (GamePlayer player in _Players)
                if (player.XmlPlayer.ID != playerID)
                    result.Add(player);

            return result;
        }

        private List<int> _WaitingForPlayers = new List<int>();

        private Route _LongestRoute = null;

        // resources of the bank
        private ResourceList _Bank = new ResourceList(19, 19, 19, 19, 19, 0);

        // the prepared board to play the game on
        private XmlBoard _Board;

        // position of robber defaults Left top position of the board
        private HexLocation _Robber = new HexLocation(0, 0);

        private XmlGameSettings _GameSettings = new XmlGameSettings();

        private ObservableCollection<DevelopmentCard> _DevCards =
            new ObservableCollection<DevelopmentCard>();

        private int _WinnerID = 0;
        public XmlGameSettings _Settings;

        #endregion

        #region Properties

        public Route LongestRoute
        {
            get
            {
                GamePlayer playerWithRoute = (from GamePlayer p in _Players
                                              where p.LongestRoute != null
                                              select p).SingleOrDefault();

                if (playerWithRoute != null)
                {
                    return playerWithRoute.LongestRoute;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns GamePlayer currently owning the route, if such a player exist
        /// Returns null when such aplayer cannot be found
        /// </summary>
        public GamePlayer LongestRouteOwner
        {
            get
            {
                return (from GamePlayer p in _Players
                        where p.LongestRoute != null
                        select p).SingleOrDefault();
            }
        }

        [DataMember]
        public int WinnerID
        {
            get { return _WinnerID; }
            set
            {
                if (value != _WinnerID)
                {
                    _WinnerID = value;
                    OnPropertyChanged("WinnerID");
                }

            }
        }

        [DataMember]
        public ObservableCollection<DevelopmentCard> DevCards
        {
            get { return _DevCards; }
            set { _DevCards = value; }
        }

        [DataMember]
        public XmlGameSettings GameSettings
        {
            get { return _GameSettings; }
            set { _GameSettings = value; }
        }

        [DataMember]
        public HexLocation Robber
        {
            get { return _Robber; }
            set
            {
                if (_Robber != value)
                {
                    OnPropertyChanging("Robber");
                    _Robber = value;
                    OnPropertyChanged("Robber");
                }
            }
        }

        [DataMember]
        public XmlBoard Board
        {
            get { return _Board; }
            set
            {
                _Board = value;
            }
        }

        [DataMember]
        public ResourceList Bank
        {
            get { return _Bank; }
            set { _Bank = value; }
        }

        [DataMember]
        public List<XmlUser> Spectators
        {
            get { return _Spectators; }
            set { _Spectators = value; }
        }

        public List<XmlUser> PlayersSpectators
        {
            get
            {
                List<XmlUser> result = new List<XmlUser>();

                foreach (XmlUser spectator in _Spectators)
                    result.Add(spectator);

                foreach (GamePlayer player in _Players)
                    result.Add(player.XmlPlayer);

                return result;
            }
        }
        [DataMember]
        public GamePhase Phase
        {
            get { return _Phase; }
            set 
            {
                if (value != _Phase)
                {
                    _Phase = value;
                    OnPropertyChanged("Phase");
                }
            }
        }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public XmlGameSettings Settings
        {
            get { return _Settings; }
            set { _Settings = value; }
        }

        [DataMember]
        public XmlUser Host { get; set; }

        [DataMember]
        public List<GamePlayer> Players
        {
            get { return _Players; }
            set 
            {
                if (_Players != value)
                {
                    _Players = value;
                    OnPropertyChanged("Players");
                }
            }
        }

        [DataMember]
        public XmlChatLog GameChat { get; set; }

        [DataMember]
        public GameLog GameLog
        {
            get { return _GameLog; }
            set
            {
                if (_GameLog != value)
                {
                    _GameLog = value;
                    OnPropertyChanged("GameLog");
                }
            }
        }


        public GamePlayer PlayerOnTurn
        {
            set
            {
                PlayerOnTurn.IsOnTurn = false;
                _PlayerOnTurn = value;
                PlayerOnTurn.IsOnTurn = true;
                OnPropertyChanged("PlayerOnTurn");
            }
            get
            {
                if (_PlayerOnTurn == null)
                {
                    _PlayerOnTurn = _Players[0];
                }
                return _PlayerOnTurn;
            }
        }

        public void EndTurn()
        {
            PlayerOnTurn.IsOnTurn = false;
            _CurrentTurn++;
            OnPropertyChanged("PlayerOnTurn");
            PlayerOnTurn.IsOnTurn = true;
        }

        public List<HexPoint> AllTownsCities()
        {
            List<HexPoint> result = new List<HexPoint>();

            foreach (GamePlayer player in Players)
                result.AddRange(player.GetTownsCities());

            return result;
        }

        public List<HexSide> AllRoadsShips()
        {
            List<HexSide> result = new List<HexSide>();

            foreach (GamePlayer player in Players)
                result.AddRange(player.GetRoadsShips());

            return result;

        }

        public List<HexPoint> AllTowns()
        {
            List<HexPoint> result = new List<HexPoint>();

            foreach (GamePlayer player in _Players)
                result.AddRange(player.Towns);

            return result;
        }

        public List<HexPoint> AllCities()
        {
            List<HexPoint> result = new List<HexPoint>();

            foreach (GamePlayer player in _Players)
                result.AddRange(player.Cities);

            return result;
        }


        public int ConnectedPlayersCount
        {
            get
            {
                if (Players != null)
                {
                    return (from XmlUser u in Players
                            where u != null
                            select u).Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion

        #region Constructors

        public XmlGame() { }

        public XmlGame(int id, int maxPlayers)
        {
            ID = id;
            //Players = new XmlUser[maxPlayers];

        }

        #endregion

        #region Public methods

        public bool MustPlacePorts()
        {
            foreach (Territory t in Board.Territories)
            {
                //if (t.
            }
            return false;
        }

        public void AddPlayer(XmlUser player)
        {
            if (ConnectedPlayersCount < Settings.MaxPlayers)
            {
                /*
                for (int i = 1; i < Players.Count; i++)
                {
                    if (Players[i] == null)
                    {
                        Players[i] = player;
                    }
                }
                 */
                OnPropertyChanged("Players");
                OnPropertyChanged("ConnectedPlayersCount");
            }
        }

        /// <summary>
        /// Checks given HexPoint location and its neighbours if a city or town
        /// is placed
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool IsTownCityPresent(HexPoint location)
        {
            //make a list of each point to check for 
            List<HexPoint> points = new List<HexPoint>();
            points.Add(location);
            points.AddRange(location.GetNeighbours());

            // check the list against all towns and cities
            foreach (HexPoint point in points)
            {
                foreach (GamePlayer player in _Players)
                {
                    if (player.Towns.Contains(point)) return true;
                    if (player.Cities.Contains(point)) return true;
                }
            }
            return false;
        }

        public bool IsRoadShipPresent(HexSide location)
        {
            foreach (GamePlayer player in _Players)
            {
                if (player.Roads.Contains(location)) return true;
                if (player.Ships.Contains(location)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns player with given userID
        /// Returns null if no player exists in this game with given ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public GamePlayer GetPlayer(int userID)
        {
            foreach (GamePlayer player in _Players)
                if (player.XmlPlayer.ID == userID) return player;

            return null;
        }

        public void PerformAction(InGameAction action)
        {
            HostStartsGameAction hostStarts = action as HostStartsGameAction;
            if (hostStarts != null)
            {
 
            }

            _Phase.ProcessAction(action, this);

            InGameAction next = _ActionsQueue.Peek();
            
        }

        /// <summary>
        /// checks if player has a ship or road connecting to the point
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool HasRoadShipAtPoint(HexPoint location, GamePlayer player)
        {
            foreach (HexSide side in location.GetNeighbourSides)
            {
                if (player.Roads.Contains(side)) return true;
                if (player.Ships.Contains(side)) return true;
            }

            //no ship or road found
            return false;
        }

        public void CalculateLargestArmy(GamePlayer soldierPlayingPlayer)
        {
            // Get current keeper of LA
            GamePlayer currentMightyPlayer = _Players.Where(p => p.HasLargestArmy == true).SingleOrDefault();

            // Check if there is a current winner
            if ((currentMightyPlayer != null &&
                // If we are already winner, no changes happens
                 !currentMightyPlayer.Equals(soldierPlayingPlayer) &&
                // New winner must have more soldiers then current winner
                 soldierPlayingPlayer.PlayedSoldierCount > currentMightyPlayer.PlayedSoldierCount) ||
                // Becoming first to get 3 devs also declares winner
                 (soldierPlayingPlayer.PlayedSoldierCount > 2))
            {
                OnLargestArmyChanged(soldierPlayingPlayer);
            }
        }

        /// <summary>
        /// Checks current situation for a new LR owner
        /// Should be called whenever a ship, road or town is added, moved or removed
        /// MoveShip: old/new ship can be part of longer route
        /// BuildRoad/BuildShip: LR can be extended
        /// BuildTown:  road/ship may be connected (previously blown up town)
        ///             new town may block other players' LR
        /// VolcanoExplosion: blown up town/city may link two parts (ship+road)
        /// </summary>
        public void CalculateLongestRoad(GamePlayer player)
        {
            Route possibleNewLR = new Route().CalculateALongestRoute(player, this);
            if (possibleNewLR != null)
            {
                if (!player.HasLongestRoute)
                {
                    // Claim the route as first when route is 5 long and not claimed yet
                    if (possibleNewLR.Count > 4 && LongestRoute == null)
                    {
                        OnLongestRouteChanged(player, possibleNewLR);
                    }
                }
                else
                {
                    if (possibleNewLR.Count > player.LongestRoute.Count)
                    {
                        OnLongestRouteChanged(player, possibleNewLR);
                    }
                }
            }
        }

        /// <summary>
        /// Recalculates every route in case of a blokage
        /// </summary>
        /// <param name="town"></param>
        /// <param name="player"></param>
        public void CalculateLongestRoad(HexPoint town, GamePlayer player)
        {
            Route current = LongestRoute;

            // When we have a current route, check if the new town is part of it
            if (current != null)
            {
                // But only when the player building the town is other then the player owning
                // the current route
                if (!player.Equals(LongestRouteOwner))
                {
                    // Create a list of hexpoints in current route
                    List<HexPoint> routePoints = new List<HexPoint>();

                    // Put all points into the list
                    foreach (RouteNode node in current)
                    {
                        if (!routePoints.Contains(node.GetNeighbourPoints()[0]))
                            routePoints.Add(node.GetNeighbourPoints()[0]);
                        if (!routePoints.Contains(node.GetNeighbourPoints()[1]))
                            routePoints.Add(node.GetNeighbourPoints()[1]);
                    }

                    // Check if the town is placed on a hexpoint in the route
                    if (routePoints.Contains(town))
                    {
                        // We have a winner! Nice play. This means given town which is built blocks 

                        // We should iterate over all players, calculating their longest routes.
                        // If current keeper of the longest route has a new route with length 
                        // equalling length of another player, current keeper keeps his LR.
                        // However, when two other players both have longest route, no one gets 
                        // the route. 

                        // TODO: make static function call of CalculateALongestRoute
                        Route phony = new Route();

                        // Get us some nice new dictionairy to store the calculated routes
                        Dictionary<GamePlayer, Route> playersRoutes = new Dictionary<GamePlayer, Route>();

                        // Fill the dictionairy with new routes
                        foreach (GamePlayer p in _Players)
                            playersRoutes.Add(p, phony.CalculateALongestRoute(p, this));

                        // Sweet! Now let's see who is the winner of the routecontest

                        // First get the longest route of current keeper of LR
                        Route currentWinnerRoute = playersRoutes[LongestRouteOwner];

                        // Get the length of the longest route(s)
                        int newWinnerLength = playersRoutes.Max(kvp => kvp.Value.Count);

                        Route newRoute = null;

                        if (currentWinnerRoute.Count == newWinnerLength)
                        {
                            // Current keeper of longest route has a new route with the maximum length.
                            // Therefore, he stays the keeper of the longest route

                            newRoute = currentWinnerRoute;
                        }
                        else
                        {
                            // Some other player(s) have a longer route then the previous route, and still longer
                            // then the longest route of the keeper of the previous route.
                            // Determine if we have a single player having so. If a single player is found, 
                            // declare him the winner. When multiple players are found, no one gets
                            // declared winner.

                            IEnumerable<KeyValuePair<GamePlayer, Route>> winners = 
                                playersRoutes.Where(kvp => kvp.Value.Count == newWinnerLength);

                            // We have more then one new winners. No one gets LR.
                            if (winners.Count() > 1)
                            {
                                newRoute = null;
                            }

                            // Only one new winner
                            if (winners.Count() == 1)
                            { 
                                newRoute = winners.First().Value;
                            }

                            // Old player lost route, no players with routes > 4
                            if (winners.Count() == 0)
                            {
                                newRoute = null;
                            }
                        }
                    }

                }
            }
            else
            {
                CalculateLongestRoad(player);
            }
        }

        /// <summary>
        /// 2 or more players have rolled the same top dice roll
        /// </summary>
        /// <param name="rolledDices"></param>
        /// <param name="highRoll">Highrolling dice</param>
        public List<int> RolledSame(List<RollDiceAction> rolledDices, int highRoll)
        {
            // list of player IDs who rolled equal highroll
            List<int> sameRolledPlayers = new List<int>();

            // create a list of players who need to roll again
            // enqueue each highrollig player with a nw chance to roll the dice
            foreach (RollDiceAction highestRoll in from rd in rolledDices
                                                   where rd.Dice == highRoll
                                                   select rd)
            {
                _ActionsQueue.Enqueue(new RollDiceAction()
                {
                    GamePlayer = highestRoll.GamePlayer
                });
                sameRolledPlayers.Add(highestRoll.Sender);
            }
            return sameRolledPlayers;
        }

        public bool CanBuyDevcard()
        {
            if (DevCards.Count == 0) return false;

            return true;
        }

        public XmlGame Copy()
        {
            DataContractSerializer dc = new DataContractSerializer(this.GetType());
            MemoryStream ms = new MemoryStream();
            dc.WriteObject(ms, this);
            ms.Position = 0;
            XmlGame game = (XmlGame)dc.ReadObject(ms);
            return game;
        }

        #endregion

        #region INotifyPropertyChanging implementation

        private event PropertyChangingEventHandler _PropertyChanging;

        public event PropertyChangingEventHandler PropertyChanging
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanging = (PropertyChangingEventHandler)Delegate.Combine(_PropertyChanging, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanging = (PropertyChangingEventHandler)Delegate.Remove(_PropertyChanging, value); }
        }

        private void OnPropertyChanging(string p)
        {
            if (_PropertyChanging != null)
            {
                _PropertyChanging(this, new PropertyChangingEventArgs(p));
            }
        }

        #endregion

        #region LongestRoute/Largest army implementation

        /// <summary>
        /// Seperate LA/LR implementation
        /// </summary>
        /// <param name="player"></param>
        public delegate void LongestRouteChangedHandler(GamePlayer player, Route newRoute);
        public delegate void LargestArmyChangedHandler(GamePlayer player);

        private event LongestRouteChangedHandler _LongestRouteChanged;
        private event LargestArmyChangedHandler _LargestArmyChanged;

        public event LargestArmyChangedHandler LargestArmyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _LargestArmyChanged = (LargestArmyChangedHandler)Delegate.Combine(_LargestArmyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _LargestArmyChanged = (LargestArmyChangedHandler)Delegate.Remove(_LargestArmyChanged, value); }
        }

        public event LongestRouteChangedHandler LongestRouteChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _LongestRouteChanged = (LongestRouteChangedHandler)Delegate.Combine(_LongestRouteChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _LongestRouteChanged = (LongestRouteChangedHandler)Delegate.Remove(_LongestRouteChanged, value); }
        }

        private void OnLongestRouteChanged(GamePlayer player, Route newRoute)
        {
            if (_LongestRouteChanged != null)
            {
                _LongestRouteChanged(player, newRoute);
            }
        }
        private void OnLargestArmyChanged(GamePlayer player)
        {
            if (_LargestArmyChanged != null)
            {
                _LargestArmyChanged(player);
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        private event PropertyChangedEventHandler _PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }

        private void OnPropertyChanged(string p)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        #endregion
    }

}
