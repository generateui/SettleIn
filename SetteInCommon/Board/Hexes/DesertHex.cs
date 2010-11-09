using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents a desert hex
    /// 
    /// A desert hex is a landhex, but has no resources.
    /// </summary>
    [DataContract]
    public class DesertHex : LandHex , ICloneable
    {

        #region ICloneable Members

        public object Clone()
        {
            return new DesertHex();
        }

        #endregion
    }
}
