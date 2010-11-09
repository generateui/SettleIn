using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SettleIn.Engine.Boards;

namespace SettleIn
{
    /// <summary>
    /// Base type for each devcard type (like original devcard, and Cities dev card)
    /// </summary>
    public class Card : Piece
    {
        public Card(Point2D point)
            : base(point)
        { }
    }
}
