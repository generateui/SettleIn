using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using SettleIn.Engine.Boards;

namespace SettleIn.Engine.Pieces
{
    /// <summary>
    /// 3D hex object. Contains data object (XmlHex) and appropriately adapts texture.
    /// </summary>
    public class HexVisual : Piece
    {
        #region private variables

        public delegate void HexChangedHandler(Hex hex);
        public HexChangedHandler HexChanged;

        private Hex _Hex;
        private MeshGeometry3D hex = new MeshGeometry3D();
        private ImageBrush _ImageBrush;

        private bool _Selected = false;
        private bool _Locked = false;
        private bool _Visible = true;
        private bool _IsDarkened = false;

        private PortVisual _PortVisual;
        private ChitVisual _ChitVisual;
        private TerritoryVisual _TerritoryVisual;

        #endregion

        #region Constructors

        public HexVisual(Point2D point, Hex hex)
            : base(point)
        {
            _Hex = hex;
            Init();
            InitHex();
        }

        private void InitHex()
        {
            UpdateChit();
            UpdatePort();
            UpdatePortRotation();
            UpdateTerritory();
        }

        #endregion

        #region Properties

        public Hex Hex
        {
            get { return _Hex; }

        }

        /// <summary>
        /// Set this to true when the user shouldnt be able to change the hex on the board
        /// </summary>
        public bool Locked
        {
            get { return _Locked; }
            set { _Locked = value; }
        }

        /// <summary>
        /// Returns true if the hex should be visible
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }

