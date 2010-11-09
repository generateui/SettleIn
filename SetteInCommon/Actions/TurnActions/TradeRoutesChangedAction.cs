using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.TurnActions
{
    public class TradeRoutesChangedAction: TurnAction
    {
        //List of routes which have becamo invalid for a given player
        public Dictionary<int, TradeRoute> AddedRoutes { get; set; }

        // List of routes which given player lost
        public Dictionary<int, TradeRoute> RemovedRoutes { get; set; }

        //List o routes of given player which are changed 
        // (same start/end, different route length
        public Dictionary<int, TradeRoute> ChangedRoutes { get; set; }

        public override bool IsValid(SettleInCommon.Gaming.XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (AddedRoutes == null &&
                RemovedRoutes == null &&
                ChangedRoutes == null)
            {
                _InvalidMessage = "Added/removed/changed -routes: at least one needs to be non-null";
                return false;
            }

            return true;
        }

        public override void PerformTurnAction(SettleInCommon.Gaming.XmlGame xmlGame)
        {
            foreach (KeyValuePair<int, TradeRoute> removedRoute in RemovedRoutes)
            {
                GamePlayer player = xmlGame.GetPlayer(removedRoute.Key);
                player.TradeRoutes.Remove(
                    player.TradeRoutes
                    .Where(tr => tr.Equals(removedRoute.Value)).First());
            }

            base.PerformTurnAction(xmlGame);
        }
    }
}
