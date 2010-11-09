using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the base type for hexes that have different resource 
    /// production then a raw resource, such as gold or discoveries.
    /// </summary>
    [DataContract]
    [KnownType(typeof(GoldHex))]
    [KnownType(typeof(VolcanoHex))]
    [KnownType(typeof(JungleHex))]
    public abstract class SpecialResourceHex : ResourceHex
    {
    }
}
