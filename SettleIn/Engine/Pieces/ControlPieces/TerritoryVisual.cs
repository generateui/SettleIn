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
using SettleIn.Engine.Boards;
using SettleInCommon.Board;

namespace SettleIn
{
    [Serializable]
    public class TerritoryVisual : Piece
    {
        MeshGeometry3D icon = new MeshGeometry3D();
        private ImageBrush _Brush;
        protected int _ID = 1;
        private Territory _Territory;

        public Territory Territory
        {
            get { return _Territory; }
        }

        public int ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                }
            }
        }

        public TerritoryVisual(Point2D point, Territory territory)
            : base(point)
        {
            _Territory = territory;
            Init();
        }

        protected void Init()
        {
            int offsetx = 5;
            int offsety = 1;
            int offsetz = 2;
            int s = 3;

            Point3D[] pos = new Point3D[4] 
            {
                new Point3D(_Point.X + offsetx + 1, offsety + 1, _Point.Y + offsetz + 1),
                new Point3D(_Point.X + offsetx + 2 + s, offsety + 1, _Point.Y + offsetz + 1),
                new Point3D(_Point.X + offsetx + 2 + s, offsety + 1, _Point.Y + offsetz + 2 + s),
                new Point3D(_Point.X + offsetx + 1, offsety + 1, _Point.Y + offsetz + 2 + s),
            };
            foreach (Point3D p in pos)
            {
                icon.Positions.Add(p);
            }
            Int32[] indices = 
            {
                0,2,1,
                0,3,2,
            };
            Int32Collection col = new Int32Collection();
            foreach (Int32 i in indices)
            {
                col.Add(i);
            }

            icon.TriangleIndices = col;
            icon.TextureCoordinates.Add(new System.Windows.Point(offsetx + 1, offsetz + 1));
            icon.TextureCoordinates.Add(new System.Windows.Point(offsetx + 2 + s, offsetz + 1));
            icon.TextureCoordinates.Add(new System.Windows.Point(offsetx + 2 + s, offsetz + 2 + s));
            icon.TextureCoordinates.Add(new System.Windows.Point(offsetx + 1, offsetz + 2 + s));

            _Brush = new ImageBrush(GetSource());
            Material m = new DiffuseMaterial(_Brush);
            GeometryModel3D triangleModel = new GeometryModel3D(icon, m);
            this.Content = triangleModel;
        }

        private ImageSource GetSource()
        {
            if (_Territory != null)
            {
                if (_Territory.IsMainland)
                    return (ImageSource)Core.Instance.Textures["MainLandTexture"];
                else
                    switch (_Territory.ID)
                    {
                        case 1: return (ImageSource)Core.Instance.Textures["IslandTexture1"];
                        case 2: return (ImageSource)Core.Instance.Textures["IslandTexture2"];
                        case 3: return (ImageSource)Core.Instance.Textures["IslandTexture3"];
                        case 4: return (ImageSource)Core.Instance.Textures["IslandTexture4"];
                        case 5: return (ImageSource)Core.Instance.Textures["IslandTexture5"];
                        case 6: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                        case 7: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                        case 8: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                        case 9: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                    }
            }
            return (ImageSource)Core.Instance.Textures["MainLandTexture"];
            throw new Exception("WHOAA");
        }

    }
}
