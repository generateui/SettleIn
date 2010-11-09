using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using SettleInCommon.Board;
using SettleIn.Engine.Boards;

namespace SettleIn.Engine.Pieces
{
    public class Pirate : Traveller, IMoveable
    {
        private TranslateTransform3D _Move = new TranslateTransform3D();

        private HexLocation _Location;

        public HexLocation Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        public Pirate(Point2D point, HexLocation location)
            : base(point)
        {
            _Location = location;
            Init();
        }

        private void Init()
        {
            MeshGeometry3D m1 = (MeshGeometry3D)Core.Instance.Models["Ship1"];
            MeshGeometry3D m2 = (MeshGeometry3D)Core.Instance.Models["Ship2"];
            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            DiffuseMaterial material = new DiffuseMaterial(b);
            GeometryModel3D model1 = new GeometryModel3D(m1, material);
            GeometryModel3D model2 = new GeometryModel3D(m2, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model1);
            group.Children.Add(model2);
            this.Content = group;
            ScaleTransform3D scale = new ScaleTransform3D();
            TranslateTransform3D move = new TranslateTransform3D();
            AxisAngleRotation3D rotate = new AxisAngleRotation3D();
            AxisAngleRotation3D rotate2 = new AxisAngleRotation3D();
            Transform3DGroup t = new Transform3DGroup();

            double angle = 0;

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
            t.Children.Add(_Move);

            this.Transform = t;

        }

        #region IMoveable Members

        public TranslateTransform3D Move
        {
            get { return _Move; }
        }

        #endregion
    }
}
