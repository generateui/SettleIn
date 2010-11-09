using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board;

namespace SettleIn
{
    public interface IRule
    {
        bool Invoke(XmlBoard b);
        string Problem { get; }
        RuleSeverity Severity { get; }
    }
}
