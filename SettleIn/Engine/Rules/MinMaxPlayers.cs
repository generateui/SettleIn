using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks if minimum amount of players doesnt exceed the maximum
    /// number of players
    /// </summary>
    public class MinMaxPlayers:IRule
    {

        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            if (b.MinPlayers > b.MaxPlayers)
                return false;
            return true;
        }

        public string Problem
        {
            get { return "Minimum number of players cannot exceed maximum number of players"; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
