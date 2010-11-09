using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleIn.Engine.Boards;

namespace SettleIn
{
    /// <summary>
    /// Base class for pieces that act as a UI helper, HexSideVisual and HexPointVisual being examples.
    /// These are no pieces directly in use for the player, but merely act as helper UI objects.
    /// </summary>
    public class ControlPiece : Piece
    {
        public ControlPiece(Point2D point)
            : base(point)
        {
        }
    }
}
