using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Gaming;
using System.Runtime.Serialization;
using SettleInCommon.Gaming.GamePhases;

namespace SettleInCommon.Actions.InGame
{
    [DataContract]
    public class HostStartsGameAction : InGameAction
    {
        [DataMember]
        public XmlGameSettings Settings { get; set; }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            //xmlGame.Phase = new DetermineFirstPlayerGamePhase();

            base.PerformTurnAction(xmlGame);
        }
    }
}
