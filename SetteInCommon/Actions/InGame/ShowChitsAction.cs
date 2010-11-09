using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Board;

namespace SettleInCommon.Actions.InGame
{
    /// <summary>
    /// Server initiated action where the chitnumber off the board are showed
    /// </summary>
    public class ShowChitsAction: InGameAction
    {
        public Dictionary<HexLocation, EChitNumber> Chits { get; set; }
    }
}
