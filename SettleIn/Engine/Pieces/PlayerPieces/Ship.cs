using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;

using System.Windows.Media.Animation;
using System.Windows;

using SettleInCommon.Board;
using SettleIn.Engine.Pieces;
using SettleIn.Engine.Boards;

namespace SettleIn
{
    /// <summary>
    /// Represent the city 
    /// </summary>
    public class Ship : PlayerPiece
    {
        private HexSide _Location;
        private Model3DGroup _SelectedModel;
        private bool _IsSelected = false;

        public HexSide Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        public bool IsSelected
        {
            get 
            {
                return _IsSelected;
            }
            set
            {
                if (_SelectedModel == null)
                    _SelectedModel = MakeSelectedModel();
                if (value != _IsSelected)
                {
                    if (value)
                        ((Model3DGroup)Content).Children.Add(_SelectedModel);
                    else
                        ((Model3DGroup)Content).Children.Remove(_SelectedModel);
                    _IsSelected = value;
                }
            }
        }

        public Ship(Point2D point, Color color, HexSide location)
            : base(point, color)
        {
            _Location = location;
            Init();
        }

        private Model3DGroup MakeSelectedModel()
        {
            MeshGeometry3D m1 = (MeshGeometry3D)Core.Instance.Models["Ship1"];
            MeshGeometry3D m2 = (MeshGeometry3D)Core.Instance.Models["Ship2"];
            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(75,255,255,0));
            EmissiveMaterial material = new EmissiveMaterial(b);
            GeometryModel3D model1 = new GeometryModel3D(m1, material);
            GeometryModel3D model2 = new GeometryModel3D(m2, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model1);
            group.Children.Add(model2);
            group.Transform = new ScaleTransform3D(1.05, 1.05, 1.05);
            return group;
        }

        private void Init()
        {
            MeshGeometry3D m1 = (MeshGeometry3D)Core.Instance.Models["Ship1"];
            MeshGeometry3D m2 = (MeshGeometry3D)Core.Instance.Models["Ship2"];
            SolidColorBrush b = new SolidColorBrush(_Color);
            DiffuseMaterial material = new DiffuseMaterial(b);
            GeometryModel3D model1 = new GeometryModel3D(m1, material);
            GeometryModel3D model2 = new GeometryModel3D(m2, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model1);
            group.Children.Add(model2);
            this.Content = group;
            ScaleTransform3D scale = new ScaleTransform3D();
            TranslateTransform3D move = new TranslateTransform3D();
            TranslateTransform3D move2 = new TranslateTransform3D();
            AxisAngleRotation3D rotate = new AxisAngleRotation3D();
            AxisAngleRotation3D rotate2 = new AxisAngleRotation3D();
            Transform3DGroup t = new Transform3DGroup();

            double angle = 0;

            switch (_Location.Direction)
            {
                case ESideDirection.SlopeDown: angle = 150; break;
                case ESideDirection.UpDown: angle = 90; break;
                case ESideDirection.SlopeUp: angle = -150; break;
            }
            double factor = 25;
            scale.ScaleZ = factor;
            scale.ScaleY = factor;
            scale.ScaleX = factor;

            move.OffsetX = _Point.X;
            move.OffsetZ = _Point.Y;
            move.OffsetY = 0;

            rotate.Axis = new Vector3D(0, 1, 0);
            rotate.Angle = angle;

            t.Children.Add(scale);
            t.Children.Add(new RotateTransform3D(rotate));
            t.Children.Add(move);

            this.Transform = t;

        }
    }
}
