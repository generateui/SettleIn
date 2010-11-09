using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the wheat hex
    /// </summary>
    [DataContract]
    public class WheatHex : RawResourceHex, ICloneable
    {
        public override EResource Resource
        { get { return EResource.Wheat; } }


        #region ICloneable Members

        public object Clone()
        {
            return new WheatHex();
        }

        #endregion
    }
}
