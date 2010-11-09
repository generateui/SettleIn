using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleIn.Engine.Boards;

namespace SettleIn
{
    /// <summary>
    /// Base class for the Robber and the Pirate
    /// </summary>
    public class Traveller:Piece
    {
        public Traveller(Point2D point)
            : base(point)
        { }
    }
}
