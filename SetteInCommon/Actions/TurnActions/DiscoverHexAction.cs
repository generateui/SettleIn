using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SettleInCommon.Board;

namespace SettleInCommon.Actions.TurnActions
{
    public class DiscoverHexAction: TurnAction
    {
        /// <summary>
        /// Discoveryhex player build a road/ship on a corner edge
        /// </summary>
        [DataMember]
        public HexLocation DiscoveryHex { get; set; }

        /// <summary>
        /// Resourcehex to replace the discoveryhex with
        /// </summary>
        [DataMember]
        public EResource DiscoveredHex { get; set; }

        /// <summary>
        /// Number to put on newly discovered resourcehex
        /// May be null: in this case, chits are swapped from mainland when a player builds
        /// alongside the newly discovered hex
        /// </summary>
        [DataMember]
        public int Number { get; set; }
    }
}
