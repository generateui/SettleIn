using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the base type for each hex that behaves like a land hex. Roads
    /// can be built on land hexes, and land hexes can have a territory
    /// </summary>
    [DataContract]
    [KnownType(typeof(ResourceHex))]
    public abstract class LandHex : Hex, ITerritoryHex
    {
        //ID of territory hex belongs to. Default on mainland (ID=0).
        private int _TerritoryID = 1;

        // Show the territory in the viewport (the icon)
        private bool _ShowTerritory = true;

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
