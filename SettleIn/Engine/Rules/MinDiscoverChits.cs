using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks if the user selected enough randomchits to put on discovery hexes
    /// </summary>
    public class MinDiscoverChits : IRule
    {

        #region IRule Members

        private int _DiscoverHexesCount = 0;

        public bool Invoke(XmlBoard b)
        {
            /*
            _DiscoverHexesCount = (from h in b.Hexes where h is DiscoveryHex select h).Count();
            if (_DiscoverHexesCount > b.RandomChits.CountAll)
                return false;
            else
                return true;
             */
            return true;
        }

        public string Problem
        {
            get
            {
                return "Since you have " + _DiscoverHexesCount +
                    " discovery hex(es), you need at least " + _DiscoverHexesCount +
                    " chit(s) in the \"discover chits\" stack.";
            }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
