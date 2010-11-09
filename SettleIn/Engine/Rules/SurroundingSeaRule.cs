using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks if the board has all sea hexes on the edges. 
    /// Breaks if a non-seahex is found on an edge.
    /// </summary>
    public class SurroundingSeaRule:IRule
    {
        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            //check top  + bottom hexes
            for (int i = 0; i < b.Width; i++)
            {
                if (!(b.Hexes[i, 0] is SeaHex && b.Hexes[i,b.Height-1] is SeaHex))
                {
                    return false;
                }
            }
            //check left + right hexes
            for (int i = 1; i < b.Height; i++)
            {
                if (!(b.Hexes[0,i] is SeaHex && b.Hexes[b.Width-1,i] is SeaHex))
                {
                    return false;
                }
            }
            return true;
        }

        public string Problem
        {
            get { return "Outer hexes should all be a sea hex"; }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
