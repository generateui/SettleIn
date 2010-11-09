using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the ore hex
    /// </summary>
    [Serializable]
    public class OreHex : RawResourceHex, ICloneable
    {
        public override EResource Resource
        { get { return EResource.Ore; } }

        #region ICloneable Members

        public object Clone()
        {
            return new OreHex();
        }

        #endregion
    }
}
