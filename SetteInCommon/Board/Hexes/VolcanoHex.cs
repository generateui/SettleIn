using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// represents the volcano hex. Volcanoes are gold hexes, but add a
    /// 1/6 chance the city/town blows up when it produces resources.
    /// A town should be destroyed when blowing up, a city should be reduced 
    /// to a town. When the player does not have any towns left when a city
    /// blows up, the city is not replaced by a town but completely removed.
    /// </summary>
    [DataContract]
    public class VolcanoHex : SpecialResourceHex, ICloneable
    {
        public override EResource Resource
        { get { return EResource.Gold; } }

        #region ICloneable Members

        public object Clone()
        {
            return new VolcanoHex();
        }

        #endregion
    }
}
