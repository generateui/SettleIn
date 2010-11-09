using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Gaming.DevCards;
using SettleInCommon.Gaming.GamePhases;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class EndTurnAction : TurnAction
    {
        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game))
                return false;

            if (game.Phase.GetType() != typeof(PlayTurnsGamePhase))
            {
                _InvalidMessage = "Wrong gamephase: expected playTurns gamephase";
                return false;
            }

            return true;
        }
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            // Set the turnID before we end the turn
            int currentTurn = xmlGame.CurrentTurn;

            GamePlayer player = xmlGame.GetPlayer(Sender);

            // Reset ability to play devcards
            foreach (DevelopmentCard dc in player.DevCards)
                dc.IsPlayable = true;

            xmlGame.PlayerOnTurn = xmlGame.NextPlayer;

            xmlGame.EndTurn();

            // Set the message
            _Message = String.Format("{0} ended turn.", player.XmlPlayer.Name);

            base.PerformTurnAction(xmlGame);

            _TurnID = currentTurn;
        }
    }
}
