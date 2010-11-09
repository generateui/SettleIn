using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;

namespace SettleIn.Classes.Rules
{
    public class AllTerritoriesHaveLandHexes:IRule
    {
        private string _Message=string.Empty;
        
        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            List<Territory> landlessTerritories = new List<Territory>();
            
            // make a list per territory with the hexcount 
            // (number of hexes having that territory)
            foreach (Territory t in b.Territories)
            {
                bool hasLand = false;

                foreach (Hex h in b.Hexes)
                {
                    ITerritoryHex terrHex = h as ITerritoryHex;
                    if (terrHex != null)
                    {
                        if (terrHex.TerritoryID.Equals(t.ID))
                        {
                            hasLand=true;
                            break;
                        }
                    }
                }

                // If no land found for the territory, add to landless territory list
                if (!hasLand)
                {
                    landlessTerritories.Add(t);
                }
            }

            // When we found any territories that do not have any land associated, 
            // the rule is broken. 
            // If so, generate a verbose error message.
            if (landlessTerritories.Count > 0)
            {
                string _Message = String.Format("{0} territory(ies) do not have any land hexes associated: ", landlessTerritories.Count.ToString());

                if (landlessTerritories.Count > 1)
                {
                    for (int t = 0; t < landlessTerritories.Count; t++)
                    {
                        if (t != landlessTerritories.Count - 1)
                        {
                            _Message += String.Format("{0}, ", landlessTerritories[t].Name);
                        }
                        else
                        {
                            _Message += String.Format("and {0}.", landlessTerritories[t].Name);
                        }
                    }
                }
                //only one landless territory
                else
                {
                    _Message += String.Format("{0}.", landlessTerritories[0].Name);
                }
                return false;
            }
            return true;
        }

        public string Problem
        {
            get { return _Message; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
