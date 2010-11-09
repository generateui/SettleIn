using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;

namespace SettleIn.Engine.Rules
{
    public class AllHexesMustBelongToTerritory : IRule
    {
        private List<HexLocation> hexesWithoutTerritory = new List<HexLocation>();
        private string _Problem=string.Empty;

        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            //create a ist of territory IDs
            IEnumerable<int> territoryIDs= from t in b.Territories
                                           select t.ID;

            foreach (Hex hex in from h in b.Hexes
                                where h is ITerritoryHex
                                select h)
                if (!territoryIDs.Contains(((ITerritoryHex)hex).TerritoryID))
                    hexesWithoutTerritory.Add(hex.Location);

            if (hexesWithoutTerritory.Count > 0)
            {
                _Problem = String.Format("{0} hexes without a territory",
                    hexesWithoutTerritory.Count);
                return false;
            }

            return true;
        }

        public string Problem
        {
            get { return _Problem; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
