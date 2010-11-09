using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// represents the base type for each hex with a raw resource, which
    /// can be timber, wheat, ore, clay or sheep.
    /// </summary>
    [DataContract]
    [KnownType(typeof(TimberHex))]
    [KnownType(typeof(WheatHex))]
    [KnownType(typeof(OreHex))]
    [KnownType(typeof(ClayHex))]
    [KnownType(typeof(SheepHex))]
    public abstract class RawResourceHex : ResourceHex
    {
    }
}
