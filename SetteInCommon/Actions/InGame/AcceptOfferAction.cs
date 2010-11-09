using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.InGame
{
    /// <summary>
    /// Indicates the player accepted latest offer
    /// </summary>
    [DataContract]
    public class AcceptOfferAction : InGameAction
    {
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            
            base.PerformTurnAction(xmlGame);
        }
    }
}
