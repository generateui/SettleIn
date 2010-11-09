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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using SettleInCommon;
using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Pieces;
using SettleIn.Engine.Boards;
using SettleInCommon.Gaming;
using System.Collections.Specialized;
using SettleIn.Classes.Pieces;
using SettleIn.Engine.Pieces.ControlPieces;

namespace SettleIn
{
    public partial class BoardVisual : ModelVisual3D
    {
        #region Fields

        private XmlGame _Game;
        private XmlBoard _Board;
        private HexLocation _OldRobberLocation;
        private HexLocation _OldPirateLocation;
        private Robber _Robber = new Robber(new Point2D(0, 0), new HexLocation(0, 0));
        private Pirate _Pirate = new Pirate(new Point2D(0, 0), new HexLocation(1, 0));
        private RoutePlaces _ShipPlaces;
        private RoutePlaces _RoadPlaces;
        private BuildPlaces _BuildPlaces;
        //placeholder for the thumbnail data
        private byte[] _ThumbnailData;
        //should the board show territory icons?
        private bool _ShowTerritory = true;
        [NonSerialized]
        private BitmapSource _Thumbnail;

        #endregion

        #region Properties

        public XmlGame Game
        {
            get { return _Game; }
            set
            {
                if (value != _Game)
                {
                    _Game = value;
                    CreateUI();
                    AddBoardHandlers();
                    if (_Game != null)
                        AddGameHandlers();
                    //OnPropertyChanged("Board");
                }
            }
        }

        public XmlBoard Board
        {
            get
            {
                if (_Game != null)
                    return _Game.Board;
                else
                    return _Board;
            }
            set
            {
                if (_Game != null)
                    _Game.Board = value;
                else
                    _Board = value;
                if (value != null)
                {
                    ResetUI();
                    CreateUI();
                    AddBoardHandlers();
                }
                //OnPropertyChanged("Board");
            }
        }

        /// <summary>
        /// Returns a generated thumbnail
        /// </summary>
        public BitmapSource Thumbnail
        {
            get
            {
                if (_ThumbnailData != null)
                {
                    BitmapSource bs = BitmapSource.Create(120, 90, 96, 96,
                        PixelFormats.Pbgra32,
                        BitmapPalettes.Halftone256Transparent,
                        _ThumbnailData, 120 * 4);
                    _Thumbnail = bs;
                }
                return _Thumbnail;
            }
        }

        /// <summary>
        /// Gets or sets whether or not to show icons on the hexes showing their territory
        /// </summary>
        public bool ShowTerritory
        {
            get { return _ShowTerritory; }
            set
            {
                var landhexes = from h in Children.OfType<HexVisual>()
                                where (h.Hex is ITerritoryHex)
                                select h;
                foreach (HexVisual h in landhexes)
                        h.ShowTerritory(_ShowTerritory);
            }
        }

        #endregion

        #region Constructors

        public BoardVisual()
        {
            Children.Add(_Robber);
            Children.Add(_Pirate);
        }

        public BoardVisual(XmlGame game)
            : this()
        {
            Game = game;
        }

        public BoardVisual(XmlBoard board)
            : this()
        {
            Board = board;
        }

        private void ResetUI()
        {
            Children.Clear();
        }

        private void CreateUI()
        {
            if (Board != null)
            {
                foreach (Hex xmlHex in Board.Hexes)
                {
                    Point2D point = CalculatePosition(xmlHex.Location);

                    //make a new hex with the parameters
                    HexVisual hexToAdd = new HexVisual(point, xmlHex);
                    hexToAdd.Parent = this;
                    xmlHex.PropertyChanged += new PropertyChangedEventHandler(xmlHex_PropertyChanged);

                    //add hex to data structure and visual structure
                    this.Children.Add(hexToAdd);
                }
            }
        }

        /// <summary>
        /// Invoked when a hex in the list of hexes changed a property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void xmlHex_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Hex hex = sender as Hex;
            HexVisual hexVisual = GetHexVisualByHex(hex);
            switch (e.PropertyName)
            {
                case "PortPosition": hexVisual.UpdatePortRotation(); return;
                case "PortType": hexVisual.UpdatePortType(); return;
                case "Port": hexVisual.UpdatePort(); return;
                case "Chit": hexVisual.UpdateChit(); return;
                case "TerritoryID": hexVisual.UpdateTerritory(); return;
            }
        }

