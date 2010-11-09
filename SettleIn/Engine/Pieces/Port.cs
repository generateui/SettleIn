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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Boards;

namespace SettleIn
{
    [Serializable]
    public class PortVisual : Piece
    {
        private MeshGeometry3D port = new MeshGeometry3D();
        private ImageBrush _Brush;
        private Port _XmlPort;

        /// <summary>
        /// The actual port data
        /// </summary>
        public Port XmlPort
        {
            get { return _XmlPort; }
        }

        public delegate void PortTypeChangedHandler();
        public event PortTypeChangedHandler PortTypeChanged;
        private void OnPortTypeChanged()
        {
            if (PortTypeChanged != null) PortTypeChanged();
        }

        public PortVisual(Point2D point, Port pt)
            :base(point)
        {
            this._XmlPort = pt;
            Init();
        }

        private void Init()
        {
            double w = Hex.Width;
            double h = Hex.BottomHeight;
            double offsety = .01;
            double x = _Point.X;
            double y = _Point.Y;
            double r = Hex.HalfWidth;
            double s = Hex.Size;
            double a = Hex.Width;
            double b = Hex.Height;

            Point3D[] pos = new Point3D[7] 
            {
                new Point3D(x + r, offsety, y + 0),
                new Point3D(x + a, offsety, y + h),
                new Point3D(x + a, offsety, y + s + h),
                new Point3D(x + r, offsety, y + b),
                new Point3D(x + 0, offsety, y + s + h),
                new Point3D(x + 0, offsety, y + h),
                new Point3D(x + r, offsety, y + (b/2)),
            };
            foreach (Point3D p in pos)
            {
                port.Positions.Add(p);
            }
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
            port.TriangleIndices = col;

            //ImageBrush ib = new ImageBrush();
            //RenderOptions.SetCachingHint(ib,CachingHint.Cache);
            //RenderOptions.SetCacheInvalidationThresholdMinimum(ib, 0.5);
            //RenderOptions.SetCacheInvalidationThresholdMaximum(ib, 3.0);
            //ib.ImageSource = new BitmapImage(new Uri("c:\\test.png"));
            //Material material = new DiffuseMaterial(ib);

            port.TextureCoordinates.Add(new System.Windows.Point(r, 0));
            port.TextureCoordinates.Add(new System.Windows.Point(a, h));
            port.TextureCoordinates.Add(new System.Windows.Point(a, s + h));
            port.TextureCoordinates.Add(new System.Windows.Point(r, b));
            port.TextureCoordinates.Add(new System.Windows.Point(0, s + h));
            port.TextureCoordinates.Add(new System.Windows.Point(0, h));
            port.TextureCoordinates.Add(new System.Windows.Point(r, b / 2));
            _Brush = new ImageBrush(Texture);
            Material m = new DiffuseMaterial(_Brush);
            GeometryModel3D triangleModel = new GeometryModel3D(port, m);
            this.Content = triangleModel;
        }
        public void Attach(Model3DGroup m)
        {
            m.Children.Add(this.Content);
        }

        public ImageSource Texture
        {
            get
            {
                switch (_XmlPort.PortType)
                {
                    case EPortType.Timber:     return (ImageSource)Core.Instance.Textures["TimberPortTexture"];
                    case EPortType.Wheat:      return (ImageSource)Core.Instance.Textures["WheatPortTexture"];
                    case EPortType.Ore:        return (ImageSource)Core.Instance.Textures["OrePortTexture"];
                    case EPortType.Clay:       return (ImageSource)Core.Instance.Textures["ClayPortTexture"];
                    case EPortType.Sheep:      return (ImageSource)Core.Instance.Textures["SheepPortTexture"];
                    case EPortType.ThreeToOne: return (ImageSource)Core.Instance.Textures["31PortTexture"];
                    case EPortType.Random:     return (ImageSource)Core.Instance.Textures["RandomPortTexture"];
                }
                return null;
            }
        }

        public ImageBrush Brush
        { get { return _Brush; } }

    }
}
