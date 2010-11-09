using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using SettleIn.Engine.Boards;

namespace SettleIn.Engine.Pieces
{
    public class PlayerPiece:Piece
    {
        protected Color _Color = Color.FromRgb(255, 128, 64);
        
        public virtual Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
        public PlayerPiece(Point2D point,  Color color)
            :base(point)
        {
            _Color = color;
        }
    }
}
