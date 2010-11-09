using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleIn;
using SettleInCommon.Board;

namespace SettleIn.Engine.Rules
{
    public class MaxOneMainlandRule  :IRule
    {
        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            if ((from t in b.Territories
                 where t.IsMainland
                 select t).Count() > 1)
                return false;
            else
                return true;
        }

        public string Problem
        {
            get { return "Maximum one mainland territory allowed"; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
