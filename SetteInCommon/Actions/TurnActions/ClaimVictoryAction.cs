using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class ClaimVictoryAction : TurnAction
    {
        public override bool IsValid(SettleInCommon.Gaming.XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            GamePlayer gamePlayer = game.GetPlayer(Sender);
            if (gamePlayer.VictoryPoints < game.Settings.VpToWin)
            {
                _InvalidMessage = String.Format("You must have at least {0} victory point to claim victory",
                    game.Settings.VpToWin);
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            xmlGame.Phase = xmlGame.Phase.Next(xmlGame);
            xmlGame.WinnerID = Sender;
            GamePlayer player = xmlGame.GetPlayer(Sender);
            _Message = String.Format("{0} successfully claimed victory and won the game!",
                player.XmlPlayer.Name);

            base.PerformTurnAction(xmlGame);
        }
    }
}
