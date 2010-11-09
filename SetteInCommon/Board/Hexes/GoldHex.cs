using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents a gold hex. Gold hexes produce a resource which is chosen by the player.
    /// </summary>
    [Serializable]
    public class GoldHex : SpecialResourceHex, ICloneable
    {
        public override EResource Resource
        { 
            get { return EResource.Gold; } 
        }


        #region ICloneable Members

        public object Clone()
        {
            return new GoldHex();
        }

        #endregion
    }
}
