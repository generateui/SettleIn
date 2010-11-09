using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming;
using System.Collections.ObjectModel;

namespace SettleInCommon.Board
{
    /// <summary>
    /// Represents the board data structure. 
    /// 
    /// A board is made up of hexes in a 2D matrix. The even rows of the matrix 
    /// have an indentation length on the left side half the width of a hex.
    /// For example, a 5x5 sized board will have the layout of:
    /// 
    /// <code>
    /// |H| |H| |H| |H| |H|     0
    ///   |H| |H| |H| |H| |H|   1
    /// |H| |H| |H| |H| |H|     2
    ///   |H| |H| |H| |H| |H|   3
    /// |H| |H| |H| |H| |H|     4
    /// </code>
    /// 
    /// Sea3D has the same layout, only has the last hexes of the even rows 
    /// omitted. Thus, a Sea3D 'compatible' board would have the following
    /// layout:
    /// 
    /// <code>
    /// |H| |H| |H| |H| |H|     0
    ///   |H| |H| |H| |H|       1
    /// |H| |H| |H| |H| |H|     2
    ///   |H| |H| |H| |H|       3
    /// |H| |H| |H| |H| |H|     4
    /// </code>
    /// 
    /// The last hexes of each even row in a 'Sea3D compatible' configuration
    /// should be made invisible, and/or locked.
    /// </summary>
    /// 
    /// the implementation
    [KnownType(typeof(SeaHex))]
    [KnownType(typeof(WheatHex))]
    [KnownType(typeof(TimberHex))]
    [KnownType(typeof(ClayHex))]
    [KnownType(typeof(OreHex))]
    [KnownType(typeof(SheepHex))]
    [KnownType(typeof(RandomHex))]
    [KnownType(typeof(DiscoveryHex))]
    [KnownType(typeof(DesertHex))]
    [KnownType(typeof(JungleHex))]
    [KnownType(typeof(VolcanoHex))]
    [KnownType(typeof(GoldHex))]
    [KnownType(typeof(LandHex))]
    [KnownType(typeof(ResourceHex))]
    [KnownType(typeof(RawResourceHex))]
    [KnownType(typeof(SpecialResourceHex))]
    [KnownType(typeof(RuleHex))]
    [KnownType(typeof(NoneHex))]

    [KnownType(typeof(List<Hex>))]
    [DataContract]
    public class XmlBoard : INotifyPropertyChanged
    {
        #region Private variables

        [NonSerialized]
        private PropertyChangedEventHandler _PropertyChanged;

        // data fields
        private string _Name = "New Board";
        private ObservableCollection<Territory> _Territories = Territory.CreateList();
        private bool _IsSeafarers = false;
        private bool _UseTradeRoutes = false;
        private bool _AssignPortsBeforePlacement = false;
        private bool _RequiresInitialShips = false;
        private bool _IsCitiesKnights = true;
        private int _AllowedCards = 7;
        private int _BankResources = 19;
        private int _StockRoads = 15;
        private int _StockShips = 15;
        private int _StockTowns = 5;
        private int _StockCities = 4;
        private int _BonusNewIsland;
        private int _MaxPlayers = 4;
        private int _MinPlayers = 3;
        private int _VpToWin = 10;
        private int _Width = 0;
        private int _Height = 0;

        private int _MaximumCardsInHandWhenSeven = 7;
        private StandardDevCardStack _DevCards = new StandardDevCardStack(5, 2, 14, 2, 2);
        private EBoardCreatedType _BoardType = EBoardCreatedType.Official;
        private ListArrayOfT _Hexes;

        //Name of the creator of the board
        private string _Creator = "Unknown player";

        #endregion

        #region Properties

        [DataMember]
        public int Width
        {
            get { return _Width; }
            set 
            {
                _Width = value;
                if (_Hexes != null)
                    _Hexes.Width = value;
                OnPropertyChanged("Width");
            }
        }


        [DataMember]
        public int Height
        {
            get { return _Height; }
            set 
            {
                _Height = value;
                if (_Hexes != null)
                    _Hexes.Height = value;
                OnPropertyChanged("Height");
            }
        }

