using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks if the user has any NoneHexes on the board.
    /// </summary>
    public class NoneHexRule:IRule
    {
        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            var x = (from h in b.Hexes where h.GetType() == typeof(NoneHex) select h).Count();
            return x == 0;
        }

        public string Problem
        {
            get { return "Empty hexes found in your board."; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Warning; }
        }

        #endregion
    }
}
