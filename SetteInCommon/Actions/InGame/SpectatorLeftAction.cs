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
    /// <summary>
    /// Spectator left the game
    /// </summary>
    [DataContract]
    public class SpectatorLeftAction : InGameAction
    {
        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            XmlUser spectator = xmlGame.Spectators.Find(user => user.ID == Sender);

            xmlGame.Spectators.Remove(spectator);

            _Message = String.Format("Spectator {0} left the game.",
                spectator.Name);
        }
    }
}
