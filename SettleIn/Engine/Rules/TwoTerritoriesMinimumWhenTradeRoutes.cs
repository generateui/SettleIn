using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn.Classes.Rules
{
    public class TwoTerritoriesMinimumWhenTradeRoutes :IRule
    {
        private string _Message;

        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            if (b.UseTradeRoutes)
            {
                if (b.Territories.Count < 2)
                {
                    _Message = String.Format("When having trade routes enabled, the board need at least 2 territories defined. Now you have {0} defined", 
                        b.Territories.Count);
                    return false;
                }
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
