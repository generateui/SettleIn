using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;

using System.Windows.Media.Animation;
using System.Windows;

using SettleInCommon.Board;
using SettleIn.Engine.Boards;

namespace SettleIn
{
    /// <summary>
    /// This is a cilindrical shape placed when the user
    /// wants to see the options to build on. In the game,
    /// this is the thing a player clicks when he wants to
    /// place a town/city. 
    /// </summary>
    public class HexSideVisual : ControlPiece
    {
        private HexSide _Location;
        private ScaleTransform3D _Scale = new ScaleTransform3D(1, 1, 1);

        public ScaleTransform3D Scale
        {
            get { return _Scale; }
        }

        public HexSide Location
        {
            get
            {
                return _Location;
            }
        }


        public HexSideVisual(Point2D point, HexSide location)
            :base(point)
        {
            _Location = location;
            Init();
        }

        /// <summary>
        /// Add a 3D model to the instance
        /// </summary>
        private void Init()
        {
            MeshGeometry3D m1 = (MeshGeometry3D)Core.Instance.Models["Road"];
            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
            DiffuseMaterial material = new DiffuseMaterial(b);
            GeometryModel3D model1 = new GeometryModel3D(m1, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model1);
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
                case ESideDirection.SlopeDown: angle = 60; break;
                case ESideDirection.UpDown: angle = 0; break;
                case ESideDirection.SlopeUp: angle = -60; break;
            }
            scale.ScaleZ = 1.2;
            scale.ScaleX = 3;

            move.OffsetX = _Point.X;
            move.OffsetZ = _Point.Y;
            move.OffsetY = 0.5;

            rotate.Axis = new Vector3D(0, 1, 0);
            rotate.Angle = angle;

            t.Children.Add(scale);
            t.Children.Add(new RotateTransform3D(rotate));
            t.Children.Add(move);
            t.Children.Add(_Scale);

            ColorAnimation da = new ColorAnimation(Color.FromArgb(255, 255, 255, 0), Color.FromArgb(150, 255, 255, 0), new Duration(new TimeSpan(0, 0, 1)));
            da.RepeatBehavior = RepeatBehavior.Forever;
            da.AutoReverse = true;
            material.BeginAnimation(DiffuseMaterial.ColorProperty, da);

            this.Transform = t;
        }
    }
}
