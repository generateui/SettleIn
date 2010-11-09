using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the sheep hex
    /// </summary>
    [DataContract]
    public class SheepHex : RawResourceHex,  ICloneable
    {
        public override EResource Resource
        { get { return EResource.Sheep; } }

        #region ICloneable Members

        public object Clone()
        {
            return new SheepHex();
        }

        #endregion
    }
}
