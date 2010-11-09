using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents an hex which is not known to the player when the game starts. "Hidden hex".
    /// When a player builds near a DiscoveryHex, the hex gets 'discovered'.
    /// </summary>
    [DataContract]
    public class DiscoveryHex : RuleHex, ICloneable,  ITerritoryHex
    {
        //ID of territory hex belongs to. Default on mainland (ID=0).
        private int _TerritoryID = 0;

        #region ITerritoryHex Members

        /// <summary>
        /// Number of the territory
        /// </summary>
        [DataMember]
        public int TerritoryID
        {
            get { return _TerritoryID; }
            set
            {
                if (value != _TerritoryID)
                {
                    _TerritoryID = value;
                    OnPropertyChanged("TerritoryID");
                }
            }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new DiscoveryHex();
        }

        #endregion
    }
}
