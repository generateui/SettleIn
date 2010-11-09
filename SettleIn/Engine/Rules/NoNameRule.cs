using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks if the board has a name
    /// </summary>
    public class NoNameRule : IRule
    {
        private RuleSeverity _Severity = RuleSeverity.Error;

        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            if (b.Name != null)
            {
                if (b.Name.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public RuleSeverity Severity
        {
            get { return  RuleSeverity.Error; }
        }
        public string Problem
        {
            get { return "Board does not have a name"; }
        }

        #endregion
    }
}
