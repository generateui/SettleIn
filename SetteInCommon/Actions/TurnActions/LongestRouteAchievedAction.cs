using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.TurnActions
{
    public class LongestRouteAchievedAction : TurnAction
    {
        Route _Route = null;

        /// <summary>
        /// Route can actually be null. This can happen on two occasions:
        /// - Current LR is split when building a town in the middle, no other 
        ///   players have route > 4
        /// - Current LR is split when building town in the middle, and other players
        ///   draw on road length (example: current holder of LR has 8 length, gets town in
        ///   the middle, effecting in two roads of 4 length. Player 2 and player 3 both have
        ///   roads of 6 length)
        /// </summary>
        public Route Route
        {
            get { return _Route; }
            set { _Route = value; }
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game))
                return false;

            return true;
        }
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer player = xmlGame.GetPlayer(Sender);

            // Old owner looses route
            if (xmlGame.LongestRouteOwner != null)
            {
                xmlGame.LongestRouteOwner.LongestRoute = null;
            }
            
            // New owner gets route
            player.LongestRoute = Route;

            if (Route == null)
            {
                _Message = String.Format("No one gets a new route!");
            }
            else
            {
                _Message = String.Format("{0} has the longest route of length {1}",
                    player.XmlPlayer.Name, Route.Count);
            }

            base.PerformTurnAction(xmlGame);
        }

    }
}
