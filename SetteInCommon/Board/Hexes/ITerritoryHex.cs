using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Gives hexes the possibility to have a Territory
    /// </summary>
    public interface ITerritoryHex
    {
        int TerritoryID { get; set; }
    }
}
