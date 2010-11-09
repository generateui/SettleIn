using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.TurnActions
{
    public class LargestArmyAchievedAction : TurnAction
    {
        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game))
                return false;

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            // set new owner of largest army
            GamePlayer player = xmlGame.GetPlayer(Sender);
            _GamePlayer = player;

            // reset old owner of largest army
            player.HasLargestArmy = true;
            foreach (GamePlayer p in xmlGame.Players)
                if (p != player && p.HasLargestArmy)
                    p.HasLargestArmy = false;

            _Message = String.Format("{0} is the mightiest of all players and gets largest army using {1} soldiers",
                player.XmlPlayer.Name, player.PlayedSoldierCount.ToString());

            base.PerformTurnAction(xmlGame);
        }
    }
}
