using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;

namespace SettleIn
{
    public class HexChangedEventArgs
    {
        private Hex hex;
        public HexChangedEventArgs(Hex h)
        {
            this.hex = h;
        }
        public Hex Hex
        {
            get { return hex; }
        }
    }
}