        public void HighlightTerritory(int ID)
        {
            foreach (HexVisual hv in Children.OfType<HexVisual>())
            {
                ITerritoryHex ith = hv.Hex as ITerritoryHex;
                if (ith == null)
                    hv.Selected = true;
                else
                    hv.Selected = false;
            }
        }

        #endregion

        #region private methods

        public void PickRoad(HexPoint townOrCity)
        {
            if (Children.Contains(_RoadPlaces)) Children.Remove(_RoadPlaces);

            if (townOrCity != null)
            {
                _RoadPlaces = new RoutePlaces(_Game, this, townOrCity);
            }
            else
            {
                _RoadPlaces = new RoutePlaces(_Game, this, false);
            }
            Children.Add(_RoadPlaces);
        }
        public void PickShip()
        {
            if (Children.Contains(_ShipPlaces)) Children.Remove(_ShipPlaces);
            _ShipPlaces = new RoutePlaces(_Game, this, true);
            Children.Add(_ShipPlaces);
        }
        public void PickCity(bool isStart)
        {
            if (Children.Contains(_BuildPlaces))
                Children.Remove(_BuildPlaces);

            if (isStart)
                _BuildPlaces = new BuildPlaces(this, _Game.AllTownsCities());
            else
                _BuildPlaces = new BuildPlaces(this, Game.PlayerOnTurn.Towns.ToList<HexPoint>(), true);

            Children.Add(_BuildPlaces);

        }
        public void ShowNeighbours(HexSide side)
        {
            if (Children.Contains(_BuildPlaces))
                Children.Remove(_BuildPlaces);

            _BuildPlaces = new BuildPlaces(this, side);
            Children.Add(_BuildPlaces);
        }

        public void ShowPoints(List<HexPoint> points)
        {
            if (Children.Contains(_BuildPlaces))
                Children.Remove(_BuildPlaces);
            _BuildPlaces = new BuildPlaces(points, this);
            Children.Add(_BuildPlaces);
        }
        internal void ShowSideNeighbours(HexSide hexSide)
        {
            if (Children.Contains(_RoadPlaces))
                Children.Remove(_RoadPlaces);

            _RoadPlaces = new RoutePlaces(this, hexSide);
            Children.Add(_RoadPlaces);
        }

        public void PickPoint(bool isStart)
        {
            if (Children.Contains(_BuildPlaces)) 
                Children.Remove(_BuildPlaces);

            if (isStart)
                _BuildPlaces = new BuildPlaces(this, _Game.AllTownsCities());
            else
                _BuildPlaces = new BuildPlaces(this, Game.PlayerOnTurn);
            
            Children.Add(_BuildPlaces);
        }
        public void SetMoveShip()
        {
            if (Children.Contains(_ShipPlaces))
                Children.Remove(_ShipPlaces);

            _ShipPlaces = new RoutePlaces(Game.PlayerOnTurn.GetMoveableShips(Game, Board), this);

            Children.Add(_ShipPlaces);
        }
        public void SetMoveToShip(HexSide movedShip)
        {
            if (Children.Contains(_ShipPlaces))
                Children.Remove(_ShipPlaces);

            _ShipPlaces = new RoutePlaces(Game.PlayerOnTurn.GetShipBuildPlaces(Game, Board, movedShip), this);

            Children.Add(_ShipPlaces);
        }
        public void SetNeutral()
        {
            if (Children.Contains(_BuildPlaces))
                Children.Remove(_BuildPlaces);

            if (Children.Contains(_RoadPlaces))
                Children.Remove(_RoadPlaces);

            if (Children.Contains(_ShipPlaces))
                Children.Remove(_ShipPlaces);
        }

        /// <summary>
        /// Adds necessary handlers to the hex' properties
        /// </summary>
        private void AddBoardHandlers()
        {
            if (Board != null)
            {
                Board.PropertyChanged += new PropertyChangedEventHandler(_XmlBoard_PropertyChanged);
                Board.Hexes.HexChanged += new ListArrayOfT.HexChangedEventHandler(Hexes_HexChanged);
            }

            //_XmlBoard.PropertyChanged
            /*
            foreach (ResourceHex h in from h in _XmlBoard.Hexes where h is ResourceHex select h)
                //if (h.XmlChit != null)
                    //h.XmlChit.ChitNumberChanged += new ChitVisual.ChitNumberChangedHandler(Chit_ChitChanged);

            foreach (SeaHex h in from h in _Hexes where h is SeaHex select h)
            {
                h.PortChanged += new SeaHex.PortChangedHandler(h_PortChanged);
                if (h.Port != null)
                    h.Port.PortTypeChanged += new Port.PortTypeChangedHandler(Port_PortTypeChanged);
            }
             */
        }

