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
    public class Road : PlayerPiece
    {
        private HexSide _Location;

        public HexSide Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        private bool _IsRoadSelected;
        private GeometryModel3D _SelectedModel;
        private ScaleTransform3D _Scale = new ScaleTransform3D(1, 1, 1);

        public ScaleTransform3D Scale
        {
            get { return _Scale; }
        }
        private GeometryModel3D MakeSelectedModel()
        {
            MeshGeometry3D m1 = (MeshGeometry3D)Core.Instance.Models["Road"];
            EmissiveMaterial glow = new EmissiveMaterial(new SolidColorBrush(_Color));
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(_Color));
            GeometryModel3D model1 = new GeometryModel3D(m1, glow);
            model1.Transform = new ScaleTransform3D(1.05, 1.05, 1.05);
            return model1;
        }

        public bool IsRoadSelected
        {
            get { return _IsRoadSelected; }
            set
            {
                if (_SelectedModel == null)
                    _SelectedModel = MakeSelectedModel();

                if (value)
                {
                    if (!((Model3DGroup)Content).Children.Contains(_SelectedModel))
                        ((Model3DGroup)Content).Children.Add(_SelectedModel);
                }
                else
                {
                    if (_SelectedModel != null)
                        if (((Model3DGroup)Content).Children.Contains(_SelectedModel))
                            ((Model3DGroup)Content).Children.Remove(_SelectedModel);
                }
            }
        }

        public Road(Point2D point, Color color, HexSide location)
            : base(point, color)
        {
            _Location = location;
            Init();
        }
        private void Init()
        {
            MeshGeometry3D m1 = (MeshGeometry3D)Core.Instance.Models["Road"];
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(_Color));
            GeometryModel3D model1 = new GeometryModel3D(m1, material);

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model1);
            this.Content = group;
            ScaleTransform3D scale = new ScaleTransform3D();
            TranslateTransform3D move = new TranslateTransform3D();
            AxisAngleRotation3D rotate = new AxisAngleRotation3D();
            Transform3DGroup t = new Transform3DGroup();

            double angle = 0;

            switch (_Location.Direction)
            {
                case ESideDirection.SlopeDown: angle = 60; break;
                case ESideDirection.UpDown: angle = 0; break;
                case ESideDirection.SlopeUp: angle = -60; break;
            }
            scale.ScaleZ = 1.2;

            move.OffsetX = _Point.X;
            move.OffsetZ = _Point.Y;
            move.OffsetY = 0.5;

            rotate.Axis = new Vector3D(0, 1, 0);
            rotate.Angle = angle;

            t.Children.Add(scale);
            t.Children.Add(new RotateTransform3D(rotate));
            t.Children.Add(move);
            t.Children.Add(_Scale);

            this.Transform = t;

        }
    }
}
