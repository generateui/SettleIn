using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the base type for hexes that have special rules
    /// </summary>
    [DataContract]
    [KnownType(typeof(DiscoveryHex))]
    [KnownType(typeof(RandomHex))]
    public abstract class RuleHex : Hex
    {

    }
}
