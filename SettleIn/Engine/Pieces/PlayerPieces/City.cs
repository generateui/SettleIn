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
    public class City : PlayerPiece
    {
        public HexPoint Location { get; set; }
        public City(Point2D point, Color color, HexPoint location)
            :base(point, color)
        {
            Location = location;
            Init();
        }
        private void Init()
        {
            MeshGeometry3D m1 = (MeshGeometry3D)Core.Instance.Models["City1"];
            MeshGeometry3D m2 = (MeshGeometry3D)Core.Instance.Models["City2"];
           
            DiffuseMaterial material =  
                new DiffuseMaterial(new SolidColorBrush(_Color));
            
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

            move2.OffsetX = 0;
            move2.OffsetY = -1.7;

            move.OffsetX = _Point.X - 2;
            move.OffsetZ = _Point.Y;
            move.OffsetY = 0;

            rotate.Axis = new Vector3D(1, 0, 0);
            rotate.Angle = 270;

            rotate2.Axis = new Vector3D(0, 0, 1);
            rotate2.Angle = 45;
            
            double scaleFactor = 35;
            scale.ScaleX = scaleFactor;
            scale.ScaleY = scaleFactor;
            scale.ScaleZ = scaleFactor;
            t.Children.Add(scale);
            t.Children.Add(move);

            this.Transform = t;
        }
    }
}
