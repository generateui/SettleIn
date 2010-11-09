using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using SettleInCommon.Board;

namespace SettleIn.Engine.ViewPort
{
    /// <summary>
    /// Represents a read-only game board, for presentation purposes.
    /// Used in the new board menu, so the user sees the board when
    /// selecting it for a new game, or new map creation.
    /// </summary>
    public class BoardViewerViewPort3D : SettleInViewPort3D
    {
        public BoardViewerViewPort3D()
        {
            //Make the board spin nicely. This way we can make 
            //the window smaller, since rotation shows the user the
            //whole board over time
            base.BoardChanged += new BoardChangedHandler(BoardViewerViewPort3D_BoardChanged);
        }

        void BoardViewerViewPort3D_BoardChanged()
        {
            if (Board != null)
            {
                //this.Board = new BoardVisual(new XmlBoard(8, 8));
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = 360;
                da.Duration = TimeSpan.FromSeconds(15);
                da.RepeatBehavior = RepeatBehavior.Forever;
                AxisAngleRotation3D r = new AxisAngleRotation3D();
                r.Axis = new Vector3D(0, 1, 0);
                this.Board.Transform = new RotateTransform3D(r);
                r.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            }
        }
    }
}
