using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettleIn.UI.Elements
{
    public class DiceRoll
    {
        public int Roll1 { get; set; }
        public int Roll2 { get; set; }
        public int Roll { get { return Roll1 + Roll2; } }
    }
}