        private void AddGameHandlers()
        {
            if (_Game != null)
            {
                foreach (GamePlayer player in _Game.Players)
                {
                    player.Towns.CollectionChanged +=
                        new NotifyCollectionChangedEventHandler(Towns_CollectionChanged);
                    player.Cities.CollectionChanged +=
                        new NotifyCollectionChangedEventHandler(Cities_CollectionChanged);
                    player.Roads.CollectionChanged +=
                        new NotifyCollectionChangedEventHandler(Roads_CollectionChanged);
                    player.Ships.CollectionChanged +=
                        new NotifyCollectionChangedEventHandler(Ships_CollectionChanged);
                }
                _Game.LongestRouteChanged += new XmlGame.LongestRouteChangedHandler(_Game_LongestRouteChanged);
                _Game.PropertyChanged += new PropertyChangedEventHandler(_Game_PropertyChanged);
                _Game.PropertyChanging += new PropertyChangingEventHandler(_Game_PropertyChanging);
            }
        }

        void _Game_LongestRouteChanged(GamePlayer player, Route newRoute)
        {
            ShowLongestRoute();   
        }

        void _Game_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "Robber")
                _OldRobberLocation = _Game.Robber;
            if (e.PropertyName == "Pirate")
                _OldPirateLocation = _Game.Pirate;
        }

        void _Game_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Robber")
                MoveThing((IMoveable)_Robber, _OldRobberLocation, Game.Robber);
            if (e.PropertyName == "Pirate")
                MoveThing((IMoveable)_Pirate, _OldPirateLocation, Game.Pirate);
        }

        /// <summary>
        /// Shows animation of thing moved from start- to endposition
        /// </summary>
        /// <param name="mover"></param>
        /// <param name="old"></param>
        /// <param name="newLoc"></param>
        private void MoveThing(IMoveable mover, HexLocation old, HexLocation newLoc)
        {
            Point2D oldPoint = AddRobberOffset(CalculatePosition(old));
            Point2D newPoint = AddRobberOffset(CalculatePosition(newLoc));
            int duration = 1000;
            DoubleAnimation dax = new DoubleAnimation(oldPoint.X, newPoint.X, new Duration(new TimeSpan(0, 0, 0, 0, duration)));
            DoubleAnimation day = new DoubleAnimation(oldPoint.Y, newPoint.Y, new Duration(new TimeSpan(0, 0, 0, 0, duration)));
            DoubleAnimation daz = new DoubleAnimation(0, 20, new Duration(new TimeSpan(0, 0, 0, 0, duration / 2)));
            daz.AutoReverse = true;
            day.AccelerationRatio = 0.4;
            day.DecelerationRatio = 0.4;
            mover.Move.BeginAnimation(TranslateTransform3D.OffsetXProperty, dax);
            mover.Move.BeginAnimation(TranslateTransform3D.OffsetZProperty, day);
            mover.Move.BeginAnimation(TranslateTransform3D.OffsetYProperty, daz);
        }

        private Point2D AddRobberOffset(Point2D point)
        {
            point.X += Hex.HalfWidth;
            point.Y += (Hex.Height / 2);
            return point;
        }

        void Ships_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                HexSide newShip = (HexSide)e.NewItems[0];
                if (newShip != null)
                {
                    Ship s = new Ship(CalculatePosition(newShip), 
                                      GetColorFromPlayer(newShip), 
                                      newShip);
                    Children.Add(s);
                }
            }
        }

        void Roads_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                HexSide newRoad = (HexSide)e.NewItems[0];
                if (newRoad != null)
                {
                    Road r = new Road(CalculatePosition(newRoad), 
                                      GetColorFromPlayer(newRoad), 
                                      newRoad);
                    Children.Add(r);
                }
            }
        }

        public void ShowLongestRoute()
        {
            if (_Game.LongestRoute != null)
            {
                List<Road> roads = Children.OfType<Road>().ToList();
                List<Ship> ships = Children.OfType<Ship>().ToList();
                
                foreach (Ship s in ships) s.IsSelected = false;
                foreach (Road r in roads) r.IsRoadSelected = false;

                foreach (RouteNode rn in _Game.LongestRoute)
                {
                    foreach (Road rr in roads)
                    {
                        if (rr.Location.Equals((HexSide)rn))
                            rr.IsRoadSelected = true;
                    }
                    foreach (Ship s in ships)
                    {
                        if (s.Location.Equals((HexSide)rn))
                            s.IsSelected = true;
                    }
                }
            }
        }

        void Cities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                HexPoint newCity = (HexPoint)e.NewItems[0];
                if (newCity != null)
                {
                    City t = new City(CalculatePosition(newCity), 
                                      GetColorFromPlayer(newCity), 
                                      newCity);
                    Children.Add(t);
                }
            }
        }

        void Towns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                HexPoint newTown = (HexPoint)e.NewItems[0];
                if (newTown != null)
                {
                    Town t = new Town(CalculatePosition(newTown),
                                      GetColorFromPlayer(newTown),
                                      (HexPoint)e.NewItems[0]);
                    Children.Add(t);
                }
            } 
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // get the location of the old town
                HexPoint oldTown = (HexPoint)e.OldItems[0];
                if (oldTown != null)
                {
                    //get the town visual 
                    Visual3D townToRemove = (from t in Children.OfType<Town>() 
                                            where t.Location.Equals(oldTown)
                                            select t).First();
                    
                    // rmove town visual from children
                    this.Children.Remove(townToRemove);
                }
            }
        }

        private Color GetColorFromPlayer(HexPoint point)
        {
            GamePlayer player = (from p in _Game.Players
                                 where p.GetTownsCities().Contains(point)
                                 select p).First();
            return ColorFromString(player.Color);
        }

        private Color GetColorFromPlayer(HexSide side)
        {
            GamePlayer player = (from p in _Game.Players
                                 where p.GetRoadsShips().Contains(side)
                                 select p).First();
            return ColorFromString(player.Color);
        }

        private Color ColorFromString(string val)
        {
            val = val.Replace("#", "");
    
            byte a = System.Convert.ToByte("ff", 16);
            byte pos = 0;
            if (val.Length == 8)
            {
                a = System.Convert.ToByte(val.Substring(pos, 2), 16);
                pos = 2;
            }
    
            byte r = System.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;
    
            byte g = System.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;
    
            byte b = System.Convert.ToByte(val.Substring(pos, 2), 16);
    
            return Color.FromArgb(a, r, g, b);
    
       }
        private void RemoveHandlers()
        {
            if (Game.Board != null)
            {
                Game.Board.PropertyChanged -= _XmlBoard_PropertyChanged;
                Game.Board.Hexes.HexChanged -= Hexes_HexChanged;
            }
        }

        void _XmlBoard_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Hexes")
            {
                Game.Board.Hexes.HexChanged += new ListArrayOfT.HexChangedEventHandler(Hexes_HexChanged);
            }
            if (e.PropertyName == "TownAdded")
            {
                HexPoint newLocation = _Game.PlayerOnTurn.Towns.Last();
                Point2D point = CalculatePosition(newLocation);
                Children.Add(new Town(point, Color.FromRgb(255, 0, 0), newLocation));
            }
        }

        void Hexes_HexChanged(Hex oldHex, Hex newHex)
        {
            oldHex.PropertyChanged -= xmlHex_PropertyChanged;

            HexVisual hexVisual = GetHexVisualByHex(oldHex);
            // lookup hex in children
            Children.Remove(hexVisual);
            Point2D point = CalculatePosition(oldHex.Location);
            HexVisual newHexVisual = new HexVisual(point, newHex);
            newHexVisual.Parent = this;
            //newHexVisual.Transform = new TranslateTransform3D(-MiddleX, 0, -MiddleY);
            Children.Add(newHexVisual);
            newHex.PropertyChanged += new PropertyChangedEventHandler(xmlHex_PropertyChanged);
        }

        public void SelectHexPoint(HexPoint point)
        {
            foreach (HexVisual h in
                from hv in Children.OfType<HexVisual>()
                where hv.Hex.Location.Equals(point.Hex1) ||
                        hv.Hex.Location.Equals(point.Hex2) ||
                        hv.Hex.Location.Equals(point.Hex3)
                select hv)
                h.Selected = true;
        }

        public void UnSelectHexes()
        {
            foreach (HexVisual hv in Children.OfType<HexVisual>())
                hv.Selected = false;
        }

        private HexVisual GetHexVisualByHex(Hex h)
        {
            return (from hv in Children.OfType<HexVisual>()
                    where hv.Hex.Equals(h)
                    select hv).SingleOrDefault();
        }

        public Point2D CalculatePosition(HexLocation location)
        {
            double x = location.W * Hex.Width;
            double y = location.H * Hex.PartialHeight;

            //Alternate half the width of an hex 
            if (location.H % 2 == 0) x += Hex.HalfWidth;

            // center the position
            x -= Hex.HalfWidth * Board.Width;
            y -= ((Hex.PartialHeight * Board.Height) + Hex.BottomHeight) / 2;

            return new Point2D(x, y);
        }

        public Point2D CalculatePosition(HexSide location)
        {
            Point2D result = CalculatePosition(location.HighestOrLeftestHex);

            switch (location.Direction)
            {
                case ESideDirection.SlopeDown:
                    result.X += Hex.Width * 0.25;
                    result.Y += (Hex.BottomHeight * 0.5) + Hex.PartialHeight;
                    break;
                case ESideDirection.SlopeUp:
                    result.X += Hex.Width * 0.75;
                    result.Y += (Hex.BottomHeight * 0.5) + Hex.PartialHeight;
                    break;
                case ESideDirection.UpDown:
                    result.X += Hex.Width;
                    result.Y += Hex.Height * 0.5;
                    break;
            }
            return result;
        }

        public Point2D CalculatePosition(HexPoint location)
        {
            Point2D point = CalculatePosition(location.TopMost);
            double offsetx = 0;// -(_Game.Board.Width * Hex.Width) / 2;
            double offsety = 0;// -((_Game.Board.Height * Hex.PartialHeight) + Hex.BottomHeight);

            if (location.Type == EHexPointType.UpperRow1)
            {

                point.X = point.X + offsetx + Hex.HalfWidth;
                point.Y = point.Y + offsety + Hex.Height;
            }
            else
            {
                point.X = point.X + offsetx + Hex.Width;
                point.Y = point.Y + offsety + Hex.PartialHeight;
            }

            return point;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Calculates thumbnail. 
        /// TODO: make stable fix bugs rehaul code **save planet** #$^*(*di76arm 7665764bomb8888111~~~!!! msg 78&*%*% over563345
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="dpi"></param>
        public void SetImage(Visual visual, Size dpi)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
            int w = (int)(bounds.Width * dpi.Width / 96.0);
            int h = (int)(bounds.Height * dpi.Height / 96.0);
            RenderTargetBitmap rtb = new RenderTargetBitmap(w, h
            ,
            dpi.Width,
            dpi.Height,
            PixelFormats.Pbgra32);


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(visual);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            _ThumbnailData = new byte[120 * 90 * 4];
            byte[] temp = new byte[w * h * 4];
            rtb.CopyPixels(temp, w * 4, 0);
            MemoryStream ms = new MemoryStream();
            BitmapImage bi = new BitmapImage();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] result = new byte[ms.Length];
            BinaryReader br = new BinaryReader(ms);
            br.Read(result, 0, (int)ms.Length);
            br.Close();
            ms.Close();
            bi.BeginInit();
            //bi.DecodePixelHeight = 90;
            bi.DecodePixelWidth = 120;
            bi.StreamSource = new MemoryStream(result);
            bi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bi.CacheOption = BitmapCacheOption.Default;
            bi.EndInit();
            bi.CopyPixels(this._ThumbnailData, 120 * 4, 0);

            this._Thumbnail = rtb;
        }

        public int ToIndex(int w, int h)
        {
            return (Game.Board.Width * h) + w;
        }

        public void FromIndex(int index, out int w, out int h)
        {
            h = index / Game.Board.Height;
            w = index % Game.Board.Width;
            Console.WriteLine("Fromindex {0}, {1}, {2}", w, h, index);
        }

        #endregion

    }
}
