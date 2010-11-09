using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Checks if the user has selected enough randomly assignable chits 
    /// for the hexes randomized at game start
    /// </summary>
    public class MinRandomChits : IRule
    {
        #region IRule Members

        private int _RandomHexesCount=0;
               
        public bool Invoke(XmlBoard b)
        {
            /*
            _RandomHexesCount = (from h in b.Hexes 
                                 where h is RandomHex
                                 select h).Count();

            if (_RandomHexesCount > b.RandomChits.CountAll) 
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
                return String.Format("Since you have {0}" +  
                    " random hex(es), you need at least {0}" +
                    " chit(s) in the \"randomly assigned chits\" stack.",_RandomHexesCount); }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
