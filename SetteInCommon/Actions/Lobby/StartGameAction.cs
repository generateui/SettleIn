using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Gaming;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using SettleInCommon.Actions.InGame;

namespace SettleInCommon.Actions.TurnActions
{
    //Server message to all players with game in starting status
    [DataContract]
    public class StartGameAction : InGameAction
    {
        [DataMember]
        public XmlGame Game { get; set; }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            xmlGame.PlayerOnTurn = xmlGame.Players[0];

            base.PerformTurnAction(xmlGame);
        }
    }
}
