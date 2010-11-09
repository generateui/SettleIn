using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Board;

namespace SettleIn.Engine.Rules
{
    public class MaxBonusIslandVPEqualOrLessThenTerritoriesCount : IRule
    {
        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            throw new NotImplementedException();
        }

        public string Problem
        {
            get { return "You have more island bonus VP then the amount of teritories. Setting more than the amount of territories has no effect."; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Warning; }
        }

        #endregion
    }
}
