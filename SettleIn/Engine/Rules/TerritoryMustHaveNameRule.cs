using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board;

namespace SettleIn
{
    public class TerritoryMustHaveNameRule : IRule
    {
        private string _Problem = string.Empty;
        private List<Territory> _InvalidTerritories = new List<Territory>();
        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            foreach (Territory t in b.Territories)
            {
                if (String.IsNullOrEmpty(t.Name))
                    _InvalidTerritories.Add(t);
            }

            if (_InvalidTerritories.Count > 0)
            {
                _Problem = String.Format("{0} territories do not have a name!",
                    _InvalidTerritories.Count);
                return false;
            }
            else
            {
                return true;
            }
        }

        public string Problem
        {
            get { return _Problem; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
