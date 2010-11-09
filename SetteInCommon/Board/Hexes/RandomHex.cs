using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents a hex of which the type is randomly produced at start of the game,
    /// when the map is rendered.
    /// </summary>
    [DataContract]
    public class RandomHex : RuleHex, ICloneable, ITerritoryHex
    {
        private int _TerritoryID;

        #region ICloneable Members

        public object Clone()
        {
            return new RandomHex();
        }

        #endregion

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
    }
}
