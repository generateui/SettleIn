using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents a clay hex
    /// </summary>
    [DataContract]
    public class ClayHex : RawResourceHex, ICloneable
    {
        public override EResource Resource
        { get { return EResource.Clay; } }


        #region ICloneable Members

        public object Clone()
        {
            return new ClayHex();
        }

        #endregion
    }
}
