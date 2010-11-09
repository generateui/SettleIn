using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents an hex that is omitted in generating the playing board.
    /// Used as placeholder for a width*height grid for boards that do not
    /// have a square layout, but different like the standard settlers map
    /// </summary>
    [DataContract]
    public class NoneHex : Hex, ICloneable
    {
        #region ICloneable Members

        public object Clone()
        {
            return new NoneHex();
        }

        #endregion
    }
}
