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
    public class BuildPointVisual : ControlPiece
    {
        private ScaleTransform3D _Scale = new ScaleTransform3D(1,1,1);

        public ScaleTransform3D Scale
        {
            get { return _Scale; }
        }
        private Piece _Piece;

        private HexPoint _Location;

        public HexPoint Location
        {
            get 
            {
                return _Location;   
            }
        }
        public Piece Piece
        {
            get { return _Piece; }
            set { _Piece = value; }
        }

        public BuildPointVisual(Point2D point, HexPoint location)
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
            Model3DGroup group = ((Model3DGroup)Core.Instance.Models["point"]).Clone();
            MeshGeometry3D mesh = ((MeshGeometry3D)Core.Instance.Models["pointMesh"]).Clone();
            //Model3DGroup g2 = group.Clone();
            
            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
            DiffuseMaterial material = new DiffuseMaterial(b);
            GeometryModel3D model2 = new GeometryModel3D(mesh, material);
            
            ScaleTransform3D scale = new ScaleTransform3D();
            TranslateTransform3D move = new TranslateTransform3D();
            TranslateTransform3D move2 = new TranslateTransform3D();
            AxisAngleRotation3D rotate = new AxisAngleRotation3D();
            Transform3DGroup t = new Transform3DGroup();
            double factor = 3;
            Console.WriteLine(_Point.X + "  " + _Point.Y);
            move.OffsetX = (1 / factor) * _Point.X;
            move.OffsetZ = 0.5;//;// new Random().NextDouble();
            move.OffsetY = -(1 / factor) * _Point.Y;//Y*factor;

            move2.OffsetY = -2;

            rotate.Axis = new Vector3D(1, 0, 0);
            rotate.Angle = 270;

            scale.ScaleX = factor;
            scale.ScaleY = factor;
            scale.ScaleZ = factor;
            t.Children.Add(move);
            t.Children.Add(scale);
            t.Children.Add(new RotateTransform3D(rotate));
            t.Children.Add(move2);
            t.Children.Add(_Scale);
            group.Transform = t;
            this.Content = group;// (Model3DGroup)Core.Instance.Models["side"];

            ColorAnimation da = new ColorAnimation(Color.FromArgb(150, 255, 255, 0), Color.FromArgb(255, 255, 255, 0),  new Duration(new TimeSpan(0, 0, 1)));
            da.RepeatBehavior = RepeatBehavior.Forever;
            da.AutoReverse = true;
            GeometryModel3D m1 = (GeometryModel3D)group.Children[0];
            GeometryModel3D m2 = (GeometryModel3D)group.Children[0];
            ((DiffuseMaterial)m1.Material).BeginAnimation(DiffuseMaterial.ColorProperty, da);
            ((DiffuseMaterial)m2.Material).BeginAnimation(DiffuseMaterial.ColorProperty, da);
            //model2.Material.colo
            //move2.BeginAnimation(TranslateTransform3D.OffsetYProperty, da);
        }
        //private static int w;
        //private static int wx
        //{ get { HexPoint.w += 10; return w; } }
    }
}
