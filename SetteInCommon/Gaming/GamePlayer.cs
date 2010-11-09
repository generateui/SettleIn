using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using SettleInCommon;
using SettleInCommon.Board;
using SettleInCommon.User;
using SettleInCommon.Gaming.DevCards;
using System.Collections.Specialized;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Actions.TurnActions;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming
{
    /// <summary>
    /// Represent a player which is in a game. Used both on server and client. 
    /// </summary>
    [DataContract]
    public class GamePlayer : INotifyPropertyChanged
    {
        #region Variables

        private PropertyChangedEventHandler _PropertyChanged;
        private string _Color = "#FFFF00";
        private bool _IsOnTurn = false;

        private HexPoint _StartingTown;
        private HexPoint _SecondTownOrCity;
        private int _FirstTerritory = -1;
        private int _SecondTerritory = -1;
        private Route _LongestRoute =null;
        private int _DevRoadShips = 0;

        private ObservableCollection<TradeRoute> _TradeRoutes =
            new ObservableCollection<TradeRoute>();

        int ResourcesCount { get; set; }

        public int DevcardCount { get; set; }

        private XmlUser _XmlPlayer = new XmlUser("piet");

        // True if instance is on a client, false if instance is on server
        private bool _IsClient = true;


        // List of resource cards the player has. No resources means we have a client 
        // opponent object, of which we may not know the resources, but only the amount
        // of resources, which is kept in ResourcesCount.
        // Either ResourcesCount or Resources should be null.
        // When utilizing the count of resources when it is the current player,
        // use Resources.CountAll.
        private ResourceList _Resources = new ResourceList();

        // List of towns the player owns
        private ObservableCollection<HexPoint> _Towns =
            new ObservableCollection<HexPoint>();

        //List of cities the player owns
        private ObservableCollection<HexPoint> _Cities =
            new ObservableCollection<HexPoint>();

        // List of the towns of player
        private ObservableCollection<HexSide> _Ships =
            new ObservableCollection<HexSide>();

        //List of Roads player has
        private ObservableCollection<HexSide> _Roads = 
            new ObservableCollection<HexSide>();

        // Whether or not the player has the largest army
        private bool _HasLargestArmy = false;

        // Whether or not the player has the largest trading route
        private bool _HasLongestRoute = false;

        private bool _HasPlayedDevcardThisTurn = false;

        // Amount of trading routes the player has
        private int _TradingRoutesCount = 0;

        // Amount of bonus VPs the player has
        private ObservableCollection<HexPoint> _BonusIslandVPs =
            new ObservableCollection<HexPoint>();


        private int _StockShips = 15;
        private int _StockCities = 4;
        private int _StockRoads = 15;
        private int _StockTowns = 4;

        // List f ports player has a town on
        private XmlPortList _Ports = XmlPortList.Empty();

        // played devcards of player
        private ObservableCollection<DevelopmentCard> _PlayedDevcards =
            new ObservableCollection<DevelopmentCard>();

        // nonplayed devcards
        private ObservableCollection<DevelopmentCard> _Devcards =
            new ObservableCollection<DevelopmentCard>();

        #endregion

        #region Properties

        [DataMember]
        public ObservableCollection<HexPoint> BonusIslandVPs
        {
            get { return _BonusIslandVPs; }
            set { _BonusIslandVPs = value; }
        }

        // amount of roads/ships player has this turn from playing road building devcard
        [DataMember]
        public int DevRoadShips
        {
            get { return _DevRoadShips; }
            set
            {
                if (value != _DevRoadShips)
                {
                    _DevRoadShips = value;
                    OnPropertyChanged("DevRoadShips");
                }
            }
        }

        [DataMember]
        public bool IsOnTurn
        {
            get { return _IsOnTurn; }
            set
            {
                if (value != _IsOnTurn)
                {
                    _IsOnTurn = value;
                    OnPropertyChanged("IsOnTurn");
                }
            }
        }

        public Route LongestRoute
        {
            get { return _LongestRoute; }
            set
            {
                _LongestRoute = value;
                OnPropertyChanged("LongestRoute");
                OnPropertyChanged("VictoryPoints");
            }
        }

        [DataMember]
        public ObservableCollection<TradeRoute> TradeRoutes
        {
            get { return _TradeRoutes; }

            set
            {
                if (value != _TradeRoutes)
                {
                    _TradeRoutes = value;
                    OnPropertyChanged("TradeRoutes");
                }
            }
        }

        public int FirstTerritory
        {
            get
            {
                if (_StartingTown != null)
                    return _FirstTerritory;
                else
                {
                    _FirstTerritory = 0;
                    return _FirstTerritory;
                }
            }
        }

        public int SecondTerritory
        {
            get
            {
                if (_SecondTownOrCity != null)
                    return _FirstTerritory;
                else
                {
                    _SecondTerritory = 0;
                    return _SecondTerritory;
                }
            }
        }

        public List<int> StartingTerritories()
        {
            List<int> result = new List<int>();

            if (_FirstTerritory != 0)
                result.Add(FirstTerritory);
            if (SecondTerritory != 0 &&
                !result.Contains(SecondTerritory))
                result.Add(SecondTerritory);

            return result;
        }

        // hand cards of the player
        [DataMember]
        public ResourceList Resources
        {
            get { return _Resources; }
            set
            {
                if (value != _Resources)
                {
                    if (_Resources != null)
                        _Resources.PropertyChanged -= _Resources_PropertyChanged;
                    _Resources = value;
                    OnPropertyChanged("Resources");
                    _Resources.PropertyChanged += new PropertyChangedEventHandler(_Resources_PropertyChanged);
                }
            }
        }

        [DataMember]
        public HexPoint StartingTownOrCity
        {
            get { return _SecondTownOrCity; }
            set { _SecondTownOrCity = value; }
        }

        [DataMember]
        public HexPoint StartingTown
        {
            get { return _StartingTown; }
            set { _StartingTown = value; }
        }

        [DataMember]
        public ObservableCollection<DevelopmentCard> PlayedDevcards
        {
            get { return _PlayedDevcards; }
            set
            {
                if (value != null)
                {
                    if (_PlayedDevcards != null)
                        _PlayedDevcards.CollectionChanged -= _PlayedDevcards_CollectionChanged;
                    _PlayedDevcards = value;
                    _PlayedDevcards.CollectionChanged += new NotifyCollectionChangedEventHandler(_PlayedDevcards_CollectionChanged);
                }
            }
        }

        [DataMember]
        public ObservableCollection<DevelopmentCard> DevCards
        {
            get { return _Devcards; }
            set
            {
                if (value != _Devcards)
                {
                    _Devcards = value;
                    OnPropertyChanged("Devcards");
                }
            }
        }

        [DataMember]
        public XmlPortList Ports
        {
            get { return _Ports; }
            set { _Ports = value; }
        }

        [DataMember]
        public XmlUser XmlPlayer
        {
            get { return _XmlPlayer; }
            set { _XmlPlayer = value; }
        }

        [DataMember]
        public ObservableCollection<HexPoint> Cities
        {
            get { return _Cities; }
            set
            {
                if (value != _Cities)
                {
                    if (_Cities != null)
                        _Cities.CollectionChanged -= _Cities_CollectionChanged;
                    _Cities = value;
                    OnPropertyChanged("Cities");
                    _Cities.CollectionChanged += new NotifyCollectionChangedEventHandler(_Cities_CollectionChanged);
                }
            }
        }

        [DataMember]
        public ObservableCollection<HexSide> Ships
        {
            get { return _Ships; }
            set
            {
                if (value != _Ships)
                {
                    if (_Ships != null)
                        _Ships.CollectionChanged -= _Ships_CollectionChanged;
                    _Ships = value;
                    OnPropertyChanged("Ships");
                    _Ships.CollectionChanged += new NotifyCollectionChangedEventHandler(_Ships_CollectionChanged);
                }
            }
        }

        [DataMember]
        public ObservableCollection<HexSide> Roads
        {
            get { return _Roads; }
            set
            {
                if (value != _Roads)
                {
                    if (_Roads != null)
                        _Roads.CollectionChanged -= _Roads_CollectionChanged;
                    _Roads = value;
                    OnPropertyChanged("Roads");
                    _Roads.CollectionChanged += new NotifyCollectionChangedEventHandler(_Roads_CollectionChanged);
                }
            }
        }

        [DataMember]
        public bool HasLargestArmy
        {
            get { return _HasLargestArmy; }
            set
            {
                if (value != _HasLargestArmy)
                {
                    _HasLargestArmy = value;
                    OnPropertyChanged("HasLargestArmy");
                }
            }
        }
        public int RoadLength
        {
            get
            {
                return 3;
            }
        }
        [DataMember]
        public string Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
        [DataMember]
        public bool HasPlayedDevcardThisTurn
        {
            get { return _HasPlayedDevcardThisTurn; }
            set
            {
                _HasPlayedDevcardThisTurn = value;
                OnPropertyChanged("HasPlayedDevcardThisTurn");
            }
        }

        public bool HasLongestRoute
        {
            get { return _LongestRoute != null; }
        }

        public int PlayedSoldierCount
        {
            get
            {
                return _PlayedDevcards.OfType<Soldier>().Count();
            }
        }

        public IEnumerable<Soldier> PlayedSoldiers
        {
            get
            {
                return _PlayedDevcards.OfType<Soldier>();
            }
        }

        [DataMember]
        public int StockTowns
        {
            get { return _StockTowns; }
            set
            {
                _StockTowns = value;
                OnPropertyChanged("StockTowns");
            }
        }

        [DataMember]
        public int StockCities
        {
            get { return _StockCities; }
            set
            {
                _StockCities = value;
                OnPropertyChanged("StockCities");
            }
        }

        /// <summary>
        /// amount of ships player has in stock
        /// </summary>
        [DataMember]
        public int StockShips
        {
            get { return _StockShips; }
            set
            {
                _StockShips = value;
                OnPropertyChanged("StockShips");
            }
        }

        /// <summary>
        /// amount of roads player has in stock
        /// </summary>
        [DataMember]
        public int StockRoads
        {
            get { return _StockRoads; }
            set
            {
                _StockRoads = value;
                OnPropertyChanged("StockRoads");
            }
        }

        /// <summary>
        /// Returns a new list containing all the ships and roads of a player
        /// </summary>
        /// <returns></returns>
        public List<HexSide> GetRoadsShips()
        {
            List<HexSide> result = new List<HexSide>();

            result.AddRange(_Ships);
            result.AddRange(_Roads);

            return result;
        }

        /// <summary>
        /// Returns a new list containing all cities and towns of the player
        /// </summary>
        /// <returns></returns>
        public List<HexPoint> GetTownsCities()
        {
            List<HexPoint> result = new List<HexPoint>();

            result.AddRange(_Towns);
            result.AddRange(_Cities);

            return result;
        }

        /// <summary>
        /// Returns amount of Victory points a player has
        /// </summary>
        public int VictoryPoints
        {
            get
            {
                return _Towns.Count +                   // amount of towns
                    (_Cities.Count * 2) +                     // amount of cities    
                    (_HasLargestArmy ? 2 : 0) +         // +2vp when having largest army
                    (HasLongestRoute ? 2 : 0) +        // +2vp when having longest road
                    _PlayedDevcards.OfType<VictoryPoint>().Count() +           // amount of played VP devcards
                    _TradingRoutesCount +               // amount of trading routes
                    _BonusIslandVPs.Count +              // amount of bonus island VPs
                    _TradeRoutes.Count;
            }
        }

        [DataMember]
        public ObservableCollection<HexPoint> Towns
        {
            get { return _Towns; }
            set
            {
                if (value != _Towns)
                {
                    if (_Towns != null)
                        _Towns.CollectionChanged -= _Towns_CollectionChanged;
                    _Towns = value;
                    OnPropertyChanged("Towns");
                    _Towns.CollectionChanged += new NotifyCollectionChangedEventHandler(_Towns_CollectionChanged);
                }
            }
        }

        #endregion

        #region Methods

        public IEnumerable<TradeOfferAction> OffersThisTurn(XmlGame game)
        {
            return  from TradeOfferAction tradeOffer in game.GameLog
                    where tradeOffer.TurnID == game.CurrentTurn
                    select tradeOffer;
        }

        public void MakeClient()
        {
            // Only send amount of resources a player has
            ResourcesCount = _Resources.CountAllExceptDiscovery;
            _Resources.Timber = 0;
            _Resources.Wheat = 0;
            _Resources.Ore = 0;
            _Resources.Clay = 0;
            _Resources.Sheep = 0;

            //Only send amount of devcards the player has
            DevcardCount = _Devcards.Count;
            DevCards = null;
            /*
            Devcards.mo = 0;
            Devcards.RbCount = 0;
            Devcards.RobberCount = 0;
            Devcards.VpCount = 0;
            Devcards.YopCount = 0;
             */

        }

        #endregion

        #region Constructors

        public GamePlayer()
        {
            Towns = new ObservableCollection<HexPoint>();
            Ships = new ObservableCollection<HexSide>();
            Cities = new ObservableCollection<HexPoint>();
            Roads = new ObservableCollection<HexSide>();
            PlayedDevcards = new ObservableCollection<DevelopmentCard>();
        }

        #endregion

        #region CanPay for stuff

        public int GoldCount(ResourceList list)
        {
            int result = 0;

            ResourceList workinglist = list.Copy();

            if (_Ports.Timber > 0)
            {
                result += _Resources.Timber / 2;
                workinglist.Timber--;
            }
            if (_Ports.Wheat > 0)
            {
                result += _Resources.Wheat / 2;
                workinglist.Wheat--;
            }
            if (_Ports.Ore > 0)
            {
                result += _Resources.Ore / 2;
                workinglist.Ore--;
            }
            if (_Ports.Clay > 0)
            {
                result += _Resources.Clay / 2;
                workinglist.Clay--;
            }
            if (_Ports.Sheep > 0)
            {
                result += _Resources.Sheep / 2;
                workinglist.Sheep--;
            }
            if (_Ports.ThreeToOne > 0)
            {
                if (workinglist.Timber > 0) result += workinglist.Timber / 3;
                if (workinglist.Wheat > 0) result += workinglist.Wheat / 3;
                if (workinglist.Ore > 0) result += workinglist.Ore / 3;
                if (workinglist.Clay > 0) result += workinglist.Clay / 3;
                if (workinglist.Sheep > 0) result += workinglist.Sheep / 3;
            }
            else
            {
                if (workinglist.Timber > 0) result += workinglist.Timber / 4;
                if (workinglist.Wheat > 0) result += workinglist.Wheat / 4;
                if (workinglist.Ore > 0) result += workinglist.Ore / 4;
                if (workinglist.Clay > 0) result += workinglist.Clay / 4;
                if (workinglist.Sheep > 0) result += workinglist.Sheep / 4;
            }

            return result;
        }


        public bool CanPayDevcard
        {
            get
            {
                ResourceList devRes = GetDevcardResources();
                if (devRes.Count < 3)
                {
                    ResourceList temp = _Resources.Copy();
                    temp.SubtractCards(devRes);
                    return GoldCount(temp) >= (3 - devRes.Count);
                }
                else
                {
                    return true;
                }
            }
        }

        public bool CanPayRoad
        {
            get
            {
                // Free ship is free
                if (_DevRoadShips > 0) return true;

                // Check if we can simply pay for it
                if (_Resources.Clay > 0 && _Resources.Timber > 0) return true;
                if (_Resources.Clay > 0 && _Resources.Timber == 0)
                {
                    ResourceList temp = _Resources.Copy();
                    temp.Clay--;

                    // we can pay a road: we have a clay and we can afford another gold
                    // for the timber we dont have
                    return GoldCount(temp) > 0;
                }
                if (_Resources.Clay == 0 && _Resources.Timber > 0)
                {
                    ResourceList temp = _Resources.Copy();
                    temp.Timber--;
                    // we can pay a road: we have a clay and we can afford another gold
                    // for the clay we dont have
                    return GoldCount(temp) > 0;
                }
                if (_Resources.Clay == 0 && _Resources.Timber == 0)
                {
                    // we can pay a road if we have at least 2 gold 
                    return GoldCount(_Resources) > 1;
                }
                throw new Exception("Whoa!");
            }
        }

        public int RoadNeededGold()
        {
            if (_DevRoadShips > 0) return 0;

            int neededGold = 0;

            if (_Resources.Timber == 0) neededGold++;
            if (_Resources.Clay == 0) neededGold++;

            return neededGold;
        }
        public int ShipNeededGold()
        {
            if (_DevRoadShips > 0) return 0;

            int neededGold = 0;

            if (_Resources.Timber == 0) neededGold++;
            if (_Resources.Sheep == 0) neededGold++;

            return neededGold;
        }

        public int DevcardNeededGold()
        {
            int neededGold = 0;
            ResourceList temp = _Resources.Copy();

            // Having 3 or more discoveries, we don't need any gold
            if (_Resources.Discoveries > 2)
                return 0;

            if (_Resources.Wheat > 0)
                temp.Wheat--;
            else
                neededGold++;

            if (_Resources.Ore > 0)
                temp.Ore--;
            else
                neededGold++;

            if (_Resources.Sheep > 0)
                temp.Sheep--;
            else
                neededGold++;

            if (_Resources.Discoveries == 1 || _Resources.Discoveries == 2)
                neededGold -= _Resources.Discoveries;

            // We can't need less then 0 gold
            if (neededGold < 0) neededGold = 0;

            return neededGold;
        }

        /// <summary>
        /// Whether or not the player is able to claim victory
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool CanClaimVictory(XmlGame game)
        {
            return game.Settings.VpToWin >= VictoryPoints;
        }

        /// <summary>
        /// Whether or not player can move a ship
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool CanMoveShip(XmlGame game)
        {
            // If there is no ship to move, bugger out
            if (Ships.Count == 0)
                return false;

            // when can only move a ship once per turn
            var movedShip = (from MoveShipAction ms in game.GameLog.OfType<MoveShipAction>()
                             where ms.TurnID == game.CurrentTurn
                             select ms).SingleOrDefault();
            if (movedShip != null)
                return false;

            // We should be able to have a moveable ship, and a location to put the moved ship
            // in order to be able to move te ship
            if (GetMoveableShips(game, game.Board).Count == 0)
                return false;

            return true;
        }
        /// <summary>
        /// Whether or not we can pay a ship
        /// We can pay a ship if:
        /// -we have a roadbuilding token
        /// -we have enough resources
        /// </summary>
        public bool CanPayShip
        {
            get
            {
                if (_DevRoadShips > 0) return true;

                //check if we can simply pay for it
                if (_Resources.Sheep > 0 && _Resources.Timber > 0) return true;

                // we have sheep, no timber
                if (_Resources.Sheep > 0 && _Resources.Timber == 0)
                {
                    ResourceList temp = _Resources.Copy();
                    temp.Sheep--;
                    // we can pay a road: we have a clay and we can afford another gold
                    // for the timber we dont have
                    return GoldCount(temp) > 0;
                }
                //we have timber, no sheep
                if (_Resources.Sheep == 0 && _Resources.Timber > 0)
                {
                    ResourceList temp = _Resources.Copy();
                    temp.Timber--;
                    // we can pay a road: we have a clay and we can afford another gold
                    // for the sheep we dont have
                    return GoldCount(temp) > 0;
                }
                // we have neither, we must pay with 2 gold
                if (_Resources.Sheep == 0 && _Resources.Timber == 0)
                {
                    // we can pay a road if we have at least 2 gold 
                    return GoldCount(_Resources) > 1;
                }
                throw new Exception("Whoa!");
            }
        }
        /// <summary>
        /// Whether or not player can build a town
        /// A town may be built when:
        /// -player has at least one stock town
        /// -a spot is available to put the town on
        /// </summary>
        /// <param name="game"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool CanBuildTown(XmlGame game, XmlBoard board)
        {
            if (StockTowns == 0) return false;

            // We should have at least one spot to build on
            if (GetTownBuildPlaces(game, board).Count == 0) return false;

            return true;
        }

        /// <summary>
        /// Whether or not the player can build a city
        /// A player may build a city when:
        /// -At least one stock city is present
        /// -At least one town is present
        /// </summary>
        /// <returns></returns>
        public bool CanBuildCity()
        {
            if (StockCities == 0) return false;
            if (Towns.Count == 0) return false;

            return true;
        }

        /// <summary>
        /// Whether or not the player can build a road
        /// A road maybe build when:
        /// -A stock road is present
        /// -A spot is available to place a road
        /// </summary>
        /// <param name="game"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool CanBuildRoad(XmlGame game, XmlBoard board)
        {
            if (StockRoads == 0) return false;

            if (GetRoadBuildPlaces(game, board).Count == 0) return false;

            return true;
        }

        /// <summary>
        /// Returns a list of places where player may build a road
        /// </summary>
        /// <param name="game"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<HexSide> GetRoadBuildPlaces(XmlGame game, XmlBoard board)
        {
            // Keep track of which sides have been added on the list to create a visual for
            List<HexSide> _AddedSides = new List<HexSide>();

            // list of all roads+ships on the board
            List<HexSide> _AllTravellers = game.AllRoadsShips();

            // each town has three starting points to build roads from, add them to the list
            foreach (HexPoint point in game.PlayerOnTurn.GetTownsCities())
                _AddedSides.AddRange(point.GetNeighbourSides);

            // each side has two neighboring sides, add them too
            foreach (HexSide side in game.PlayerOnTurn.Roads)
                _AddedSides.AddRange(side.GetNeighbours());

            // Make items in list unique, remove sides with two seahexes
            IEnumerable<HexSide> uniqueSides = from s in _AddedSides.Distinct()
                                               where
                                                   // exclude sides with two seahexes
                                                       !(board.Hexes[s.Hex1] is SeaHex &&
                                                       board.Hexes[s.Hex2] is SeaHex)
                                                       &&
                                                   // exclude sides already owned by any player
                                                       !(_AllTravellers.Contains(s))
                                               select s;

            // convert to a list
            List<HexSide> result = new List<HexSide>();

            foreach (HexSide side in uniqueSides)
                result.Add(side);

            return result;
        }

        /// <summary>
        /// Returns a list of ships allowed to move
        /// </summary>
        /// <param name="game"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<HexSide> GetMoveableShips(XmlGame game, XmlBoard board)
        {
            List<HexSide> result = new List<HexSide>();

            // only create a list if we have any ships to move
            if (_Ships.Count > 0)
            {
                // Create a list of ships built this turn
                IEnumerable<HexSide> buildShips = from BuildShipAction ta in game.GameLog.OfType<BuildShipAction>()
                                                  where ta.TurnID == game.CurrentTurn
                                                  select ta.Intersection;
                
                // Get the moved ship if the player has done so
                MoveShipAction movedShip = game.GameLog.OfType<MoveShipAction>()
                    .Where(msa => msa.TurnID == game.CurrentTurn)
                    .SingleOrDefault();

                // Create a list of allowed ships
                // Add all ships to the resultlist which passed the test
                foreach (HexSide ship in _Ships)
                {
                    // allowed ships are:
                    //
                    // -ships not having a ship or town on other point/point neighbours
                    // -are not part of a traderoute

                    // Check against traderoutes whether or not ship is moveable

                    // first see if traderouts are relevant
                    if (game.Board.UseTradeRoutes)
                    {
                        // check if we have traderoutes at all
                        if (_TradeRoutes.Count > 0)
                        {
                            bool isPartOfRoute = false;
                            // TODO: optimize by first creating a list of hexsides part of players' traderoutes
                            foreach (TradeRoute tradeRoute in _TradeRoutes)
                            {
                                if (tradeRoute.Contains(new RouteNode(ship)))
                                {
                                    isPartOfRoute = true;
                                    break;
                                }
                            }
                            // continue with next ship when a route uses it
                            if (isPartOfRoute) continue;
                        }
                    }

                    // If the ship is moved this turn, player is not allowed to move it again
                    if (movedShip != null)
                    {
                        if (ship.Equals(movedShip))
                            continue;
                    }

                    // If the ship is built this turn, don't allow moving the ship
                    if (buildShips.Contains(ship))
                        continue;

                    // Check if a combination of point + hexside neighbours have
                    // either a town or at least one ship

                    // Get the points
                    List<HexPoint> points = ship.GetNeighbourPoints();

                    // Make a list of neighbours of either point
                    var sides1 = from HexSide s in points[0].GetNeighbourSides
                                 where !s.Equals(ship)
                                 select s;

                    var sides2 = from HexSide s in points[1].GetNeighbourSides
                                 where !s.Equals(ship)
                                 select s;

                    if ((_Ships.Contains(sides1.ElementAt(0)) ||
                         _Ships.Contains(sides1.ElementAt(1)) ||
                         _Towns.Contains(points[0]))
                         &&
                       (_Ships.Contains(sides2.ElementAt(0)) ||
                        _Ships.Contains(sides2.ElementAt(1)) ||
                        _Towns.Contains(points[1])))
                    {
                        // A ship has a town or 1-2 ships at both sides, ignore ship
                        continue;
                    }

                    // if a ship has no spot to move to, it isn't moveable either
                    if (GetShipBuildPlaces(game, board, ship).Count == 0)
                        continue;

                    // we passed the test, we have a moveable ship
                    result.Add(ship);
                }
            }

            return result;

        }

        /// <summary>
        /// Returns a list of places a player may build a ship on, excluding a specified ship
        /// </summary>
        /// <param name="game"></param>
        /// <param name="board"></param>
        /// <param name="shipToMove">Ship to exclude</param>
        /// <returns></returns>
        public List<HexSide> GetShipBuildPlaces(XmlGame game, XmlBoard board, HexSide shipToMove)
        {
            // Keep track of which sides have been added on the list
            List<HexSide> result = new List<HexSide>();

            //create a list of all possible (nonunique) sides a ship may be built
            List<HexSide> allSides = new List<HexSide>();

            // add sides starting from town/city
            foreach (HexPoint point in game.PlayerOnTurn.Towns.Union(game.PlayerOnTurn.Cities))
                allSides.AddRange(point.GetNeighbourSides);

            List<HexSide> shipsWithoutMoveable = game.PlayerOnTurn.Ships.ToList();
            shipsWithoutMoveable.Remove(shipToMove);

            //add neighbouring sides from ships
            foreach (HexSide side in shipsWithoutMoveable)
                allSides.AddRange(side.GetNeighbours());

            // remove all used sides by ships & roads from the list
            //allSides.Except(game.AllRoadsShips());
            List<HexSide> emptySpots = new List<HexSide>();

            // used spot from any player
            List<HexSide> forbiddenSides = game.AllRoadsShips();

            forbiddenSides.Add(shipToMove);

            // pirate places
            forbiddenSides.AddRange(game.Pirate.GetPoints());

            foreach (HexSide s in allSides)
            {
                if (!forbiddenSides.Contains(s)) emptySpots.Add(s);
            }

            foreach (HexSide side in emptySpots)
            {
                // we need at least one seahex
                if (board.Hexes[side.Hex1] is SeaHex || board.Hexes[side.Hex2] is SeaHex)
                    result.Add(side);
            }

            return result;
        }

        public List<HexSide> GetShipBuildPlaces(XmlGame game, XmlBoard board)
        {
            // Keep track of which sides have been added on the list
            List<HexSide> result = new List<HexSide>();

            //create a list of all possible (nonunique) sides a ship may be built
            List<HexSide> allSides = new List<HexSide>();

            // add sides starting from town/city
            foreach (HexPoint point in game.PlayerOnTurn.Towns.Union(game.PlayerOnTurn.Cities))
                allSides.AddRange(point.GetNeighbourSides);

            //add neighbouring sides from ships
            foreach (HexSide side in game.PlayerOnTurn.Ships)
                allSides.AddRange(side.GetNeighbours());

            // remove all used sides by ships & roads from the list
            //allSides.Except(game.AllRoadsShips());
            List<HexSide> emptySpots = new List<HexSide>();

            // used spot from any player
            List<HexSide> forbiddenSides = game.AllRoadsShips();

            // pirate places
            forbiddenSides.AddRange(game.Pirate.GetPoints());

            foreach (HexSide s in allSides)
            {
                if (!forbiddenSides.Contains(s)) emptySpots.Add(s);
            }

            foreach (HexSide side in emptySpots)
            {
                // we need at least one seahex
                if (board.Hexes[side.Hex1] is SeaHex || board.Hexes[side.Hex2] is SeaHex)
                    result.Add(side);
            }

            return result;
        }

        public bool CanBuildShip(XmlGame game, XmlBoard board)
        {
            if (StockShips == 0) return false;

            if (GetShipBuildPlaces(game, board).Count == 0) return false;

            return true;
        }

        public bool CanPayCity
        {
            get
            {
                int neededGold = 0;
                ResourceList temp = _Resources.Copy();

                if (_Resources.Wheat > 1)
                {
                    temp.Wheat -= 2;
                }
                else
                {
                    int needed = 2 - _Resources.Wheat;
                    neededGold += needed;
                    temp.Wheat -= _Resources.Wheat;
                }
                if (_Resources.Ore > 2)
                {
                    temp.Ore -= 3;
                }
                else
                {
                    int needed = 3 - _Resources.Ore;
                    neededGold += needed;
                    temp.Ore -= _Resources.Ore;
                }
                //neededGold += _Resources.Ore > 2 ? 0 : 3 - _Resources.Wheat;

                return GoldCount(temp) >= neededGold;
            }
        }

        public bool CanTradeWithBank
        {
            get
            {

                if (_Resources.Timber < 2 &&
                    _Resources.Wheat < 2 &&
                    _Resources.Ore < 2 &&
                    _Resources.Clay < 2 &&
                    _Resources.Sheep < 2)
                    return false;

                int portDivider = 4;
                // check if the offer is valid
                if (_Resources.Timber > 1)
                {
                    portDivider = 4;
                    if (Ports.ThreeToOne > 0) portDivider = 3;
                    if (Ports.Timber > 0) portDivider = 2;
                    if (_Resources.Timber > portDivider)
                        return true;
                }
                if (_Resources.Wheat > 1)
                {
                    portDivider = 4;
                    if (Ports.ThreeToOne > 0) portDivider = 3;
                    if (Ports.Wheat > 0) portDivider = 2;
                    if (_Resources.Wheat > portDivider)
                        return true;
                }
                if (_Resources.Ore > 1)
                {
                    portDivider = 4;
                    if (Ports.ThreeToOne > 0) portDivider = 3;
                    if (Ports.Ore > 0) portDivider = 2;
                    if (_Resources.Ore > portDivider)
                        return true;
                }
                if (_Resources.Clay > 1)
                {
                    portDivider = 4;
                    if (Ports.ThreeToOne > 0) portDivider = 3;
                    if (Ports.Clay > 0) portDivider = 2;
                    if (_Resources.Clay > portDivider)
                        return true;
                }
                if (_Resources.Sheep > 1)
                {
                    portDivider = 4;
                    if (Ports.ThreeToOne > 0) portDivider = 3;
                    if (Ports.Sheep > 0) portDivider = 2;
                    if (_Resources.Sheep > portDivider)
                        return true;
                }
                return false;
            }
        }
        public int CityNeededGold()
        {
            int neededGold = 0;
            ResourceList temp = _Resources.Copy();

            if (_Resources.Wheat > 1)
            {
                temp.Wheat -= 2;
            }
            else
            {
                int needed = 2 - _Resources.Wheat;
                neededGold += needed;
                temp.Wheat -= _Resources.Wheat;
            }
            if (_Resources.Ore > 2)
            {
                temp.Ore -= 3;
            }
            else
            {
                int needed = 3 - _Resources.Ore;
                neededGold += needed;
                temp.Ore -= _Resources.Ore;
            }
            //neededGold += _Resources.Ore > 3 ? 0 : 3 - _Resources.Wheat;

            return neededGold;
        }

        public bool CanPayTown
        {
            get
            {
                int neededGold = 0;
                ResourceList temp = _Resources.Copy();

                if (_Resources.Wheat > 0)
                    temp.Wheat--;
                else
                    neededGold++;

                if (_Resources.Timber > 0)
                    temp.Timber--;
                else
                    neededGold++;

                if (_Resources.Clay > 0)
                    temp.Clay--;
                else
                    neededGold++;

                if (_Resources.Sheep > 0)
                    temp.Sheep--;
                else
                    neededGold++;

                return GoldCount(temp) >= neededGold;
            }
        }

        public override string ToString()
        {
            if (_XmlPlayer != null)
            {
                return _XmlPlayer.Name;
            }
            return string.Empty;
        }

        public int TownNeededGold()
        {
            int neededGold = 0;
            ResourceList temp = _Resources.Copy();

            if (_Resources.Wheat > 0)
                temp.Wheat--;
            else
                neededGold++;

            if (_Resources.Timber > 0)
                temp.Timber--;
            else
                neededGold++;

            if (_Resources.Clay > 0)
                temp.Clay--;
            else
                neededGold++;

            if (_Resources.Sheep > 0)
                temp.Sheep--;
            else
                neededGold++;

            return neededGold;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void OnPropertyChanged(string name, CollectionChangeAction action)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new CollectionPropertyChangedEventArgs(name) { Action = action });
            }
        }

        void _Resources_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Resources");
        }

        void _Towns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Remove)
                OnPropertyChanged("VictoryPoints");
        }

        void _PlayedDevcards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0] is VictoryPoint)
                    OnPropertyChanged("VictoryPoints");
                if (e.NewItems[0] is Soldier)
                {
                    OnPropertyChanged("HasLargestArmy");
                    OnPropertyChanged("PlayedSoldierCount");
                }
            }
        }

        void _Ships_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
        void _Cities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Remove)
                OnPropertyChanged("VictoryPoints");
        }

        void _Devcards_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VictoryPoints")
                OnPropertyChanged("VictoryPoints");
        }

        void _Roads_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
        #endregion

        public GamePlayer Copy()
        {
            return (GamePlayer)this.MemberwiseClone();
        }

        public List<HexPoint> GetTownBuildPlaces(XmlGame game, XmlBoard board)
        {
            List<HexPoint> result = new List<HexPoint>();

            // List of all ships+roads of player
            List<HexSide> sides = GetRoadsShips();

            List<HexPoint> allPoints = new List<HexPoint>();
            List<HexPoint> allTownsCities = game.AllTownsCities();

            //create a list of all possible points a town can be placed on
            foreach (HexSide side in sides)
                allPoints.AddRange(side.GetNeighbourPoints());// HexPoint.BuildPointsFromSide(side));

            // filter list on unique points
            // .Distinct doesnt wotk.... horror. See online.
            List<HexPoint> allUniquePoints = new List<HexPoint>();
            foreach (HexPoint p in allPoints)
            {
                if (!allUniquePoints.Contains(p)) allUniquePoints.Add(p);
            }

            foreach (HexPoint point in allUniquePoints)
            {
                List<HexPoint> pointsToCheck = new List<HexPoint>();
                pointsToCheck.Add(point);
                pointsToCheck.AddRange(point.GetNeighbours());

                // if the intersection produces more then one, a neighbour is present
                // where a city/town is built on, so we must omit this point
                if (pointsToCheck.Intersect<HexPoint>(allTownsCities).Count() == 0 &&
                    // We cannot build in the sea, we need at least a nonseahex
                    !((board.Hexes[point.Hex1] is SeaHex) &&
                      (board.Hexes[point.Hex2] is SeaHex) &&
                      (board.Hexes[point.Hex3] is SeaHex)))
                    result.Add(point);
            }
            return result;
        }


        public ResourceList GetDevcardResources()
        {
            ResourceList tmp = _Resources.Copy();
            ResourceList result = new ResourceList();
            for (int i = 1; i < 4; i++)
            {
                if (tmp.Contains(EResource.Discovery))
                {
                    result.Add(EResource.Discovery);
                    tmp.Discoveries--;
                    continue;
                }
                if (!result.Contains(EResource.Wheat) && tmp.Wheat > 0)
                {
                    result.Add(EResource.Wheat);
                    tmp.Wheat--;
                    continue;
                }
                if (!result.Contains(EResource.Ore) && tmp.Ore > 0)
                {
                    result.Add(EResource.Ore);
                    tmp.Ore--;
                    continue;
                }
                if (!result.Contains(EResource.Sheep) && tmp.Sheep > 0)
                {
                    result.Add(EResource.Sheep);
                    tmp.Sheep--;
                }
            }
            return result;
        }
    }
}