        [DataMember]
        public ObservableCollection<Territory> Territories
        {
            get { return _Territories; }
            set { _Territories = value; }
        }

        [DataMember]
        public bool IsSeafarers
        {
            get { return _IsSeafarers; }
            set 
            { 
                _IsSeafarers = value;
                OnPropertyChanged("IsSeafarers");
            }
        }

        public string Size
        {
            get
            {
                return _Width + " x " + _Height;
            }
        }

        public string Players
        {
            get
            {
                if (_MinPlayers != _MaxPlayers)
                {
                    return _MinPlayers + " x " + _MaxPlayers;
                }
                else
                {
                    return _MinPlayers.ToString();
                }
                
            }
        }

        [DataMember]
        public bool IsCitiesKnights
        {
            get 
            { return _IsCitiesKnights; }
            set 
            { 
                _IsCitiesKnights = value;
                OnPropertyChanged("IsCitiesKnights");
            }
        } 

        [DataMember]
        public int MaximumCardsInHandWhenSeven
        {
            get { return _MaximumCardsInHandWhenSeven; }
            set 
            { 
                _MaximumCardsInHandWhenSeven = value;
                OnPropertyChanged("MaximumCardsInHandWhenSeven");
            }
        }

        /// <summary>
        /// Gets or sets whether or not trade routes (as in Great Crossing) are used or not.
        /// TODO: determine if default result should be negative when IsSeafarers=off
        /// </summary>
        [DataMember]
        public bool UseTradeRoutes
        {
            get { return _UseTradeRoutes; }
            set
            {
                if (value != _UseTradeRoutes)
                {
                    _UseTradeRoutes = value;
                    OnPropertyChanged("UseTradeRoutes");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not players should assign ports before the game starts
        /// (as in New World)
        /// </summary>
        [DataMember]
        public bool AssignPortsBeforePlacement
        {
            get { return _AssignPortsBeforePlacement; }
            set
            {
                if (value != _AssignPortsBeforePlacement)
                {
                    _AssignPortsBeforePlacement = value;
                    OnPropertyChanged("AssignPortsBeforePlacement");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not players are required to build a ship, when their initial
        /// placement is on a sea hex (as in Four Islands)
        /// 
        /// TODO:implement in placementphase
        /// </summary>
        [DataMember]
        public bool RequiresInitialShips
        {
            get { return _RequiresInitialShips; }
            set
            {
                if (value != _RequiresInitialShips)
                {
                    _RequiresInitialShips = value;
                    OnPropertyChanged("RequiresInitialShips");
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of allowed cards in hand when a 7 is rolled.
        /// </summary>
        [DataMember]
        public int AllowedCards
        {
            get { return _AllowedCards; }
            set
            {
                if (value != _AllowedCards)
                {
                    _AllowedCards = value;
                    OnPropertyChanged("AllowedCards");
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of resourfces the bank has for each raw resource type
        /// (timber, wheat, ore, clay, sheep)
        /// </summary>
        [DataMember]
        public int BankResources
        {
            get { return _BankResources; }
            set
            {
                if (value != _BankResources)
                {
                    _BankResources = value;
                    OnPropertyChanged("BankResources");
                }
            }
        }
        /// <summary>
        /// Gets or sets the amount of stock roads
        /// </summary>
        [DataMember]
        public int StockRoads
        {
            get { return _StockRoads; }
            set
            {
                if (value != _StockRoads)
                {
                    _StockRoads = value;
                    OnPropertyChanged("StockRoads");
                }
            }
        }
        /// <summary>
        /// Gets or sets the amount of stock Ships
        /// </summary>
        [DataMember]
        public int StockShips
        {
            get { return _StockShips; }
            set
            {
                if (value != _StockShips)
                {
                    _StockShips = value;
                    OnPropertyChanged("StockShips");
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of stock Towns
        /// </summary>
        [DataMember]
        public int StockTowns
        {
            get { return _StockTowns; }
            set
            {
                if (value != _StockTowns)
                {
                    _StockTowns = value;
                    OnPropertyChanged("StockTowns");
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of stock Cities
        /// </summary>
        [DataMember]
        public int StockCities
        {
            get { return _StockCities; }
            set
            {
                if (value != _StockCities)
                {
                    _StockCities = value;
                    OnPropertyChanged("StockCities");
                }
            }
        }

        [DataMember]
        public int BonusNewIsland { get; set; }

        /// <summary>
        /// Gets or sets the amount of minimum players required to play the board
        /// </summary>
        [DataMember]
        public int MinPlayers
        {
            get { return _MinPlayers; }
            set
            {
                if (value > 2)
                {
                    if (value != _MinPlayers)
                    {
                        _MinPlayers = value;
                        OnPropertyChanged("MinPlayers");
                    }
                    if (value > _MaxPlayers) MaxPlayers = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of maximum players required to play the board
        /// </summary>
        [DataMember]
        public int MaxPlayers
        {
            get { return _MaxPlayers; }
            set
            {
                if (value != _MaxPlayers)
                {
                    _MaxPlayers = value;
                    OnPropertyChanged("MaxPlayers");
                }
            }
        }

        /// <summary>
        /// Returns amount of min/max players in human readably format
        /// </summary>
        public string TotalPlayers
        {
            get
            {
                if (_MinPlayers == _MaxPlayers)
                {
                    return _MinPlayers.ToString();
                }
                else
                {
                    return _MinPlayers.ToString() + "-" + _MaxPlayers.ToString();
                }
            }
        }

        [DataMember]
        //Gets or sets the amount of VP needed to win on this board
        public int VpToWin
        {
            get { return _VpToWin; }
            set
            {
                if (value != _VpToWin)
                {
                    _VpToWin = value; 
                    OnPropertyChanged("VpToWin");
                }

            }
        }

        [DataMember]
        public StandardDevCardStack DevCards { get; set; }

        [DataMember]
        public EBoardCreatedType BoardType { get; set; }

        /// <summary>
        /// Gets or sets the name of the board
        /// TODO: Add logic to prevent official names to be used
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value; 
                    OnPropertyChanged("Name");
                }

            }
        }
        /// <summary>
        /// Gets or sets the name of the creator of the board.
        /// </summary>
        [DataMember]
        public string Creator
        {
            get { return _Creator; }
            set
            {
                if (value != _Creator)
                {
                    _Creator = value; 
                    OnPropertyChanged("Creator");
                }

            }
        }

        [DataMember]
        public ListArrayOfT Hexes
        {
            get { return _Hexes; }
            set 
            {
                if (_Hexes != value)
                {
                    _Hexes = value;
                    if (_Hexes.Width == 0 || _Hexes.Height == 0)
                    {
                        _Hexes.Width = _Width;
                        _Hexes.Height = _Height;
                    }
                    OnPropertyChanged("Hexes");
                }

            }
        }

        [DataMember]
        public Guid ID { get; set; }

        #endregion

        public XmlBoard()
        {
            SetDefault();
        }

        public Territory GetTerritory(int id)
        {
            var possibleResults = from t in _Territories
                                  where t.ID == id
                                  select t;

            if (possibleResults.Count() > 0)
                return possibleResults.First();
            else
                return null;
        }

        public IEnumerable<HexPoint> DiscoverPoints()
        {
            throw new NotImplementedException();
 
        }


        private void SetDefault()
        {
            // Rules settings fields. Default on standard rules
            Territories = Territory.CreateList();
            IsSeafarers = false;
            UseTradeRoutes = false;
            AssignPortsBeforePlacement = false;
            RequiresInitialShips = false;
            AllowedCards = 7;
            BankResources = 19;
            StockRoads = 15;
            StockShips = 15;
            StockTowns = 5;
            StockCities = 4;
            BonusNewIsland = 1;
            MaxPlayers = 4;
            MinPlayers = 3;
            VpToWin = 10;
            DevCards = new StandardDevCardStack(5, 2, 14, 2, 2);
            BoardType = EBoardCreatedType.Official;
            Creator = "Unknown player";
            Name = String.Format("{0}'s ground", Creator);
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new board with specified height and width
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public XmlBoard(int width, int height)
            : this()
        {
            _Width = width;
            _Height = height;
            _Hexes = new ListArrayOfT(width, height);
            
            // Default empty board is filled with seahexes
            for (int h = 0; h < height; h++)
                for (int w = 0; w < width; w++)
                    _Hexes.Add(new SeaHex() { Location = new HexLocation() { W = w, H = h } });
        }

        public XmlBoard(ListArrayOfT hexes)
        {
            _Hexes = new ListArrayOfT(hexes.Width, hexes.Height);
            foreach (Hex h in hexes)
                _Hexes.Add(h);
        }

        public void Save()
        {
            DataContractSerializer xmlSerializer = new DataContractSerializer(this.GetType());
            XmlWriter xmlWriter = new XmlTextWriter(@"e:\documents\SettleIn\Boards" + @"\" + Name + ".xml.sib", Encoding.UTF8);
            //xmlSerializer.WriteObject(Console.Out, this);
            xmlSerializer.WriteObject(xmlWriter, this);

            xmlWriter.Close();
        }

        public bool FileExists()
        {
            return File.Exists(@"d:\documents\SettleIn\Boards" + @"\" + Name + ".xml.sib");
        }

        /// <summary>
        /// Prepares a saved board definition into a playable board.
        /// 1. Puts hexes from InitialRandomHexes list on RandomHexes
        /// 2. Replaces random ports from those out of RandomPorts bag
        /// 3. Replaces deserts by volcano/jungles if necessary
        /// </summary>
        public void PrepareForPlay(XmlGameSettings settings)
        {
            Random random = new Random();
            Hex newHex = null;

            // we assume each hex has a territory associated with it
            for (int i=0; i< Hexes.Count;i++)
            {
                Hex hex = Hexes[i];
                ITerritoryHex terrHex = hex as ITerritoryHex;
                if (terrHex != null)
                {
                    // grab the according territory from the list
                    Territory territory = _Territories.ToList().Find
                        (t => t.ID == terrHex.TerritoryID);

                    // Replace randomhexes with a new hex picked form the list of resources
                    RandomHex randomHex = hex as RandomHex;
                    if (randomHex != null)
                    {
                        // pick a hex from the list of hexes in the territory
                        newHex = territory.HexList.PickHexFromList(
                            (int)(random.NextDouble() * territory.HexList.CountAll));

                        //copy terrtoryID to new hex
                        ((ITerritoryHex)newHex).TerritoryID = randomHex.TerritoryID;
                        newHex.Location = hex.Location;

                        if (newHex is ResourceHex)
                        {
                            int chitno = (int)(random.NextDouble() * territory.ChitList.CountAll);
                            ((ResourceHex)newHex).XmlChit = new Chit() { ChitNumber = territory.ChitList.PickNumberFromBag() };
                        }
                    }

                    //Replace randomchit by actual chitnumbers from the correct bag
                    if (hex is ResourceHex)
                    {
                        ResourceHex resourceHex = hex as ResourceHex;
                        if (resourceHex.XmlChit == null)
                        {
                            resourceHex.XmlChit = new Chit() { ChitNumber = EChitNumber.None };
                        }
                        if (resourceHex.XmlChit.ChitNumber == EChitNumber.Random)
                        {
                            int chitno = (int)(random.NextDouble() * territory.ChitList.CountAll);
                            resourceHex.XmlChit = new Chit();
                            resourceHex.XmlChit.ChitNumber = territory.ChitList.PickNumberFromBag();
                        }
                    }

                    // swap randomport for ports from the list of randomports
                    /* TODO: determine ownership of port for landhex where port
                     * is connected to
                    if (hex is SeaHex)
                    {
                        SeaHex seaHex = hex as SeaHex;
                        if (seaHex.XmlPort != null)
                        {
                            if (seaHex.XmlPort.PortType == EPortType.Random)
                            {
                                EPortType porttype = territory.PortList.PickPortFromBag();
                                seaHex.XmlPort = new Port() { PortType = porttype };
                            }
                        }
                    }
                     */

                    //Replace desert by volcano or jungle if setting is on on mainland
                    if (!settings.DoNotReplaceDeserts &&
                        hex is DesertHex &&
                        territory.IsMainland)
                    {
                        if (settings.ReplaceDesertWithVolcanos)
                        {
                            newHex = new VolcanoHex()
                            {
                                XmlChit = new Chit() { ChitNumber = territory.ChitList.PickRandomChit() },
                                Location = hex.Location
                            };
                        }
                        if (settings.ReplaceDesertWithJungles)
                        {
                            newHex = new JungleHex()
                            {
                                XmlChit = new Chit() { ChitNumber = territory.ChitList.PickRandomChit() },
                                Location = hex.Location
                            };
                        }
                    }

                    // swap the new hex with the old one
                    if (newHex != null)
                    {
                        _Hexes[hex.Location] = newHex;
                        newHex = null;
                    }
                }
            }

            //TODO: Create a list of chit swaps to swap out 6/8 and 9/9 neighbours
            // This allows for a nice visualization where this process is 
            // clearly showed to the players
        }

        /// <summary>
        /// Resizes the board to a new size. 
        /// </summary>
        /// <param name="newWidth">New width of the board</param>
        /// <param name="newHeight">New height of the board</param>
        public void Resize(int newWidth, int newHeight, Hex defaultHex)
        {
            // default on seahexes if we have no default
            if (defaultHex == null) defaultHex = new SeaHex();
            
            //return if there is nothing to resize
            if (_Width == newWidth && _Height == newHeight)
            {
                return;
            }

            //Instantiate a new board
            ListArrayOfT newboard = new ListArrayOfT(newWidth, newHeight);

            //loop through new sized matrix.
            for (int h = 0; h < newHeight; h++)
            {
                for (int w = 0; w < newWidth; w++)
                {
                    //when width or height is bigger then original, add hexes
                    if (w >= _Width || h >= _Height)
                    {
                        Hex newHex = null;

                        //if outer bounds, put a SeaHex in place, otherwise a defaulthex
                        if (w == newWidth - 1 || w == 0 || h == newHeight - 1 || h == 0)
                            newHex = new SeaHex();
                        else
                            newHex = defaultHex.Copy();

                        newHex.Location = new HexLocation() { W = w, H = h };
                        newboard[w, h] = newHex;
                    }
                    else
                    {
                        //if outer bounds, put a seahex in place, 
                        // otherwise the defaulthex
                        if (w == newWidth - 1 || w == 0 || h == newHeight - 1 || h == 0)
                        {
                            newboard[w, h] = new SeaHex();
                        }
                        else
                        {
                            newboard[w, h] = defaultHex.Copy();
                        }

                        newboard[w, h] = Hexes[w, h].Copy();
                    }

                }
            }
            Hexes = newboard;
        }

        public XmlBoard Copy()
        {
            XmlBoard result = new XmlBoard(_Hexes);

            result.AllowedCards = _AllowedCards;
            result.AssignPortsBeforePlacement = _AssignPortsBeforePlacement;
            result.BankResources = _BankResources;
            result.BoardType = _BoardType;
            result.BonusNewIsland = _BonusNewIsland;
            result.Creator = _Creator;
            result.DevCards = _DevCards;
            result.IsSeafarers = _IsSeafarers;
            result.MaxPlayers = _MaxPlayers;
            result.MinPlayers = _MinPlayers;
            result.Name = _Name;
            result.RequiresInitialShips= _RequiresInitialShips;
            result.StockCities = _StockCities;
            result.StockRoads = _StockRoads;
            result.StockShips = _StockShips;
            result.StockTowns = _StockTowns;
            result.Territories = new ObservableCollection<Territory>();
            if (_Territories != null)
            {
                foreach (Territory t in _Territories)
                    result._Territories.Add(t.Copy());
            }
            result._UseTradeRoutes = _UseTradeRoutes;
            result._VpToWin = _VpToWin;
            result.Width = _Width;
            result.Height = _Height;

            return result;
        }

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


        #endregion
    }
}
