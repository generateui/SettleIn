using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents a jungle hex. Jungle hexes produce discoveries. 
    /// Discoveries can be exchanged for ore, wheat or sheep when
    /// buying development cards
    /// </summary>
    [DataContract]
    public class JungleHex : SpecialResourceHex, ICloneable
    {
        [DataMember]
        public override EResource Resource
        { get { return EResource.Discovery; } }

        #region ICloneable Members

        public object Clone()
        {
            return new JungleHex();
        }

        #endregion
    }
}
