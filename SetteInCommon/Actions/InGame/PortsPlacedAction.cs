using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.InGame
{
    [DataContract]
    public class PortsPlacedAction : InGameAction
    {
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            base.PerformTurnAction(xmlGame);
        }
    }
}
