using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.User;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.InGame
{
    [DataContract]
    public class GameChatAction : InGameAction
    {
        [DataMember]
        public string ChatMessage { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid()) return false;

            if (String.IsNullOrEmpty(Message))
            {
                _InvalidMessage = "You gotta have something to say. Message IsNullOrEmpty.";
                return false;
            }

            return true;
        }
    
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            _Message = String.Format("{0} said: {1}", xmlGame.GetPlayer(Sender), ChatMessage);
            //xmlGame.GameChat.

            base.PerformTurnAction(xmlGame);
        }

    }
}
