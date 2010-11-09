using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// represents the timber hex
    /// </summary>
    [Serializable]
    public class TimberHex : RawResourceHex, ICloneable
    {
        public override EResource Resource
        { get { return EResource.Timber; } }

        #region ICloneable Members

        public object Clone()
        {
            return new TimberHex();
        }

        #endregion
    }
}
