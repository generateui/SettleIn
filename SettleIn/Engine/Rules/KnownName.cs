using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks for a known name in the board name. Breaks when such name is found
    /// </summary>
    public class KnownName : IRule
    {

        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            switch (b.Name)
            {
                case "Standard Settlers": return false;
                case "Greater Catan": return false;
                default: return true;
            }
        }

        public string Problem
        {
            get { return "Name already used by an official map, choose a different name"; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Information; }
        }

        #endregion
    }
}
