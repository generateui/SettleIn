using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks devstack for minimum amounts of cards. 
    /// </summary>
    public class MinDevStack : IRule
    {
        private RuleSeverity _Severity = RuleSeverity.Warning;
        private int _DevCount = 0;

        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            _DevCount = b.DevCards.CountAll;
            if (_DevCount < 25) return false;
            return true;
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }
        public string Problem
        {
            get { return "There are less then 25 dev cards in the stack. It is recommended that there are at least 25: 14 robbers, 5 VP's, 2 road buildings, 2 year of plenty's and 2 monopoly's."; }
        }

        #endregion
    }
}