        /// <summary>
        /// Returns true if the hex is mouseover
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                //if (value != _Selected)
                {
                    _Selected = value;
                    //SetImage();
                    if (value)
                    {
                        this.Brush.Opacity = .75;
                        //TODO: Set children also opacityvalue
                        //foreach (ModelVisual3D m in Children) m.br
                    }
                    else
                    {
                        this.Brush.Opacity = 1;
                    }
                }
            }
        }
        public bool IsDarkened
        {
            get { return _IsDarkened; }
            set 
            { 
                _IsDarkened = value;
                if (value)
                    Brush.Opacity = .3;
                else
                    Brush.Opacity = 1;

            }
        }

        
        /// <summary>
        /// returns brush
        /// </summary>
        public System.Windows.Media.Brush Brush 
        { get { return _ImageBrush; } }

        protected virtual ImageSource Texture
        {
            get 
            {
                // the name of the class (SeaHex, OreHex, etc) is used as 
                // dictionary key
                return (ImageSource)Core.Instance.Textures[_Hex.GetType().Name];
            }
        }

        #endregion

        /// <summary>
        /// Create a 3D model according to the size specified in Hex.s.
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        private void Init()
        {
            Point3D[] pos = new Point3D[7] 
            {
                new Point3D(_Point.X + Hex.HalfWidth, 0,   _Point.Y + 0),
                new Point3D(_Point.X + Hex.Width, 0,       _Point.Y + Hex.BottomHeight),
                new Point3D(_Point.X + Hex.Width, 0,       _Point.Y + Hex.Size + Hex.BottomHeight),
                new Point3D(_Point.X + Hex.HalfWidth, 0,   _Point.Y + Hex.Height),
                new Point3D(_Point.X + 0, 0,               _Point.Y + Hex.Size + Hex.BottomHeight),
                new Point3D(_Point.X + 0, 0,               _Point.Y + Hex.BottomHeight),
                new Point3D(_Point.X + Hex.HalfWidth, 0,   _Point.Y + (Hex.Height/2)),
            };
            foreach (Point3D p in pos)
            {
                hex.Positions.Add(p);
            }

            // 6 is the middle of the hex
            Int32[] indices = 
            {
                0,6,1,
                1,6,2,
                2,6,3,
                3,6,4,
                4,6,5,
                5,6,0,
            };
            Int32Collection col = new Int32Collection();
            foreach (Int32 i in indices)
            {
                col.Add(i);
            }
            hex.TriangleIndices = col;

            //ImageBrush ib = new ImageBrush();
            //RenderOptions.SetCachingHint(ib,CachingHint.Cache);
            //RenderOptions.SetCacheInvalidationThresholdMinimum(ib, 0.5);
            //RenderOptions.SetCacheInvalidationThresholdMaximum(ib, 3.0);
            //ib.ImageSource = new BitmapImage(new Uri("c:\\test.png"));
            //Material material = new DiffuseMaterial(ib);

            hex.TextureCoordinates.Add(new System.Windows.Point(Hex.HalfWidth, 0));
            hex.TextureCoordinates.Add(new System.Windows.Point(Hex.Width, Hex.BottomHeight));
            hex.TextureCoordinates.Add(new System.Windows.Point(Hex.Width, Hex.PartialHeight));
            hex.TextureCoordinates.Add(new System.Windows.Point(Hex.HalfWidth, Hex.Height));
            hex.TextureCoordinates.Add(new System.Windows.Point(0, Hex.PartialHeight));
            hex.TextureCoordinates.Add(new System.Windows.Point(0, Hex.BottomHeight));
            hex.TextureCoordinates.Add(new System.Windows.Point(Hex.HalfWidth, Hex.Height / 2));

            SetImage();
        }
        private void SetImage()
        {
            hex.Freeze();
            _ImageBrush = new ImageBrush(Texture);

            RenderOptions.SetCachingHint(_ImageBrush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(_ImageBrush, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(_ImageBrush, 3.0);

            Material m = new DiffuseMaterial(_ImageBrush);
            GeometryModel3D triangleModel = new GeometryModel3D(hex, m);
            this.Content = new Model3DGroup();
            ((Model3DGroup)this.Content).Children.Add(triangleModel);
        }

        public void Attach(Model3DGroup m)
        {
            m.Children.Add(this.Content);
        }

        /// <summary>
        /// Used to change the rotation of the port on the hex. 
        /// </summary>
        public void UpdatePortRotation()
        {
            if (_PortVisual != null)
            {
                double angle = 0;
                switch (((SeaHex)_Hex).XmlPort.PortPosition)
                {
                    case ERotationPosition.Deg0: angle = 120; break;
                    case ERotationPosition.Deg60: angle = 180; break;
                    case ERotationPosition.Deg120: angle = 240; break;
                    case ERotationPosition.Deg180: angle = 300; break;
                    case ERotationPosition.Deg240: angle = 0; break;
                    case ERotationPosition.Deg300: angle = 60; break;
                }

                double x = _Point.X + Hex.HalfWidth;
                double y = _Point.Y + (Hex.Height / 2);
                _PortVisual.Content.Transform =
                    new RotateTransform3D(
                        new AxisAngleRotation3D(
                            new Vector3D(0, 1, 0), angle), x, 0, y);
            }
        }

        public void UpdatePort()
        {
            SeaHex seaHex = _Hex as SeaHex;
            if (seaHex != null)
            {
                // remove the old visual
                if (Children.Contains(_PortVisual))
                    Children.Remove(_PortVisual);
                _PortVisual = null;

                if (seaHex.XmlPort != null)
                {
                    _PortVisual = new PortVisual(_Point, seaHex.XmlPort);
                    Children.Add(_PortVisual);
                }
            }
        }

        public void UpdateTerritory()
        {
            ITerritoryHex terrHex = _Hex as ITerritoryHex;
            if (terrHex != null)
            {
                if (Children.Contains(_TerritoryVisual)) 
                    Children.Remove(_TerritoryVisual);
                _TerritoryVisual = null;

                Territory territory = null;
                BoardVisual parentBoard = Parent as BoardVisual;
                if (parentBoard != null)
                    territory = parentBoard.Board.GetTerritory(terrHex.TerritoryID);

                _TerritoryVisual = new TerritoryVisual(_Point, territory);
                Children.Add(_TerritoryVisual);
            }
        }

        public void UpdateChit()
        {
            ResourceHex resourceHex = _Hex as ResourceHex;
            if (resourceHex != null)
            {
                //remove old visual
                if (Children.Contains(_ChitVisual))
                    Children.Remove(_ChitVisual);
                _ChitVisual = null;
              
                if (resourceHex.XmlChit != null)
                {
                    _ChitVisual = new ChitVisual(_Point, resourceHex.XmlChit.ChitNumber);
                    _ChitVisual.Parent = this;
                    Children.Add(_ChitVisual);
                }
            }
        }

        public void ShowTerritory(bool show)
        {
            ITerritoryHex terrHex = _Hex as ITerritoryHex;
            if (terrHex != null)
            {
                if (show)
                {
                    // remove the visual from viewing area
                    _TerritoryVisual.Transform = null;
                }
                else
                {
                    _TerritoryVisual.Transform = new TranslateTransform3D(1000, 0, 0);
                }
            }
        }
        public void AnimatePortSetted()
        {
            if (_PortVisual != null)
            {
                _PortVisual.Brush.Opacity = 0;
                DoubleAnimation da = new DoubleAnimation(1.0, new Duration(new TimeSpan(2000000)));
                _PortVisual.Brush.BeginAnimation(System.Windows.Media.Brush.OpacityProperty, da);
            }
        }

        public void UpdatePortType()
        {
            UpdatePort();
        }
    }
}
